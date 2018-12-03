using Renamer.Exceptions;
using Renamer.Presenter;
using System;
using System.Collections.Generic;
using System.IO;

namespace Renamer.Model
{
    public interface IModel
    {
        string State { get; set; }
        /// <summary>
        /// IncludeFile, IncludeFolder, IncludeRootFolder 각 값을 2진수로 하여 순서대로 자리에 배치한 값으로, 세 option 의 값 상태를 나타낸다. 
        /// (예를 들어, true, false, true 이면 2진수 101 이 된다)
        /// 파일 찾기 할때 저장되며, 파일 변환시의 상태와 비교하여 다르면 변환이 진행되지 않도록 한다.
        /// </summary>
        short OptionState { get; set; }
        bool IncludeFile { get; set; }
        bool IncludeFolder { get; set; }
        bool IncludeRootFolder { get; set; }
        DirectoryInfo Directory { get; set; }
        List<BaseVo> QueriedFileList { get; set; }
        string Where { get; set; }
        string To { get; set; }
        
        void Init();
        void SelectDirectory(string directory);
        void QueryFileList(string where, ListAdder adder);
        ActionResult ConvertFiles(string to, StatusListen listen);
        ActionResult Undo(StatusListen listen);
        bool AbleToConvert();
        bool AbleToUndo();
        void ClearList();
    }


    public class RenamerModel : IModel
    {
        private Properties props;
        private ConvertAction prevAction;

        public /*override*/ string State { get; set; } 

        public /*override*/ short OptionState { get; set; }

        public /*override*/ bool IncludeFile { get; set; }
        public /*override*/ bool IncludeFolder { get; set; }
        public /*override*/ bool IncludeRootFolder { get; set; }
        public /*override*/ List<BaseVo> QueriedFileList { get; set; }  // 디렉토리도 포함될 수 있음
        public /*override*/ DirectoryInfo Directory { get; set; }
        public /*override*/ string Where { get; set; }
        public /*override*/ string To { get; set; }


        public /*override*/ void ClearList()
        {
            prevAction = null;
            QueriedFileList.Clear();
        }

        public RenamerModel()
        {
            props = new Properties("renamer.ini");
            QueriedFileList = new List<BaseVo>();
        }

        void IModel.Init()
        {
            State = AppState.INIT;

            string readDir = props.get("last_dir");
            if (string.IsNullOrEmpty(readDir) || System.IO.Directory.Exists(readDir) == false)
            {
                Directory = new DirectoryInfo(@"C:\");
            }
            else
            {
                Directory = new DirectoryInfo(readDir);
            }

            IncludeFile = true;
            IncludeFolder = false;
            IncludeRootFolder = false;
            //OptionState = (1 << 2) + (0 << 1) + (0);
            OptionState = getCurrentOptionState();
        }

        private short getCurrentOptionState()
        {
            return (short)((Convert.ToInt16(IncludeFile) << 2) + (Convert.ToInt16(IncludeFolder) << 1) + (Convert.ToInt16(IncludeRootFolder)));
        }

        void IModel.SelectDirectory(string directory)
        {
            prevAction = null;
            DirectoryInfo dir = new DirectoryInfo(directory);
            if (dir.Exists)
            {
                this.Directory = dir;
                props.set("last_dir", this.Directory.FullName);
                props.Save();
            }
            else
            {
                throw new InvalidOperationException("Directory [" + directory + "] Not found.");
            }
            State = AppState.SELECT_DIR_DONE;
        }

        public /*override*/ void QueryFileList(string where, ListAdder adder)
        {
            prevAction = null;

            this.Where = where;

            //QueriedFileList = new List<BaseVo>();
            QueriedFileList.Clear();
            
            //Queue<DirectoryInfo> q = new Queue<DirectoryInfo>();
            Queue<QNode> q = new Queue<QNode>();
            
            //q.Enqueue(Directory);
            q.Enqueue(new QNode() { Root = true, DirInfo = Directory });

            while (q.Count > 0)
            {
                //DirectoryInfo poll = q.Dequeue();
                QNode poll = q.Dequeue();
                //if (poll.Exists)
                if (poll.DirInfo.Exists)
                {
                    // 폴더 포함 체크가 되어 있는 경우, 해당 디렉토리 먼저 file list 에 담는다. 물론, 경로명에 검색 문자열이 있다면!!
                    if (IncludeFolder)
                    {
                        //if (poll.Name.IndexOf(where) >= 0)
                        if (poll.DirInfo.Name.IndexOf(where) >= 0)
                        {
                            //if (IncludeRootFolder || QueriedFileList.Count > 0)  // IncludeRootFoler 옵션이 true 이거나, 그렇지 않다면 (QueriedFielList 에 값이 없다면 검색을 시작하는 루트 폴더이므로) 루트 풀더는 변경하지 않는다.
                            if (IncludeRootFolder || !poll.Root)  // IncludeRootFoler 옵션이 true 인 경우, 또는 IncludeRootFolder 옵션이 false 이면 루트 풀더는 변경하지 않는다(root 가 아닌 경우만 add 한다는 말).
                            {
                                //QueriedFileList.Add(new DirectoryVo(QueriedFileList.Count + 1, poll, Status.STANDBY));
                                QueriedFileList.Add(new DirectoryVo(QueriedFileList.Count + 1, poll.DirInfo, Status.STANDBY));
                                adder(QueriedFileList[QueriedFileList.Count - 1]);
                            }
                        }
                    }

                    if (IncludeFile)
                    {
                        // 위에서 담긴 디렉토리에 있는 파일들이 그 다음에 리스트에 담긴다.
                        //foreach (FileInfo fi in poll.GetFiles())
                        foreach (FileInfo fi in poll.DirInfo.GetFiles())
                        {
                            //if (fi.Name.ToUpper().IndexOf(where.ToUpper()) >= 0)
                            if (fi.Name.IndexOf(where) >= 0)
                            {
                                QueriedFileList.Add(new FileVo(QueriedFileList.Count + 1, fi, Status.STANDBY));
                                adder(QueriedFileList[QueriedFileList.Count - 1]);
                            }
                        }
                    }

                    // Include Option 이 어떻든지 상관 없이 다음 폴더를 queue 에 넣어줘야 한다.
                    //foreach (DirectoryInfo di in poll.GetDirectories())
                    foreach (DirectoryInfo di in poll.DirInfo.GetDirectories())
                    {
                        //q.Enqueue(di);
                        q.Enqueue(new QNode() { Root = false, DirInfo = di });
                    }
                }
            }

            OptionState = getCurrentOptionState();

            State = AppState.QUERY_LIST_DONE;
        }


        public /*override*/ ActionResult ConvertFiles(string to, StatusListen listen)
        {
            prevAction = null;  // throw exception 되면 prevAction 이 남을수도 있기 때문에..
            this.To = to;
            ActionResult convertResult = new ActionResult();
            convertResult.CountTotal = QueriedFileList.Count;

            if (OptionState != getCurrentOptionState()) throw new DifferentOptionState();

            ConvertAction convertAction = new ConvertAction(QueriedFileList, IncludeFolder, Where, To);
            prevAction = convertAction;

            // As of 2018.12.01 : QueriedFileList 에 이제 폴더도 포함되어 있을 수 있다. 
            // 따라서 변환 작업은 FileVo 먼저 변환을 한다음 폴더는 QueriedFileList 뒤에서 부터 변환 하여야 한다. 
            // 왜냐하면 폴더명 먼저 바꿀 경우 그 안에 파일들은 변환이 안될것이기 때문임.
            if (IncludeFile)    // 파일 찾기 할 때는 includeFile true 로 했다가, 변환하기 직전에 false 로 바꿀 수 있음. 이럴 경우 파일은 포함 안하는게 맞음
            {
                foreach (BaseVo vo in QueriedFileList)
                {
                    if (vo.TypeOfVo != VoType.FILE) continue;

                    convertVo(vo, listen, ref convertResult);

                    #region old code
                    //bool succ = false;
                    ////if (File.Exists(vo.PathOriginal))
                    //if (vo.ExistsInOriginalPath())
                    //{
                    //    vo.SetPathMoved(this.Where, this.To);
                    //    vo.Status = Status.ON_CONV;
                    //    listen(vo);
                    //    try
                    //    {
                    //        File.Move(vo.PathOriginal, vo.PathMoved);
                    //        succ = true;
                    //    }
                    //    catch
                    //    {
                    //        succ = false;
                    //    }                    
                    //}
                    //else
                    //{
                    //    succ = false;
                    //}

                    //if (succ)
                    //{
                    //    vo.Status = Status.CONV_COMPLETE;
                    //    listen(vo);
                    //    ++convertResult.CountSuccess;
                    //}
                    //else
                    //{
                    //    vo.Status = Status.NOT_ABLE_TO_CONV;
                    //    listen(vo);
                    //    ++convertResult.CountFail;
                    //}
                    #endregion
                }
            }
            
            if (IncludeFolder)  // 마찬가지 디렉토리를 바꿀거라고 처음에 파일찾기 시에는 포함시켰다가 폴더 포함 체크 박스 해제하고 변환하기 하는 경우도 있을 수 있으므로 
            {
                // 디렉토리는 상위 먼저 바꿀 경우 하위 폴더는 나중에 접근이 불가 할 수 있다.
                // 따라서 하위 부터 변환을 하기 위해 QueriedFileList 의 역순으로
                for (int vndex = QueriedFileList.Count - 1; vndex >= 0; --vndex)
                {
                    BaseVo vo = QueriedFileList[vndex];

                    if (vo.TypeOfVo != VoType.DIRECTORY) continue;

                    convertVo(vo, listen, ref convertResult);
                }
            }

            State = AppState.CONVERT_DONE;

            return convertResult;

        }


        private void convertVo(BaseVo vo, StatusListen listen, ref ActionResult convertResult)
        {
            bool succ = false;
            //if (File.Exists(vo.PathOriginal))
            if (vo.ExistsInOriginalPath())
            {
                //vo.SetPathMoved(this.Where, this.To);
                vo.DefinePathMoved(this.Where, this.To);
                vo.Status = Status.ON_CONV;
                listen(vo);
                try
                {
                    //File.Move(vo.PathOriginal, vo.PathMoved);
                    vo.Convert();   // throws Exception if not availale
                    succ = true;
                }
                catch
                {
                    succ = false;
                }
            }
            else
            {
                succ = false;
            }

            if (succ)
            {
                vo.Status = Status.CONV_COMPLETE;
                listen(vo);
                ++convertResult.CountSuccess;
            }
            else
            {
                vo.Status = Status.NOT_ABLE_TO_CONV;
                listen(vo);
                ++convertResult.CountFail;
            }
        }


        public /*override*/ ActionResult Undo(StatusListen listen)
        {
            if (AbleToUndo() == false) throw new InvalidOperationException("더이상 실행 취소 할 수 없습니다... 변환 하기 작업을 이용해 주세요.");
            
            ActionResult undoResult = new ActionResult();
            undoResult.CountTotal = QueriedFileList.Count;

            #region old code 이렇게 하면 순서가 틀린듯
            ////foreach (FileVo vo in prevAction.FileList)
            //foreach (BaseVo vo in prevAction.VoList)
            //{
            //    // As of 2018.12.01 : 우선 FILE VO 부터 먼저 처리
            //    if (vo.TypeOfVo != VoType.FILE) continue;

            //    revertVo(vo, listen, prevAction, ref undoResult);
            //    #region old code
            //    //bool succ = false;
            //    //// vo.PathMoved 는 변환 작업 중 vo 의 PathOriginal 에서 where 문자열을 to 문자열로 replace 한 파일명이 되겠다.
            //    //// 정상적으로 변환이 되었으면 상태는 conv_complete 이 되어 있을 것이고, PathMoved에도 바뀐 파일명으로 들어가 있어야 함.
            //    ////if (vo.Status == Status.CONV_COMPLETE && File.Exists(vo.PathMoved))
            //    //if (vo.Status == Status.CONV_COMPLETE && vo.ExistsInMovedPath())
            //    //{
            //    //    vo.Status = Status.ON_REVERT;
            //    //    listen(vo);
            //    //    try
            //    //    {
            //    //        //File.Move(vo.PathMoved, vo.PathOriginal);
            //    //        vo.Revert();    // throws Exception if not possible
            //    //        succ = true;
            //    //    }
            //    //    catch
            //    //    {
            //    //        succ = false;
            //    //    }
            //    //}
            //    //else
            //    //{
            //    //    succ = false;
            //    //}

            //    //if (succ)
            //    //{
            //    //    vo.Status = Status.REVERT_COMPLETE;
            //    //    listen(vo);
            //    //    ++undoResult.CountSuccess;
            //    //}
            //    //else
            //    //{
            //    //    vo.Status = Status.NOT_ABLE_TO_REVERT;
            //    //    listen(vo);
            //    //    ++undoResult.CountFail;
            //    //}
            //    #endregion

            //}

            //// prevAction 안의 QueriedFileList 에는 폴더 기준으로 봤을 때 index 가 작을 수록 상위 폴더이다.
            //// 따라서, 마찬가지로 undo 할 때도 역순으로 해야 상위 폴더가 나중에 원복될 수 있다.
            //// 원복 시에는 IncludeFolder 와 상관없이 QueriedFileList 에 있는 건 다 원복해야 한다.
            //for (int vndex = QueriedFileList.Count - 1; vndex >= 0; --vndex)
            //{
            //    BaseVo vo = QueriedFileList[vndex];

            //    if (vo.TypeOfVo != VoType.DIRECTORY) continue;

            //    revertVo(vo, listen, prevAction, ref undoResult);
            //}
            #endregion


            // UNDO 할 때는 CONVER 때와는 반대로 폴더, 그것도 상위 폴더 부터 원복하여야 한다. 크게 의도하지 않았지만 그렇게 하는 것으로 BaseVo class 가 설계가 되었기 때문에...
            foreach (BaseVo vo in prevAction.VoList)
            {
                // 그냥 처음 conver 할 때 그 순서대로 쭉 원복하면 될거 같음.
                //if (vo.TypeOfVo != VoType.FILE) continue;
                revertVo(vo, listen, prevAction, ref undoResult);
            }

            prevAction = null;

            State = AppState.UNDONE;

            return undoResult;
        }

        private void revertVo(BaseVo vo, StatusListen listen, ConvertAction prevAction, ref ActionResult undoResult)
        {
            bool succ = false;

            // vo.PathMoved 는 변환 작업 중 vo 의 PathOriginal 에서 where 문자열을 to 문자열로 replace 한 파일명이 되겠다.
            // 정상적으로 변환이 되었으면 상태는 conv_complete 이 되어 있을 것이고, PathMoved에도 바뀐 파일명으로 들어가 있어야 함.
            //if (vo.Status == Status.CONV_COMPLETE && File.Exists(vo.PathMoved))
            if (vo.Status == Status.CONV_COMPLETE && vo.ExistsInMovedPath())
            {
                vo.Status = Status.ON_REVERT;
                listen(vo);
                try
                {
                    //File.Move(vo.PathMoved, vo.PathOriginal);
                    vo.Revert(prevAction.Where, prevAction.To);    // throws Exception if not possible
                    succ = true;
                }
                catch
                {
                    succ = false;
                }
            }
            else
            {
                succ = false;
            }

            if (succ)
            {
                vo.Status = Status.REVERT_COMPLETE;
                listen(vo);
                ++undoResult.CountSuccess;
            }
            else
            {
                vo.Status = Status.NOT_ABLE_TO_REVERT;
                listen(vo);
                ++undoResult.CountFail;
            }
        }

        public /*override*/ bool AbleToUndo()
        {
            // As of 2018.12.01 : 이번 변경으로 prevAction != null 의 의미는 단순히 convert 작업을 한 번 한 적이 있는 지 없는지 여부를 알아 보는 것. 한 번 convert 한적 있고 실행 취소 한 경우 다시 실행 취소 안된다.
            return prevAction != null;
        }

        public /*override*/ bool AbleToConvert()
        {
            return State == AppState.QUERY_LIST_DONE;
        }

    }
}

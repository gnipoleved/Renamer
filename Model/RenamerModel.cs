using Renamer.Presenter;
using System;
using System.Collections.Generic;
using System.IO;

namespace Renamer.Model
{
    public interface IModel
    {
        string State { get; set; }
        bool IncludeFolder { get; set; }
        DirectoryInfo Directory { get; set; }
        List<BaseVo> QueriedFileList { get; set; }
        string Where { get; set; }
        string To { get; set; }
        
        //ConvertResult ConvResult { get; set; }

        void Init();
        void SelectDirectory(string directory);
        void QueryFileList(string where, ListAdder adder);
        ActionResult ConvertFiles(string to, StatusListen listen);
        ActionResult Undo(StatusListen listen);
        bool AbleToConvert();
        bool AbleToUndo();
    }


    public class RenamerModel : IModel
    {
        private Properties props;
        private ConvertAction prevAction;

        public /*override*/ string State { get; set; } 

        private bool _includeFolder;
        public /*override*/ bool IncludeFolder 
        {
            get
            {
                return _includeFolder;
            }
            set
            {
                props.set("last_flagIncludeFolder", string.Format("{0}", value));
                _includeFolder = value;
            }
        }
        public /*override*/ List<BaseVo> QueriedFileList { get; set; }  // 디렉토리도 포함될 수 있음
        public /*override*/ DirectoryInfo Directory { get; set; }
        public /*override*/ string Where { get; set; }
        public /*override*/ string To { get; set; }


        public RenamerModel()
        {
            props = new Properties("renamer.ini");
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

            string readFlagIncludeFolder = props.get("last_flagIncludeFolder");
            if (string.IsNullOrEmpty(readFlagIncludeFolder))
            {
                IncludeFolder = false;
            }
            else
            {
                if (readFlagIncludeFolder == "true") IncludeFolder = true;
                else IncludeFolder = false;
            }
        }

        void IModel.SelectDirectory(string directory)
        {
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
            this.Where = where;

            QueriedFileList = new List<BaseVo>();
            
            Queue<DirectoryInfo> q = new Queue<DirectoryInfo>();
            q.Enqueue(Directory);

            while (q.Count > 0)
            {
                DirectoryInfo poll = q.Dequeue();
                if (poll.Exists)
                {
                    // 폴더 포함 체크가 되어 있는 경우, 해당 디렉토리 먼저 file list 에 담는다. 물론, 경로명에 검색 문자열이 있다면!!
                    if (IncludeFolder)
                    {
                        if (poll.Name.IndexOf(where) >= 0)
                        {
                            QueriedFileList.Add(new DirectoryVo(QueriedFileList.Count + 1, poll, Status.STANDBY));
                            adder(QueriedFileList[QueriedFileList.Count - 1]);
                        }
                    }

                    // 위에서 담긴 디렉토리에 있는 파일들이 그 다음에 리스트에 담긴다.
                    foreach (FileInfo fi in poll.GetFiles())
                    {
                        //if (fi.Name.ToUpper().IndexOf(where.ToUpper()) >= 0)
                        if (fi.Name.IndexOf(where) >= 0)
                        {
                            QueriedFileList.Add(new FileVo(QueriedFileList.Count + 1, fi, Status.STANDBY));
                            adder(QueriedFileList[QueriedFileList.Count - 1]);
                        }
                    }

                    foreach (DirectoryInfo di in poll.GetDirectories())
                    {
                        q.Enqueue(di);
                    }
                }
            }

            State = AppState.QUERY_LIST_DONE;
        }


        public /*override*/ ActionResult ConvertFiles(string to, StatusListen listen)
        {
            this.To = to;
            ActionResult convertResult = new ActionResult();
            convertResult.CountTotal = QueriedFileList.Count;

            ConvertAction convertAction = new ConvertAction(QueriedFileList, IncludeFolder, Where, To);
            prevAction = convertAction;

            // As of 2018.12.01 : QueriedFileList 에 이제 폴더도 포함되어 있을 수 있다. 
            // 따라서 변환 작업은 FileVo 먼저 변환을 한다음 폴더는 QueriedFileList 뒤에서 부터 변환 하여야 한다. 
            // 폴더명 먼저 바꿀 경우 그 안에 파일들은 변환이 안될것임.
            //foreach (FileVo vo in QueriedFileList)
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
            
            if (IncludeFolder)  // 디렉토리를 바꿀거라고 처음에 포함시켰다가 폴더 포함 체크 박스 해제하고 변환하기 하는 경우도 있을 수 있으므로 
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
            return prevAction != null;  // As of 2018.12.01 : 이번 변경으로 prevAction != null 의 의미는 단순히 convert 작업을 한 번 한 적이 있는 지 없는지 여부를 알아 보는 것. 한 번 convert 한적 있고 실행 취소 한 경우 다시 실행 취소 안된다.
        }

        public /*override*/ bool AbleToConvert()
        {
            return State == AppState.QUERY_LIST_DONE;
        }


    }
}

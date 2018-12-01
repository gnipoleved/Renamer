using Renamer.Presenter;
using System;
using System.Collections.Generic;
using System.IO;

namespace Renamer.Model
{
    public interface IModel
    {
        bool IncludeFolder { get; set; }
        DirectoryInfo Directory { get; set; }
        List<FileVo> QueriedFileList { get; set; }
        string Where { get; set; }
        string To { get; set; }
        
        //ConvertResult ConvResult { get; set; }

        void Init();
        void SelectDirectory(string directory);
        void QueryFileList(string where, ListAdder adder);
        ActionResult ConvertFiles(string to, StatusListen listen);
        ActionResult Undo(StatusListen listen);
        bool AbleToUndo();
    }


    public class RenamerModel : IModel
    {
        private Properties props;
        private ConvertAction prevAction;

        public /*override*/ bool IncludeFolder { get; set; }
        public /*override*/ List<FileVo> QueriedFileList { get; set; }
        public /*override*/ DirectoryInfo Directory { get; set; }
        public /*override*/ string Where { get; set; }
        public /*override*/ string To { get; set; }


        public RenamerModel()
        {
            props = new Properties("renamer.ini");
        }

        void IModel.Init()
        {
            string readDir = props.get("last_dir");
            if (string.IsNullOrEmpty(readDir) || System.IO.Directory.Exists(readDir) == false)
            {
                Directory = new DirectoryInfo(@"C:\");
            }
            else
            {
                Directory = new DirectoryInfo(readDir);
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
        }

        public /*override*/ void QueryFileList(string where, ListAdder adder)
        {
            this.Where = where;

            QueriedFileList = new List<FileVo>();

            Queue<DirectoryInfo> q = new Queue<DirectoryInfo>();
            q.Enqueue(Directory);

            while (q.Count > 0)
            {
                DirectoryInfo poll = q.Dequeue();
                if (poll.Exists)
                {
                    foreach (FileInfo fi in poll.GetFiles())
                    {
                        //if (fi.Name.ToUpper().IndexOf(where.ToUpper()) >= 0)
                        if (fi.Name.IndexOf(where) >= 0)
                        {
                            QueriedFileList.Add(new FileVo(QueriedFileList.Count + 1, fi, Status.STANDBY));
                            adder(QueriedFileList[QueriedFileList.Count - 1]);
                        }
                    }

                    foreach(DirectoryInfo di in poll.GetDirectories())
                    {
                        q.Enqueue(di);
                    }
                }
            }
        }


        public /*override*/ ActionResult ConvertFiles(string to, StatusListen listen)
        {
            this.To = to;
            ActionResult convertResult = new ActionResult();
            convertResult.CountTotal = QueriedFileList.Count;

            ConvertAction convertAction = new ConvertAction(QueriedFileList, Where, To);
            prevAction = convertAction;

            foreach (FileVo vo in QueriedFileList)
            {
                bool succ = false;
                if (File.Exists(vo.PathOriginal))
                {
                    vo.SetPathMoved(this.Where, this.To);
                    vo.Status = Status.ON_CONV;
                    listen(vo);
                    try
                    {
                        File.Move(vo.PathOriginal, vo.PathMoved);
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

            return convertResult;

        }

        public /*override*/ ActionResult Undo(StatusListen listen)
        {
            if (AbleToUndo() == false) throw new InvalidOperationException("더이상 실행 취소 할 수 없는데...");
            
            ActionResult undoResult = new ActionResult();
            undoResult.CountTotal = QueriedFileList.Count;

            foreach (FileVo vo in prevAction.FileList)
            {

                bool succ = false;

                // vo.PathMoved 는 변환 작업 중 vo 의 PathOriginal 에서 where 문자열을 to 문자열로 replace 한 파일명이 되겠다.
                // 정상적으로 변환이 되었으면 상태는 conv_complete 이 되어 있을 것이고, PathMoved에도 바뀐 파일명으로 들어가 있어야 함.
                if (vo.Status == Status.CONV_COMPLETE && File.Exists(vo.PathMoved))
                {
                    vo.Status = Status.ON_REVERT;
                    listen(vo);
                    try
                    {
                        File.Move(vo.PathMoved, vo.PathOriginal);
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

            prevAction = null;
            return undoResult;
        }

        public /*override*/ bool AbleToUndo()
        {
            return prevAction != null;
        }


    }
}

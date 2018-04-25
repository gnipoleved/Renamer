using Renamer.Presenter;
using System;
using System.Collections.Generic;
using System.IO;

namespace Renamer.Model
{
    public interface IModel
    {
        DirectoryInfo Directory { get; set; }

        void Init();
        void SelectDirectory(string directory);
        void QueryFileList(string where, ListAdder adder);
    }


    public class RenamerModel : IModel
    {
        private Properties props;

        public /*override*/ DirectoryInfo Directory { get; set; }

        public RenamerModel()
        {
            props = new Properties("renamer.ini");
        }

        void IModel.Init()
        {
            string readDir = props.get("last_dir");
            if (string.IsNullOrEmpty(readDir) || File.Exists(readDir) == false)
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
            if (dir.Exists) this.Directory = dir;
            else throw new InvalidOperationException("Directory [" + directory + "] Not found.");
        }

        public /*override*/ void QueryFileList(string where, ListAdder adder)
        {
            List<FileVo> list = new List<FileVo>();

            Queue<DirectoryInfo> q = new Queue<DirectoryInfo>();
            q.Enqueue(Directory);

            while (q.Count > 0)
            {
                DirectoryInfo poll = q.Dequeue();
                if (poll.Exists)
                {
                    foreach (FileInfo fi in poll.GetFiles())
                    {
                        if (fi.Name.ToUpper().IndexOf(where.ToUpper()) >= 0)
                        {
                            list.Add(new FileVo(list.Count + 1, fi, "대기"));
                            adder(list[list.Count - 1]);
                        }
                    }
                }
            }
        }
    }
}

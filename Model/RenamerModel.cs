
using System;
using System.IO;
namespace Renamer.Model
{
    public interface IModel
    {
        string Directory { get; set; }

        void Init();
        void SelectDirectory(string directory);
    }


    public class RenamerModel : IModel
    {
        private Properties props;

        public /*override*/ string Directory { get; set; }

        public RenamerModel()
        {
            props = new Properties("renamer.ini");
        }

        void IModel.Init()
        {
            string readDir = props.get("last_dir");
            if (string.IsNullOrEmpty(readDir) || File.Exists(readDir) == false)
            {
                Directory = @"C:\";
            }
            else
            {
                Directory = readDir;
            }
        }

        void IModel.SelectDirectory(string directory)
        {
            DirectoryInfo dir = new DirectoryInfo(directory);
            if (dir.Exists) this.Directory = directory;
            else throw new InvalidOperationException("Directory [" + directory + "] Not found.");
        }

    }
}

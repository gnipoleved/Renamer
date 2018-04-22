
using System.IO;
namespace Renamer.Model
{
    public interface IModel
    {
        string SelectedDirectory { get; set; }

        void Init();
    }


    public class RenamerModel : IModel
    {
        private Properties props;

        public /*override*/ string SelectedDirectory { get; set; }

        public RenamerModel()
        {
            props = new Properties("renamer.ini");
        }

        void IModel.Init()
        {
            string readDir = props.get("last_dir");
            if (string.IsNullOrEmpty(readDir) || File.Exists(readDir) == false)
            {
                SelectedDirectory = @"C:\";
            }
            else
            {
                SelectedDirectory = readDir;
            }
        }

    }
}

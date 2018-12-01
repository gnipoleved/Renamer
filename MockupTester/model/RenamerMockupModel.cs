using Renamer.Model;
using System.IO;
using System.Windows.Forms;

namespace Renamer.MockupTester.model
{
    class RenamerMockupModel : IModel 
    {
        private bool _includeFolder;
        public bool IncludeFolder
        {
            get
            {
                return _includeFolder;
            }
            set
            {
                _includeFolder = value;
                MessageBox.Show(string.Format("IncludeFolder set to : {0}", value), "TestConfirm");
            }
        }

        public System.IO.DirectoryInfo Directory
        {
            get;
            set;
        }

        public System.Collections.Generic.List<BaseVo> QueriedFileList
        {
            get;
            set;
        }

        public string Where
        {
            get;
            set;
        }

        public string To
        {
            get;
            set;
        }

        public void Init()
        {
            Directory = new DirectoryInfo(@"C:\temp");
        }

        public void SelectDirectory(string directory)
        {
            
        }

        public void QueryFileList(string where, Presenter.ListAdder adder)
        {
            //adder(new FileVo(0, new FileInfo("mockuptestfile"), "status"));
        }

        public ActionResult ConvertFiles(string to, Presenter.StatusListen listen)
        {
            //listen(new FileVo(0, new FileInfo("mockuptestConvertedFile"), "converted"));
            return new ActionResult();
        }

        public ActionResult Undo(Presenter.StatusListen listen)
        {
            //listen(new FileVo(0, new FileInfo("mockuptestUndoneFile"), "converted"));
            return new ActionResult();
        }

        public bool AbleToUndo()
        {
            return false;
        }

        public string State
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public bool AbleToConvert()
        {
            throw new System.NotImplementedException();
        }
    }
}

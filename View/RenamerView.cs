using Renamer.Model;
using System.Windows.Forms;

namespace Renamer.View
{
    public delegate void ViewEventHandler();
    public delegate void ViewEventHandler<T>(T param);
    public delegate void ViewEventHandler<T, F, P>(T param1, F param2, P param3);


    public interface IView
    {
        string ConfirmMsg { set; }
        string ErrorMsg { set; }

        string Directory { set; }
        bool IncludeFile { set; }
        bool IncludeFolder { set; }
        bool IncludeRootFolder { set; }

        void Build();
        event ViewEventHandler OnBuilt;
        event ViewEventHandler<string> OnDirectorySelected;
        event ViewEventHandler<string> OnQueryFileListRequest;
        event ViewEventHandler<string> OnConvertRequest;
        event ViewEventHandler OnUndoRequest;
        //event ViewEventHandler<bool> OnFlagIncldeFolderChanged;
        event ViewEventHandler<bool, bool, bool> OnIncludeOptionsChanged;

        void ClearListView();
        void AddVo(BaseVo vo);
        void ChangeVoStatus(BaseVo vo);
        void OnSearchListDone(int numFilesSearched);
        void OnConvertDone(ActionResult convertResult);
        void OnUndoFinished(ActionResult undoResult);

        DialogResult AskConvertSure(string modelWhere, string viewTo);
        DialogResult AskEmptyConvertSure();
        DialogResult AskUndoSure(string modelWhere, string modelTo);
    }
}

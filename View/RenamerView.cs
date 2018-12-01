using Renamer.Model;

namespace Renamer.View
{
    public delegate void ViewEventHandler();
    public delegate void ViewEventHandler<T>(T param);


    public interface IView
    {
        string ConfirmMsg { set; }
        string ErrorMsg { set; }

        string Directory { set; }

        void Build();
        event ViewEventHandler OnBuilt;
        event ViewEventHandler<string> OnDirectorySelected;
        event ViewEventHandler<string> OnQueryFileListRequest;
        event ViewEventHandler<string> OnConvertRequest;
        event ViewEventHandler OnUndoRequest;
        event ViewEventHandler<bool> OnFlagIncldeFolderChanged;

        void ClearListView();
        void AddFileVo(FileVo vo);
        void ChangeFileVoStatus(FileVo fileVo);
        void OnSearchListDone(int numFilesSearched);
        void OnConvertDone(ActionResult convertResult);
        void OnUndoFinished(ActionResult undoResult);

        System.Windows.Forms.DialogResult AskConvertSure(string modelWhere, string viewTo);
        System.Windows.Forms.DialogResult AskUndoSure(string modelWhere, string modelTo);
    }
}

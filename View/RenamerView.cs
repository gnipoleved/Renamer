using Renamer.Model;
using System.Windows.Forms;

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

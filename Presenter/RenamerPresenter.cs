using Renamer.Model;
using Renamer.View;
using System;
using System.Windows.Forms;

namespace Renamer.Presenter
{
    public delegate void ListAdder(FileVo fileVo);

    public delegate void StatusListen(FileVo fileVo);


    public interface IPresenter
    {
        void LoadView();
        void OnViewLoaded();
        void OnViewDirectorySelected(string directory);
        void OnViewFileListQueried(string where);
        void OnViewUndoRequest();
        void OnViewFlagIncludeFolderChanged(bool flag);
    }


    public class RenamerPresenter : IPresenter
    {
        private IView view;
        private IModel model;

        public RenamerPresenter(IView view, IModel model)
        {
            this.view = view;
            this.model = model;

            Bind();
        }

        void IPresenter.LoadView()
        {
            view.Build();   // Window Form으로 view를 사용한다면, 대부분의 경우 이 Build 안에 Application.Run 이 있게 되며, Thread 는 이 Build 안에서만 존재하게 될 것임. Form 이 close 가 되어서 Close() 된 이후에야 이 밑 줄이 실행될것임...
        }
        

        private void Bind()
        {
            view.OnBuilt += new ViewEventHandler(OnViewLoaded);
            view.OnDirectorySelected += OnViewDirectorySelected;
            view.OnQueryFileListRequest += OnViewFileListQueried;
            view.OnConvertRequest += new ViewEventHandler<string>(OnViewConvertRequest);
            view.OnUndoRequest += OnViewUndoRequest;
            view.OnFlagIncldeFolderChanged += OnViewFlagIncludeFolderChanged;
        }


        public /*override*/ void OnViewLoaded()
        {
            model.Init();
            view.Directory = model.Directory.FullName;
        }

        public /*override*/ void OnViewDirectorySelected(string directory)
        {
            try
            {
                //view.ClearListView();
                model.SelectDirectory(directory);
            }
            catch (Exception ex)
            {
                view.ErrorMsg = ex.ToString();
            }
            finally
            {
                // Exception이 발생한 경우에도 기존 dir 유지
                view.Directory = model.Directory.FullName;
            }
        }

        public /*override*/ void OnViewFileListQueried(string where)
        {
            try
            {
                //view.ClearListView();
                model.QueryFileList(where, new ListAdder(AddFileVoToView));
                view.OnSearchListDone(model.QueriedFileList.Count);
            }
            catch (Exception ex)
            {
                view.ErrorMsg = ex.ToString();
            }
        }

        public /*override*/ void OnViewConvertRequest(string to)
        {
            try
            {
                DialogResult dRes = view.AskConvertSure(model.Where, to);
                if (dRes == DialogResult.Yes)
                {
                    ActionResult convertResult = model.ConvertFiles(to, new StatusListen(ChangeFileVoStatusInView));
                    view.OnConvertDone(convertResult);
                    //view.ConfirmMsg = "변환 작업이 완료되었습니다.";
                }
            }
            catch (Exception ex)
            {
                view.ErrorMsg = ex.ToString();
            }
        }

        public /*override*/ void OnViewUndoRequest()
        {
            try
            {
                if (model.AbleToUndo())
                {
                    DialogResult dRes = view.AskUndoSure(model.Where, model.To);
                    if (dRes == DialogResult.Yes)
                    {
                        ActionResult undoResult = model.Undo(ChangeFileVoStatusInView);
                        view.OnUndoFinished(undoResult);
                    }
                }
                else
                {
                    view.ErrorMsg = "더 이상 실행 취소를 할 수 없습니다.";
                }
            }
            catch (Exception ex)
            {
                view.ErrorMsg = ex.ToString();
            }
        }

        public /*override*/ void OnViewFlagIncludeFolderChanged(bool flag)
        {
            try
            {
                model.IncludeFolder = flag;
            } 
            catch (Exception ex)
            {
                view.ErrorMsg = ex.ToString();
            }
        }

        
        private void AddFileVoToView(FileVo vo)
        {
            view.AddFileVo(vo);
        }

        private void ChangeFileVoStatusInView(FileVo fileVo)
        {
            view.ChangeFileVoStatus(fileVo);
        }


    }
}

using Renamer.Model;
using Renamer.View;
using System;
using System.Windows.Forms;

namespace Renamer.Presenter
{
    public delegate void ListAdder(BaseVo vo);

    public delegate void StatusListen(BaseVo vo);


    public interface IPresenter
    {
        void LoadView();
        void OnViewLoaded();
        void OnViewDirectorySelected(string directory);
        void OnViewFileListQueried(string where);
        void OnViewUndoRequest();
        void onViewIncludeOptionsChanged(bool includeFile, bool includeFolder, bool includeRootFolder);
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
            //view.OnFlagIncldeFolderChanged += OnViewFlagIncludeFolderChanged;
            view.OnIncludeOptionsChanged += onViewIncludeOptionsChanged;
        }


        public /*override*/ void OnViewLoaded()
        {
            model.Init();
            view.IncludeFile = model.IncludeFile;
            view.IncludeFolder = model.IncludeFolder;
            view.IncludeRootFolder = model.IncludeRootFolder;
            view.Directory = model.Directory.FullName;
        }

        public /*override*/ void OnViewDirectorySelected(string directory)
        {
            try
            {
                view.ClearListView();
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
                model.QueryFileList(where, new ListAdder(AddVoToView));
                view.OnSearchListDone(model.QueriedFileList.Count);
            }
            catch (Exception ex)
            {
                view.ClearListView();
                view.ErrorMsg = ex.ToString();
            }
        }

        public /*override*/ void OnViewConvertRequest(string to)
        {
            try
            {
                if (model.AbleToConvert())
                {
                    DialogResult dRes = view.AskConvertSure(model.Where, to);
                    if (dRes == DialogResult.Yes)
                    {
                        if (!string.IsNullOrEmpty(to) || view.AskEmptyConvertSure() == DialogResult.Yes)
                        {
                            ActionResult convertResult = model.ConvertFiles(to, new StatusListen(ChangeVoStatusInView));
                            view.OnConvertDone(convertResult);
                            //view.ConfirmMsg = "변환 작업이 완료되었습니다.";
                        }
                    }
                }
                else
                {
                    view.ErrorMsg = "Convert 할 수 없는 상태입니다. 파일 찾기 부터 다시 해 주세요.";
                }
            }
            catch (DifferntOptionStateException dx)
            {
                // ui에서 옵션 바꾸면 refresh 되기 때문에 이쪽으로 들어올일은 없겠지만...
                view.ErrorMsg = "파일 찾기 할 때와 Include Option 이 다릅니다. 재설정 후 파일 찾기 부터 다시 해 주세요.";
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
                        ActionResult undoResult = model.Undo(ChangeVoStatusInView);
                        view.OnUndoFinished(undoResult);
                    }
                }
                else
                {
                    view.ErrorMsg = "이상 실행 취소를 할 수 없는 상태입니다.\r\n파일 찾기 부터 다시 해 주세요.";
                }
            }
            catch (Exception ex)
            {
                view.ErrorMsg = ex.ToString();
            }
        }


        private void setIncludeOptions(bool includeFile, bool includeFolder, bool includeRootFolder)
        {
            model.IncludeFile = includeFile;
            view.IncludeFile = includeFile;
            model.IncludeFolder = includeFolder;
            view.IncludeFolder = includeFolder;
            model.IncludeRootFolder = includeRootFolder;
            view.IncludeRootFolder = includeRootFolder;

            //string f1 = model.IncludeFile ? "O" : "X";
            //string f2 = model.IncludeFolder ? "O" : "X";
            //string f3 = model.IncludeRootFolder ? "O" : "X";
            //MessageBox.Show(f1 + f2 + f3);
        }

        public /*override*/ void onViewIncludeOptionsChanged(bool includeFile, bool includeFolder, bool includeRootFolder)
        {
            try
            {
                #region 이건 좀 복잡함
                //if (!includeFile && !includeFolder) // one of them should be true
                //{
                //    if (includeRootFolder)
                //    {
                //        #region 모든 case들
                //        //if (!model.IncludeRootFolder)   // View 에서 includeRootFolder 가 false 에서 true 로 바꿨다는 말
                //        //    setIncludeOptions(false, true, true);   // 물론 UI에서 동시에 3개의 option 값을 바꾸진 않았겠지.. 그렇지만 그렇게 되는 UI 가 있다면 우선순위는 root folder option 에 둔다.
                //        //else if (model.IncludeFile && model.IncludeFolder) // 둘이 동시에 false 로 바뀌었을 때
                //        //    setIncludeOptions(false, true, true);
                //        //else if (model.IncludeFile) // file option만 false 로 바뀐 경우다
                //        //    setIncludeOptions(false, true, true);
                //        //else if (model.IncludeFolder) // folder option 만 false 로 바뀐 경우
                //        //    setIncludeOptions(true, false, false);
                //        //else  // 이것도 있어야 겠지
                //        //    setIncludeOptions(false, true, true);
                //        // 위 case 들을 간략히 하면 아래와 같다.
                //        #endregion
                //        if (!model.IncludeFile && model.IncludeFolder) // folder option 만 false 로 바뀐 경우
                //            setIncludeOptions(true, false, false);
                //        else 
                //            setIncludeOptions(false, true, true);
                //    }
                //    else
                //    {
                //        #region 모든 case들
                //        //if (model.IncludeFile && model.IncludeFolder) // 둘이 동시에 false 로 바뀌었을 때
                //        //    setIncludeOptions(true, false, false);
                //        //else if (model.IncludeFile) // file option만 false 로 바뀐 경우다
                //        //    setIncludeOptions(false, true, false);
                //        //else if (model.IncludeFolder) // folder option 만 false 로 바뀐 경우
                //        //    setIncludeOptions(true, false, false);
                //        //else if (model.IncludeRootFolder) // 이까지 왔다면 file 과 folder 는 원래 false 였다. root 만 false 로 바뀐 경우. false, false, true 인 상태가 존재 하면 안됨. 그치만 만에 하나 있었을 경우
                //        //    setIncludeOptions(true, false, false);
                //        //위 case 들을 간략히 하면 아래와 같다.
                //        #endregion
                //        if (model.IncludeFile && !model.IncludeFolder)
                //            setIncludeOptions(false, true, false);
                //        else
                //            setIncludeOptions(true, false, false);
                //    }
                //}
                //else if (includeFile && includeFolder) 
                //{
                //    setIncludeOptions(includeFile, includeFolder, includeRootFolder);   // true, true, root
                //}
                //else if (!includeFile && includeFolder) 
                //{
                //    setIncludeOptions(includeFile, includeFolder, includeRootFolder);   // false, true, root
                //}
                //else // includeFile && !includeFolder
                //{
                //    setIncludeOptions(includeFile, includeFolder, false);
                //}
                #endregion

                if (!model.IncludeFile && includeFile) // false, x, x > true, x, x
                    setIncludeOptions(true, includeFolder, includeFolder ? includeRootFolder : false);
                else if (model.IncludeFile && !includeFile) // true, x, x > false, x, x
                    if (model.IncludeFolder && !includeFolder) // true, true, x > false, false, x
                        setIncludeOptions(true, false, false);
                    else if (model.IncludeFolder && includeFolder) // true, true, x > false, true, x
                        setIncludeOptions(false, true, includeRootFolder);
                    else if (!model.IncludeFolder && !includeFolder) // true, false, x > false, false, x
                        setIncludeOptions(false, true, includeRootFolder);
                    else // !model.IncludeFolder && includeFolder // true, false, x > false, true, x
                        setIncludeOptions(false, true, includeRootFolder);
                // 이까지 왔다는 말은 IncludeFile 옵션은 변하지 않는다는 말. 즉, true > true 또는 false > false 로 그대로 유지
                else if (!model.IncludeFolder && includeFolder) // file, false, x > file, true, x
                    setIncludeOptions(includeFile, true, includeRootFolder);   // file 이 true 였던지 false 였던지 상관 없이 , file, true, root 로 세팅됨
                else if (model.IncludeFolder && !includeFolder) // file, true, x > file, false, x
                    if (includeFile) // true, true, x > true, false, x
                        setIncludeOptions(true, false, false);
                    else // false, true, x > false, false, x
                        setIncludeOptions(true, false, false);
                // 이까지 왔다는 말은 IncludeFolder 옵션 또한 변하지 않는다는 말. 즉, true > true 또는 false > false 로 그대로 유지
                else if (model.IncludeRootFolder && !includeRootFolder) // file, folder, true > file, folder, false
                    if (includeFile) // true, folder, true > true, folder, false
                        setIncludeOptions(includeFile, includeFolder, includeRootFolder);
                    else if (includeFolder) // false, true, true > false, true, false
                        setIncludeOptions(includeFile, includeFolder, includeRootFolder);
                    else // false, false, true > false, false > false
                        setIncludeOptions(true, false, false);
                else if (!model.IncludeRootFolder && includeRootFolder) // file, folder, false > file, folder, true
                    setIncludeOptions(includeFile, true, true);  // ttf > ttt, // tff > tft, // ftf > ftt, // fff > fft : 이 4개의 모든 case 를 다 만족한다.
                // 이까지 왔다는 말은 file, folder, root 모두 변하지 않았다는 말. 
                else // 이런 경우는 view 에서 거의 발생시키지 않겠지만.. 혹시 발생시킬수 있는 구조의 view 가 있다면
                {
                    setIncludeOptions(includeFile, includeFolder, includeRootFolder);
                }

                model.ClearList(); ;
                view.ClearListView();
            }
            catch (Exception ex)
            {
                view.ErrorMsg = ex.ToString();
            }
        }

        
        private void AddVoToView(BaseVo vo)
        {
            view.AddVo(vo);
        }

        private void ChangeVoStatusInView(BaseVo vo)
        {
            view.ChangeVoStatus(vo);
        }


    }
}

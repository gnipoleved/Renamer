﻿using Renamer.Model;
using Renamer.View;
using System;

namespace Renamer.Presenter
{
    public interface IPresenter
    {
        void LoadView();
        void OnViewLoaded();
        void OnViewDirectorySelected(string directory);
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
        }


        public /*override*/ void OnViewLoaded()
        {
            model.Init();
            view.Directory = model.Directory;
        }

        public /*override*/ void OnViewDirectorySelected(string directory)
        {
            try
            {
                model.SelectDirectory(directory);
            }
            catch (Exception ex)
            {
                view.ErrorMsg = ex.ToString();
            }
            finally
            {
                // Exception이 발생한 경우에도 기존 dir 유지
                view.Directory = model.Directory;
            }
        }

        
    }
}

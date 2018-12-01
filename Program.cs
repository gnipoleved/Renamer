using Renamer.Model;
using Renamer.Presenter;
using Renamer.View;
using System;

namespace Renamer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IView view = new RenamerForm();
            IModel model = new RenamerModel();
            //IModel model = new Renamer.MockupTester.model.RenamerMockupModel();
            IPresenter presenter = new RenamerPresenter(view, model);
            presenter.LoadView();

        }
    }
}

using Renamer.Model;
using Renamer.Presenter;
using Renamer.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            IView view = new RenamerForm();
            IModel model = new RenamerModel();
            IPresenter presenter = new RenamerPresenter(view, model);
            presenter.LoadView();

        }
    }
}

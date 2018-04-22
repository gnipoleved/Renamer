using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Renamer.View
{
    public partial class RenamerForm : Form, IView
    {
        static RenamerForm()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }


        public RenamerForm()
        {
            InitializeComponent();
        }

        void IView.Build()
        {
            Application.Run(this as Form);
        }

        public /*override*/ event ViewEventHandler OnBuilt;
    }
}

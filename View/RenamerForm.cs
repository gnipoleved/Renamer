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

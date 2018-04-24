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


        string IView.ErrorMsg
        {
            set
            {
                MessageBox.Show(value, "Error");
            }
        }

        string IView.Directory
        {
            set 
            {
                tbx_directory.Text = value;
            }
        }


        void IView.Build()
        {
            Application.Run(this as Form);
        }

        public /*override*/ event ViewEventHandler OnBuilt;

        private void RenamerForm_Load(object sender, System.EventArgs e)
        {
            if (OnBuilt != null) OnBuilt();
        }

        public /*override*/ event ViewEventHandler<string> OnDirectorySelected;

        private void btn_choose_Click(object sender, System.EventArgs e)
        {
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult == DialogResult.OK && string.IsNullOrEmpty(folderBrowserDialog.SelectedPath) == false)
            {
                if (OnDirectorySelected != null) OnDirectorySelected(folderBrowserDialog.SelectedPath);
            }
        }

        private void tbx_directory_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (OnDirectorySelected != null) OnDirectorySelected(tbx_directory.Text.Trim());
            }
        }

        

        

    }
}

using Renamer.Model;
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

        
        public /*override*/ string ErrorMsg
        {
            set
            {
                MessageBox.Show(value, "Error");
            }
        }

        public /*override*/ string Directory
        {
            set 
            {
                tbx_directory.Text = value;
                lv_file_list.Items.Clear();
            }
        }


        void IView.Build()
        {
            Application.Run(this as Form);
        }

        public /*override*/ event ViewEventHandler OnBuilt;

        private void RenamerForm_Load(object sender, System.EventArgs e)
        {
            InitializeFileListView();
            if (OnBuilt != null) OnBuilt();
        }

        public /*override*/ event ViewEventHandler<string> OnDirectorySelected;

        public /*override*/ event ViewEventHandler<string> OnQueryFileListRequest;

        public /*override*/ void AddFileVo(FileVo vo)
        {
            ListViewItem lvi = new ListViewItem(vo.Index.ToString());
            lvi.SubItems.Add(vo.PathOriginal);
            lvi.SubItems.Add(vo.Status);
            lv_file_list.Items.Add(lvi);
        }


        private void InitializeFileListView()
        {
            lv_file_list.View = System.Windows.Forms.View.Details;
            lv_file_list.Columns.Add("No", 30, HorizontalAlignment.Right);
            lv_file_list.Columns.Add("파일명", 470, HorizontalAlignment.Left);
            lv_file_list.Columns.Add("상태", 100, HorizontalAlignment.Center);
        }

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

        private void btn_search_Click(object sender, System.EventArgs e)
        {
            string where = tbx_where.Text.Trim();
            if (string.IsNullOrEmpty(where))
            {
                ErrorMsg = "검색 조건을 입력하시용";
            }
            else
            {
                if (OnQueryFileListRequest != null) OnQueryFileListRequest(where);
            }
        }

        

        

    }
}

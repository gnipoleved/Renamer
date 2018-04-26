using Renamer.Model;
using System;
using System.Drawing;
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
            this.Text = string.Format("{0} Ver-{1}, 파일명 변경 프로그램, by {2}", Application.ProductName, Application.ProductVersion, Application.CompanyName);
            this.Icon = Properties.Resources.icon;
        }

        public /*override*/ string ConfirmMsg
        {
            set
            {
                MessageBox.Show(value, "Confirm");
            }
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
                //lv_file_list.Items.Clear();
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

        public /*override*/ event ViewEventHandler<string> OnConvertRequest;

        public /*override*/ event ViewEventHandler OnUndoRequest;


        public /*override*/ void ClearListView()
        {
            lv_file_list.Items.Clear();
        }

        public /*override*/ void AddFileVo(FileVo vo)
        {
            ListViewItem lvi = new ListViewItem(vo.Index.ToString());
            lvi.SubItems.Add(vo.PathOriginal);
            lvi.SubItems.Add(vo.Status);
            lv_file_list.Items.Add(lvi);
        }

        public /*override*/ void ChangeFileVoStatus(FileVo fileVo)
        {
            //lv_file_list.Items[0].SubItems[2].Text = "kkk";
            lv_file_list.Items[fileVo.Index - 1].SubItems[2].Text = fileVo.Status;
        }


        public /*override*/ System.Windows.Forms.DialogResult AskConvertSure(string modelWhere, string viewTo)
        {
            return MessageBox.Show(string.Format("문자열 [{0}] (을)를 [{1}] (으)로 변환할 건가요?\r\n정말? 실행취소는 한번밖에 안되요.", modelWhere, viewTo), "Confirm", MessageBoxButtons.YesNo);
        }

        public /*override*/ System.Windows.Forms.DialogResult AskUndoSure(string modelWhere, string modelTo)
        {
            return MessageBox.Show(string.Format("정말로 실행취소를 원하십니까?\r\n문자열 [{0}] (이)가 [{1}] (으)로 변환됩니다.", modelTo, modelWhere), "Confirm", MessageBoxButtons.YesNo);
        }

        private void InitializeFileListView()
        {
            lv_file_list.View = System.Windows.Forms.View.Details;
            lv_file_list.Columns.Add("No", 50, HorizontalAlignment.Right);
            lv_file_list.Columns.Add("파일명", 450, HorizontalAlignment.Left);
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
            RequestSearching();
        }

        private void btn_convert_Click(object sender, System.EventArgs e)
        {
            RequestConvert();
        }

        private void btn_undo_Click(object sender, System.EventArgs e)
        {
            RequestUndo();
        }

        private void tbx_where_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && tbx_where.Text.Trim().Length > 0)
            {
                RequestSearching();
            }
        }

        private void tbx_to_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && tbx_to.Text.Trim().Length > 0)
            {
                RequestConvert();
            }
        }

        private void RequestSearching()
        {
            string where = tbx_where.Text;
            if (string.IsNullOrEmpty(where))
            {
                ErrorMsg = "검색 조건을 입력하시용";
            }
            else
            {
                if (OnQueryFileListRequest != null) OnQueryFileListRequest(where);
            }
        }

        private void RequestConvert()
        {
            string to = tbx_to.Text;
            if (string.IsNullOrEmpty(to)) to = string.Empty;
            if (OnConvertRequest != null) OnConvertRequest(to);
        }

        private void RequestUndo()
        {
            if (OnUndoRequest != null) OnUndoRequest();
        }


    }
}

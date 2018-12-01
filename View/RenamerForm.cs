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

        public /*override*/ event ViewEventHandler<bool> OnFlagIncldeFolderChanged;


        public /*override*/ void ClearListView()
        {
            lv_file_list.Items.Clear();
            lbl_num_files.Text = "0";
        }

        public /*override*/ void AddVo(BaseVo vo)
        {
            ListViewItem lvi = new ListViewItem(vo.Index.ToString());
            lvi.SubItems.Add(vo.PathOriginal);
            lvi.SubItems.Add(vo.Status);
            lv_file_list.Items.Add(lvi);
        }

        public /*override*/ void ChangeVoStatus(BaseVo vo)
        {
            //lv_file_list.Items[0].SubItems[2].Text = "kkk";
            lv_file_list.Items[vo.Index - 1].SubItems[2].Text = vo.Status;
        }


        public /*override*/ DialogResult AskConvertSure(string modelWhere, string viewTo)
        {
            return MessageBox.Show(string.Format("문자열 [{0}] (을)를 [{1}] (으)로 변환할 건가요?", modelWhere, viewTo), "Confirm", MessageBoxButtons.YesNo);
        }

        public /*override*/ DialogResult AskEmptyConvertSure()
        {
            return MessageBox.Show("변경 문자열을 입력하지 않아 공백으로 변환됩니다. 정말인가요?", "Confirm", MessageBoxButtons.YesNo);
        }

        public /*override*/ DialogResult AskUndoSure(string modelWhere, string modelTo)
        {
            return MessageBox.Show(string.Format("정말로 실행취소를 원하십니까?\r\n문자열 [{0}] (이)가 [{1}] (으)로 변환됩니다.", modelTo, modelWhere), "Confirm", MessageBoxButtons.YesNo);
        }

        public /*override*/ void OnSearchListDone(int numFilesSearched)
        {
            lbl_num_files.Text = numFilesSearched.ToString();
        }

        public /*override*/ void OnConvertDone(ActionResult convertResult)
        {
            this.ConfirmMsg = string.Format("변환 결과 : ({0} / {1})", convertResult.CountSuccess, convertResult.CountTotal);
        }

        public /*override*/ void OnUndoFinished(ActionResult undoResult)
        {
            this.ConfirmMsg = string.Format("원복 결과 : ({0} / {1})", undoResult.CountSuccess, undoResult.CountTotal);
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
                SelectDirectory(folderBrowserDialog.SelectedPath);
            }
        }        

        private void tbx_directory_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SelectDirectory(tbx_directory.Text.Trim());
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


        private void SelectDirectory(string dirPath)
        {
            if (OnDirectorySelected != null)
            {
                ClearListView();
                OnDirectorySelected(dirPath);
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
                ClearListView();
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

        private void cbx_includeFolder_CheckedChanged(object sender, EventArgs e)
        {
            if (OnFlagIncldeFolderChanged != null) OnFlagIncldeFolderChanged(cbx_includeFolder.Checked);
        }


    }
}

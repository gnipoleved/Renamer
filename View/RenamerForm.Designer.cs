namespace Renamer.View
{
    partial class RenamerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbx_directory = new System.Windows.Forms.TextBox();
            this.btn_choose = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.tbx_where = new System.Windows.Forms.TextBox();
            this.tbx_to = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lv_file_list = new System.Windows.Forms.ListView();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_num_files = new System.Windows.Forms.Label();
            this.btn_search = new System.Windows.Forms.Button();
            this.btn_convert = new System.Windows.Forms.Button();
            this.btn_undo = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cbx_includeFolder = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbx_directory
            // 
            this.tbx_directory.Location = new System.Drawing.Point(123, 56);
            this.tbx_directory.Name = "tbx_directory";
            this.tbx_directory.Size = new System.Drawing.Size(482, 21);
            this.tbx_directory.TabIndex = 1;
            this.tbx_directory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbx_directory_KeyPress);
            // 
            // btn_choose
            // 
            this.btn_choose.Location = new System.Drawing.Point(13, 55);
            this.btn_choose.Name = "btn_choose";
            this.btn_choose.Size = new System.Drawing.Size(105, 23);
            this.btn_choose.TabIndex = 0;
            this.btn_choose.Text = "Directory";
            this.btn_choose.UseVisualStyleBackColor = true;
            this.btn_choose.Click += new System.EventHandler(this.btn_choose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(12, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "검색 문자열";
            // 
            // tbx_where
            // 
            this.tbx_where.Location = new System.Drawing.Point(93, 94);
            this.tbx_where.Name = "tbx_where";
            this.tbx_where.Size = new System.Drawing.Size(332, 21);
            this.tbx_where.TabIndex = 2;
            this.tbx_where.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbx_where_KeyPress);
            // 
            // tbx_to
            // 
            this.tbx_to.Location = new System.Drawing.Point(93, 462);
            this.tbx_to.Name = "tbx_to";
            this.tbx_to.Size = new System.Drawing.Size(332, 21);
            this.tbx_to.TabIndex = 4;
            this.tbx_to.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbx_to_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(12, 468);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "변경 문자열";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(12, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "검색된 파일 리스트";
            // 
            // lv_file_list
            // 
            this.lv_file_list.Location = new System.Drawing.Point(14, 152);
            this.lv_file_list.Name = "lv_file_list";
            this.lv_file_list.Size = new System.Drawing.Size(604, 304);
            this.lv_file_list.TabIndex = 0;
            this.lv_file_list.UseCompatibleStateImageBehavior = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(144, 133);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "파일개수 :";
            // 
            // lbl_num_files
            // 
            this.lbl_num_files.AutoSize = true;
            this.lbl_num_files.Location = new System.Drawing.Point(209, 133);
            this.lbl_num_files.Name = "lbl_num_files";
            this.lbl_num_files.Size = new System.Drawing.Size(47, 12);
            this.lbl_num_files.TabIndex = 0;
            this.lbl_num_files.Text = "1000000";
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(431, 93);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(88, 23);
            this.btn_search.TabIndex = 3;
            this.btn_search.Text = "파일 찾기";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // btn_convert
            // 
            this.btn_convert.Location = new System.Drawing.Point(431, 462);
            this.btn_convert.Name = "btn_convert";
            this.btn_convert.Size = new System.Drawing.Size(88, 23);
            this.btn_convert.TabIndex = 5;
            this.btn_convert.Text = "변환 하기";
            this.btn_convert.UseVisualStyleBackColor = true;
            this.btn_convert.Click += new System.EventHandler(this.btn_convert_Click);
            // 
            // btn_undo
            // 
            this.btn_undo.Location = new System.Drawing.Point(522, 462);
            this.btn_undo.Name = "btn_undo";
            this.btn_undo.Size = new System.Drawing.Size(88, 23);
            this.btn_undo.TabIndex = 6;
            this.btn_undo.Text = "실행 취소";
            this.btn_undo.UseVisualStyleBackColor = true;
            this.btn_undo.Click += new System.EventHandler(this.btn_undo_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(260, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "개";
            // 
            // cbx_includeFolder
            // 
            this.cbx_includeFolder.AutoSize = true;
            this.cbx_includeFolder.Location = new System.Drawing.Point(6, 8);
            this.cbx_includeFolder.Name = "cbx_includeFolder";
            this.cbx_includeFolder.Size = new System.Drawing.Size(76, 16);
            this.cbx_includeFolder.TabIndex = 8;
            this.cbx_includeFolder.Text = "폴더 포함";
            this.cbx_includeFolder.UseVisualStyleBackColor = true;
            this.cbx_includeFolder.CheckedChanged += new System.EventHandler(this.cbx_includeFolder_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cbx_includeFolder);
            this.panel1.Location = new System.Drawing.Point(14, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(87, 31);
            this.panel1.TabIndex = 9;
            // 
            // RenamerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 498);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_undo);
            this.Controls.Add(this.btn_convert);
            this.Controls.Add(this.btn_search);
            this.Controls.Add(this.lbl_num_files);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lv_file_list);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbx_to);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbx_where);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_choose);
            this.Controls.Add(this.tbx_directory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "RenamerForm";
            this.Text = "Renamer, 파일이름 변환기, by mirsolution";
            this.Load += new System.EventHandler(this.RenamerForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbx_directory;
        private System.Windows.Forms.Button btn_choose;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbx_where;
        private System.Windows.Forms.TextBox tbx_to;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView lv_file_list;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbl_num_files;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.Button btn_convert;
        private System.Windows.Forms.Button btn_undo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbx_includeFolder;
        private System.Windows.Forms.Panel panel1;
    }
}
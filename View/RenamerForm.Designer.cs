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
            this.SuspendLayout();
            // 
            // tbx_directory
            // 
            this.tbx_directory.Location = new System.Drawing.Point(123, 23);
            this.tbx_directory.Name = "tbx_directory";
            this.tbx_directory.Size = new System.Drawing.Size(482, 21);
            this.tbx_directory.TabIndex = 0;
            this.tbx_directory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbx_directory_KeyPress);
            // 
            // btn_choose
            // 
            this.btn_choose.Location = new System.Drawing.Point(13, 22);
            this.btn_choose.Name = "btn_choose";
            this.btn_choose.Size = new System.Drawing.Size(105, 23);
            this.btn_choose.TabIndex = 1;
            this.btn_choose.Text = "Directory";
            this.btn_choose.UseVisualStyleBackColor = true;
            this.btn_choose.Click += new System.EventHandler(this.btn_choose_Click);
            // 
            // RenamerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 261);
            this.Controls.Add(this.btn_choose);
            this.Controls.Add(this.tbx_directory);
            this.Name = "RenamerForm";
            this.Text = "RenamerForm";
            this.Load += new System.EventHandler(this.RenamerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbx_directory;
        private System.Windows.Forms.Button btn_choose;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}
namespace ServiceFairy.WinForm
{
    partial class UploadPackageDialog
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
            this._ctlAddFiles = new System.Windows.Forms.Button();
            this._ctlAddDirectories = new System.Windows.Forms.Button();
            this._fileList = new System.Windows.Forms.ListBox();
            this._ctlRemove = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this._ctlTitle = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _ctlAddFiles
            // 
            this._ctlAddFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._ctlAddFiles.Location = new System.Drawing.Point(12, 270);
            this._ctlAddFiles.Name = "_ctlAddFiles";
            this._ctlAddFiles.Size = new System.Drawing.Size(98, 23);
            this._ctlAddFiles.TabIndex = 2;
            this._ctlAddFiles.Text = "添加文件 ...";
            this._ctlAddFiles.UseVisualStyleBackColor = true;
            this._ctlAddFiles.Click += new System.EventHandler(this._ctlAddFiles_Click);
            // 
            // _ctlAddDirectories
            // 
            this._ctlAddDirectories.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._ctlAddDirectories.Location = new System.Drawing.Point(116, 270);
            this._ctlAddDirectories.Name = "_ctlAddDirectories";
            this._ctlAddDirectories.Size = new System.Drawing.Size(98, 23);
            this._ctlAddDirectories.TabIndex = 4;
            this._ctlAddDirectories.Text = "添加目录 ...";
            this._ctlAddDirectories.UseVisualStyleBackColor = true;
            this._ctlAddDirectories.Click += new System.EventHandler(this._ctlAddDirectories_Click);
            // 
            // _fileList
            // 
            this._fileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._fileList.FormattingEnabled = true;
            this._fileList.IntegralHeight = false;
            this._fileList.ItemHeight = 12;
            this._fileList.Location = new System.Drawing.Point(59, 42);
            this._fileList.Name = "_fileList";
            this._fileList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this._fileList.Size = new System.Drawing.Size(523, 222);
            this._fileList.TabIndex = 1;
            // 
            // _ctlRemove
            // 
            this._ctlRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._ctlRemove.Location = new System.Drawing.Point(220, 270);
            this._ctlRemove.Name = "_ctlRemove";
            this._ctlRemove.Size = new System.Drawing.Size(98, 23);
            this._ctlRemove.TabIndex = 5;
            this._ctlRemove.Text = "删除";
            this._ctlRemove.UseVisualStyleBackColor = true;
            this._ctlRemove.Click += new System.EventHandler(this._ctlRemove_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2147483549;
            this.label1.Text = "名称：";
            // 
            // _ctlTitle
            // 
            this._ctlTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this._ctlTitle.Border.Class = "TextBoxBorder";
            this._ctlTitle.Location = new System.Drawing.Point(59, 12);
            this._ctlTitle.Name = "_ctlTitle";
            this._ctlTitle.Size = new System.Drawing.Size(523, 21);
            this._ctlTitle.TabIndex = 0;
            this._ctlTitle.WatermarkText = "可以留空，将自动命名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2147483549;
            this.label2.Text = "文件：";
            // 
            // UploadPackageDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 305);
            this.Controls.Add(this._ctlTitle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._fileList);
            this.Controls.Add(this._ctlRemove);
            this.Controls.Add(this._ctlAddDirectories);
            this.Controls.Add(this._ctlAddFiles);
            this.Name = "UploadPackageDialog";
            this.Text = "上传安装包";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _ctlAddFiles;
        private System.Windows.Forms.Button _ctlAddDirectories;
        private System.Windows.Forms.ListBox _fileList;
        private System.Windows.Forms.Button _ctlRemove;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.Controls.TextBoxX _ctlTitle;
        private System.Windows.Forms.Label label2;
    }
}
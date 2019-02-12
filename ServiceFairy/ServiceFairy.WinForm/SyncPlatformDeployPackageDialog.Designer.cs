namespace ServiceFairy.WinForm
{
    partial class SyncPlatformDeployPackageDialog
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
            this.components = new System.ComponentModel.Container();
            this._txtDir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.selectPathButton1 = new Common.WinForm.SelectPathButton();
            this.label2 = new System.Windows.Forms.Label();
            this._ctlList = new System.Windows.Forms.ListBox();
            this._ctlSelectAll = new Common.WinForm.SelectAllCheckBox(this.components);
            this.SuspendLayout();
            // 
            // _txtDir
            // 
            this._txtDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._txtDir.Location = new System.Drawing.Point(63, 18);
            this._txtDir.Name = "_txtDir";
            this._txtDir.Size = new System.Drawing.Size(463, 21);
            this._txtDir.TabIndex = 2147483549;
            this._txtDir.TextChanged += new System.EventHandler(this._txtDir_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2147483550;
            this.label1.Text = "路径：";
            // 
            // selectPathButton1
            // 
            this.selectPathButton1.ActionType = Common.WinForm.SelectPathActionType.Folder;
            this.selectPathButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectPathButton1.DefaultExt = null;
            this.selectPathButton1.Filter = null;
            this.selectPathButton1.FilterIndex = 0;
            this.selectPathButton1.Location = new System.Drawing.Point(532, 16);
            this.selectPathButton1.Name = "selectPathButton1";
            this.selectPathButton1.ReadOnlyChecked = false;
            this.selectPathButton1.RelationControl = this._txtDir;
            this.selectPathButton1.ShowNewButton = false;
            this.selectPathButton1.Size = new System.Drawing.Size(38, 23);
            this.selectPathButton1.TabIndex = 2147483551;
            this.selectPathButton1.Text = "...";
            this.selectPathButton1.Title = null;
            this.selectPathButton1.UseVisualStyleBackColor = true;
            this.selectPathButton1.Value = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2147483550;
            this.label2.Text = "内容：";
            // 
            // _ctlList
            // 
            this._ctlList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._ctlList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this._ctlList.FormattingEnabled = true;
            this._ctlList.IntegralHeight = false;
            this._ctlList.ItemHeight = 20;
            this._ctlList.Location = new System.Drawing.Point(63, 45);
            this._ctlList.Name = "_ctlList";
            this._ctlList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this._ctlList.Size = new System.Drawing.Size(507, 290);
            this._ctlList.TabIndex = 2147483555;
            this._ctlList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this._ctlList_DrawItem);
            // 
            // _ctlSelectAll
            // 
            this._ctlSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._ctlSelectAll.AutoSize = true;
            this._ctlSelectAll.Checked = true;
            this._ctlSelectAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this._ctlSelectAll.ListBox = this._ctlList;
            this._ctlSelectAll.Location = new System.Drawing.Point(14, 353);
            this._ctlSelectAll.Name = "_ctlSelectAll";
            this._ctlSelectAll.Size = new System.Drawing.Size(48, 16);
            this._ctlSelectAll.TabIndex = 2147483556;
            this._ctlSelectAll.Text = "全选";
            this._ctlSelectAll.UseVisualStyleBackColor = true;
            // 
            // SyncPlatformDeployPackageDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 381);
            this.Controls.Add(this._ctlSelectAll);
            this.Controls.Add(this._ctlList);
            this.Controls.Add(this.selectPathButton1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._txtDir);
            this.Name = "SyncPlatformDeployPackageDialog";
            this.Text = "同步平台程序集";
            this.Load += new System.EventHandler(this.SyncPlatformDeployPackageDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _txtDir;
        private System.Windows.Forms.Label label1;
        private Common.WinForm.SelectPathButton selectPathButton1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox _ctlList;
        private Common.WinForm.SelectAllCheckBox _ctlSelectAll;
    }
}
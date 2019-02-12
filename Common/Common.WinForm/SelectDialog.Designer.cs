namespace Common.WinForm
{
    partial class SelectDialog
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
            this._lbList = new System.Windows.Forms.ListBox();
            this._ctlSelectAll = new Common.WinForm.SelectAllCheckBox(this.components);
            this.SuspendLayout();
            // 
            // _lbList
            // 
            this._lbList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this._lbList.FormattingEnabled = true;
            this._lbList.HorizontalScrollbar = true;
            this._lbList.IntegralHeight = false;
            this._lbList.ItemHeight = 20;
            this._lbList.Location = new System.Drawing.Point(12, 12);
            this._lbList.Name = "_lbList";
            this._lbList.Size = new System.Drawing.Size(399, 226);
            this._lbList.TabIndex = 0;
            this._lbList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this._lbList_DrawItem);
            this._lbList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this._lbList_MouseDoubleClick);
            // 
            // _ctlSelectAll
            // 
            this._ctlSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._ctlSelectAll.AutoSize = true;
            this._ctlSelectAll.Checked = true;
            this._ctlSelectAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this._ctlSelectAll.ListBox = this._lbList;
            this._ctlSelectAll.Location = new System.Drawing.Point(12, 257);
            this._ctlSelectAll.Name = "_ctlSelectAll";
            this._ctlSelectAll.Size = new System.Drawing.Size(48, 16);
            this._ctlSelectAll.TabIndex = 1;
            this._ctlSelectAll.Text = "全选";
            this._ctlSelectAll.UseVisualStyleBackColor = true;
            // 
            // SelectDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 287);
            this.Controls.Add(this._ctlSelectAll);
            this.Controls.Add(this._lbList);
            this.KeyPreview = true;
            this.Name = "SelectDialog";
            this.Text = "CollectionEditorDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox _lbList;
        private SelectAllCheckBox _ctlSelectAll;
    }
}
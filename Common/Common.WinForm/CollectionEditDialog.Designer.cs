namespace Common.WinForm
{
    partial class CollectionEditDialog
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
            this._listbox = new System.Windows.Forms.ListBox();
            this._ctlAdd = new System.Windows.Forms.Button();
            this._ctlRemove = new System.Windows.Forms.Button();
            this._ctlEdit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _listbox
            // 
            this._listbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._listbox.FormattingEnabled = true;
            this._listbox.ItemHeight = 12;
            this._listbox.Location = new System.Drawing.Point(12, 12);
            this._listbox.Name = "_listbox";
            this._listbox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this._listbox.Size = new System.Drawing.Size(451, 220);
            this._listbox.TabIndex = 0;
            this._listbox.SelectedIndexChanged += new System.EventHandler(this._listbox_SelectedIndexChanged);
            this._listbox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this._listbox_MouseDoubleClick);
            // 
            // _ctlAdd
            // 
            this._ctlAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._ctlAdd.Location = new System.Drawing.Point(12, 246);
            this._ctlAdd.Name = "_ctlAdd";
            this._ctlAdd.Size = new System.Drawing.Size(75, 23);
            this._ctlAdd.TabIndex = 1;
            this._ctlAdd.Text = "添加 ...";
            this._ctlAdd.UseVisualStyleBackColor = true;
            this._ctlAdd.Click += new System.EventHandler(this._ctlAdd_Click);
            // 
            // _ctlRemove
            // 
            this._ctlRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._ctlRemove.Location = new System.Drawing.Point(93, 246);
            this._ctlRemove.Name = "_ctlRemove";
            this._ctlRemove.Size = new System.Drawing.Size(75, 23);
            this._ctlRemove.TabIndex = 2;
            this._ctlRemove.Text = "删除 ...";
            this._ctlRemove.UseVisualStyleBackColor = true;
            this._ctlRemove.Click += new System.EventHandler(this._ctlRemove_Click);
            // 
            // _ctlEdit
            // 
            this._ctlEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._ctlEdit.Location = new System.Drawing.Point(174, 246);
            this._ctlEdit.Name = "_ctlEdit";
            this._ctlEdit.Size = new System.Drawing.Size(75, 23);
            this._ctlEdit.TabIndex = 3;
            this._ctlEdit.Text = "编辑 ...";
            this._ctlEdit.UseVisualStyleBackColor = true;
            this._ctlEdit.Click += new System.EventHandler(this._ctlEdit_Click);
            // 
            // CollectionEditDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 281);
            this.Controls.Add(this._ctlEdit);
            this.Controls.Add(this._ctlRemove);
            this.Controls.Add(this._ctlAdd);
            this.Controls.Add(this._listbox);
            this.Name = "CollectionEditDialog";
            this.Text = "CollectionEditDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox _listbox;
        private System.Windows.Forms.Button _ctlAdd;
        private System.Windows.Forms.Button _ctlRemove;
        private System.Windows.Forms.Button _ctlEdit;
    }
}
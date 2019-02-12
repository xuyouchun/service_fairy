namespace Common.WinForm
{
    partial class ErrorDialog
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
            this._btnDetail = new System.Windows.Forms.Button();
            this._txtMsg = new System.Windows.Forms.TextBox();
            this._btnCopy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _btnDetail
            // 
            this._btnDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._btnDetail.Location = new System.Drawing.Point(12, 159);
            this._btnDetail.Name = "_btnDetail";
            this._btnDetail.Size = new System.Drawing.Size(75, 23);
            this._btnDetail.TabIndex = 1;
            this._btnDetail.Text = "详细信息";
            this._btnDetail.UseVisualStyleBackColor = true;
            this._btnDetail.Click += new System.EventHandler(this._btnDetail_Click);
            // 
            // _txtMsg
            // 
            this._txtMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._txtMsg.Location = new System.Drawing.Point(12, 12);
            this._txtMsg.Multiline = true;
            this._txtMsg.Name = "_txtMsg";
            this._txtMsg.ReadOnly = true;
            this._txtMsg.Size = new System.Drawing.Size(468, 141);
            this._txtMsg.TabIndex = 0;
            // 
            // _btnCopy
            // 
            this._btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this._btnCopy.Location = new System.Drawing.Point(93, 159);
            this._btnCopy.Name = "_btnCopy";
            this._btnCopy.Size = new System.Drawing.Size(75, 23);
            this._btnCopy.TabIndex = 2;
            this._btnCopy.Text = "复制";
            this._btnCopy.UseVisualStyleBackColor = true;
            this._btnCopy.Visible = false;
            this._btnCopy.Click += new System.EventHandler(this._btnCopy_Click);
            // 
            // ErrorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ButtonModel = Common.WinForm.XDialogButtonModel.Close;
            this.ClientSize = new System.Drawing.Size(492, 194);
            this.Controls.Add(this._btnCopy);
            this.Controls.Add(this._btnDetail);
            this.Controls.Add(this._txtMsg);
            this.Name = "ErrorDialog";
            this.Text = "Error";
            this.Controls.SetChildIndex(this._txtMsg, 0);
            this.Controls.SetChildIndex(this._btnDetail, 0);
            this.Controls.SetChildIndex(this._btnCopy, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _btnDetail;
        private System.Windows.Forms.TextBox _txtMsg;
        private System.Windows.Forms.Button _btnCopy;
    }
}
namespace ServiceFairy.WinForm
{
    partial class InputServiceAddressDialog
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
            this._lbCaption = new System.Windows.Forms.Label();
            this._ipAddress = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _lbCaption
            // 
            this._lbCaption.AutoSize = true;
            this._lbCaption.Location = new System.Drawing.Point(12, 18);
            this._lbCaption.Name = "_lbCaption";
            this._lbCaption.Size = new System.Drawing.Size(113, 12);
            this._lbCaption.TabIndex = 2147483551;
            this._lbCaption.Text = "导航服务器的地址：";
            // 
            // _ipAddress
            // 
            this._ipAddress.Location = new System.Drawing.Point(50, 47);
            this._ipAddress.Name = "_ipAddress";
            this._ipAddress.Size = new System.Drawing.Size(238, 21);
            this._ipAddress.TabIndex = 2147483552;
            this._ipAddress.Text = "127.0.0.1:9001";
            // 
            // InputServiceAddressDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 129);
            this.Controls.Add(this._lbCaption);
            this.Controls.Add(this._ipAddress);
            this.Name = "InputServiceAddressDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.Controls.SetChildIndex(this._ipAddress, 0);
            this.Controls.SetChildIndex(this._lbCaption, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _lbCaption;
        private System.Windows.Forms.TextBox _ipAddress;

    }
}
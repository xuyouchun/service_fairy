namespace Common.WinForm
{
    partial class InputDialog
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
            this._lbText = new System.Windows.Forms.Label();
            this._txtInput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _lbText
            // 
            this._lbText.AutoSize = true;
            this._lbText.Location = new System.Drawing.Point(12, 9);
            this._lbText.Name = "_lbText";
            this._lbText.Size = new System.Drawing.Size(53, 12);
            this._lbText.TabIndex = 2147483549;
            this._lbText.Text = "请输入：";
            // 
            // _txtInput
            // 
            this._txtInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._txtInput.Location = new System.Drawing.Point(14, 41);
            this._txtInput.Name = "_txtInput";
            this._txtInput.Size = new System.Drawing.Size(417, 21);
            this._txtInput.TabIndex = 2147483550;
            // 
            // InputDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 124);
            this.Controls.Add(this._lbText);
            this.Controls.Add(this._txtInput);
            this.Name = "InputDialog";
            this.Text = "";
            this.Controls.SetChildIndex(this._txtInput, 0);
            this.Controls.SetChildIndex(this._lbText, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _lbText;
        private System.Windows.Forms.TextBox _txtInput;
    }
}
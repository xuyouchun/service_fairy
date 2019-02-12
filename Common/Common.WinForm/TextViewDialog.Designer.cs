namespace Common.WinForm
{
    partial class TextViewDialog
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
            this._txt = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // _txt
            // 
            this._txt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._txt.BackColor = System.Drawing.SystemColors.Window;
            this._txt.Location = new System.Drawing.Point(12, 12);
            this._txt.Name = "_txt";
            this._txt.ReadOnly = true;
            this._txt.Size = new System.Drawing.Size(568, 300);
            this._txt.TabIndex = 2147483548;
            this._txt.Text = "";
            // 
            // TextViewDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ButtonModel = Common.WinForm.XDialogButtonModel.Close;
            this.ClientSize = new System.Drawing.Size(592, 353);
            this.Controls.Add(this._txt);
            this.MaximizeBox = true;
            this.Name = "TextViewDialog";
            this.Text = "文本阅读器";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox _txt;
    }
}
namespace ServiceFairy.WinForm.AppCommandInvokeControls
{
    partial class ArgumentSampleInvokeControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._ctlText = new System.Windows.Forms.RichTextBox();
            this._ctlCopy = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // _ctlText
            // 
            this._ctlText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._ctlText.DetectUrls = false;
            this._ctlText.Font = new System.Drawing.Font("Bookman Old Style", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ctlText.Location = new System.Drawing.Point(0, 0);
            this._ctlText.Margin = new System.Windows.Forms.Padding(10);
            this._ctlText.Name = "_ctlText";
            this._ctlText.ReadOnly = true;
            this._ctlText.Size = new System.Drawing.Size(433, 285);
            this._ctlText.TabIndex = 0;
            this._ctlText.Text = "";
            // 
            // _ctlCopy
            // 
            this._ctlCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._ctlCopy.AutoSize = true;
            this._ctlCopy.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this._ctlCopy.Location = new System.Drawing.Point(353, 288);
            this._ctlCopy.Name = "_ctlCopy";
            this._ctlCopy.Size = new System.Drawing.Size(65, 12);
            this._ctlCopy.TabIndex = 1;
            this._ctlCopy.TabStop = true;
            this._ctlCopy.Text = "=> 复制 <=";
            this._ctlCopy.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._ctlCopy_LinkClicked);
            // 
            // ArgumentSampleInvokeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._ctlCopy);
            this.Controls.Add(this._ctlText);
            this.Name = "ArgumentSampleInvokeControl";
            this.Size = new System.Drawing.Size(433, 309);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox _ctlText;
        private System.Windows.Forms.LinkLabel _ctlCopy;
    }
}

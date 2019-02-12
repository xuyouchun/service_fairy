namespace Common.WinForm
{
    partial class ProgressWindow
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
            this._lbTxt = new System.Windows.Forms.Label();
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this._ctlCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _lbTxt
            // 
            this._lbTxt.AutoSize = true;
            this._lbTxt.Location = new System.Drawing.Point(12, 16);
            this._lbTxt.Name = "_lbTxt";
            this._lbTxt.Size = new System.Drawing.Size(41, 12);
            this._lbTxt.TabIndex = 0;
            this._lbTxt.Text = "label1";
            // 
            // _progressBar
            // 
            this._progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._progressBar.Location = new System.Drawing.Point(12, 41);
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(365, 23);
            this._progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this._progressBar.TabIndex = 1;
            // 
            // _ctlCancel
            // 
            this._ctlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._ctlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._ctlCancel.Location = new System.Drawing.Point(302, 71);
            this._ctlCancel.Name = "_ctlCancel";
            this._ctlCancel.Size = new System.Drawing.Size(75, 23);
            this._ctlCancel.TabIndex = 2;
            this._ctlCancel.Text = "取消";
            this._ctlCancel.UseVisualStyleBackColor = true;
            this._ctlCancel.Click += new System.EventHandler(this._ctlCancel_Click);
            // 
            // ProgressWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._ctlCancel;
            this.ClientSize = new System.Drawing.Size(389, 106);
            this.Controls.Add(this._ctlCancel);
            this.Controls.Add(this._progressBar);
            this.Controls.Add(this._lbTxt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.FormShown = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProgressWindow_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _lbTxt;
        private System.Windows.Forms.ProgressBar _progressBar;
        private System.Windows.Forms.Button _ctlCancel;
    }
}
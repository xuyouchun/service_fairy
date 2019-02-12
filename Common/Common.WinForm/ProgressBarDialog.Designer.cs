namespace Common.WinForm
{
    partial class ProgressBarDialog
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
            this._pb = new System.Windows.Forms.ProgressBar();
            this._lbText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _pb
            // 
            this._pb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._pb.Location = new System.Drawing.Point(12, 49);
            this._pb.MarqueeAnimationSpeed = 50;
            this._pb.Name = "_pb";
            this._pb.Size = new System.Drawing.Size(462, 23);
            this._pb.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this._pb.TabIndex = 2147483548;
            // 
            // _lbText
            // 
            this._lbText.AutoSize = true;
            this._lbText.Location = new System.Drawing.Point(12, 19);
            this._lbText.Name = "_lbText";
            this._lbText.Size = new System.Drawing.Size(65, 12);
            this._lbText.TabIndex = 2147483549;
            this._lbText.Text = "请稍候 ...";
            // 
            // ProgressBarDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ButtonModel = Common.WinForm.XDialogButtonModel.Cancel;
            this.ClientSize = new System.Drawing.Size(486, 126);
            this.Controls.Add(this._pb);
            this.Controls.Add(this._lbText);
            this.Name = "ProgressBarDialog";
            this.Text = "";
            this.Controls.SetChildIndex(this._lbText, 0);
            this.Controls.SetChildIndex(this._pb, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar _pb;
        private System.Windows.Forms.Label _lbText;
    }
}
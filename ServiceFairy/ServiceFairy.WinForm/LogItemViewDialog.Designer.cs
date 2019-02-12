namespace ServiceFairy.WinForm
{
    partial class LogItemViewDialog
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
            this._ctlTitleLabel = new System.Windows.Forms.Label();
            this._ctlTitle = new System.Windows.Forms.TextBox();
            this._ctlDetail = new System.Windows.Forms.TextBox();
            this._ctlTime = new System.Windows.Forms.TextBox();
            this._ctlSource = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._ctlCopy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _ctlTitleLabel
            // 
            this._ctlTitleLabel.AutoSize = true;
            this._ctlTitleLabel.Location = new System.Drawing.Point(12, 18);
            this._ctlTitleLabel.Name = "_ctlTitleLabel";
            this._ctlTitleLabel.Size = new System.Drawing.Size(41, 12);
            this._ctlTitleLabel.TabIndex = 2147483548;
            this._ctlTitleLabel.Text = "标题：";
            // 
            // _ctlTitle
            // 
            this._ctlTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._ctlTitle.BackColor = System.Drawing.SystemColors.Window;
            this._ctlTitle.Location = new System.Drawing.Point(59, 15);
            this._ctlTitle.Name = "_ctlTitle";
            this._ctlTitle.ReadOnly = true;
            this._ctlTitle.Size = new System.Drawing.Size(597, 21);
            this._ctlTitle.TabIndex = 0;
            // 
            // _ctlDetail
            // 
            this._ctlDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._ctlDetail.BackColor = System.Drawing.SystemColors.Window;
            this._ctlDetail.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ctlDetail.Location = new System.Drawing.Point(59, 42);
            this._ctlDetail.Multiline = true;
            this._ctlDetail.Name = "_ctlDetail";
            this._ctlDetail.ReadOnly = true;
            this._ctlDetail.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._ctlDetail.Size = new System.Drawing.Size(597, 252);
            this._ctlDetail.TabIndex = 1;
            // 
            // _ctlTime
            // 
            this._ctlTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._ctlTime.BackColor = System.Drawing.SystemColors.Window;
            this._ctlTime.Location = new System.Drawing.Point(59, 300);
            this._ctlTime.Name = "_ctlTime";
            this._ctlTime.ReadOnly = true;
            this._ctlTime.Size = new System.Drawing.Size(597, 21);
            this._ctlTime.TabIndex = 2;
            // 
            // _ctlSource
            // 
            this._ctlSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._ctlSource.BackColor = System.Drawing.SystemColors.Window;
            this._ctlSource.Location = new System.Drawing.Point(59, 327);
            this._ctlSource.Name = "_ctlSource";
            this._ctlSource.ReadOnly = true;
            this._ctlSource.Size = new System.Drawing.Size(597, 21);
            this._ctlSource.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2147483548;
            this.label1.Text = "详细：";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 303);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2147483548;
            this.label2.Text = "时间：";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 330);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 2147483548;
            this.label3.Text = "源：";
            // 
            // _ctlCopy
            // 
            this._ctlCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._ctlCopy.Location = new System.Drawing.Point(14, 367);
            this._ctlCopy.Name = "_ctlCopy";
            this._ctlCopy.Size = new System.Drawing.Size(75, 23);
            this._ctlCopy.TabIndex = 4;
            this._ctlCopy.Text = "复制";
            this._ctlCopy.UseVisualStyleBackColor = true;
            this._ctlCopy.Click += new System.EventHandler(this._ctlCopy_Click);
            // 
            // LogItemViewDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ButtonModel = Common.WinForm.XDialogButtonModel.Close;
            this.ClientSize = new System.Drawing.Size(668, 402);
            this.Controls.Add(this._ctlCopy);
            this.Controls.Add(this._ctlDetail);
            this.Controls.Add(this._ctlSource);
            this.Controls.Add(this._ctlTime);
            this.Controls.Add(this._ctlTitle);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._ctlTitleLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.Name = "LogItemViewDialog";
            this.Text = "查看日志";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _ctlTitleLabel;
        private System.Windows.Forms.TextBox _ctlTitle;
        private System.Windows.Forms.TextBox _ctlDetail;
        private System.Windows.Forms.TextBox _ctlTime;
        private System.Windows.Forms.TextBox _ctlSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button _ctlCopy;
    }
}
﻿namespace ServiceFairy.WinForm
{
    partial class EditTitleAndDescDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this._ctlClientTitle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._ctlClientDesc = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2147483549;
            this.label1.Text = "名称：";
            // 
            // _ctlClientTitle
            // 
            this._ctlClientTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._ctlClientTitle.Location = new System.Drawing.Point(75, 19);
            this._ctlClientTitle.Name = "_ctlClientTitle";
            this._ctlClientTitle.Size = new System.Drawing.Size(295, 21);
            this._ctlClientTitle.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2147483549;
            this.label2.Text = "描述：";
            // 
            // _ctlClientDesc
            // 
            this._ctlClientDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._ctlClientDesc.Location = new System.Drawing.Point(75, 46);
            this._ctlClientDesc.Name = "_ctlClientDesc";
            this._ctlClientDesc.Size = new System.Drawing.Size(295, 21);
            this._ctlClientDesc.TabIndex = 1;
            // 
            // EditTitleAndDescDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 135);
            this.Controls.Add(this._ctlClientDesc);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._ctlClientTitle);
            this.Controls.Add(this.label1);
            this.Name = "EditTitleAndDescDialog";
            this.Text = "";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _ctlClientTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _ctlClientDesc;
    }
}
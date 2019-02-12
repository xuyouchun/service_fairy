namespace Common.WinForm
{
    partial class DropdownSelectDialog
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
            this._ctlList = new System.Windows.Forms.ComboBox();
            this._ctlPrompt = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _ctlList
            // 
            this._ctlList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._ctlList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._ctlList.FormattingEnabled = true;
            this._ctlList.Location = new System.Drawing.Point(12, 44);
            this._ctlList.Name = "_ctlList";
            this._ctlList.Size = new System.Drawing.Size(421, 20);
            this._ctlList.TabIndex = 2147483549;
            // 
            // _ctlPrompt
            // 
            this._ctlPrompt.AutoSize = true;
            this._ctlPrompt.Location = new System.Drawing.Point(13, 13);
            this._ctlPrompt.Name = "_ctlPrompt";
            this._ctlPrompt.Size = new System.Drawing.Size(53, 12);
            this._ctlPrompt.TabIndex = 2147483550;
            this._ctlPrompt.Text = "请输入：";
            // 
            // DropdownSelectDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 130);
            this.Controls.Add(this._ctlPrompt);
            this.Controls.Add(this._ctlList);
            this.Name = "DropdownSelectDialog";
            this.Text = "DropdownSelectDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox _ctlList;
        private System.Windows.Forms.Label _ctlPrompt;
    }
}
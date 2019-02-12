namespace PhoneAreaQuerier
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._txtPhoneNumber = new DevComponents.DotNetBar.Controls.TextBoxX();
            this._lbAreaText = new DevComponents.DotNetBar.LabelX();
            this._btnSet = new System.Windows.Forms.Button();
            this._lbInfo = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // _txtPhoneNumber
            // 
            this._txtPhoneNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this._txtPhoneNumber.Border.Class = "TextBoxBorder";
            this._txtPhoneNumber.Font = new System.Drawing.Font("Arial Rounded MT Bold", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._txtPhoneNumber.ForeColor = System.Drawing.Color.Olive;
            this._txtPhoneNumber.HideSelection = false;
            this._txtPhoneNumber.Location = new System.Drawing.Point(29, 82);
            this._txtPhoneNumber.Name = "_txtPhoneNumber";
            this._txtPhoneNumber.Size = new System.Drawing.Size(524, 64);
            this._txtPhoneNumber.TabIndex = 2147483548;
            this._txtPhoneNumber.Text = "13717674043";
            this._txtPhoneNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._txtPhoneNumber.WatermarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._txtPhoneNumber.WatermarkFont = new System.Drawing.Font("LiSu", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._txtPhoneNumber.WatermarkText = "输入手机号码";
            this._txtPhoneNumber.TextChanged += new System.EventHandler(this._txtPhoneNumber_TextChanged);
            // 
            // _lbAreaText
            // 
            this._lbAreaText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbAreaText.Font = new System.Drawing.Font("LiSu", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._lbAreaText.Location = new System.Drawing.Point(29, 183);
            this._lbAreaText.Name = "_lbAreaText";
            this._lbAreaText.Size = new System.Drawing.Size(524, 50);
            this._lbAreaText.TabIndex = 2147483549;
            this._lbAreaText.Text = "黑龙江鹤岗移动";
            this._lbAreaText.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // _btnSet
            // 
            this._btnSet.Location = new System.Drawing.Point(12, 292);
            this._btnSet.Name = "_btnSet";
            this._btnSet.Size = new System.Drawing.Size(75, 23);
            this._btnSet.TabIndex = 2147483551;
            this._btnSet.Text = "选项 ...";
            this._btnSet.UseVisualStyleBackColor = true;
            this._btnSet.Click += new System.EventHandler(this._btnSet_Click);
            // 
            // _lbInfo
            // 
            this._lbInfo.Location = new System.Drawing.Point(29, 12);
            this._lbInfo.Name = "_lbInfo";
            this._lbInfo.Size = new System.Drawing.Size(524, 51);
            this._lbInfo.TabIndex = 2147483552;
            this._lbInfo.Text = "所在国家：中国";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ButtonModel = Common.WinForm.XDialogButtonModel.Close;
            this.ClientSize = new System.Drawing.Size(583, 327);
            this.Controls.Add(this._lbInfo);
            this.Controls.Add(this._btnSet);
            this.Controls.Add(this._txtPhoneNumber);
            this.Controls.Add(this._lbAreaText);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.ShowIcon = true;
            this.ShowInTaskbar = true;
            this.Text = "世界电话号码归属地查询器";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX _txtPhoneNumber;
        private DevComponents.DotNetBar.LabelX _lbAreaText;
        private System.Windows.Forms.Button _btnSet;
        private DevComponents.DotNetBar.LabelX _lbInfo;
    }
}


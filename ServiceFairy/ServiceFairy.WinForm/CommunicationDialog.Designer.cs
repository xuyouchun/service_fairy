namespace ServiceFairy.WinForm
{
    partial class CommunicationDialog
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
            this._ctlProtocal = new System.Windows.Forms.ComboBox();
            this._ctlPort = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._ctlIps = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this._ctlSimpleDirect = new System.Windows.Forms.RadioButton();
            this._ctlDuplexDirect = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this._ctlPort)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2147483549;
            this.label1.Text = "协议：";
            // 
            // _ctlProtocal
            // 
            this._ctlProtocal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._ctlProtocal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._ctlProtocal.FormattingEnabled = true;
            this._ctlProtocal.Location = new System.Drawing.Point(95, 24);
            this._ctlProtocal.Name = "_ctlProtocal";
            this._ctlProtocal.Size = new System.Drawing.Size(246, 20);
            this._ctlProtocal.TabIndex = 0;
            this._ctlProtocal.SelectedIndexChanged += new System.EventHandler(this._ctlProtocal_SelectedIndexChanged);
            // 
            // _ctlPort
            // 
            this._ctlPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._ctlPort.Location = new System.Drawing.Point(95, 80);
            this._ctlPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this._ctlPort.Name = "_ctlPort";
            this._ctlPort.Size = new System.Drawing.Size(162, 21);
            this._ctlPort.TabIndex = 2;
            this._ctlPort.Value = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2147483549;
            this.label2.Text = "端口：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 2147483549;
            this.label3.Text = "IP：";
            // 
            // _ctlIps
            // 
            this._ctlIps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._ctlIps.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._ctlIps.FormattingEnabled = true;
            this._ctlIps.Location = new System.Drawing.Point(95, 53);
            this._ctlIps.Name = "_ctlIps";
            this._ctlIps.Size = new System.Drawing.Size(246, 20);
            this._ctlIps.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 2147483549;
            this.label4.Text = "连接方式：";
            // 
            // _ctlSimpleDirect
            // 
            this._ctlSimpleDirect.AutoSize = true;
            this._ctlSimpleDirect.Checked = true;
            this._ctlSimpleDirect.Location = new System.Drawing.Point(95, 107);
            this._ctlSimpleDirect.Name = "_ctlSimpleDirect";
            this._ctlSimpleDirect.Size = new System.Drawing.Size(47, 16);
            this._ctlSimpleDirect.TabIndex = 3;
            this._ctlSimpleDirect.TabStop = true;
            this._ctlSimpleDirect.Text = "单向";
            this._ctlSimpleDirect.UseVisualStyleBackColor = true;
            // 
            // _ctlDuplexDirect
            // 
            this._ctlDuplexDirect.AutoSize = true;
            this._ctlDuplexDirect.Location = new System.Drawing.Point(148, 107);
            this._ctlDuplexDirect.Name = "_ctlDuplexDirect";
            this._ctlDuplexDirect.Size = new System.Drawing.Size(179, 16);
            this._ctlDuplexDirect.TabIndex = 4;
            this._ctlDuplexDirect.Text = "双向（支持服务器推送消息）";
            this._ctlDuplexDirect.UseVisualStyleBackColor = true;
            // 
            // CommunicationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 178);
            this.Controls.Add(this._ctlDuplexDirect);
            this.Controls.Add(this._ctlSimpleDirect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._ctlIps);
            this.Controls.Add(this._ctlProtocal);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._ctlPort);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Name = "CommunicationDialog";
            this.Text = "新信道";
            ((System.ComponentModel.ISupportInitialize)(this._ctlPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox _ctlProtocal;
        private System.Windows.Forms.NumericUpDown _ctlPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox _ctlIps;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton _ctlSimpleDirect;
        private System.Windows.Forms.RadioButton _ctlDuplexDirect;
    }
}
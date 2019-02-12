namespace ServiceFairy.TrayManagement
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this._tsStrip = new System.Windows.Forms.StatusStrip();
            this._tsServiceStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this._ctlModibyName = new System.Windows.Forms.LinkLabel();
            this._ctlRunningServices = new System.Windows.Forms.LinkLabel();
            this._ctlCommunicationOption = new System.Windows.Forms.LinkLabel();
            this._ctlMasterAddress = new System.Windows.Forms.LinkLabel();
            this._ctlRunningServicesLabel = new System.Windows.Forms.Label();
            this._ctlCommunicationLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._ctlModelGroupBox = new System.Windows.Forms.GroupBox();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this._tsMenu = new System.Windows.Forms.ToolStrip();
            this._tsStart = new System.Windows.Forms.ToolStripButton();
            this._tsStop = new System.Windows.Forms.ToolStripButton();
            this._tsRestart = new System.Windows.Forms.ToolStripButton();
            this._timer = new System.Windows.Forms.Timer(this.components);
            this._balloonTip = new DevComponents.DotNetBar.BalloonTip();
            this._ctlRefresh = new System.Windows.Forms.LinkLabel();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this._tsStrip.SuspendLayout();
            this._ctlModelGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this._tsMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this._tsStrip);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this._ctlRefresh);
            this.toolStripContainer1.ContentPanel.Controls.Add(this._ctlModibyName);
            this.toolStripContainer1.ContentPanel.Controls.Add(this._ctlRunningServices);
            this.toolStripContainer1.ContentPanel.Controls.Add(this._ctlCommunicationOption);
            this.toolStripContainer1.ContentPanel.Controls.Add(this._ctlMasterAddress);
            this.toolStripContainer1.ContentPanel.Controls.Add(this._ctlRunningServicesLabel);
            this.toolStripContainer1.ContentPanel.Controls.Add(this._ctlCommunicationLabel);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.label1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this._ctlModelGroupBox);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.pictureBox1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(552, 225);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(552, 272);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this._tsMenu);
            // 
            // _tsStrip
            // 
            this._tsStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._tsStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._tsServiceStatus});
            this._tsStrip.Location = new System.Drawing.Point(0, 0);
            this._tsStrip.Name = "_tsStrip";
            this._tsStrip.Size = new System.Drawing.Size(552, 22);
            this._tsStrip.TabIndex = 0;
            // 
            // _tsServiceStatus
            // 
            this._tsServiceStatus.Name = "_tsServiceStatus";
            this._tsServiceStatus.Size = new System.Drawing.Size(56, 17);
            this._tsServiceStatus.Text = "正在运行";
            // 
            // _ctlModibyName
            // 
            this._ctlModibyName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._ctlModibyName.AutoSize = true;
            this._ctlModibyName.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this._ctlModibyName.LinkColor = System.Drawing.SystemColors.ControlText;
            this._ctlModibyName.Location = new System.Drawing.Point(487, 202);
            this._ctlModibyName.Name = "_ctlModibyName";
            this._ctlModibyName.Size = new System.Drawing.Size(53, 12);
            this._ctlModibyName.TabIndex = 6;
            this._ctlModibyName.TabStop = true;
            this._ctlModibyName.Text = "修改名称";
            this._ctlModibyName.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._ctlModibyName_LinkClicked);
            // 
            // _ctlRunningServices
            // 
            this._ctlRunningServices.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this._ctlRunningServices.LinkColor = System.Drawing.SystemColors.ControlText;
            this._ctlRunningServices.Location = new System.Drawing.Point(121, 169);
            this._ctlRunningServices.Name = "_ctlRunningServices";
            this._ctlRunningServices.Size = new System.Drawing.Size(419, 12);
            this._ctlRunningServices.TabIndex = 5;
            this._ctlRunningServices.TabStop = true;
            this._ctlRunningServices.Text = "System.Master 1.0.0.0";
            this._ctlRunningServices.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._ctlRunningServices_LinkClicked);
            // 
            // _ctlCommunicationOption
            // 
            this._ctlCommunicationOption.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this._ctlCommunicationOption.LinkColor = System.Drawing.SystemColors.ControlText;
            this._ctlCommunicationOption.Location = new System.Drawing.Point(121, 143);
            this._ctlCommunicationOption.Name = "_ctlCommunicationOption";
            this._ctlCommunicationOption.Size = new System.Drawing.Size(419, 12);
            this._ctlCommunicationOption.TabIndex = 5;
            this._ctlCommunicationOption.TabStop = true;
            this._ctlCommunicationOption.Text = "127.0.0.1:9000";
            this._ctlCommunicationOption.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._ctlCommunicationOption_LinkClicked);
            // 
            // _ctlMasterAddress
            // 
            this._ctlMasterAddress.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this._ctlMasterAddress.LinkColor = System.Drawing.SystemColors.ControlText;
            this._ctlMasterAddress.Location = new System.Drawing.Point(121, 119);
            this._ctlMasterAddress.Name = "_ctlMasterAddress";
            this._ctlMasterAddress.Size = new System.Drawing.Size(419, 12);
            this._ctlMasterAddress.TabIndex = 5;
            this._ctlMasterAddress.TabStop = true;
            this._ctlMasterAddress.Text = "MASTER:8090";
            this._ctlMasterAddress.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._ctlMasterAddress_LinkClicked);
            // 
            // _ctlRunningServicesLabel
            // 
            this._ctlRunningServicesLabel.Location = new System.Drawing.Point(21, 169);
            this._ctlRunningServicesLabel.Name = "_ctlRunningServicesLabel";
            this._ctlRunningServicesLabel.Size = new System.Drawing.Size(89, 12);
            this._ctlRunningServicesLabel.TabIndex = 4;
            this._ctlRunningServicesLabel.Text = "运行中的服务：";
            // 
            // _ctlCommunicationLabel
            // 
            this._ctlCommunicationLabel.Location = new System.Drawing.Point(21, 143);
            this._ctlCommunicationLabel.Name = "_ctlCommunicationLabel";
            this._ctlCommunicationLabel.Size = new System.Drawing.Size(89, 12);
            this._ctlCommunicationLabel.TabIndex = 4;
            this._ctlCommunicationLabel.Text = "已开启的端口：";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(21, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "中心服务地址：";
            // 
            // _ctlModelGroupBox
            // 
            this._ctlModelGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._ctlModelGroupBox.Controls.Add(this.radioButton4);
            this._ctlModelGroupBox.Controls.Add(this.radioButton3);
            this._ctlModelGroupBox.Controls.Add(this.radioButton2);
            this._ctlModelGroupBox.Controls.Add(this.radioButton1);
            this._ctlModelGroupBox.Location = new System.Drawing.Point(109, 3);
            this._ctlModelGroupBox.Name = "_ctlModelGroupBox";
            this._ctlModelGroupBox.Size = new System.Drawing.Size(431, 67);
            this._ctlModelGroupBox.TabIndex = 3;
            this._ctlModelGroupBox.TabStop = false;
            this._ctlModelGroupBox.Text = "模式";
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(291, 29);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(71, 16);
            this.radioButton4.TabIndex = 0;
            this.radioButton4.Tag = "Proxy";
            this.radioButton4.Text = "代理模式";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(214, 29);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(71, 16);
            this.radioButton3.TabIndex = 0;
            this.radioButton3.Tag = "Navigation";
            this.radioButton3.Text = "导航模式";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(137, 29);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(71, 16);
            this.radioButton2.TabIndex = 0;
            this.radioButton2.Tag = "Master";
            this.radioButton2.Text = "中心服务";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(60, 29);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(71, 16);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Tag = "Normal";
            this.radioButton1.Text = "普通终端";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // _tsMenu
            // 
            this._tsMenu.Dock = System.Windows.Forms.DockStyle.None;
            this._tsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._tsStart,
            this._tsStop,
            this._tsRestart});
            this._tsMenu.Location = new System.Drawing.Point(0, 0);
            this._tsMenu.Name = "_tsMenu";
            this._tsMenu.Size = new System.Drawing.Size(552, 25);
            this._tsMenu.Stretch = true;
            this._tsMenu.TabIndex = 0;
            // 
            // _tsStart
            // 
            this._tsStart.Image = ((System.Drawing.Image)(resources.GetObject("_tsStart.Image")));
            this._tsStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tsStart.Name = "_tsStart";
            this._tsStart.Size = new System.Drawing.Size(52, 22);
            this._tsStart.Text = "启动";
            this._tsStart.Click += new System.EventHandler(this._tsStart_Click);
            // 
            // _tsStop
            // 
            this._tsStop.Image = ((System.Drawing.Image)(resources.GetObject("_tsStop.Image")));
            this._tsStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tsStop.Name = "_tsStop";
            this._tsStop.Size = new System.Drawing.Size(52, 22);
            this._tsStop.Text = "停止";
            this._tsStop.Click += new System.EventHandler(this._tsStop_Click);
            // 
            // _tsRestart
            // 
            this._tsRestart.Image = ((System.Drawing.Image)(resources.GetObject("_tsRestart.Image")));
            this._tsRestart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tsRestart.Name = "_tsRestart";
            this._tsRestart.Size = new System.Drawing.Size(76, 22);
            this._tsRestart.Text = "重新启动";
            this._tsRestart.Click += new System.EventHandler(this._tsRestart_Click);
            // 
            // _timer
            // 
            this._timer.Enabled = true;
            this._timer.Interval = 5000;
            this._timer.Tick += new System.EventHandler(this._timer_Tick);
            // 
            // _ctlRefresh
            // 
            this._ctlRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._ctlRefresh.AutoSize = true;
            this._ctlRefresh.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this._ctlRefresh.LinkColor = System.Drawing.SystemColors.ControlText;
            this._ctlRefresh.Location = new System.Drawing.Point(12, 202);
            this._ctlRefresh.Name = "_ctlRefresh";
            this._ctlRefresh.Size = new System.Drawing.Size(29, 12);
            this._ctlRefresh.TabIndex = 6;
            this._ctlRefresh.TabStop = true;
            this._ctlRefresh.Text = "刷新";
            this._ctlRefresh.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._ctlRefresh_LinkClicked);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 272);
            this.Controls.Add(this.toolStripContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "ServiceFairy Endpoint Console";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this._tsStrip.ResumeLayout(false);
            this._tsStrip.PerformLayout();
            this._ctlModelGroupBox.ResumeLayout(false);
            this._ctlModelGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this._tsMenu.ResumeLayout(false);
            this._tsMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStrip _tsMenu;
        private System.Windows.Forms.StatusStrip _tsStrip;
        private System.Windows.Forms.ToolStripStatusLabel _tsServiceStatus;
        private System.Windows.Forms.GroupBox _ctlModelGroupBox;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.LinkLabel _ctlMasterAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel _ctlCommunicationOption;
        private System.Windows.Forms.Label _ctlCommunicationLabel;
        private System.Windows.Forms.Label _ctlRunningServicesLabel;
        private System.Windows.Forms.LinkLabel _ctlRunningServices;
        private System.Windows.Forms.Timer _timer;
        private System.Windows.Forms.ToolStripButton _tsStop;
        private System.Windows.Forms.ToolStripButton _tsRestart;
        private System.Windows.Forms.ToolStripButton _tsStart;
        private DevComponents.DotNetBar.BalloonTip _balloonTip;
        private System.Windows.Forms.LinkLabel _ctlModibyName;
        private System.Windows.Forms.LinkLabel _ctlRefresh;




    }
}


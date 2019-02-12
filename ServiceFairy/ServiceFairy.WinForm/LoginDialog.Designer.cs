namespace ServiceFairy.WinForm
{
    partial class LoginDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginDialog));
            this.label1 = new System.Windows.Forms.Label();
            this._ctOption = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this._ctlNavigation = new System.Windows.Forms.ComboBox();
            this._ctlUserName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this._ctlPassword = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this._msContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._tsCommunicationType = new System.Windows.Forms.ToolStripMenuItem();
            this.自动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tCPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hTTPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._tsDataFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.自动ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.jSONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.二进制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this._tsProxy = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this._msContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(131, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2147483551;
            this.label1.Text = "导　航：";
            // 
            // _ctOption
            // 
            this._ctOption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._ctOption.Location = new System.Drawing.Point(12, 164);
            this._ctOption.Name = "_ctOption";
            this._ctOption.Size = new System.Drawing.Size(77, 23);
            this._ctOption.TabIndex = 3;
            this._ctOption.Text = "选项 ...";
            this._ctOption.UseVisualStyleBackColor = true;
            this._ctOption.Click += new System.EventHandler(this._ctOption_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(131, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2147483551;
            this.label2.Text = "用户名：";
            // 
            // _ctlNavigation
            // 
            this._ctlNavigation.FormattingEnabled = true;
            this._ctlNavigation.Location = new System.Drawing.Point(204, 37);
            this._ctlNavigation.Name = "_ctlNavigation";
            this._ctlNavigation.Size = new System.Drawing.Size(205, 20);
            this._ctlNavigation.TabIndex = 0;
            // 
            // _ctlUserName
            // 
            this._ctlUserName.Location = new System.Drawing.Point(204, 69);
            this._ctlUserName.Name = "_ctlUserName";
            this._ctlUserName.Size = new System.Drawing.Size(205, 21);
            this._ctlUserName.TabIndex = 1;
            this._ctlUserName.Text = "su";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(131, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 2147483551;
            this.label3.Text = "密　码：";
            // 
            // _ctlPassword
            // 
            this._ctlPassword.Location = new System.Drawing.Point(204, 101);
            this._ctlPassword.Name = "_ctlPassword";
            this._ctlPassword.PasswordChar = '*';
            this._ctlPassword.Size = new System.Drawing.Size(205, 21);
            this._ctlPassword.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 94);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2147483555;
            this.pictureBox1.TabStop = false;
            // 
            // _msContextMenu
            // 
            this._msContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._tsCommunicationType,
            this._tsDataFormat,
            this.toolStripMenuItem3,
            this._tsProxy});
            this._msContextMenu.Name = "_msContextMenu";
            this._msContextMenu.Size = new System.Drawing.Size(125, 76);
            // 
            // _tsCommunicationType
            // 
            this._tsCommunicationType.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.自动ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.tCPToolStripMenuItem,
            this.hTTPToolStripMenuItem});
            this._tsCommunicationType.Name = "_tsCommunicationType";
            this._tsCommunicationType.Size = new System.Drawing.Size(124, 22);
            this._tsCommunicationType.Text = "网络协议";
            // 
            // 自动ToolStripMenuItem
            // 
            this.自动ToolStripMenuItem.Checked = true;
            this.自动ToolStripMenuItem.CheckOnClick = true;
            this.自动ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.自动ToolStripMenuItem.Name = "自动ToolStripMenuItem";
            this.自动ToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.自动ToolStripMenuItem.Tag = "PROTOCAL:UNKNOWN";
            this.自动ToolStripMenuItem.Text = "自动";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(103, 6);
            // 
            // tCPToolStripMenuItem
            // 
            this.tCPToolStripMenuItem.CheckOnClick = true;
            this.tCPToolStripMenuItem.Name = "tCPToolStripMenuItem";
            this.tCPToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.tCPToolStripMenuItem.Tag = "PROTOCAL:TCP";
            this.tCPToolStripMenuItem.Text = "TCP";
            // 
            // hTTPToolStripMenuItem
            // 
            this.hTTPToolStripMenuItem.CheckOnClick = true;
            this.hTTPToolStripMenuItem.Name = "hTTPToolStripMenuItem";
            this.hTTPToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.hTTPToolStripMenuItem.Tag = "PROTOCAL:HTTP";
            this.hTTPToolStripMenuItem.Text = "HTTP";
            // 
            // _tsDataFormat
            // 
            this._tsDataFormat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.自动ToolStripMenuItem1,
            this.toolStripMenuItem2,
            this.jSONToolStripMenuItem,
            this.二进制ToolStripMenuItem});
            this._tsDataFormat.Name = "_tsDataFormat";
            this._tsDataFormat.Size = new System.Drawing.Size(124, 22);
            this._tsDataFormat.Text = "数据格式";
            // 
            // 自动ToolStripMenuItem1
            // 
            this.自动ToolStripMenuItem1.Checked = true;
            this.自动ToolStripMenuItem1.CheckOnClick = true;
            this.自动ToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.自动ToolStripMenuItem1.Name = "自动ToolStripMenuItem1";
            this.自动ToolStripMenuItem1.Size = new System.Drawing.Size(112, 22);
            this.自动ToolStripMenuItem1.Tag = "DATA_FORMAT:UNKNOWN";
            this.自动ToolStripMenuItem1.Text = "自动";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(109, 6);
            // 
            // jSONToolStripMenuItem
            // 
            this.jSONToolStripMenuItem.CheckOnClick = true;
            this.jSONToolStripMenuItem.Name = "jSONToolStripMenuItem";
            this.jSONToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.jSONToolStripMenuItem.Tag = "DATA_FORMAT:JSON";
            this.jSONToolStripMenuItem.Text = "JSON";
            // 
            // 二进制ToolStripMenuItem
            // 
            this.二进制ToolStripMenuItem.CheckOnClick = true;
            this.二进制ToolStripMenuItem.Name = "二进制ToolStripMenuItem";
            this.二进制ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.二进制ToolStripMenuItem.Tag = "DATA_FORMAT:BINARY";
            this.二进制ToolStripMenuItem.Text = "二进制";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(121, 6);
            // 
            // _tsProxy
            // 
            this._tsProxy.Name = "_tsProxy";
            this._tsProxy.Size = new System.Drawing.Size(124, 22);
            this._tsProxy.Text = "代理选择";
            // 
            // LoginDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 199);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._ctOption);
            this.Controls.Add(this._ctlUserName);
            this.Controls.Add(this._ctlNavigation);
            this.Controls.Add(this._ctlPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoginDialog";
            this.ShowIcon = true;
            this.ShowInTaskbar = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Service Fairy Management";
            this.Load += new System.EventHandler(this.InputNavigationAddressDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this._msContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _ctOption;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox _ctlNavigation;
        private System.Windows.Forms.TextBox _ctlUserName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _ctlPassword;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ContextMenuStrip _msContextMenu;
        private System.Windows.Forms.ToolStripMenuItem _tsCommunicationType;
        private System.Windows.Forms.ToolStripMenuItem tCPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hTTPToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 自动ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _tsDataFormat;
        private System.Windows.Forms.ToolStripMenuItem jSONToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 二进制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 自动ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem _tsProxy;
    }
}
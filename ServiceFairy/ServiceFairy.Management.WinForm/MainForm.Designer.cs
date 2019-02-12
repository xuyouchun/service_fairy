namespace ServiceFairy.Management.WinForm
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
            WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin5 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
            WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin5 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient13 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient29 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin5 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient5 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient30 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient14 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient31 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient5 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient32 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient33 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient15 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient34 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient35 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this._tsConnectionStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this._mainDockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this._msMenu = new System.Windows.Forms.MenuStrip();
            this.服务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._tsConnect = new System.Windows.Forms.ToolStripMenuItem();
            this._tsDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this._tsExit = new System.Windows.Forms.ToolStripMenuItem();
            this._tsService = new System.Windows.Forms.ToolStripMenuItem();
            this.窗口ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._tsAbout = new System.Windows.Forms.ToolStripMenuItem();
            this._tsCurrent = new System.Windows.Forms.ToolStripMenuItem();
            this._msPath = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this._msMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this._mainDockPanel);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(814, 418);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(814, 467);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this._msMenu);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this._tsConnectionStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(814, 22);
            this.statusStrip1.TabIndex = 0;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // _tsConnectionStatus
            // 
            this._tsConnectionStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._tsConnectionStatus.Image = ((System.Drawing.Image)(resources.GetObject("_tsConnectionStatus.Image")));
            this._tsConnectionStatus.Name = "_tsConnectionStatus";
            this._tsConnectionStatus.Size = new System.Drawing.Size(16, 17);
            // 
            // _mainDockPanel
            // 
            this._mainDockPanel.ActiveAutoHideContent = null;
            this._mainDockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._mainDockPanel.DockBackColor = System.Drawing.SystemColors.Control;
            this._mainDockPanel.DockBottomPortion = 200D;
            this._mainDockPanel.DockLeftPortion = 200D;
            this._mainDockPanel.DockRightPortion = 200D;
            this._mainDockPanel.DockTopPortion = 200D;
            this._mainDockPanel.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this._mainDockPanel.Location = new System.Drawing.Point(0, 0);
            this._mainDockPanel.Name = "_mainDockPanel";
            this._mainDockPanel.Size = new System.Drawing.Size(814, 418);
            dockPanelGradient13.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient13.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin5.DockStripGradient = dockPanelGradient13;
            tabGradient29.EndColor = System.Drawing.SystemColors.Control;
            tabGradient29.StartColor = System.Drawing.SystemColors.Control;
            tabGradient29.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin5.TabGradient = tabGradient29;
            autoHideStripSkin5.TextFont = new System.Drawing.Font("Microsoft YaHei", 9F);
            dockPanelSkin5.AutoHideStripSkin = autoHideStripSkin5;
            tabGradient30.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient30.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient30.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient5.ActiveTabGradient = tabGradient30;
            dockPanelGradient14.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient14.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient5.DockStripGradient = dockPanelGradient14;
            tabGradient31.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient31.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient31.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient5.InactiveTabGradient = tabGradient31;
            dockPaneStripSkin5.DocumentGradient = dockPaneStripGradient5;
            dockPaneStripSkin5.TextFont = new System.Drawing.Font("Microsoft YaHei", 9F);
            tabGradient32.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient32.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient32.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient32.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient5.ActiveCaptionGradient = tabGradient32;
            tabGradient33.EndColor = System.Drawing.SystemColors.Control;
            tabGradient33.StartColor = System.Drawing.SystemColors.Control;
            tabGradient33.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient5.ActiveTabGradient = tabGradient33;
            dockPanelGradient15.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient15.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient5.DockStripGradient = dockPanelGradient15;
            tabGradient34.EndColor = System.Drawing.SystemColors.InactiveCaption;
            tabGradient34.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient34.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient34.TextColor = System.Drawing.SystemColors.InactiveCaptionText;
            dockPaneStripToolWindowGradient5.InactiveCaptionGradient = tabGradient34;
            tabGradient35.EndColor = System.Drawing.Color.Transparent;
            tabGradient35.StartColor = System.Drawing.Color.Transparent;
            tabGradient35.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient5.InactiveTabGradient = tabGradient35;
            dockPaneStripSkin5.ToolWindowGradient = dockPaneStripToolWindowGradient5;
            dockPanelSkin5.DockPaneStripSkin = dockPaneStripSkin5;
            this._mainDockPanel.Skin = dockPanelSkin5;
            this._mainDockPanel.TabIndex = 1;
            // 
            // _msMenu
            // 
            this._msMenu.Dock = System.Windows.Forms.DockStyle.None;
            this._msMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.服务ToolStripMenuItem,
            this._tsService,
            this.窗口ToolStripMenuItem,
            this.帮助ToolStripMenuItem,
            this._tsCurrent,
            this._msPath});
            this._msMenu.Location = new System.Drawing.Point(0, 0);
            this._msMenu.Name = "_msMenu";
            this._msMenu.Size = new System.Drawing.Size(814, 27);
            this._msMenu.TabIndex = 0;
            this._msMenu.Text = "menuStrip1";
            this._msMenu.Resize += new System.EventHandler(this._msMenu_Resize);
            // 
            // 服务ToolStripMenuItem
            // 
            this.服务ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._tsConnect,
            this._tsDisconnect,
            this.toolStripMenuItem1,
            this._tsExit});
            this.服务ToolStripMenuItem.Name = "服务ToolStripMenuItem";
            this.服务ToolStripMenuItem.Size = new System.Drawing.Size(59, 23);
            this.服务ToolStripMenuItem.Text = "连接(&S)";
            // 
            // _tsConnect
            // 
            this._tsConnect.Name = "_tsConnect";
            this._tsConnect.Size = new System.Drawing.Size(129, 22);
            this._tsConnect.Tag = "Connect";
            this._tsConnect.Text = "连接(&C) ...";
            // 
            // _tsDisconnect
            // 
            this._tsDisconnect.Name = "_tsDisconnect";
            this._tsDisconnect.Size = new System.Drawing.Size(129, 22);
            this._tsDisconnect.Tag = "Disconnect";
            this._tsDisconnect.Text = "断开(&D)";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(126, 6);
            // 
            // _tsExit
            // 
            this._tsExit.Name = "_tsExit";
            this._tsExit.Size = new System.Drawing.Size(129, 22);
            this._tsExit.Tag = "Exit";
            this._tsExit.Text = "退出(&X)";
            // 
            // _tsService
            // 
            this._tsService.Name = "_tsService";
            this._tsService.Size = new System.Drawing.Size(59, 23);
            this._tsService.Text = "服务(&S)";
            // 
            // 窗口ToolStripMenuItem
            // 
            this.窗口ToolStripMenuItem.Name = "窗口ToolStripMenuItem";
            this.窗口ToolStripMenuItem.Size = new System.Drawing.Size(64, 23);
            this.窗口ToolStripMenuItem.Text = "窗口(&W)";
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._tsAbout});
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(61, 23);
            this.帮助ToolStripMenuItem.Text = "帮助(&H)";
            // 
            // _tsAbout
            // 
            this._tsAbout.Name = "_tsAbout";
            this._tsAbout.Size = new System.Drawing.Size(116, 22);
            this._tsAbout.Text = "关于(&A)";
            // 
            // _tsCurrent
            // 
            this._tsCurrent.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._tsCurrent.Name = "_tsCurrent";
            this._tsCurrent.Size = new System.Drawing.Size(84, 23);
            this._tsCurrent.Text = "活动窗口(&A)";
            this._tsCurrent.Visible = false;
            // 
            // _msPath
            // 
            this._msPath.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._msPath.AutoSize = false;
            this._msPath.Name = "_msPath";
            this._msPath.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this._msPath.ReadOnly = true;
            this._msPath.Size = new System.Drawing.Size(450, 23);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 467);
            this.Controls.Add(this.toolStripContainer1);
            this.DoubleBuffered = true;
            this.FormShown = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this._msMenu;
            this.MinimumSize = new System.Drawing.Size(830, 505);
            this.Name = "MainForm";
            this.SetCurSizeAsMinSize = true;
            this.Text = "ServiceFairy Management";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this._msMenu.ResumeLayout(false);
            this._msMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip _msMenu;
        private System.Windows.Forms.ToolStripMenuItem 服务ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _tsExit;
        private System.Windows.Forms.ToolStripMenuItem 窗口ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _tsAbout;
        private System.Windows.Forms.ToolStripMenuItem _tsConnect;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem _tsDisconnect;
        private WeifenLuo.WinFormsUI.Docking.DockPanel _mainDockPanel;
        private System.Windows.Forms.ToolStripMenuItem _tsService;
        private System.Windows.Forms.ToolStripMenuItem _tsCurrent;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel _tsConnectionStatus;
        private System.Windows.Forms.ToolStripTextBox _msPath;



    }
}


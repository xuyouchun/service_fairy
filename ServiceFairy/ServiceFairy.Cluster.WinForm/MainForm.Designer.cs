namespace ServiceFairy.Cluster.WinForm
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
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this._tsStatus = new System.Windows.Forms.StatusStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._xtvClientList = new Common.WinForm.XTreeView();
            this._panelViewer = new System.Windows.Forms.Panel();
            this._msMenu = new System.Windows.Forms.MenuStrip();
            this.终端ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._newClientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.删除DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.全部启动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全部停止ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.随机启动停止ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.CLIENT_START_N = new System.Windows.Forms.ToolStripMenuItem();
            this.停止N个ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this._msMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this._tsStatus);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(755, 382);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(755, 429);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this._msMenu);
            // 
            // _tsStatus
            // 
            this._tsStatus.Dock = System.Windows.Forms.DockStyle.None;
            this._tsStatus.Location = new System.Drawing.Point(0, 0);
            this._tsStatus.Name = "_tsStatus";
            this._tsStatus.Size = new System.Drawing.Size(755, 22);
            this._tsStatus.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._xtvClientList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._panelViewer);
            this.splitContainer1.Size = new System.Drawing.Size(755, 382);
            this.splitContainer1.SplitterDistance = 250;
            this.splitContainer1.TabIndex = 0;
            // 
            // _xtvClientList
            // 
            this._xtvClientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._xtvClientList.HideSelection = false;
            this._xtvClientList.HotTracking = true;
            this._xtvClientList.ItemHeight = 20;
            this._xtvClientList.Location = new System.Drawing.Point(0, 0);
            this._xtvClientList.Name = "_xtvClientList";
            this._xtvClientList.ShowRootLines = false;
            this._xtvClientList.Size = new System.Drawing.Size(250, 382);
            this._xtvClientList.TabIndex = 0;
            // 
            // _panelViewer
            // 
            this._panelViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panelViewer.Location = new System.Drawing.Point(0, 0);
            this._panelViewer.Name = "_panelViewer";
            this._panelViewer.Size = new System.Drawing.Size(501, 382);
            this._panelViewer.TabIndex = 0;
            // 
            // _msMenu
            // 
            this._msMenu.Dock = System.Windows.Forms.DockStyle.None;
            this._msMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.终端ToolStripMenuItem});
            this._msMenu.Location = new System.Drawing.Point(0, 0);
            this._msMenu.Name = "_msMenu";
            this._msMenu.Size = new System.Drawing.Size(755, 25);
            this._msMenu.TabIndex = 0;
            this._msMenu.Text = "menuStrip1";
            // 
            // 终端ToolStripMenuItem
            // 
            this.终端ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._newClientToolStripMenuItem,
            this.toolStripMenuItem1,
            this.删除DToolStripMenuItem,
            this.toolStripMenuItem2,
            this.全部启动ToolStripMenuItem,
            this.全部停止ToolStripMenuItem,
            this.toolStripMenuItem3,
            this.随机启动停止ToolStripMenuItem,
            this.toolStripMenuItem4,
            this.CLIENT_START_N,
            this.停止N个ToolStripMenuItem});
            this.终端ToolStripMenuItem.Name = "终端ToolStripMenuItem";
            this.终端ToolStripMenuItem.Size = new System.Drawing.Size(59, 21);
            this.终端ToolStripMenuItem.Text = "终端(&E)";
            // 
            // _newClientToolStripMenuItem
            // 
            this._newClientToolStripMenuItem.Name = "_newClientToolStripMenuItem";
            this._newClientToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this._newClientToolStripMenuItem.Tag = "CLIENT_NEW";
            this._newClientToolStripMenuItem.Text = "新建(&N) ...";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(150, 6);
            // 
            // 删除DToolStripMenuItem
            // 
            this.删除DToolStripMenuItem.Name = "删除DToolStripMenuItem";
            this.删除DToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.删除DToolStripMenuItem.Tag = "CLIENT_DELETE";
            this.删除DToolStripMenuItem.Text = "删除(&D) ...";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(150, 6);
            // 
            // 全部启动ToolStripMenuItem
            // 
            this.全部启动ToolStripMenuItem.Name = "全部启动ToolStripMenuItem";
            this.全部启动ToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.全部启动ToolStripMenuItem.Tag = "CLIENT_STARTALL";
            this.全部启动ToolStripMenuItem.Text = "全部启动";
            // 
            // 全部停止ToolStripMenuItem
            // 
            this.全部停止ToolStripMenuItem.Name = "全部停止ToolStripMenuItem";
            this.全部停止ToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.全部停止ToolStripMenuItem.Tag = "CLIENT_STOPALL";
            this.全部停止ToolStripMenuItem.Text = "全部停止";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(150, 6);
            // 
            // 随机启动停止ToolStripMenuItem
            // 
            this.随机启动停止ToolStripMenuItem.CheckOnClick = true;
            this.随机启动停止ToolStripMenuItem.Name = "随机启动停止ToolStripMenuItem";
            this.随机启动停止ToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.随机启动停止ToolStripMenuItem.Tag = "CLIENT_RANDOM_START_STOP";
            this.随机启动停止ToolStripMenuItem.Text = "随机启动/停止";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(150, 6);
            // 
            // CLIENT_START_N
            // 
            this.CLIENT_START_N.Name = "CLIENT_START_N";
            this.CLIENT_START_N.Size = new System.Drawing.Size(153, 22);
            this.CLIENT_START_N.Tag = "CLIENT_START_N";
            this.CLIENT_START_N.Text = "启动N个 ...";
            // 
            // 停止N个ToolStripMenuItem
            // 
            this.停止N个ToolStripMenuItem.Name = "停止N个ToolStripMenuItem";
            this.停止N个ToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.停止N个ToolStripMenuItem.Tag = "CLIENT_STOP_N";
            this.停止N个ToolStripMenuItem.Text = "停止N个 ...";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 429);
            this.Controls.Add(this.toolStripContainer1);
            this.FormShown = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this._msMenu;
            this.Name = "MainForm";
            this.Text = "ServiceFairy Cluster";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this._msMenu.ResumeLayout(false);
            this._msMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.StatusStrip _tsStatus;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.MenuStrip _msMenu;
        private Common.WinForm.XTreeView _xtvClientList;
        private System.Windows.Forms.ToolStripMenuItem 终端ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _newClientToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 删除DToolStripMenuItem;
        private System.Windows.Forms.Panel _panelViewer;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 全部启动ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部停止ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem 随机启动停止ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CLIENT_START_N;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem 停止N个ToolStripMenuItem;
    }
}


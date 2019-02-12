namespace Common.WinForm.Docking.DockingWindows
{
    partial class ListViewDockingWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListViewDockingWindow));
            this._imgLarge = new System.Windows.Forms.ImageList(this.components);
            this._imgSmall = new System.Windows.Forms.ImageList(this.components);
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this._listView = new Common.WinForm.Docking.DockingWindows.MyListView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._tsViewType = new System.Windows.Forms.ToolStripDropDownButton();
            this.大图标ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.小图标ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.平铺ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.详细信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._tsBack = new System.Windows.Forms.ToolStripSplitButton();
            this._tsForward = new System.Windows.Forms.ToolStripSplitButton();
            this._tsUp = new System.Windows.Forms.ToolStripButton();
            this._tsRefresh = new System.Windows.Forms.ToolStripSplitButton();
            this._pre2SecondsRefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._tsAutoRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this._pre10SecondsRefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._pre30SecondsRefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._pre1MinutesRefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._pre5MinutesRefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this._closeAutoRefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._tsFirstPage = new System.Windows.Forms.ToolStripButton();
            this._tsPrePage = new System.Windows.Forms.ToolStripButton();
            this._tsPageNumber = new System.Windows.Forms.ToolStripComboBox();
            this._tsNextPage = new System.Windows.Forms.ToolStripButton();
            this._tsLastPage = new System.Windows.Forms.ToolStripButton();
            this._timer = new System.Windows.Forms.Timer(this.components);
            this._pre1SecondsRefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _imgLarge
            // 
            this._imgLarge.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this._imgLarge.ImageSize = new System.Drawing.Size(48, 48);
            this._imgLarge.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // _imgSmall
            // 
            this._imgSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this._imgSmall.ImageSize = new System.Drawing.Size(24, 24);
            this._imgSmall.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this._listView);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(645, 372);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(645, 397);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // _listView
            // 
            this._listView.AllowColumnReorder = true;
            this._listView.AllowDrop = true;
            this._listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._listView.FullRowSelect = true;
            this._listView.GridLines = true;
            this._listView.LargeImageList = this._imgLarge;
            this._listView.Location = new System.Drawing.Point(0, 0);
            this._listView.MultiSelect = false;
            this._listView.Name = "_listView";
            this._listView.OwnerDraw = true;
            this._listView.ShowItemToolTips = true;
            this._listView.Size = new System.Drawing.Size(645, 372);
            this._listView.SmallImageList = this._imgSmall;
            this._listView.TabIndex = 0;
            this._listView.UseCompatibleStateImageBehavior = false;
            this._listView.View = System.Windows.Forms.View.Details;
            this._listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this._listView_ColumnClick);
            this._listView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this._listView_ItemSelectionChanged);
            this._listView.KeyUp += new System.Windows.Forms.KeyEventHandler(this._listView_KeyUp);
            this._listView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this._listView_MouseDoubleClick);
            this._listView.MouseUp += new System.Windows.Forms.MouseEventHandler(this._listView_MouseUp);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._tsViewType,
            this._tsBack,
            this._tsForward,
            this._tsUp,
            this._tsRefresh,
            this.toolStripSeparator1,
            this._tsFirstPage,
            this._tsPrePage,
            this._tsPageNumber,
            this._tsNextPage,
            this._tsLastPage});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(645, 25);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 0;
            // 
            // _tsViewType
            // 
            this._tsViewType.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._tsViewType.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.大图标ToolStripMenuItem,
            this.小图标ToolStripMenuItem,
            this.平铺ToolStripMenuItem,
            this.列表ToolStripMenuItem,
            this.详细信息ToolStripMenuItem});
            this._tsViewType.Image = ((System.Drawing.Image)(resources.GetObject("_tsViewType.Image")));
            this._tsViewType.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tsViewType.Name = "_tsViewType";
            this._tsViewType.Size = new System.Drawing.Size(85, 22);
            this._tsViewType.Text = "显示方式";
            this._tsViewType.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this._tsViewType_DropDownItemClicked);
            // 
            // 大图标ToolStripMenuItem
            // 
            this.大图标ToolStripMenuItem.Name = "大图标ToolStripMenuItem";
            this.大图标ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.大图标ToolStripMenuItem.Tag = "LargeIcon";
            this.大图标ToolStripMenuItem.Text = "大图标";
            // 
            // 小图标ToolStripMenuItem
            // 
            this.小图标ToolStripMenuItem.Name = "小图标ToolStripMenuItem";
            this.小图标ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.小图标ToolStripMenuItem.Tag = "SmallIcon";
            this.小图标ToolStripMenuItem.Text = "小图标";
            // 
            // 平铺ToolStripMenuItem
            // 
            this.平铺ToolStripMenuItem.Name = "平铺ToolStripMenuItem";
            this.平铺ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.平铺ToolStripMenuItem.Tag = "Tile";
            this.平铺ToolStripMenuItem.Text = "平铺";
            // 
            // 列表ToolStripMenuItem
            // 
            this.列表ToolStripMenuItem.Name = "列表ToolStripMenuItem";
            this.列表ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.列表ToolStripMenuItem.Tag = "List";
            this.列表ToolStripMenuItem.Text = "列表";
            // 
            // 详细信息ToolStripMenuItem
            // 
            this.详细信息ToolStripMenuItem.Name = "详细信息ToolStripMenuItem";
            this.详细信息ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.详细信息ToolStripMenuItem.Tag = "Details";
            this.详细信息ToolStripMenuItem.Text = "详细信息";
            // 
            // _tsBack
            // 
            this._tsBack.Image = ((System.Drawing.Image)(resources.GetObject("_tsBack.Image")));
            this._tsBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tsBack.Name = "_tsBack";
            this._tsBack.Size = new System.Drawing.Size(64, 22);
            this._tsBack.Text = "后退";
            this._tsBack.ButtonClick += new System.EventHandler(this._tsBack_Click);
            this._tsBack.DropDownOpening += new System.EventHandler(this._tsBack_DropDownOpening);
            // 
            // _tsForward
            // 
            this._tsForward.Image = ((System.Drawing.Image)(resources.GetObject("_tsForward.Image")));
            this._tsForward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tsForward.Name = "_tsForward";
            this._tsForward.Size = new System.Drawing.Size(64, 22);
            this._tsForward.Text = "前进";
            this._tsForward.ButtonClick += new System.EventHandler(this._tsForward_Click);
            this._tsForward.DropDownOpening += new System.EventHandler(this._tsForward_DropDownOpening);
            // 
            // _tsUp
            // 
            this._tsUp.Image = ((System.Drawing.Image)(resources.GetObject("_tsUp.Image")));
            this._tsUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tsUp.Name = "_tsUp";
            this._tsUp.Size = new System.Drawing.Size(52, 22);
            this._tsUp.Text = "向上";
            this._tsUp.Click += new System.EventHandler(this._tsUp_Click);
            // 
            // _tsRefresh
            // 
            this._tsRefresh.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._pre1SecondsRefreshToolStripMenuItem,
            this._pre2SecondsRefreshToolStripMenuItem,
            this._tsAutoRefresh,
            this.toolStripMenuItem2,
            this._pre10SecondsRefreshToolStripMenuItem,
            this._pre30SecondsRefreshToolStripMenuItem,
            this._pre1MinutesRefreshToolStripMenuItem,
            this._pre5MinutesRefreshToolStripMenuItem,
            this.toolStripMenuItem1,
            this._closeAutoRefreshToolStripMenuItem});
            this._tsRefresh.Image = ((System.Drawing.Image)(resources.GetObject("_tsRefresh.Image")));
            this._tsRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tsRefresh.Name = "_tsRefresh";
            this._tsRefresh.Size = new System.Drawing.Size(64, 22);
            this._tsRefresh.Text = "刷新";
            this._tsRefresh.ButtonClick += new System.EventHandler(this._tsRefresh_ButtonClick);
            // 
            // _pre2SecondsRefreshToolStripMenuItem
            // 
            this._pre2SecondsRefreshToolStripMenuItem.Name = "_pre2SecondsRefreshToolStripMenuItem";
            this._pre2SecondsRefreshToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this._pre2SecondsRefreshToolStripMenuItem.Tag = "AutoRefresh_2";
            this._pre2SecondsRefreshToolStripMenuItem.Text = "自动刷新－2秒";
            this._pre2SecondsRefreshToolStripMenuItem.Click += new System.EventHandler(this._tsAutoRefresh_Click);
            // 
            // _tsAutoRefresh
            // 
            this._tsAutoRefresh.Name = "_tsAutoRefresh";
            this._tsAutoRefresh.Size = new System.Drawing.Size(167, 22);
            this._tsAutoRefresh.Tag = "AutoRefresh_5";
            this._tsAutoRefresh.Text = "自动刷新 - 5秒";
            this._tsAutoRefresh.Click += new System.EventHandler(this._tsAutoRefresh_Click);
            // 
            // _pre10SecondsRefreshToolStripMenuItem
            // 
            this._pre10SecondsRefreshToolStripMenuItem.Name = "_pre10SecondsRefreshToolStripMenuItem";
            this._pre10SecondsRefreshToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this._pre10SecondsRefreshToolStripMenuItem.Tag = "AutoRefresh_10";
            this._pre10SecondsRefreshToolStripMenuItem.Text = "自动刷新 - 10秒";
            this._pre10SecondsRefreshToolStripMenuItem.Click += new System.EventHandler(this._tsAutoRefresh_Click);
            // 
            // _pre30SecondsRefreshToolStripMenuItem
            // 
            this._pre30SecondsRefreshToolStripMenuItem.Name = "_pre30SecondsRefreshToolStripMenuItem";
            this._pre30SecondsRefreshToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this._pre30SecondsRefreshToolStripMenuItem.Tag = "AutoRefresh_30";
            this._pre30SecondsRefreshToolStripMenuItem.Text = "自动刷新 - 30秒";
            this._pre30SecondsRefreshToolStripMenuItem.Click += new System.EventHandler(this._tsAutoRefresh_Click);
            // 
            // _pre1MinutesRefreshToolStripMenuItem
            // 
            this._pre1MinutesRefreshToolStripMenuItem.Name = "_pre1MinutesRefreshToolStripMenuItem";
            this._pre1MinutesRefreshToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this._pre1MinutesRefreshToolStripMenuItem.Tag = "AutoRefresh_60";
            this._pre1MinutesRefreshToolStripMenuItem.Text = "自动刷新 - 60秒";
            this._pre1MinutesRefreshToolStripMenuItem.Click += new System.EventHandler(this._tsAutoRefresh_Click);
            // 
            // _pre5MinutesRefreshToolStripMenuItem
            // 
            this._pre5MinutesRefreshToolStripMenuItem.Name = "_pre5MinutesRefreshToolStripMenuItem";
            this._pre5MinutesRefreshToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this._pre5MinutesRefreshToolStripMenuItem.Tag = "AutoRefresh_300";
            this._pre5MinutesRefreshToolStripMenuItem.Text = "自动刷新－5分钟";
            this._pre5MinutesRefreshToolStripMenuItem.Click += new System.EventHandler(this._tsAutoRefresh_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(164, 6);
            // 
            // _closeAutoRefreshToolStripMenuItem
            // 
            this._closeAutoRefreshToolStripMenuItem.Name = "_closeAutoRefreshToolStripMenuItem";
            this._closeAutoRefreshToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this._closeAutoRefreshToolStripMenuItem.Tag = "AutoRefresh_0";
            this._closeAutoRefreshToolStripMenuItem.Text = "关闭自动刷新";
            this._closeAutoRefreshToolStripMenuItem.Click += new System.EventHandler(this._tsAutoRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // _tsFirstPage
            // 
            this._tsFirstPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._tsFirstPage.Image = ((System.Drawing.Image)(resources.GetObject("_tsFirstPage.Image")));
            this._tsFirstPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tsFirstPage.Name = "_tsFirstPage";
            this._tsFirstPage.Size = new System.Drawing.Size(23, 22);
            this._tsFirstPage.Text = "第一页";
            this._tsFirstPage.Click += new System.EventHandler(this._tsNavigation_Click);
            // 
            // _tsPrePage
            // 
            this._tsPrePage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._tsPrePage.Image = ((System.Drawing.Image)(resources.GetObject("_tsPrePage.Image")));
            this._tsPrePage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tsPrePage.Name = "_tsPrePage";
            this._tsPrePage.Size = new System.Drawing.Size(23, 22);
            this._tsPrePage.Text = "上一页";
            this._tsPrePage.Click += new System.EventHandler(this._tsNavigation_Click);
            // 
            // _tsPageNumber
            // 
            this._tsPageNumber.Name = "_tsPageNumber";
            this._tsPageNumber.Size = new System.Drawing.Size(75, 25);
            // 
            // _tsNextPage
            // 
            this._tsNextPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._tsNextPage.Image = ((System.Drawing.Image)(resources.GetObject("_tsNextPage.Image")));
            this._tsNextPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tsNextPage.Name = "_tsNextPage";
            this._tsNextPage.Size = new System.Drawing.Size(23, 22);
            this._tsNextPage.Text = "下一页";
            this._tsNextPage.Click += new System.EventHandler(this._tsNavigation_Click);
            // 
            // _tsLastPage
            // 
            this._tsLastPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._tsLastPage.Image = ((System.Drawing.Image)(resources.GetObject("_tsLastPage.Image")));
            this._tsLastPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tsLastPage.Name = "_tsLastPage";
            this._tsLastPage.Size = new System.Drawing.Size(23, 22);
            this._tsLastPage.Text = "最后一页";
            this._tsLastPage.Click += new System.EventHandler(this._tsNavigation_Click);
            // 
            // _timer
            // 
            this._timer.Tick += new System.EventHandler(this._timer_Tick);
            // 
            // _pre1SecondsRefreshToolStripMenuItem
            // 
            this._pre1SecondsRefreshToolStripMenuItem.Name = "_pre1SecondsRefreshToolStripMenuItem";
            this._pre1SecondsRefreshToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this._pre1SecondsRefreshToolStripMenuItem.Tag = "AutoRefresh_1";
            this._pre1SecondsRefreshToolStripMenuItem.Text = "自动刷新－1秒";
            this._pre1SecondsRefreshToolStripMenuItem.Click += new System.EventHandler(this._tsAutoRefresh_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(164, 6);
            // 
            // ListViewDockingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 397);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "ListViewDockingWindow";
            this.Text = "列表";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ListViewDockingWindow_FormClosed);
            this.Load += new System.EventHandler(this.ListViewDockingWindow_Load);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MyListView _listView;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton _tsViewType;
        private System.Windows.Forms.ToolStripMenuItem 大图标ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 小图标ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 详细信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 平铺ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 列表ToolStripMenuItem;
        private System.Windows.Forms.ImageList _imgLarge;
        private System.Windows.Forms.ImageList _imgSmall;
        private System.Windows.Forms.ToolStripSplitButton _tsBack;
        private System.Windows.Forms.ToolStripSplitButton _tsForward;
        private System.Windows.Forms.ToolStripButton _tsUp;
        private System.Windows.Forms.ToolStripSplitButton _tsRefresh;
        private System.Windows.Forms.ToolStripMenuItem _tsAutoRefresh;
        private System.Windows.Forms.Timer _timer;
        private System.Windows.Forms.ToolStripMenuItem _pre10SecondsRefreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _pre30SecondsRefreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _pre1MinutesRefreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem _closeAutoRefreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton _tsFirstPage;
        private System.Windows.Forms.ToolStripButton _tsPrePage;
        private System.Windows.Forms.ToolStripComboBox _tsPageNumber;
        private System.Windows.Forms.ToolStripButton _tsNextPage;
        private System.Windows.Forms.ToolStripButton _tsLastPage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem _pre5MinutesRefreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _pre2SecondsRefreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _pre1SecondsRefreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
    }
}
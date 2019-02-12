namespace ServiceFairy.Client.WinForm
{
    partial class GroupMessageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupMessageForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._msgTextBox = new System.Windows.Forms.RichTextBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this._sendMsgTextBox = new System.Windows.Forms.TextBox();
            this._sendButton = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.navigationPane1 = new DevComponents.DotNetBar.NavigationPane();
            this.navigationPanePanel1 = new DevComponents.DotNetBar.NavigationPanePanel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._addMemberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._removeMemberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this._refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._memberItemPanel = new DevComponents.DotNetBar.ItemPanel();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._clearTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.navigationPane1.SuspendLayout();
            this.navigationPanePanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._msgTextBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(448, 381);
            this.splitContainer1.SplitterDistance = 346;
            this.splitContainer1.TabIndex = 0;
            // 
            // _msgTextBox
            // 
            this._msgTextBox.BackColor = System.Drawing.SystemColors.Window;
            this._msgTextBox.ContextMenuStrip = this.contextMenuStrip2;
            this._msgTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._msgTextBox.Location = new System.Drawing.Point(0, 0);
            this._msgTextBox.Name = "_msgTextBox";
            this._msgTextBox.ReadOnly = true;
            this._msgTextBox.Size = new System.Drawing.Size(448, 346);
            this._msgTextBox.TabIndex = 0;
            this._msgTextBox.Text = "";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this._sendMsgTextBox);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this._sendButton);
            this.splitContainer3.Size = new System.Drawing.Size(448, 31);
            this.splitContainer3.SplitterDistance = 340;
            this.splitContainer3.TabIndex = 0;
            // 
            // _sendMsgTextBox
            // 
            this._sendMsgTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._sendMsgTextBox.Location = new System.Drawing.Point(0, 0);
            this._sendMsgTextBox.Multiline = true;
            this._sendMsgTextBox.Name = "_sendMsgTextBox";
            this._sendMsgTextBox.Size = new System.Drawing.Size(340, 31);
            this._sendMsgTextBox.TabIndex = 0;
            this._sendMsgTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._sendMsgTextBox_KeyDown);
            // 
            // _sendButton
            // 
            this._sendButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this._sendButton.Location = new System.Drawing.Point(0, 0);
            this._sendButton.Name = "_sendButton";
            this._sendButton.Size = new System.Drawing.Size(104, 31);
            this._sendButton.TabIndex = 0;
            this._sendButton.Text = "发送";
            this._sendButton.UseVisualStyleBackColor = true;
            this._sendButton.Click += new System.EventHandler(this._sendButton_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.navigationPane1);
            this.splitContainer2.Size = new System.Drawing.Size(620, 381);
            this.splitContainer2.SplitterDistance = 448;
            this.splitContainer2.TabIndex = 1;
            // 
            // navigationPane1
            // 
            this.navigationPane1.Controls.Add(this.navigationPanePanel1);
            this.navigationPane1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationPane1.ItemPaddingBottom = 2;
            this.navigationPane1.ItemPaddingTop = 2;
            this.navigationPane1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem1});
            this.navigationPane1.Location = new System.Drawing.Point(0, 0);
            this.navigationPane1.Name = "navigationPane1";
            this.navigationPane1.NavigationBarHeight = 67;
            this.navigationPane1.Padding = new System.Windows.Forms.Padding(1);
            this.navigationPane1.Size = new System.Drawing.Size(168, 381);
            this.navigationPane1.TabIndex = 0;
            // 
            // 
            // 
            this.navigationPane1.TitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.navigationPane1.TitlePanel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.navigationPane1.TitlePanel.Location = new System.Drawing.Point(1, 1);
            this.navigationPane1.TitlePanel.Name = "panelTitle";
            this.navigationPane1.TitlePanel.Size = new System.Drawing.Size(166, 24);
            this.navigationPane1.TitlePanel.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.navigationPane1.TitlePanel.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.navigationPane1.TitlePanel.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.navigationPane1.TitlePanel.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.navigationPane1.TitlePanel.Style.BorderSide = DevComponents.DotNetBar.eBorderSide.Bottom;
            this.navigationPane1.TitlePanel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.navigationPane1.TitlePanel.Style.GradientAngle = 90;
            this.navigationPane1.TitlePanel.Style.MarginLeft = 4;
            this.navigationPane1.TitlePanel.TabIndex = 0;
            this.navigationPane1.TitlePanel.Text = "群组成员";
            // 
            // navigationPanePanel1
            // 
            this.navigationPanePanel1.ContextMenuStrip = this.contextMenuStrip1;
            this.navigationPanePanel1.Controls.Add(this._memberItemPanel);
            this.navigationPanePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationPanePanel1.Location = new System.Drawing.Point(1, 25);
            this.navigationPanePanel1.Name = "navigationPanePanel1";
            this.navigationPanePanel1.ParentItem = this.buttonItem1;
            this.navigationPanePanel1.Size = new System.Drawing.Size(166, 288);
            this.navigationPanePanel1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.navigationPanePanel1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.navigationPanePanel1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.navigationPanePanel1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.navigationPanePanel1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.navigationPanePanel1.Style.GradientAngle = 90;
            this.navigationPanePanel1.TabIndex = 2;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._addMemberToolStripMenuItem,
            this._removeMemberToolStripMenuItem,
            this.toolStripMenuItem1,
            this._refreshToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(138, 76);
            // 
            // _addMemberToolStripMenuItem
            // 
            this._addMemberToolStripMenuItem.Name = "_addMemberToolStripMenuItem";
            this._addMemberToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this._addMemberToolStripMenuItem.Text = "添加成员 ...";
            this._addMemberToolStripMenuItem.Click += new System.EventHandler(this._addMemberToolStripMenuItem_Click);
            // 
            // _removeMemberToolStripMenuItem
            // 
            this._removeMemberToolStripMenuItem.Name = "_removeMemberToolStripMenuItem";
            this._removeMemberToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this._removeMemberToolStripMenuItem.Text = "删除成员 ...";
            this._removeMemberToolStripMenuItem.Click += new System.EventHandler(this._removeMemberToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(134, 6);
            // 
            // _refreshToolStripMenuItem
            // 
            this._refreshToolStripMenuItem.Name = "_refreshToolStripMenuItem";
            this._refreshToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this._refreshToolStripMenuItem.Text = "刷新";
            this._refreshToolStripMenuItem.Click += new System.EventHandler(this._refreshToolStripMenuItem_Click);
            // 
            // _memberItemPanel
            // 
            // 
            // 
            // 
            this._memberItemPanel.BackgroundStyle.BackColor = System.Drawing.Color.White;
            this._memberItemPanel.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this._memberItemPanel.BackgroundStyle.BorderBottomWidth = 1;
            this._memberItemPanel.BackgroundStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this._memberItemPanel.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this._memberItemPanel.BackgroundStyle.BorderLeftWidth = 1;
            this._memberItemPanel.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this._memberItemPanel.BackgroundStyle.BorderRightWidth = 1;
            this._memberItemPanel.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this._memberItemPanel.BackgroundStyle.BorderTopWidth = 1;
            this._memberItemPanel.BackgroundStyle.PaddingBottom = 1;
            this._memberItemPanel.BackgroundStyle.PaddingLeft = 1;
            this._memberItemPanel.BackgroundStyle.PaddingRight = 1;
            this._memberItemPanel.BackgroundStyle.PaddingTop = 1;
            this._memberItemPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._memberItemPanel.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this._memberItemPanel.Location = new System.Drawing.Point(0, 0);
            this._memberItemPanel.Name = "_memberItemPanel";
            this._memberItemPanel.Size = new System.Drawing.Size(166, 288);
            this._memberItemPanel.TabIndex = 0;
            this._memberItemPanel.Text = "itemPanel1";
            // 
            // buttonItem1
            // 
            this.buttonItem1.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem1.Checked = true;
            this.buttonItem1.Image = ((System.Drawing.Image)(resources.GetObject("buttonItem1.Image")));
            this.buttonItem1.ImagePaddingHorizontal = 8;
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.OptionGroup = "navBar";
            this.buttonItem1.Text = "群组成员";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._clearTextToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(153, 48);
            // 
            // _clearTextToolStripMenuItem
            // 
            this._clearTextToolStripMenuItem.Name = "_clearTextToolStripMenuItem";
            this._clearTextToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this._clearTextToolStripMenuItem.Text = "清空";
            this._clearTextToolStripMenuItem.Click += new System.EventHandler(this._clearTextToolStripMenuItem_Click);
            // 
            // GroupMessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 381);
            this.Controls.Add(this.splitContainer2);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.HideOnClose = true;
            this.Name = "GroupMessageForm";
            this.Text = "GroupMessageForm";
            this.Load += new System.EventHandler(this.GroupMessageForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.navigationPane1.ResumeLayout(false);
            this.navigationPanePanel1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox _msgTextBox;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private DevComponents.DotNetBar.NavigationPane navigationPane1;
        private DevComponents.DotNetBar.NavigationPanePanel navigationPanePanel1;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem _addMemberToolStripMenuItem;
        private DevComponents.DotNetBar.ItemPanel _memberItemPanel;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem _refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _removeMemberToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Button _sendButton;
        private System.Windows.Forms.TextBox _sendMsgTextBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem _clearTextToolStripMenuItem;
    }
}
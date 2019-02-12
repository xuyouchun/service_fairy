namespace ServiceFairy.WinForm
{
    partial class AppCommandInvokeDialog
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.sideBar1 = new DevComponents.DotNetBar.SideBar();
            this._inputSideBarPanelItem = new DevComponents.DotNetBar.SideBarPanelItem();
            this._inputJsonButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem5 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem6 = new DevComponents.DotNetBar.ButtonItem();
            this._outputSideBarPanelItem = new DevComponents.DotNetBar.SideBarPanelItem();
            this.buttonItem10 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem11 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem12 = new DevComponents.DotNetBar.ButtonItem();
            this._testSideBarPanelItem = new DevComponents.DotNetBar.SideBarPanelItem();
            this.buttonItem13 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem14 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem2 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem3 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem7 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem8 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem9 = new DevComponents.DotNetBar.ButtonItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.sideBar1);
            this.splitContainer1.Size = new System.Drawing.Size(771, 464);
            this.splitContainer1.SplitterDistance = 164;
            this.splitContainer1.TabIndex = 2147483548;
            // 
            // sideBar1
            // 
            this.sideBar1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.sideBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sideBar1.ExpandedPanel = this._inputSideBarPanelItem;
            this.sideBar1.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.sideBar1.Location = new System.Drawing.Point(0, 0);
            this.sideBar1.Name = "sideBar1";
            this.sideBar1.Panels.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this._inputSideBarPanelItem,
            this._outputSideBarPanelItem,
            this._testSideBarPanelItem});
            this.sideBar1.Size = new System.Drawing.Size(164, 464);
            this.sideBar1.TabIndex = 0;
            this.sideBar1.ButtonCheckedChanged += new System.EventHandler(this.sideBar1_ButtonCheckedChanged);
            this.sideBar1.ItemClick += new System.EventHandler(this.sideBar1_ItemClick);
            // 
            // _inputSideBarPanelItem
            // 
            this._inputSideBarPanelItem.Name = "_inputSideBarPanelItem";
            this._inputSideBarPanelItem.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this._inputJsonButtonItem,
            this.buttonItem5,
            this.buttonItem6});
            this._inputSideBarPanelItem.Text = "输入参数";
            // 
            // _inputJsonButtonItem
            // 
            this._inputJsonButtonItem.AutoCheckOnClick = true;
            this._inputJsonButtonItem.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this._inputJsonButtonItem.ForeColor = System.Drawing.Color.Olive;
            this._inputJsonButtonItem.HotFontUnderline = true;
            this._inputJsonButtonItem.HotForeColor = System.Drawing.Color.Silver;
            this._inputJsonButtonItem.ImagePaddingHorizontal = 8;
            this._inputJsonButtonItem.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this._inputJsonButtonItem.Name = "_inputJsonButtonItem";
            this._inputJsonButtonItem.OptionGroup = "DEFAULT";
            this._inputJsonButtonItem.Tag = "INPUT_JSON";
            this._inputJsonButtonItem.Text = "JSON";
            // 
            // buttonItem5
            // 
            this.buttonItem5.AutoCheckOnClick = true;
            this.buttonItem5.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem5.ForeColor = System.Drawing.Color.Olive;
            this.buttonItem5.HotFontUnderline = true;
            this.buttonItem5.ImagePaddingHorizontal = 8;
            this.buttonItem5.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonItem5.Name = "buttonItem5";
            this.buttonItem5.OptionGroup = "DEFAULT";
            this.buttonItem5.Tag = "INPUT_XML";
            this.buttonItem5.Text = "XML";
            // 
            // buttonItem6
            // 
            this.buttonItem6.AutoCheckOnClick = true;
            this.buttonItem6.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem6.ForeColor = System.Drawing.Color.Olive;
            this.buttonItem6.HotFontUnderline = true;
            this.buttonItem6.HotForeColor = System.Drawing.Color.Silver;
            this.buttonItem6.ImagePaddingHorizontal = 8;
            this.buttonItem6.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonItem6.Name = "buttonItem6";
            this.buttonItem6.OptionGroup = "DEFAULT";
            this.buttonItem6.Tag = "INPUT_DOC";
            this.buttonItem6.Text = "文档";
            // 
            // _outputSideBarPanelItem
            // 
            this._outputSideBarPanelItem.Name = "_outputSideBarPanelItem";
            this._outputSideBarPanelItem.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem10,
            this.buttonItem11,
            this.buttonItem12});
            this._outputSideBarPanelItem.Text = "输出参数";
            // 
            // buttonItem10
            // 
            this.buttonItem10.AutoCheckOnClick = true;
            this.buttonItem10.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem10.ForeColor = System.Drawing.Color.Olive;
            this.buttonItem10.HotFontUnderline = true;
            this.buttonItem10.ImagePaddingHorizontal = 8;
            this.buttonItem10.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonItem10.Name = "buttonItem10";
            this.buttonItem10.OptionGroup = "DEFAULT";
            this.buttonItem10.Tag = "OUTPUT_JSON";
            this.buttonItem10.Text = "JSON";
            // 
            // buttonItem11
            // 
            this.buttonItem11.AutoCheckOnClick = true;
            this.buttonItem11.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem11.ForeColor = System.Drawing.Color.Olive;
            this.buttonItem11.HotFontUnderline = true;
            this.buttonItem11.ImagePaddingHorizontal = 8;
            this.buttonItem11.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonItem11.Name = "buttonItem11";
            this.buttonItem11.OptionGroup = "DEFAULT";
            this.buttonItem11.Tag = "OUTPUT_XML";
            this.buttonItem11.Text = "XML";
            // 
            // buttonItem12
            // 
            this.buttonItem12.AutoCheckOnClick = true;
            this.buttonItem12.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem12.ForeColor = System.Drawing.Color.Olive;
            this.buttonItem12.HotFontUnderline = true;
            this.buttonItem12.ImagePaddingHorizontal = 8;
            this.buttonItem12.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonItem12.Name = "buttonItem12";
            this.buttonItem12.OptionGroup = "DEFAULT";
            this.buttonItem12.Tag = "OUTPUT_DOC";
            this.buttonItem12.Text = "文档";
            // 
            // _testSideBarPanelItem
            // 
            this._testSideBarPanelItem.Name = "_testSideBarPanelItem";
            this._testSideBarPanelItem.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem13,
            this.buttonItem14});
            this._testSideBarPanelItem.Text = "接口测试";
            // 
            // buttonItem13
            // 
            this.buttonItem13.AutoCheckOnClick = true;
            this.buttonItem13.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem13.ForeColor = System.Drawing.Color.Olive;
            this.buttonItem13.HotFontUnderline = true;
            this.buttonItem13.ImagePaddingHorizontal = 8;
            this.buttonItem13.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonItem13.Name = "buttonItem13";
            this.buttonItem13.OptionGroup = "DEFAULT";
            this.buttonItem13.Tag = "TEST_INVOKE";
            this.buttonItem13.Text = "调用方式";
            // 
            // buttonItem14
            // 
            this.buttonItem14.AutoCheckOnClick = true;
            this.buttonItem14.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem14.ForeColor = System.Drawing.Color.Olive;
            this.buttonItem14.HotFontUnderline = true;
            this.buttonItem14.ImagePaddingHorizontal = 8;
            this.buttonItem14.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonItem14.Name = "buttonItem14";
            this.buttonItem14.OptionGroup = "DEFAULT";
            this.buttonItem14.Tag = "TEST_REQUEST";
            this.buttonItem14.Text = "模拟请求";
            // 
            // buttonItem1
            // 
            this.buttonItem1.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem1.ImagePaddingHorizontal = 8;
            this.buttonItem1.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.Text = "JSON";
            // 
            // buttonItem2
            // 
            this.buttonItem2.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem2.ImagePaddingHorizontal = 8;
            this.buttonItem2.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonItem2.Name = "buttonItem2";
            this.buttonItem2.Text = "XML";
            // 
            // buttonItem3
            // 
            this.buttonItem3.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem3.ImagePaddingHorizontal = 8;
            this.buttonItem3.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonItem3.Name = "buttonItem3";
            this.buttonItem3.Text = "文档";
            // 
            // buttonItem7
            // 
            this.buttonItem7.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem7.ImagePaddingHorizontal = 8;
            this.buttonItem7.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonItem7.Name = "buttonItem7";
            this.buttonItem7.Text = "JSON";
            // 
            // buttonItem8
            // 
            this.buttonItem8.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem8.ImagePaddingHorizontal = 8;
            this.buttonItem8.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonItem8.Name = "buttonItem8";
            this.buttonItem8.Text = "XML";
            // 
            // buttonItem9
            // 
            this.buttonItem9.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem9.ImagePaddingHorizontal = 8;
            this.buttonItem9.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonItem9.Name = "buttonItem9";
            this.buttonItem9.Text = "文档";
            // 
            // AppCommandInvokeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ButtonModel = Common.WinForm.XDialogButtonModel.Close;
            this.ClientSize = new System.Drawing.Size(795, 519);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.KeyPreview = true;
            this.MaximizeBox = true;
            this.Name = "AppCommandInvokeDialog";
            this.Text = "接口测试";
            this.Load += new System.EventHandler(this.AppCommandInvokeDialog_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AppCommandInvokeDialog_KeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevComponents.DotNetBar.SideBar sideBar1;
        private DevComponents.DotNetBar.SideBarPanelItem _inputSideBarPanelItem;
        private DevComponents.DotNetBar.SideBarPanelItem _outputSideBarPanelItem;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.ButtonItem buttonItem2;
        private DevComponents.DotNetBar.ButtonItem buttonItem3;
        private DevComponents.DotNetBar.ButtonItem _inputJsonButtonItem;
        private DevComponents.DotNetBar.ButtonItem buttonItem5;
        private DevComponents.DotNetBar.ButtonItem buttonItem6;
        private DevComponents.DotNetBar.ButtonItem buttonItem7;
        private DevComponents.DotNetBar.ButtonItem buttonItem8;
        private DevComponents.DotNetBar.ButtonItem buttonItem9;
        private DevComponents.DotNetBar.SideBarPanelItem _testSideBarPanelItem;
        private DevComponents.DotNetBar.ButtonItem buttonItem10;
        private DevComponents.DotNetBar.ButtonItem buttonItem11;
        private DevComponents.DotNetBar.ButtonItem buttonItem12;
        private DevComponents.DotNetBar.ButtonItem buttonItem13;
        private DevComponents.DotNetBar.ButtonItem buttonItem14;
        private System.Windows.Forms.ToolTip toolTip1;




    }
}
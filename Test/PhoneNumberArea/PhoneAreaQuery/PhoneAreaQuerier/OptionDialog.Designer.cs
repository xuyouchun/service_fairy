namespace PhoneAreaQuerier
{
    partial class OptionDialog
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
            this.tabControl1 = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this._country = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this._lbNationalCode = new DevComponents.DotNetBar.LabelX();
            this._lbEnName = new DevComponents.DotNetBar.LabelX();
            this.lb7 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this._lbIsoCode = new DevComponents.DotNetBar.LabelX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this._lbMcc = new DevComponents.DotNetBar.LabelX();
            this.label6 = new DevComponents.DotNetBar.LabelX();
            this._lbCnName = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.tabItem1 = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabItem2 = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel2 = new DevComponents.DotNetBar.TabControlPanel();
            this._operationList = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this._lbState = new DevComponents.DotNetBar.LabelX();
            this._lbBrand = new DevComponents.DotNetBar.LabelX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this._lbRemarks = new DevComponents.DotNetBar.LabelX();
            this.labelX11 = new DevComponents.DotNetBar.LabelX();
            this._lbMnc = new DevComponents.DotNetBar.LabelX();
            this.labelX13 = new DevComponents.DotNetBar.LabelX();
            this._lbOpName = new DevComponents.DotNetBar.LabelX();
            this.labelX15 = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabControlPanel1.SuspendLayout();
            this.tabControlPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.CanReorderTabs = true;
            this.tabControl1.Controls.Add(this.tabControlPanel2);
            this.tabControl1.Controls.Add(this.tabControlPanel1);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedTabFont = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold);
            this.tabControl1.SelectedTabIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(541, 226);
            this.tabControl1.TabIndex = 2147483549;
            this.tabControl1.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControl1.Tabs.Add(this.tabItem1);
            this.tabControl1.Tabs.Add(this.tabItem2);
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Controls.Add(this._country);
            this.tabControlPanel1.Controls.Add(this._lbNationalCode);
            this.tabControlPanel1.Controls.Add(this._lbEnName);
            this.tabControlPanel1.Controls.Add(this.lb7);
            this.tabControlPanel1.Controls.Add(this.labelX2);
            this.tabControlPanel1.Controls.Add(this._lbIsoCode);
            this.tabControlPanel1.Controls.Add(this.labelX8);
            this.tabControlPanel1.Controls.Add(this._lbMcc);
            this.tabControlPanel1.Controls.Add(this.label6);
            this.tabControlPanel1.Controls.Add(this._lbCnName);
            this.tabControlPanel1.Controls.Add(this.labelX3);
            this.tabControlPanel1.Controls.Add(this.labelX1);
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(541, 200);
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(209)))), ((int)(((byte)(255)))));
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(244)))), ((int)(((byte)(255)))));
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderColor.Color = System.Drawing.SystemColors.ControlDark;
            this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel1.Style.GradientAngle = 90;
            this.tabControlPanel1.TabIndex = 1;
            this.tabControlPanel1.TabItem = this.tabItem1;
            // 
            // _country
            // 
            this._country.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._country.DisplayMember = "Text";
            this._country.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this._country.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._country.FormattingEnabled = true;
            this._country.Location = new System.Drawing.Point(101, 21);
            this._country.Name = "_country";
            this._country.Size = new System.Drawing.Size(423, 22);
            this._country.TabIndex = 0;
            this._country.SelectedIndexChanged += new System.EventHandler(this._country_SelectedIndexChanged);
            // 
            // _lbNationalCode
            // 
            this._lbNationalCode.BackColor = System.Drawing.Color.Transparent;
            this._lbNationalCode.Location = new System.Drawing.Point(337, 97);
            this._lbNationalCode.Name = "_lbNationalCode";
            this._lbNationalCode.Size = new System.Drawing.Size(187, 23);
            this._lbNationalCode.TabIndex = 1;
            this._lbNationalCode.Text = "国际区号";
            // 
            // _lbEnName
            // 
            this._lbEnName.BackColor = System.Drawing.Color.Transparent;
            this._lbEnName.Location = new System.Drawing.Point(337, 68);
            this._lbEnName.Name = "_lbEnName";
            this._lbEnName.Size = new System.Drawing.Size(187, 23);
            this._lbEnName.TabIndex = 1;
            this._lbEnName.Text = "英文名称";
            // 
            // lb7
            // 
            this.lb7.BackColor = System.Drawing.Color.Transparent;
            this.lb7.Location = new System.Drawing.Point(255, 97);
            this.lb7.Name = "lb7";
            this.lb7.Size = new System.Drawing.Size(76, 23);
            this.lb7.TabIndex = 1;
            this.lb7.Text = "国际区号：";
            this.lb7.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            this.labelX2.Location = new System.Drawing.Point(255, 68);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(76, 23);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "英文名称：";
            this.labelX2.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // _lbIsoCode
            // 
            this._lbIsoCode.BackColor = System.Drawing.Color.Transparent;
            this._lbIsoCode.Location = new System.Drawing.Point(113, 126);
            this._lbIsoCode.Name = "_lbIsoCode";
            this._lbIsoCode.Size = new System.Drawing.Size(136, 23);
            this._lbIsoCode.TabIndex = 1;
            this._lbIsoCode.Text = "ISO代号";
            // 
            // labelX8
            // 
            this.labelX8.BackColor = System.Drawing.Color.Transparent;
            this.labelX8.Location = new System.Drawing.Point(18, 126);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(83, 23);
            this.labelX8.TabIndex = 1;
            this.labelX8.Text = "ISO代号：";
            this.labelX8.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // _lbMcc
            // 
            this._lbMcc.BackColor = System.Drawing.Color.Transparent;
            this._lbMcc.Location = new System.Drawing.Point(113, 97);
            this._lbMcc.Name = "_lbMcc";
            this._lbMcc.Size = new System.Drawing.Size(136, 23);
            this._lbMcc.TabIndex = 1;
            this._lbMcc.Text = "移动国家码";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(18, 97);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 23);
            this.label6.TabIndex = 1;
            this.label6.Text = "移动国家码：";
            this.label6.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // _lbCnName
            // 
            this._lbCnName.BackColor = System.Drawing.Color.Transparent;
            this._lbCnName.Location = new System.Drawing.Point(113, 68);
            this._lbCnName.Name = "_lbCnName";
            this._lbCnName.Size = new System.Drawing.Size(136, 23);
            this._lbCnName.TabIndex = 1;
            this._lbCnName.Text = "中文名称";
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            this.labelX3.Location = new System.Drawing.Point(32, 68);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(69, 23);
            this.labelX3.TabIndex = 1;
            this.labelX3.Text = "中文名称：";
            this.labelX3.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(18, 21);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(83, 23);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "选择国家：";
            this.labelX1.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // tabItem1
            // 
            this.tabItem1.AttachedControl = this.tabControlPanel1;
            this.tabItem1.Name = "tabItem1";
            this.tabItem1.Text = "所在国家";
            // 
            // tabItem2
            // 
            this.tabItem2.AttachedControl = this.tabControlPanel2;
            this.tabItem2.Name = "tabItem2";
            this.tabItem2.Text = "当前运营商";
            // 
            // tabControlPanel2
            // 
            this.tabControlPanel2.Controls.Add(this._lbState);
            this.tabControlPanel2.Controls.Add(this._lbBrand);
            this.tabControlPanel2.Controls.Add(this.labelX7);
            this.tabControlPanel2.Controls.Add(this.labelX9);
            this.tabControlPanel2.Controls.Add(this._lbRemarks);
            this.tabControlPanel2.Controls.Add(this.labelX11);
            this.tabControlPanel2.Controls.Add(this._lbMnc);
            this.tabControlPanel2.Controls.Add(this.labelX13);
            this.tabControlPanel2.Controls.Add(this._lbOpName);
            this.tabControlPanel2.Controls.Add(this.labelX15);
            this.tabControlPanel2.Controls.Add(this._operationList);
            this.tabControlPanel2.Controls.Add(this.labelX4);
            this.tabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel2.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel2.Name = "tabControlPanel2";
            this.tabControlPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel2.Size = new System.Drawing.Size(541, 200);
            this.tabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(209)))), ((int)(((byte)(255)))));
            this.tabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(244)))), ((int)(((byte)(255)))));
            this.tabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel2.Style.BorderColor.Color = System.Drawing.SystemColors.ControlDark;
            this.tabControlPanel2.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel2.Style.GradientAngle = 90;
            this.tabControlPanel2.TabIndex = 2;
            this.tabControlPanel2.TabItem = this.tabItem2;
            // 
            // _operationList
            // 
            this._operationList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._operationList.DisplayMember = "Text";
            this._operationList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this._operationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._operationList.FormattingEnabled = true;
            this._operationList.Location = new System.Drawing.Point(101, 21);
            this._operationList.Name = "_operationList";
            this._operationList.Size = new System.Drawing.Size(423, 22);
            this._operationList.TabIndex = 2;
            this._operationList.SelectedIndexChanged += new System.EventHandler(this._operationList_SelectedIndexChanged);
            // 
            // labelX4
            // 
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            this.labelX4.Location = new System.Drawing.Point(18, 21);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(83, 23);
            this.labelX4.TabIndex = 3;
            this.labelX4.Text = "选择运营商：";
            this.labelX4.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // _lbState
            // 
            this._lbState.BackColor = System.Drawing.Color.Transparent;
            this._lbState.Location = new System.Drawing.Point(338, 89);
            this._lbState.Name = "_lbState";
            this._lbState.Size = new System.Drawing.Size(187, 23);
            this._lbState.TabIndex = 10;
            this._lbState.Text = "使用状态";
            // 
            // _lbBrand
            // 
            this._lbBrand.BackColor = System.Drawing.Color.Transparent;
            this._lbBrand.Location = new System.Drawing.Point(338, 60);
            this._lbBrand.Name = "_lbBrand";
            this._lbBrand.Size = new System.Drawing.Size(187, 23);
            this._lbBrand.TabIndex = 9;
            this._lbBrand.Text = "品牌";
            // 
            // labelX7
            // 
            this.labelX7.BackColor = System.Drawing.Color.Transparent;
            this.labelX7.Location = new System.Drawing.Point(256, 89);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(76, 23);
            this.labelX7.TabIndex = 11;
            this.labelX7.Text = "使用状态：";
            this.labelX7.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // labelX9
            // 
            this.labelX9.BackColor = System.Drawing.Color.Transparent;
            this.labelX9.Location = new System.Drawing.Point(256, 60);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(76, 23);
            this.labelX9.TabIndex = 13;
            this.labelX9.Text = "品牌：";
            this.labelX9.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // _lbRemarks
            // 
            this._lbRemarks.BackColor = System.Drawing.Color.Transparent;
            this._lbRemarks.Location = new System.Drawing.Point(114, 118);
            this._lbRemarks.Name = "_lbRemarks";
            this._lbRemarks.Size = new System.Drawing.Size(136, 23);
            this._lbRemarks.TabIndex = 12;
            this._lbRemarks.Text = "参考和注释";
            // 
            // labelX11
            // 
            this.labelX11.BackColor = System.Drawing.Color.Transparent;
            this.labelX11.Location = new System.Drawing.Point(19, 118);
            this.labelX11.Name = "labelX11";
            this.labelX11.Size = new System.Drawing.Size(83, 23);
            this.labelX11.TabIndex = 5;
            this.labelX11.Text = "参考和注释：";
            this.labelX11.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // _lbMnc
            // 
            this._lbMnc.BackColor = System.Drawing.Color.Transparent;
            this._lbMnc.Location = new System.Drawing.Point(114, 89);
            this._lbMnc.Name = "_lbMnc";
            this._lbMnc.Size = new System.Drawing.Size(136, 23);
            this._lbMnc.TabIndex = 4;
            this._lbMnc.Text = "MNC";
            // 
            // labelX13
            // 
            this.labelX13.BackColor = System.Drawing.Color.Transparent;
            this.labelX13.Location = new System.Drawing.Point(19, 89);
            this.labelX13.Name = "labelX13";
            this.labelX13.Size = new System.Drawing.Size(83, 23);
            this.labelX13.TabIndex = 6;
            this.labelX13.Text = "MNC：";
            this.labelX13.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // _lbOpName
            // 
            this._lbOpName.BackColor = System.Drawing.Color.Transparent;
            this._lbOpName.Location = new System.Drawing.Point(114, 60);
            this._lbOpName.Name = "_lbOpName";
            this._lbOpName.Size = new System.Drawing.Size(136, 23);
            this._lbOpName.TabIndex = 8;
            this._lbOpName.Text = "名称";
            // 
            // labelX15
            // 
            this.labelX15.BackColor = System.Drawing.Color.Transparent;
            this.labelX15.Location = new System.Drawing.Point(33, 60);
            this.labelX15.Name = "labelX15";
            this.labelX15.Size = new System.Drawing.Size(69, 23);
            this.labelX15.TabIndex = 7;
            this.labelX15.Text = "名称：";
            this.labelX15.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // OptionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 277);
            this.Controls.Add(this.tabControl1);
            this.Name = "OptionDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选项";
            this.Load += new System.EventHandler(this.OptionDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabControlPanel1.ResumeLayout(false);
            this.tabControlPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.TabControl tabControl1;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        private DevComponents.DotNetBar.TabItem tabItem1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx _country;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX _lbEnName;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX _lbCnName;
        private DevComponents.DotNetBar.LabelX _lbNationalCode;
        private DevComponents.DotNetBar.LabelX label6;
        private DevComponents.DotNetBar.LabelX lb7;
        private DevComponents.DotNetBar.LabelX _lbMcc;
        private DevComponents.DotNetBar.LabelX _lbIsoCode;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel2;
        private DevComponents.DotNetBar.TabItem tabItem2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx _operationList;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX _lbState;
        private DevComponents.DotNetBar.LabelX _lbBrand;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.DotNetBar.LabelX _lbRemarks;
        private DevComponents.DotNetBar.LabelX labelX11;
        private DevComponents.DotNetBar.LabelX _lbMnc;
        private DevComponents.DotNetBar.LabelX labelX13;
        private DevComponents.DotNetBar.LabelX _lbOpName;
        private DevComponents.DotNetBar.LabelX labelX15;

    }
}
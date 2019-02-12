namespace ServiceFairy.Client.WinForm
{
    partial class NavigationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NavigationForm));
            this.navigationPane1 = new DevComponents.DotNetBar.NavigationPane();
            this._usersNavPanel = new DevComponents.DotNetBar.NavigationPanePanel();
            this._usersPanel = new DevComponents.DotNetBar.ItemPanel();
            this.usersButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.navigationPanePanel2 = new DevComponents.DotNetBar.NavigationPanePanel();
            this.navigationPane1.SuspendLayout();
            this._usersNavPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // navigationPane1
            // 
            this.navigationPane1.Controls.Add(this._usersNavPanel);
            this.navigationPane1.Controls.Add(this.navigationPanePanel2);
            this.navigationPane1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationPane1.ItemPaddingBottom = 2;
            this.navigationPane1.ItemPaddingTop = 2;
            this.navigationPane1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.usersButtonItem});
            this.navigationPane1.Location = new System.Drawing.Point(0, 0);
            this.navigationPane1.Name = "navigationPane1";
            this.navigationPane1.NavigationBarHeight = 67;
            this.navigationPane1.Padding = new System.Windows.Forms.Padding(1);
            this.navigationPane1.Size = new System.Drawing.Size(216, 455);
            this.navigationPane1.TabIndex = 0;
            // 
            // 
            // 
            this.navigationPane1.TitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.navigationPane1.TitlePanel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.navigationPane1.TitlePanel.Location = new System.Drawing.Point(1, 1);
            this.navigationPane1.TitlePanel.Name = "panelTitle";
            this.navigationPane1.TitlePanel.Size = new System.Drawing.Size(214, 24);
            this.navigationPane1.TitlePanel.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.navigationPane1.TitlePanel.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.navigationPane1.TitlePanel.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.navigationPane1.TitlePanel.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.navigationPane1.TitlePanel.Style.BorderSide = DevComponents.DotNetBar.eBorderSide.Bottom;
            this.navigationPane1.TitlePanel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.navigationPane1.TitlePanel.Style.GradientAngle = 90;
            this.navigationPane1.TitlePanel.Style.MarginLeft = 4;
            this.navigationPane1.TitlePanel.TabIndex = 0;
            this.navigationPane1.TitlePanel.Text = "用户";
            // 
            // _usersNavPanel
            // 
            this._usersNavPanel.Controls.Add(this._usersPanel);
            this._usersNavPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._usersNavPanel.Location = new System.Drawing.Point(1, 25);
            this._usersNavPanel.Name = "_usersNavPanel";
            this._usersNavPanel.ParentItem = this.usersButtonItem;
            this._usersNavPanel.Size = new System.Drawing.Size(214, 362);
            this._usersNavPanel.Style.Alignment = System.Drawing.StringAlignment.Center;
            this._usersNavPanel.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this._usersNavPanel.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this._usersNavPanel.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this._usersNavPanel.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this._usersNavPanel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._usersNavPanel.Style.GradientAngle = 90;
            this._usersNavPanel.StyleMouseDown.Alignment = System.Drawing.StringAlignment.Center;
            this._usersNavPanel.StyleMouseDown.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this._usersNavPanel.StyleMouseDown.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this._usersNavPanel.StyleMouseDown.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBorder;
            this._usersNavPanel.StyleMouseDown.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedText;
            this._usersNavPanel.StyleMouseOver.Alignment = System.Drawing.StringAlignment.Center;
            this._usersNavPanel.StyleMouseOver.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotBackground;
            this._usersNavPanel.StyleMouseOver.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotBackground2;
            this._usersNavPanel.StyleMouseOver.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotBorder;
            this._usersNavPanel.StyleMouseOver.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemHotText;
            this._usersNavPanel.TabIndex = 2;
            // 
            // _usersPanel
            // 
            // 
            // 
            // 
            this._usersPanel.BackgroundStyle.BackColor = System.Drawing.Color.White;
            this._usersPanel.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this._usersPanel.BackgroundStyle.BorderBottomWidth = 1;
            this._usersPanel.BackgroundStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this._usersPanel.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this._usersPanel.BackgroundStyle.BorderLeftWidth = 1;
            this._usersPanel.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this._usersPanel.BackgroundStyle.BorderRightWidth = 1;
            this._usersPanel.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this._usersPanel.BackgroundStyle.BorderTopWidth = 1;
            this._usersPanel.BackgroundStyle.PaddingBottom = 1;
            this._usersPanel.BackgroundStyle.PaddingLeft = 1;
            this._usersPanel.BackgroundStyle.PaddingRight = 1;
            this._usersPanel.BackgroundStyle.PaddingTop = 1;
            this._usersPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._usersPanel.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this._usersPanel.Location = new System.Drawing.Point(0, 0);
            this._usersPanel.Name = "_usersPanel";
            this._usersPanel.Size = new System.Drawing.Size(214, 362);
            this._usersPanel.TabIndex = 0;
            this._usersPanel.Text = "_userPanel";
            // 
            // usersButtonItem
            // 
            this.usersButtonItem.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.usersButtonItem.Checked = true;
            this.usersButtonItem.Image = ((System.Drawing.Image)(resources.GetObject("usersButtonItem.Image")));
            this.usersButtonItem.ImagePaddingHorizontal = 8;
            this.usersButtonItem.Name = "usersButtonItem";
            this.usersButtonItem.OptionGroup = "navBar";
            this.usersButtonItem.Text = "用户";
            // 
            // navigationPanePanel2
            // 
            this.navigationPanePanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationPanePanel2.Location = new System.Drawing.Point(1, 1);
            this.navigationPanePanel2.Name = "navigationPanePanel2";
            this.navigationPanePanel2.ParentItem = null;
            this.navigationPanePanel2.Size = new System.Drawing.Size(214, 386);
            this.navigationPanePanel2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.navigationPanePanel2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.navigationPanePanel2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.navigationPanePanel2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.navigationPanePanel2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.navigationPanePanel2.Style.GradientAngle = 90;
            this.navigationPanePanel2.TabIndex = 3;
            // 
            // NavigationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(216, 455);
            this.Controls.Add(this.navigationPane1);
            this.Name = "NavigationForm";
            this.Text = "导航";
            this.Load += new System.EventHandler(this.NavigationForm_Load);
            this.navigationPane1.ResumeLayout(false);
            this._usersNavPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.NavigationPane navigationPane1;
        private DevComponents.DotNetBar.NavigationPanePanel _usersNavPanel;
        private DevComponents.DotNetBar.ButtonItem usersButtonItem;
        private DevComponents.DotNetBar.NavigationPanePanel navigationPanePanel2;
        private DevComponents.DotNetBar.ItemPanel _usersPanel;

    }
}
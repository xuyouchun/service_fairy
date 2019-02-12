using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Common.WinForm.Docking;
using Common.Contracts.Service;
using DevComponents.DotNetBar;
using Common.Contracts.UIObject;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Package;
using Common.Package.UIObject;
using Common.WinForm;
using Common.Contracts;

namespace Common.WinForm.Docking.DockingWindows
{
    public partial class NavigationDockingWindow : DockContentEx, INavigationWindow
    {
        public NavigationDockingWindow()
        {
            InitializeComponent();
        }

        public void Show(IUIObjectExecuteContext executeContext, IServiceObjectProvider provider)
        {
            Contract.Requires(executeContext != null && provider != null);

            try
            {
                navigationPane1.SuspendLayout();

                int index = 0;
                foreach (IServiceObjectTreeNode node in provider.GetTree().Root)
                {
                    index++;
                    IServiceObject serviceObject = node.ServiceObject;
                    ItemPanel panel = AddItem(executeContext, serviceObject, index == 1);

                    _ShowNode(executeContext, node, panel);
                }

                navigationPane1.NavigationBarHeight += 35 * 4;
            }
            finally
            {
                navigationPane1.ResumeLayout(false);
            }
        }

        private void _ShowNode(IUIObjectExecuteContext executeContext, IServiceObjectTreeNode node, ItemPanel panel)
        {
            panel.LayoutOrientation = eOrientation.Vertical;
            foreach (IServiceObjectTreeNode node0 in node)
            {
                IServiceObject so = node0.ServiceObject;
                IUIObject uiObject = so.GetUIObject();
                ServiceObjectInfo info = uiObject.Info;

                ButtonItem buttonItem = new ButtonItem() {
                    Text = info.Title, Name = uiObject.Info.Name, ButtonStyle = eButtonStyle.ImageAndText,
                    Image = uiObject.GetImage(24), ItemAlignment = eItemAlignment.Far, //AutoCheckOnClick = false,
                    /*OptionGroup = "AppService",*/ Tooltip = info.Desc,
                };

                IUIObjectExecuteContext exeCtx = UIObjectExecuteContextHelper.SetCurrent(executeContext, node0);
                var menuStrip = ServiceObjectContextMenuStrip.FromServiceObject(exeCtx, so);
                _ApplyButtonItemEvents(buttonItem, panel, menuStrip, true, exeCtx, so);

                panel.Items.Add(buttonItem);
            }
        }

        private void _ApplyButtonItemEvents(ButtonItem buttonItem, Control panel, ServiceObjectContextMenuStrip menuStrip, bool enableClickEvent,
            IUIObjectExecuteContext executeContext, IServiceObject serviceObject)
        {
            buttonItem.MouseUp += delegate(object sender, MouseEventArgs e) {
                if (e.Button == MouseButtons.Right)
                {
                    menuStrip.SetEnableState();
                    menuStrip.Show(panel, e.Location, ToolStripDropDownDirection.BelowRight);
                }
            };

            buttonItem.DoubleClick += delegate(object sender, EventArgs e) {
                serviceObject.ExecuteActionWithErrorDialog(executeContext, (Control.ModifierKeys == Keys.Shift) ? ServiceObjectActionType.AttachDefault : ServiceObjectActionType.Default);
            };

            if (enableClickEvent)
            {
                buttonItem.Click += delegate(object sender, EventArgs e) {
                    serviceObject.ExecuteActionWithErrorDialog(executeContext,
                        (Control.ModifierKeys == Keys.Shift) ? ServiceObjectActionType.OpenInNewWindow : ServiceObjectActionType.Open);
                };
            }
        }

        private ItemPanel AddItem(IUIObjectExecuteContext executeContext, IServiceObject serviceObject, bool @checked = false)
        {
            ServiceObjectInfo info = serviceObject.Info;
            IUIObject uiObject = serviceObject.GetUIObject();

            ButtonItem buttonItem = new ButtonItem() {
                Checked = @checked, Image = uiObject.GetImage(24),
                ImagePaddingHorizontal = 8, OptionGroup = "navBar", Text = info.Title, ButtonStyle = eButtonStyle.ImageAndText,
                /*Name = info.Name + "_ButtonItem",*/ Tooltip = info.Desc,
            };

            NavigationPanePanel navigationPanePanel = new NavigationPanePanel() {
                Dock = DockStyle.Fill, ParentItem = buttonItem
            };

            ItemPanel itemPanel = new ItemPanel() {
                Dock = DockStyle.Fill,
            };

            itemPanel.VerticalScroll.Visible = true;

            var menuStrip = ServiceObjectContextMenuStrip.FromServiceObject(executeContext, serviceObject);
            _ApplyButtonItemEvents(buttonItem, navigationPane1.NavigationBar, menuStrip, false, executeContext, serviceObject);

            navigationPanePanel.Style.Alignment = StringAlignment.Center;
            navigationPanePanel.Style.BackColor1.ColorSchemePart = eColorSchemePart.BarBackground;
            navigationPanePanel.Style.BackColor2.ColorSchemePart = eColorSchemePart.BarBackground2;
            navigationPanePanel.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
            navigationPanePanel.Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
            navigationPanePanel.Style.GradientAngle = 90;
            navigationPanePanel.Controls.Add(itemPanel);

            navigationPane1.Controls.Add(navigationPanePanel);
            navigationPane1.Items.Add(buttonItem);

            return itemPanel;
        }


        public void SetCurrent(IUIObjectExecuteContext executeContext, IServiceObject serviceObject)
        {
            
        }
    }
}

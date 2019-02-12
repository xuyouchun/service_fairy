using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Contracts;
using Common.Contracts.Service;
using Common.WinForm;
using Common.WinForm.Docking;
using DevComponents.DotNetBar;
using ServiceFairy.Entities.Master;
using ServiceFairy.Entities.Sys;
using ServiceFairy.SystemInvoke;
using ServiceFairy.WinForm.AppCommandInvokeControls;
using Common.Utility;

namespace ServiceFairy.WinForm
{
    /// <summary>
    /// 接口测试
    /// </summary>
    public partial class AppCommandInvokeDialog : XDialog
    {
        public AppCommandInvokeDialog(SystemInvoker invoker, ClientDesc clientDesc, ServiceDesc serviceDesc, AppCommandInfo cmdInfo)
        {
            Contract.Requires(clientDesc != null && serviceDesc != null && cmdInfo != null);
            _context = new InvokeControlContext(invoker, clientDesc, serviceDesc, cmdInfo);

            InitializeComponent();

            Text = "接口测试－" + serviceDesc.ToString(false) + "/" + cmdInfo.CommandDesc.ToString(false);
        }

        private readonly InvokeControlContext _context;

        private void AppCommandInvokeDialog_Load(object sender, EventArgs e)
        {
            _inputJsonButtonItem.Checked = true;
            _ShowControl(_inputJsonButtonItem);
        }

        private void sideBar1_ItemClick(object sender, EventArgs e)
        {
            _ShowItem(sender);
        }

        private void _ShowItem(object item)
        {
            ButtonItem buttonItem = item as ButtonItem;
            if (buttonItem == null)
            {
                SideBarPanelItem sideBarPanelItem = item as SideBarPanelItem;
                if (sideBarPanelItem != null)
                {
                    buttonItem = sideBarPanelItem.SubItems.OfType<ButtonItem>().FirstOrDefault(item0 => item0.Checked);
                    if (buttonItem == null && sideBarPanelItem.SubItems.Count > 0)
                    {
                        buttonItem = sideBarPanelItem.SubItems[0] as ButtonItem;
                        if (buttonItem != null)
                        {
                            buttonItem.Checked = true;
                        }
                    }
                }
            }

            _ShowControl(buttonItem);
        }

        private readonly Dictionary<string, InvokeControlContextBase> _contextDict = new Dictionary<string, InvokeControlContextBase>();

        private InvokeControlContextBase _CreateContext(string tag)
        {
            return _contextDict.GetOrSet(tag ?? string.Empty, (a) => InvokeControlContextBase.Create(tag, _context));
        }

        private void _ShowControl(ButtonItem buttonItem)
        {
            InvokeControlContextBase context = (buttonItem == null) ? EmptyInvokeControlContext.Instance
                : _CreateContext(buttonItem.Tag as string) ?? EmptyInvokeControlContext.Instance;

            Panel panel = splitContainer1.Panel2;
            panel.Controls.Clear();

            try
            {
                Control control = context.GetControl();
                panel.Controls.Add(control);
                control.Dock = DockStyle.Fill;
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void sideBar1_ButtonCheckedChanged(object sender, EventArgs e)
        {
            return;
        }

        private void AppCommandInvokeDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.I:
                        _inputSideBarPanelItem.Expanded = true;
                        _ShowItem(_inputSideBarPanelItem);
                        break;

                    case Keys.O:
                        _outputSideBarPanelItem.Expanded = true;
                        _ShowItem(_outputSideBarPanelItem);
                        break;

                    case Keys.T:
                        _testSideBarPanelItem.Expanded = true;
                        _ShowItem(_testSideBarPanelItem);
                        break;
                }
            }
        }
    }
}

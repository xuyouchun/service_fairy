using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.WinForm;
using ServiceFairy.SystemInvoke;
using Common.Package.Service;
using Common.Communication.Wcf;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Utility;
using System.Threading;
using ServiceFairy.WinForm;
using Common.WinForm.Docking;
using System.Xml;
using Common.WinForm.Docking.DockingWindows;
using WeifenLuo.WinFormsUI.Docking;
using Common.Framework.Management.DockingWindow;
using Common.Contracts.UIObject;
using Common.Framework.Management;
using DevComponents.DotNetBar;

namespace ServiceFairy.Management.WinForm
{
    public partial class MainForm : XForm, INavigation
    {
        public MainForm()
        {
            InitializeComponent();
        }

        // 窗口加载
        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 主菜单
        /// </summary>
        public MenuStrip MenuStrip
        {
            get { return _msMenu; }
        }

        /// <summary>
        /// 状态栏
        /// </summary>
        public StatusStrip StatusStrip
        {
            get { return statusStrip1; }
        }

        public DockPanel DockPanel
        {
            get { return _mainDockPanel; }
        }

        public void Show(IUIObjectExecuteContext executeContext, IServiceObjectProvider provider)
        {
            IServiceObject so = provider.GetTree().Root.ServiceObject;
            var ms = ServiceObjectContextMenuStrip.FromServiceObject(executeContext, provider.GetTree().Root);
            _tsService.DropDownOpening += delegate { ms.SetEnableState(); };
            _tsService.DropDown = ms;
        }


        public void SetCurrent(IUIObjectExecuteContext executeContext, IServiceObject serviceObject)
        {
            if (serviceObject == null)
            {
                _tsCurrent.Visible = false;
            }
            else
            {
                _tsCurrent.Visible = true;

                var ms = ServiceObjectContextMenuStrip.FromServiceObject(executeContext, serviceObject);
                _tsCurrent.DropDown = ms;
                _tsCurrent.DropDownOpening += delegate { ms.SetEnableState(); };
            }
        }

        /// <summary>
        /// 创建界面操作
        /// </summary>
        /// <returns></returns>
        public IUIOperation CreateUIOperation()
        {
            return new XUIOperation(this);
        }

        private int _msPathWidthDiff = -1;

        private void _msMenu_Resize(object sender, EventArgs e)
        {
            if (_msMenu.Width == 0)
                return;

            if (_msPathWidthDiff < 0)
                _msPathWidthDiff = _msMenu.Width - _msPath.Width;
            else
                _msPath.Width = _msMenu.Width - _msPathWidthDiff;
        }
    }
}

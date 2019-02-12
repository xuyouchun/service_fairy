using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.WinForm.Docking;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;
using System.Diagnostics.Contracts;

namespace Common.Framework.Management.DockingWindow
{
    public class DockingWindowContext
    {
        public DockingWindowContext(Form mainForm, MenuStrip menuStrip, StatusStrip statusStrip,
            DockPanel dockPanel, DockingWindowManager dockingWindowManager)
        {
            Contract.Requires(mainForm != null && menuStrip != null && statusStrip != null && dockPanel != null && dockingWindowManager != null);

            MainForm = mainForm;
            MenuStrip = menuStrip;
            StatusStrip = statusStrip;
            DockPanel = dockPanel;
            DockingWindowManager = dockingWindowManager;
        }

        /// <summary>
        /// 主窗体
        /// </summary>
        public Form MainForm { get; private set; }

        /// <summary>
        /// 主菜单
        /// </summary>
        public MenuStrip MenuStrip { get; private set; }

        /// <summary>
        /// 状态栏
        /// </summary>
        public StatusStrip StatusStrip { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public DockPanel DockPanel { get; private set; }

        /// <summary>
        /// 默认布局
        /// </summary>
        public DockingWindowManager DockingWindowManager { get; private set; }
    }
}

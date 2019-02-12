using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Framework.Management.DockingWindow;
using Common.Contracts;
using Common.Utility;
using System.Diagnostics.Contracts;

namespace ServiceFairy.Management.WinForm
{
    /// <summary>
    /// 执行上下文环境
    /// </summary>
    class SmContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sp"></param>
        public SmContext(IServiceProvider sp)
        {
            Contract.Requires(sp != null);

            ServiceProvider = sp;
            DwContext = sp.GetService<IObjectProvider<DockingWindowContext>>(true).Get();
            MainForm = (MainForm)DwContext.MainForm;
        }

        /// <summary>
        /// 服务集合
        /// </summary>
        public IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// 主窗口
        /// </summary>
        public MainForm MainForm { get; private set; }

        /// <summary>
        /// 窗口界面环境
        /// </summary>
        public DockingWindowContext DwContext { get; private set; }
    }
}

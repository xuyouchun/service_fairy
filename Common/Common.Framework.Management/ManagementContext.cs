using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Contracts;
using Common.Utility;
using System.Diagnostics.Contracts;
using Common.Framework.Management.DockingWindow;

namespace Common.Framework.Management
{
    /// <summary>
    /// 管理系统的上下文执行环境
    /// </summary>
    public class ManagementContext
    {
        public ManagementContext(IServiceProvider sp)
        {
            Contract.Requires(sp != null);

            ServiceProvider = sp;
            ServiceObjectProvider = sp.GetService<IServiceObjectProvider>(true);
            Output = sp.GetService<IOutput>(true);
            DockingWindowContext = sp.GetService<IObjectProvider<DockingWindowContext>>(true).Get();
        }

        /// <summary>
        /// 服务集合
        /// </summary>
        public IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// 有交互功能的服务对象提供者
        /// </summary>
        public IServiceObjectProvider ServiceObjectProvider { get; private set; }

        /// <summary>
        /// 输出
        /// </summary>
        public IOutput Output { get; private set; }

        /// <summary>
        /// 窗体界面元素
        /// </summary>
        public DockingWindowContext DockingWindowContext { get; private set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Utility;
using Common.Contracts;
using Common.Package;
using Common.Contracts.UIObject;
using System.Threading;

namespace Common.Framework.Management
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ManagementApplicationBase : ApplicationBase
    {
        /// <summary>
        /// 创建服务策略
        /// </summary>
        /// <returns></returns>
        protected virtual IServiceProvider GetServiceProvider()
        {
            ServiceProvider sp = new ServiceProvider();
            sp.AddService(typeof(IOutput), CreateOutput());
            sp.AddService(typeof(IServiceObjectProvider), CreateServiceObjectProvider());

            return sp;
        }

        /// <summary>
        /// 创建ServiceProvider
        /// </summary>
        /// <returns></returns>
        protected abstract IServiceObjectProvider CreateServiceObjectProvider();

        /// <summary>
        /// 创建Output
        /// </summary>
        /// <returns></returns>
        protected abstract IOutput CreateOutput();

        /// <summary>
        /// 开始运行
        /// </summary>
        public override void Run(Action<string, string[]> callback, WaitHandle waitHandle)
        {
            ManagementContext context = new ManagementContext(GetServiceProvider());
            OnRun(context);
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="context"></param>
        protected abstract void OnRun(ManagementContext context);
    }
}

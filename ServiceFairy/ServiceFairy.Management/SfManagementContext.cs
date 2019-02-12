using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities;
using Common.Package;
using Common;
using Common.Utility;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;

namespace ServiceFairy.Management
{
    /// <summary>
    /// 
    /// </summary>
    partial class SfManagementContext
    {
        public SfManagementContext(SystemInvoker systemInvoker)
        {
            Invoker = systemInvoker;
            ClientDescs = new ClientDescManager(this);
            ServiceDeployInfos = new ServiceDeployInfoManager(this);
            ServiceUIInfos = new ServiceUIInfoManager(this);
            PlatformDeployPackageInfos = new PlatformDeployPackageInfoManager(this);
            ServiceDeployPackageInfos = new ServiceDeployPackageInfoManager(this);
        }

        public SystemInvoker Invoker { get; private set; }

        /// <summary>
        /// 服务终端信息管理器
        /// </summary>
        public ClientDescManager ClientDescs { get; private set; }

        /// <summary>
        /// 服务的部署信息管理器
        /// </summary>
        public ServiceDeployInfoManager ServiceDeployInfos { get; private set; }

        /// <summary>
        /// 服务界面管理器
        /// </summary>
        public ServiceUIInfoManager ServiceUIInfos { get; private set; }

        /// <summary>
        /// 平台安装包信息
        /// </summary>
        public PlatformDeployPackageInfoManager PlatformDeployPackageInfos { get; private set; }

        /// <summary>
        /// 服务安装包信息
        /// </summary>
        public ServiceDeployPackageInfoManager ServiceDeployPackageInfos { get; private set; }

        /// <summary>
        /// 获取指定服务的组件管理器
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="sd"></param>
        /// <returns></returns>
        public AppComponentManager GetAppComponentManager(Guid clientId, ServiceDesc sd)
        {
            Contract.Requires(sd != null);

            return AppComponentManager.Get(this, clientId, sd);
        }

        /// <summary>
        /// 获取指定服务的接口管理器
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="sd"></param>
        /// <returns></returns>
        public AppCommandManager GetAppCommandManager(Guid clientId, ServiceDesc sd)
        {
            Contract.Requires(sd != null);

            return AppCommandManager.Get(this, clientId, sd);
        }

        /// <summary>
        /// 获取指定服务的文件管理器
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="sd"></param>
        /// <returns></returns>
        public AppFileManager GetAppFileManager(Guid clientId, ServiceDesc sd)
        {
            Contract.Requires(sd != null);

            return AppFileManager.Get(this, clientId, sd);
        }

        /// <summary>
        /// 获取指定插件的管理器
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="sd"></param>
        /// <returns></returns>
        public AppServiceAddinManager GetAppServiceAddinManager(Guid clientId, ServiceDesc sd)
        {
            Contract.Requires(sd != null);

            return AppServiceAddinManager.Get(this, clientId, sd);
        }

        public abstract class ManagementBase<T>
        {
            public ManagementBase(SfManagementContext ctx, TimeSpan refreshInterval = default(TimeSpan))
            {
                MgrCtx = ctx;
                _loader = new AutoLoad<T>(OnLoad, refreshInterval);
            }

            protected SfManagementContext MgrCtx { get; private set; }
            private readonly AutoLoad<T> _loader;

            protected abstract T OnLoad();

            protected T Value
            {
                get { return _loader.Value; }
            }

            public void ClearCache()
            {
                _loader.ClearCache();
            }
        }
    }
}

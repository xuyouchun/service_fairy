using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Reflection;
using Common.Utility;
using System.Diagnostics.Contracts;
using Common.Contracts;

namespace Common.Package.Service
{
    /// <summary>
    /// 自动从程序集加载的服务基类
    /// </summary>
    public abstract class AssemblyAppServiceBase : AppServiceBase
    {
        public AssemblyAppServiceBase()
        {
            
        }

        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.AddRange(new IAppComponent[] {
                AssemblyAppCommandManager = new AssemblyAppCommandManager(this),
            });
        }

        private readonly object _syncLocker = new object();

        /// <summary>
        /// 接口加载器
        /// </summary>
        public AssemblyAppCommandManager AssemblyAppCommandManager { get; private set; }

        protected override AppServiceInfo GetServiceInfo()
        {
            AppServiceInfo serviceInfo = _serviceInfo;
            if (serviceInfo != null)
                return serviceInfo;

            lock (_syncLocker)
            {
                if ((serviceInfo = _serviceInfo) == null)
                {
                    AppServiceAttribute attr = this.GetType().GetAttribute<AppServiceAttribute>();
                    if (attr == null)
                        throw new ServiceException(ServerErrorCode.ServerError, "未标明服务的名称");

                    serviceInfo = new AppServiceInfo(
                        new ServiceDesc(attr.Name, attr.Version), attr.Title, attr.Desc,
                        attr.DefaultDataFormat, attr.Weight, attr.Category);

                    _serviceInfo = serviceInfo;
                }

                return serviceInfo;
            }
        }

        private AppCommandInfo[] _GetRuntimeAppCommandInfos()
        {
            return GetAllCommands().Select(cmd => cmd.GetInfo()).ToArray();
        }

        /// <summary>
        /// 添加一个指令
        /// </summary>
        /// <param name="command"></param>
        public override void AddCommand(IAppCommand command)
        {
            base.AddCommand(command);

            lock (_syncLocker)
                _serviceInfo = null;
        }

        /// <summary>
        /// 删除一个指令
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        public override void RemoveCommand(string name, SVersion version = default(SVersion))
        {
            base.RemoveCommand(name, version);

            lock (_syncLocker)
                _serviceInfo = null;
        }

        private volatile AppServiceInfo _serviceInfo;

        internal protected virtual Assembly[] GetAssemblies()
        {
            return new Assembly[] { this.GetType().Assembly };
        }
    }
}

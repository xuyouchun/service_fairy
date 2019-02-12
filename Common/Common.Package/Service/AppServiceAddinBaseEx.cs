using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace Common.Package.Service
{
    /// <summary>
    /// AppService插件基类
    /// </summary>
    public abstract class AppServiceAddinBaseEx : AppServiceAddinBase
    {
        public AppServiceAddinBaseEx(AppServiceBase service)
        {
            Contract.Requires(service != null);

            Service = service;
            _appCommands = new Lazy<AppCommandCollection>(() => new AppCommandCollection(OnLoadAppCommands()), true);
        }

        protected virtual IAppCommand[] OnLoadAppCommands()
        {
            return AppCommandCollection.LoadFromType(GetType(), new object[] { this }).ToArray();
        }

        private readonly Lazy<AppCommandCollection> _appCommands;

        /// <summary>
        /// AppService
        /// </summary>
        public AppServiceBase Service { get; private set; }

        /// <summary>
        /// 调用指定的接口
        /// </summary>
        /// <param name="context">执行环境</param>
        /// <param name="method">接口名称</param>
        /// <param name="data">参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns>应答结果</returns>
        protected override CommunicateData OnCall(CommunicateContext context, string method, CommunicateData data, CallingSettings settings)
        {
            AppCommandExecuteContext executeContext = Service.CreateAppCommandExecuteContext(context, settings);

            MethodParser mp = new MethodParser(method);
            return _appCommands.Value.Call(executeContext, mp.Command, data);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Contracts.Service;
using ServiceFairy.Components;
using ServiceFairy.Service.RemoteProxy.Components;

namespace ServiceFairy.Service.RemoteProxy
{
    /// <summary>
    /// 远程代理
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.RemoteProxy, "1.0", "远程代理",
        category: AppServiceCategory.System, desc: "为集群之间的连接提供远程代理，将其它的集群的服务映射到本地")]
    class Service : SystemAppServiceBase
    {
        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.AddRange(new IAppComponent[] {
                RemoteServiceManager = new RemoteServiceManagerAppComponent(this),
            });
        }

        /// <summary>
        /// 远程服务管理器
        /// </summary>
        public RemoteServiceManagerAppComponent RemoteServiceManager { get; private set; }
    }
}

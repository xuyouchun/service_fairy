using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Service.Navigation.Components;
using Common.Package;
using Common.Package.Service;
using ServiceFairy.Components;

namespace ServiceFairy.Service.Navigation
{
    /// <summary>
    /// 导航管理器
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.Navigation, "1.0", "导航服务",
        category: AppServiceCategory.System, weight:0, desc:"提供代理服务的列表")]
    class Service : SystemAppServiceBase
    {
        public Service()
        {
            
        }

        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            AppComponentManager.AddRange(new IAppComponent[]{
                ProxyListManager = new ProxyListManagerAppComponent(this),
            });
        }

        /// <summary>
        /// 代理服务列表的管理器
        /// </summary>
        public ProxyListManagerAppComponent ProxyListManager { get; private set; }
    }
}

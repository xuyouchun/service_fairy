using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Contracts.Service;
using ServiceFairy.Components;
using ServiceFairy.Service.Session.Components;

namespace ServiceFairy.Service.Session
{
    /// <summary>
    /// 会话服务
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.Session, "1.0", "会话服务",
        category: AppServiceCategory.System, desc: "对可靠性要求较高的数据提供存储策略")]
    class Service : SystemAppServiceBase
    {
        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.AddRange(new IAppComponent[] {
                SessionCacheManager = new SessionCacheManagerAppComponent(this),
            });
        }

        /// <summary>
        /// 会话缓存管理器
        /// </summary>
        public SessionCacheManagerAppComponent SessionCacheManager { get; private set; }
    }
}

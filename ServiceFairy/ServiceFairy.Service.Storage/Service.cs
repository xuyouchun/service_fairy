using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Components;
using ServiceFairy.Service.Storage.Components;

namespace ServiceFairy.Service.Storage
{
    /// <summary>
    /// 存储服务
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.Storage, "1.0", "存储服务", category: AppServiceCategory.System)]
    class Service : SystemAppServiceBase
    {
        public Service()
        {
            
        }

        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.Add(new IAppComponent[] {
                DatabaseManager = new DatabaseManagerAppComponent(this),
            });
        }

        /// <summary>
        /// 数据库管理器
        /// </summary>
        public DatabaseManagerAppComponent DatabaseManager { get; private set; }
    }
}

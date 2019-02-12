using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Components;
using ServiceFairy.Service.Security.Components;

namespace ServiceFairy.Service.Security
{
    /// <summary>
    /// 安全服务
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.Security, "1.0", "安全服务",
        category: AppServiceCategory.Core, weight: 60, desc:"存储安全码的权限并提供访问")]
    class Service : CoreAppServiceBase
    {
        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.AddRange(new IAppComponent[] {
                SystemAccountManager = new SystemAccountManagerAppComponent(this),
                SidGenerator = new SidGeneratorAppComponent(this),
            });
        }

        /// <summary>
        /// 系统帐号管理器
        /// </summary>
        public SystemAccountManagerAppComponent SystemAccountManager { get; private set; }

        /// <summary>
        /// 安全码生成器
        /// </summary>
        public SidGeneratorAppComponent SidGenerator { get; private set; }
    }
}

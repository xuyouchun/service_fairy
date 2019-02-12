using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using Common.Framework.TrayPlatform;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Service.Configuration.Components;
using Common.Package;
using ServiceFairy.Components;

namespace ServiceFairy.Service.Configuration
{
    /// <summary>
    /// 配置信息服务
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.Configuration, "1.0", "配置服务",
        category: AppServiceCategory.Core, weight:40, desc: "维护各个服务的配置文件并提供下载")]
    class Service : CoreAppServiceBase
    {
        public Service()
        {
            
        }

        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            AppComponentManager.AddRange(new AppComponentBase[] {
                ConfigurationManager = new ConfigurationManagerAppComponent(this),
                EnvironmentValueManager = new EnvironmentValueManagerAppComponent(this),
            });
        }

        /// <summary>
        /// 配置文件管理器
        /// </summary>
        public ConfigurationManagerAppComponent ConfigurationManager { get; private set; }

        /// <summary>
        /// 环境变量管理器
        /// </summary>
        public EnvironmentValueManagerAppComponent EnvironmentValueManager { get; private set; }
    }
}

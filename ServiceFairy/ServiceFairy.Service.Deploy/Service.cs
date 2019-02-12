using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using Common.Framework.TrayPlatform;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Service.Deploy.Components;
using Common.Package;
using ServiceFairy.Components;

namespace ServiceFairy.Service.Deploy
{
    /// <summary>
    /// Deploy服务
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.Deploy, "1.0", "部署服务",
        category: AppServiceCategory.Core, weight:30, desc: "维护各个服务的安装包并提供下载")]
    class Service : CoreAppServiceBase
    {
        public Service()
        {
            
        }

        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            AppComponentManager.AddRange(new AppComponentBase[] {
                DeployMapManager = new DeployMapManager(this),
                DeployPackageManager = new DeployPackageManager(this),
            });
        }

        /// <summary>
        /// 部署地图管理器
        /// </summary>
        public DeployMapManager DeployMapManager { get; private set; }

        /// <summary>
        /// 安装包部署管理器
        /// </summary>
        public DeployPackageManager DeployPackageManager { get; private set; }
    }
}

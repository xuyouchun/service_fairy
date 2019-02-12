using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Contracts.Service;
using ServiceFairy.Components;
using ServiceFairy.Service.FileCenter.Components;

namespace ServiceFairy.Service.FileCenter
{
    /// <summary>
    /// 文件中心
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.FileCenter, "1.0", "文件中心",
        category: AppServiceCategory.System, desc: "分布式文件系统中心服务")]
    class Service : SystemAppServiceBase
    {
        public Service()
        {

        }

        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.AddRange(new IAppComponent[] { 
                FileInfoManager = new FileInfoManagerAppComponent(this),
            });
        }

        /// <summary>
        /// 文件信息管理器
        /// </summary>
        public FileInfoManagerAppComponent FileInfoManager { get; private set; }
    }
}

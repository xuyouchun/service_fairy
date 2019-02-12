using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Components;
using Common.Contracts.Service;
using ServiceFairy.Service.DatabaseCenter.Components;

namespace ServiceFairy.Service.DatabaseCenter
{
    /// <summary>
    /// 数据库中心
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.DatabaseCenter, "1.0", "数据库中心",
        category: AppServiceCategory.System, desc: "协调分布式数据库的运行")]
    class Service : SystemAppServiceBase
    {
        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.AddRange(new IAppComponent[] {
                DbManager = new DbManagerAppComponent(this),
                DataSync = new DataSyncAppComponent(this),
                DbIntegrityChecker = new DbIntegrityCheckerAppComponent(this),
            });
        }

        /// <summary>
        /// 数据库管理器
        /// </summary>
        public DbManagerAppComponent DbManager { get; private set; }

        /// <summary>
        /// 数据同步器
        /// </summary>
        public DataSyncAppComponent DataSync { get; private set; }

        /// <summary>
        /// 数据完整性检查器
        /// </summary>
        public DbIntegrityCheckerAppComponent DbIntegrityChecker { get; private set; }
    }
}

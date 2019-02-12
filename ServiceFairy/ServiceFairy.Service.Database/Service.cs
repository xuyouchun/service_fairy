using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Components;
using Common.Contracts.Service;
using ServiceFairy.Service.Database.Components;

namespace ServiceFairy.Service.Database
{
    /// <summary>
    /// 数据库服务
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.Database, "1.0", "数据库服务",
        category: AppServiceCategory.System, desc: "分布式数据库系统")]
    class Service : SystemAppServiceBase
    {
        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            this.AppComponentManager.AddRange(new IAppComponent[] {
                DbManager = new DbManagerAppComponent(this),
                DbQuerier = new DbQuerierAppComponent(this),
            });
        }

        /// <summary>
        /// 数据库连接管理器
        /// </summary>
        public DbManagerAppComponent DbManager { get; private set; }

        /// <summary>
        /// 数据库查询执行器
        /// </summary>
        public DbQuerierAppComponent DbQuerier { get; private set; }
    }
}

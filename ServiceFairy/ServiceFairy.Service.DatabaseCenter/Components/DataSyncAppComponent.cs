using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;

namespace ServiceFairy.Service.DatabaseCenter.Components
{
    /// <summary>
    /// 数据同步器
    /// </summary>
    [AppComponent("数据同步器", "保持数据与备份数据库的同步")]
    class DataSyncAppComponent : AppComponent
    {
        public DataSyncAppComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;
    }
}

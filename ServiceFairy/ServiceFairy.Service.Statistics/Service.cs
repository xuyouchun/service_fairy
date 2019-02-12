using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Components;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Statistics
{
    /// <summary>
    /// Service
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.Statistics, "1.0", "统计服务",
        category: AppServiceCategory.System, weight: 30, desc: "收集各服务的统计数据")]
    class Service : SystemAppServiceBase
    {

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Contracts;
using Common.Package.Service;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities;
using ServiceFairy.Components;

namespace ServiceFairy.Service.Share
{
    /// <summary>
    /// 共享服务
    /// </summary>
    [AppEntryPoint, AppService(SFNames.ServiceNames.Share, "1.0", "共享服务",
        category: AppServiceCategory.System, desc:"对用户共享信息提供存储和访问支持")]
    class Service : SystemAppServiceBase
    {

    }
}

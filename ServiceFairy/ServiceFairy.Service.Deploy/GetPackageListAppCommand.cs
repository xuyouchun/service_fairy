using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Package.Service;
using ServiceFairy.Entities.Deploy;

namespace ServiceFairy.Service.Deploy
{
    /// <summary>
    /// 获取安装包列表
    /// </summary>
    [AppCommand("GetPackageList", "获取安装包列表", SecurityLevel = SecurityLevel.CoreRunningLevel)]
    class GetPackageListAppCommand : ACS<Service>.Func<Deploy_GetPackageList_Request, Deploy_GetPackageList_Reply>
    {
        protected override Deploy_GetPackageList_Reply OnExecute(AppCommandExecuteContext<Service> context, Deploy_GetPackageList_Request req, ref ServiceResult sr)
        {
            return null;
        }
    }
}

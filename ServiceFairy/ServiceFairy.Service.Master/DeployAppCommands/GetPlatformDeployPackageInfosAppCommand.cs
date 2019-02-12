using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Master;
using Common.Contracts.Service;
using ServiceFairy.Service.Master.Components;
using ServiceFairy.Entities;

namespace ServiceFairy.Service.Master.DeployAppCommands
{
    /// <summary>
    /// 获取平安安装包的信息
    /// </summary>
    [AppCommand("GetPlatformDeployPackageInfos", "获取所有平台安装包的基础信息", SecurityLevel = SecurityLevel.CoreRunningLevel)]
    class GetPlatformDeployPackageInfosAppCommand : ACS<Service>.Func<Master_GetPlatformDeployPackageInfos_Request, Master_GetPlatformDeployPackageInfos_Reply>
    {
        protected override Master_GetPlatformDeployPackageInfos_Reply OnExecute(AppCommandExecuteContext<Service> context, Master_GetPlatformDeployPackageInfos_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;

            PlatformDeployPackageInfo[] packages = srv.PlatformDeployPackageManager.GetAllInfos();
            if (req.Ids != null)
                packages = packages.Where(package => req.Ids.Contains(package.Id)).ToArray();

            return new Master_GetPlatformDeployPackageInfos_Reply() { PackageInfos = packages };
        }
    }
}

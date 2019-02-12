using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Service.Master.DeployAppCommands
{
    /// <summary>
    /// 获取平台部署进度
    /// </summary>
    [AppCommand("GetPlatformDeployProgress", "获取平台部署进度", SecurityLevel = SecurityLevel.Admin)]
    class GetPlatformDeployProgressAppCommand : ACS<Service>.Func<Master_GetPlatformDeployProgress_Reply>
    {
        protected override Master_GetPlatformDeployProgress_Reply OnExecute(AppCommandExecuteContext<Service> context, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            PlatformDeployProgress[] progresses = srv.PlatformDeployPackageManager.GetProgress();
            return new Master_GetPlatformDeployProgress_Reply() { Progresses = progresses };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Master;
using Common.Contracts.Service;


namespace ServiceFairy.Service.Master.DeployAppCommands
{
    /// <summary>
    /// 删除平台安装包
    /// </summary>
    [AppCommand("DeletePlatformDeployPackages", "删除平台安装包", SecurityLevel = SecurityLevel.Admin)]
    class DeletePlatformDeployPackagesAppCommand : ACS<Service>.Action<Master_DeletePlatformDeployPackages_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Master_DeletePlatformDeployPackages_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;

            srv.PlatformDeployPackageManager.Delete(req.DeployIds);
        }
    }
}

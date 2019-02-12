using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Master;


namespace ServiceFairy.Service.Master.DeployAppCommands
{
    /// <summary>
    /// 将平台安装包部署到指定的终端
    /// </summary>
    [AppCommand("DeployPlatformPackage", "将平台安装包部署到指定的终端", SecurityLevel = SecurityLevel.Admin)]
    class DeployPlatformPackageAppCommand : ACS<Service>.Action<Master_DeployPlatformPackage_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Master_DeployPlatformPackage_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            Guid[] ids = req.ClientIds;

            if (ids == null)
                srv.PlatformDeployPackageManager.DeployToAllClients(req.DeployPackageId);
            else
                srv.PlatformDeployPackageManager.DeployToClients(req.DeployPackageId, ids);
        }
    }
}

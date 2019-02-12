using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Master;


namespace ServiceFairy.Service.Master.DeployAppCommands
{
    /// <summary>
    /// 部署服务安装包
    /// </summary>
    [AppCommand("DeployServicePackage", "部署服务安装包", SecurityLevel = SecurityLevel.Admin)]
    class DeployServicePackageAppCommand : ACS<Service>.Action<Master_DeployServicePackage_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Master_DeployServicePackage_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;

            Guid[] ids = req.ClientIds;

            if (ids == null)
                srv.ServiceDeployPackageManager.DeployToAllClients(req.DeployPackageId);
            else
                srv.ServiceDeployPackageManager.DeployToClients(req.DeployPackageId, ids);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Master;


namespace ServiceFairy.Service.Master.DeployAppCommands
{
    /// <summary>
    /// 删除服务安装包
    /// </summary>
    [AppCommand("DeleteServiceDeployPackages", "删除服务安装包", SecurityLevel = SecurityLevel.Admin)]
    class DeleteServiceDeployPackagesAppCommand : ACS<Service>.Action<Master_DeleteServiceDeployPackages_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Master_DeleteServiceDeployPackages_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            srv.ServiceDeployPackageManager.Delete(req.DeployIds);
        }
    }
}

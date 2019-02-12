using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Package.Service;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Service.Master.DeployAppCommands
{
    /// <summary>
    /// 下载服务部署包
    /// </summary>
    [AppCommand("DownloadServiceDeployPackage", "下载服务部署包", SecurityLevel = SecurityLevel.CoreRunningLevel)]
    class DownloadServiceDeployPackageAppCommand : ACS<Service>.Func<Master_DownloadServiceDeployPackage_Request, Master_DownloadServiceDeployPackage_Reply>
    {
        protected override Master_DownloadServiceDeployPackage_Reply OnExecute(AppCommandExecuteContext<Service> context, Master_DownloadServiceDeployPackage_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;

            DeployPackage package = req.DeployPackageId != default(Guid) ?
                srv.ServiceDeployPackageManager.Download(req.DeployPackageId) : srv.ServiceDeployPackageManager.Download(req.ServiceDesc);

            if (package == null)
                throw new ServiceException(ServerErrorCode.NoData);

            return new Master_DownloadServiceDeployPackage_Reply() { DeployPackage = package };
        }
    }
}

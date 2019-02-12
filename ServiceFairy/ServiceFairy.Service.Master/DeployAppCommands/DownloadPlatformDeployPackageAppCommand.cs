using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Master;


namespace ServiceFairy.Service.Master.DeployAppCommands
{
    /// <summary>
    /// 下载平台安装包
    /// </summary>
    [AppCommand("DownloadPlatformDeployPackage", "下载平台安装包", SecurityLevel = SecurityLevel.CoreRunningLevel)]
    class DownloadPlatformDeployPackageAppCommand : ACS<Service>.Func<Master_DownloadPlatformDeployPackage_Request, Master_DownloadPlatformDeployPackage_Reply>
    {
        protected override Master_DownloadPlatformDeployPackage_Reply OnExecute(AppCommandExecuteContext<Service> context, Master_DownloadPlatformDeployPackage_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;

            DeployPackage package = srv.PlatformDeployPackageManager.Download(req.DeployPackageId);
            return new Master_DownloadPlatformDeployPackage_Reply() { DeployPackage = package };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Package.Service;
using ServiceFairy.Entities.Deploy;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Service.Deploy
{
    /// <summary>
    /// 下载安装包
    /// </summary>
    [AppCommand("DownloadDeployPackage", "下载安装包", SecurityLevel = SecurityLevel.CoreRunningLevel)]
    class DownloadDeployPackageAppCommand : ACS<Service>.Func<Deploy_DownloadDeployPackage_Request, Deploy_DownloadDeployPackage_Reply>
    {
        protected override Deploy_DownloadDeployPackage_Reply OnExecute(AppCommandExecuteContext<Service> context, Deploy_DownloadDeployPackage_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;

            DeployPackage package;
            sr = srv.DeployPackageManager.TryGetDeployPackage(req.ServiceDesc, out package);

            if (!sr.Succeed && sr.StatusCode != (int)ServerErrorCode.DataNotReady)
                throw sr.CreateException();

            return new Deploy_DownloadDeployPackage_Reply() { DeployPackage = package };
        }
    }
}

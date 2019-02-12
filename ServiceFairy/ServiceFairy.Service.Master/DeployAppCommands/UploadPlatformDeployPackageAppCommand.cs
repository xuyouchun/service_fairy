using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Master;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Master.DeployAppCommands
{
    /// <summary>
    /// 上传平台的安装包
    /// </summary>
    [AppCommand("UploadPlatformDeployPackage", "上传平台的安装包", SecurityLevel = SecurityLevel.Admin)]
    class UploadPlatformDeployPackageAppCommand : ACS<Service>.Action<Master_UploadPlatformDeployPackage_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Master_UploadPlatformDeployPackage_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;

            srv.PlatformDeployPackageManager.Upload(req.DeployPackageInfo, req.Content);
        }
    }
}

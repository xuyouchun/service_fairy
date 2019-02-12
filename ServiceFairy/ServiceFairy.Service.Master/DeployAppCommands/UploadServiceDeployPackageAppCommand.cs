using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Service.Master.DeployAppCommands
{
    /// <summary>
    /// 上传服务的安装包
    /// </summary>
    [AppCommand("UploadServiceDeployPackage", "上传服务的安装包", SecurityLevel = SecurityLevel.Admin)]
    class UploadServiceDeployPackageAppCommand : ACS<Service>.Action<Master_UploadServiceDeployPackage_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Master_UploadServiceDeployPackage_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            srv.ServiceDeployPackageManager.Upload(req.DeployPackageInfo, req.Content);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Package.Service;
using ServiceFairy.Entities.Master;
using Common.Utility;

namespace ServiceFairy.Service.Master.DeployAppCommands
{
    /// <summary>
    /// 获取所有安装包的基础信息
    /// </summary>
    [AppCommand("GetServiceDeployPackageInfos", "获取所有服务安装包的基础信息", SecurityLevel = SecurityLevel.CoreRunningLevel)]
    class GetServiceDeployPackageInfosAppCommand : ACS<Service>.Func<Master_GetServiceDeployPackageInfos_Request, Master_GetServiceDeployPackageInfos_Reply>
    {
        protected override Master_GetServiceDeployPackageInfos_Reply OnExecute(AppCommandExecuteContext<Service> context, Master_GetServiceDeployPackageInfos_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;

            ServiceDeployPackageInfo[] infos = req.OnlyCurrent ? srv.ServiceDeployPackageManager.GetAllCurrentInfos() : srv.ServiceDeployPackageManager.GetAllInfos();

            if (req.ServiceDescs != null)
            {
                HashSet<ServiceDesc> descs = req.ServiceDescs.ToHashSet();
                infos = infos.Where(info => descs.Contains(info.ServiceDesc)).ToArray();
            }

            if (req.DeployPackageIds != null)
            {
                HashSet<Guid> ids = req.DeployPackageIds.ToHashSet();
                infos = infos.Where(info => ids.Contains(info.Id)).ToArray();
            }

            return new Master_GetServiceDeployPackageInfos_Reply() { ServiceDescDeployPackageInfos = infos };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Service.Master.DeployAppCommands
{
    /// <summary>
    /// 获取服务的部署信息
    /// </summary>
    [AppCommand("GetServiceDeployInfos", "获取服务的部署信息", SecurityLevel = SecurityLevel.CoreRunningLevel)]
    class GetServiceDeployInfosAppCommand : ACS<Service>.Func<Master_GetServiceDeployInfos_Request, Master_GetServiceDeployInfos_Reply>
    {
        protected override Master_GetServiceDeployInfos_Reply OnExecute(AppCommandExecuteContext<Service> context, Master_GetServiceDeployInfos_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;

            ServiceDeployInfo[] deployInfos = srv.AppClientManager.GetServiceDeployInfos(req.ServiceDescs);
            return new Master_GetServiceDeployInfos_Reply() { ServiceDeployInfos = deployInfos };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Service.Master.DeployAppCommands
{
    /// <summary>
    /// 获取服务的部署进度
    /// </summary>
    [AppCommand("GetServiceDeployProgress", "获取服务的部署进度", SecurityLevel = SecurityLevel.Admin)]
    class GetServiceDeployProgressAppCommand : ACS<Service>.Func<Master_GetServiceDeployProgress_Request, Master_GetServiceDeployProgress_Reply>
    {
        protected override Master_GetServiceDeployProgress_Reply OnExecute(AppCommandExecuteContext<Service> context, Master_GetServiceDeployProgress_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            ServiceDeployProgress[] progresses = srv.ServiceDeployPackageManager.GetProgreses(req.ServiceDescs);

            return new Master_GetServiceDeployProgress_Reply() { Progresses = progresses };
        }
    }
}

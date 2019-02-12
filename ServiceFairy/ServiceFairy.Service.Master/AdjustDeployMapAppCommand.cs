using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities.Master;


namespace ServiceFairy.Service.Master
{
    /// <summary>
    /// 修改部署地图
    /// </summary>
    [AppCommand("AdjustDeployMap", "修改部署地图", SecurityLevel = SecurityLevel.Admin)]
    class AdjustDeployMapAppCommand : ACS<Service>.Func<Master_AdjustDeployMap_Request, Master_AdjustDeployMap_Reply>
    {
        protected override Master_AdjustDeployMap_Reply OnExecute(AppCommandExecuteContext<Service> context, Master_AdjustDeployMap_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;

            srv.DeployMapManager.Adjust(req.AdjustInfos);
            return new Master_AdjustDeployMap_Reply();
        }
    }
}

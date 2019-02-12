using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common;
using Common.Framework.TrayPlatform;
using Common.Package.Service;
using ServiceFairy.Entities.Master;


namespace ServiceFairy.Service.Master.DeployAppCommands
{
    /// <summary>
    /// 获取部署地图
    /// </summary>
    [AppCommand("DownloadDeployMap", "获取部署地图", SecurityLevel = SecurityLevel.CoreRunningLevel)]
    class DownloadDeployMapAppCommand : ACS<Service>.Func<Master_GetDeployMap_Request, Master_GetDeployMap_Reply>
    {
        protected override Master_GetDeployMap_Reply OnExecute(AppCommandExecuteContext<Service> context, Master_GetDeployMap_Request req, ref ServiceResult sr)
        {
            Service svr = (Service)context.Service;
            DateTime lastUpdate;
            AppClientDeployInfo[] deployInfos = svr.DeployMapManager.GetAllDeployInfos(out lastUpdate);

            return new Master_GetDeployMap_Reply() { LastUpdate = lastUpdate, DeployInfos = (lastUpdate == req.LastUpdate) ? null : deployInfos };
        }
    }
}

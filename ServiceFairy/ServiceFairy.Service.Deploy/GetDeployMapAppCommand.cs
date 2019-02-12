using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Package.Service;
using ServiceFairy.Entities.Deploy;

namespace ServiceFairy.Service.Deploy
{
    /// <summary>
    /// 获取部署地图
    /// </summary>
    [AppCommand("GetDeployMap", "获取部署地图", SecurityLevel = SecurityLevel.CoreRunningLevel)]
    class GetDeployMapAppCommand : ACS<Service>.Func<Deploy_GetDeployMap_Request, Deploy_GetDeployMap_Reply>
    {
        protected override Deploy_GetDeployMap_Reply OnExecute(AppCommandExecuteContext<Service> context, Deploy_GetDeployMap_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            if(!srv.DeployMapManager.WaitForAvaliable(TimeSpan.FromSeconds(15)))
                throw CreateServiceException(ServerErrorCode.DataNotReady);

            AppClientDeployInfo[] infos = (req.ClientIDs == null) ?
                srv.DeployMapManager.GetAllDeployInfos() : srv.DeployMapManager.GetDeployInfos(req.ClientIDs);

            return new Deploy_GetDeployMap_Reply() { DeployInfos = infos };
        }
    }
}

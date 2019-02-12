using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Utility;
using Common.Package.Service;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Service.Master
{
    /// <summary>
    /// 获取所有服务版本的描述信息
    /// </summary>
    [AppCommand("GetAllVersions", "获取所有服务版本的描述信息", SecurityLevel = SecurityLevel.CoreRunningLevel)]
    class GetAllVersionsAppCommand : ACS<Service>.Func<Master_GetAllVersions_Reply>
    {
        protected override Master_GetAllVersions_Reply OnExecute(AppCommandExecuteContext<Service> context, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            AppClientDeployInfo[] dInfos = srv.DeployMapManager.GetAllDeployInfos();
            Dictionary<ServiceDesc, AppServiceConfiguration> cfgs = srv.ConfigurationManager.GetAll();

            if (dInfos == null || cfgs == null)
                throw new ServiceException(ServerErrorCode.DataNotReady);
             
            return new Master_GetAllVersions_Reply() {
                DeployVersionPairs = dInfos.ToArray(dInfo => new DeployVersionPair() { ClientID = dInfo.ClientId, Version = dInfo.LastUpdate }),
                ConfigurationVersionPairs = cfgs.Select(v => new ConfigurationVersionPair() { ServiceDesc = v.Key, Version = v.Value.LastUpdate }).ToArray()
            };
        }
    }
}

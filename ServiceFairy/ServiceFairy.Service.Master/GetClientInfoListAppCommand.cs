using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using ServiceFairy.Client;
using Common.Utility;
using Common.Package.Service;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Service.Master
{
    /// <summary>
    /// 获取服务终端列表
    /// </summary>
    [AppCommand("GetClientInfoList", "获取服务终端列表", SecurityLevel = SecurityLevel.CoreRunningLevel)]
    class GetClientInfoListAppCommand : ACS<Service>.Func<Master_GetClientList_Request, Master_GetClientList_Reply>
    {
        protected override Master_GetClientList_Reply OnExecute(AppCommandExecuteContext<Service> context, Master_GetClientList_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            AppClientInfo[] clientInfos = srv.AppClientManager.GetClientInfos(req.ClientIds);
            if (req.IncludeDetail)
                return new Master_GetClientList_Reply() { AppClientInfos = clientInfos };
            else
                return new Master_GetClientList_Reply() {
                    AppClientInfos = clientInfos.ToArray(
                        info => new AppClientInfo() { ClientId = info.ClientId, IPs = info.IPs, Desc = info.Desc, ConnectedTime = info.ConnectedTime })
                };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Package.Service;
using ServiceFairy.Entities.Navigation;

namespace ServiceFairy.Service.Navigation
{
    /// <summary>
    /// 获取代理列表
    /// </summary>
    [AppCommand("GetProxyList", "获取代理列表", SecurityLevel = SecurityLevel.Public)]
    class GetProxyListAppCommand : ACS<Service>.Func<Navigation_GetProxyList_Request, Navigation_GetProxyList_Reply>
    {
        protected override Navigation_GetProxyList_Reply OnExecute(AppCommandExecuteContext<Service> context, Navigation_GetProxyList_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;

            return new Navigation_GetProxyList_Reply() {
                CommunicationOptions = srv.ProxyListManager.GetProxyList(context.CommunicateContext.From, req.CommunicationType, req.Direction, req.MaxCount)
            };
        }
    }
}

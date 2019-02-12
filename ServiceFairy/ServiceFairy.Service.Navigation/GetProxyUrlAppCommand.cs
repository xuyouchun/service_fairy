using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Navigation;
using Common.Contracts.Service;
using Common.Communication.Wcf;
using Common.Utility;

namespace ServiceFairy.Service.Navigation
{
    /// <summary>
    /// 获取代理地址的Url
    /// </summary>
    [AppCommand("GetProxyUrl", "获取代理地址的Url", SecurityLevel = SecurityLevel.Public)]
    class GetProxyUrlAppCommand : ACS<Service>.Func<Navigation_GetProxyUrl_Request, Navigation_GetProxyUrl_Reply>
    {
        protected override Navigation_GetProxyUrl_Reply OnExecute(AppCommandExecuteContext<Service> context, Navigation_GetProxyUrl_Request req, ref ServiceResult sr)
        {
            Service service = (Service)context.Service;
            CommunicationOption[] options = service.ProxyListManager.GetProxyList(context.CommunicateContext.From, req.CommunicationType,
                req.Duplex ? CommunicationDirection.Bidirectional : CommunicationDirection.Unidirectional, 1);

            return new Navigation_GetProxyUrl_Reply {
                ProxyUrl = options.IsNullOrEmpty() ? null : options[0].ToString(true)
            };
        }
    }
}

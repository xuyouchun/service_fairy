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
    /// 获取代理地址
    /// </summary>
    [AppCommand("GetProxy", "获取代理地址", SecurityLevel = SecurityLevel.Public)]
    class GetProxyAppCommand : ACS<Service>.Func<Navigation_GetProxy_Request, Navigation_GetProxy_Reply>
    {
        protected override Navigation_GetProxy_Reply OnExecute(AppCommandExecuteContext<Service> context, Navigation_GetProxy_Request req, ref ServiceResult sr)
        {
            Service service = (Service)context.Service;
            CommunicationOption[] options = service.ProxyListManager.GetProxyList(context.CommunicateContext.From, req.CommunicationType,
                req.Duplex ? CommunicationDirection.Bidirectional : CommunicationDirection.Unidirectional, 1);

            return new Navigation_GetProxy_Reply {
                Proxy = options.IsNullOrEmpty()?null: _ToString(options[0])
            };
        }

        private string _ToString(CommunicationOption op)
        {
            if (op == null)
                return null;

            switch (op.Type)
            {
                case CommunicationType.Http:
                    return "http://" + op.Address + "/s";

                default:
                    return op.Address.ToString();
            }
        }
    }
}

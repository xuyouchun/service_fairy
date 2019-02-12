using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf.Service;
using Common.Contracts.Service;
using System.ServiceModel;
using System.ServiceModel.Configuration;

namespace Common.Communication.Wcf.Strategies.WcfConnectionStrategies
{
    /// <summary>
    /// 请求应答方式的TCP
    /// </summary>
    class HttpWcfConnection : WcfConnectionBase
    {
        public HttpWcfConnection(WcfCommunicationStrategyBase owner, WcfListener listener, CommunicationOption option)
            : base(owner, listener, option)
        {
            
        }

        internal protected override EntityMessage Reply(EntityMessage input, CommunicateData data)
        {
            if (input.Headers.Action == WcfRequestActions.RequestReply)
            {
                EntityMessage msg = Owner.CreateMessage(Option);
                msg.Data = data;

                return msg;
            }

            return null;
        }
    }
}

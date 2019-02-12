using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using Common.Communication.Wcf.Bindings;
using Common.Communication.Wcf.Service;
using Common.Communication.Wcf.Strategies;
using Common.Contracts;
using Common.Contracts.Service;
using System.ServiceModel;

namespace Common.Communication.Wcf
{
    public static class WcfUtility
    {
        /// <summary>
        /// 创建Message
        /// </summary>
        /// <param name="option"></param>
        /// <param name="context"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        internal static EntityMessage CreateMessage(CommunicationOption option, CommunicateContext context = null, CallingSettings settings = null)
        {
            IWcfCommunicationStrategy strategy = WcfFactory.CreateConnectionStrategy(option.Type);
            EntityMessage msg = strategy.CreateMessage(option, context, settings);

            return msg;
        }

        /// <summary>
        /// 创建当前的通信策略上下文执行环境
        /// </summary>
        /// <returns></returns>
        public static ServiceAddress GetCurrentServiceAddress()
        {
            OperationContext ctx = OperationContext.Current;
            if (ctx == null)
                return null;

            MessageProperties mp = ctx.IncomingMessageProperties;
            if (!mp.ContainsKey(RemoteEndpointMessageProperty.Name))
                return null;

            RemoteEndpointMessageProperty rMp = mp[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            if (rMp != null)
                return new ServiceAddress(rMp.Address, rMp.Port);

            return null;
        }
    }
}

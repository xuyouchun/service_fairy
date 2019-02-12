using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf.Service;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Net;
using Common.Communication.Wcf.Encoders;
using Common.Contracts.Service;
using Common.Contracts;

namespace Common.Communication.Wcf.Strategies.WcfConnectionStrategies
{
    /// <summary>
    /// HTTP通信方式
    /// </summary>
    [WcfCommunicationStrategy(CommunicationType.Http)]
    class HttpWcfCommunicationStrategy : WcfCommunicationStrategyBase
    {
        protected override Binding CreateBinding(CommunicationOption option)
        {
            CustomBinding binding = new CustomBinding(new BindingElement[] {
                CreateMessageEncodingBindingElement(option),
                new HttpTransportBindingElement() { MaxBufferPoolSize = WcfSettings.MaxBufferPoolSize, MaxReceivedMessageSize = WcfSettings.MaxMessageSize, MaxBufferSize = WcfSettings.MaxBufferSize }
            });

            return binding;
        }

        public override EntityMessage CreateMessage(CommunicationOption option, CommunicateContext context, CallingSettings settings)
        {
            EntityMessage msg = new EntityMessage();
            msg.Settings = settings;

            if (context != null)
            {
                msg.SessionId = context.SessionId;
                msg.Caller = context.Caller;
            }

            msg.Headers.To = new Uri(CreateUrl(option.Address));
            return msg;
        }

        protected override IWcfMessageEncoderStrategy GetMessageEncoderStrategy()
        {
            return HttpWcfMessageEncoderStrategy.Instance;
        }

        protected override string CreateUrl(ServiceAddress address)
        {
            return "http://" + address + "/s";
        }

        public override WcfConnectionBase CreateConnection(WcfListener listener, CommunicationOption option)
        {
            return new HttpWcfConnection(this, listener, option);
        }
    }
}

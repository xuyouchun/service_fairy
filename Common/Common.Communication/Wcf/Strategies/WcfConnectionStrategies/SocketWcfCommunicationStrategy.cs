using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.ServiceModel.Channels;
using Common.Communication.Wcf.Service;
using Common.Communication.Wcf.Encoders;
using Common.Communication.Wcf.Bindings.SocketTransport;
using System.ServiceModel;

namespace Common.Communication.Wcf.Strategies.WcfConnectionStrategies
{
    /// <summary>
    /// 自定义的Socket方式通信
    /// </summary>
    [WcfCommunicationStrategy(CommunicationType.Tcp)]
    class SocketWcfCommunicationStrategy : WcfCommunicationStrategyBase
    {
        public override WcfConnectionBase CreateConnection(WcfListener listener, CommunicationOption option)
        {
            return new SocketWcfConnection(this, listener, option);
        }

        protected override string CreateUrl(ServiceAddress address)
        {
            return SocketTransportUtility.CreateUrl(address);
        }

        protected override Binding CreateBinding(CommunicationOption option)
        {
            var transportBindingElement = new SocketTransportBindingElement(option) {
                MaxBufferPoolSize = WcfSettings.MaxBufferPoolSize,
                MaxReceivedMessageSize = WcfSettings.MaxMessageSize,
                MaxBufferSize = WcfSettings.MaxBufferSize,
                MaxPendingConnections = 128,
                MaxPendingAccepts = 32,
                TransferMode = TransferMode.Buffered,
            };

            /*
            var reliableSessionBindingElement = new ReliableSessionBindingElement() {
                InactivityTimeout = TimeSpan.FromSeconds(30),
            };*/

            CustomBinding binding = new CustomBinding(new BindingElement[] {
                CreateMessageEncodingBindingElement(option),
                transportBindingElement,
            });

            binding.ReceiveTimeout = TimeSpan.MaxValue;
            binding.SendTimeout = TimeSpan.FromSeconds(30);

            return binding;
        }

        public override EntityMessage CreateMessage(CommunicationOption option, CommunicateContext context = null, CallingSettings settings = null)
        {
            EntityMessage msg = new EntityMessage();
            msg.Settings = settings;

            if (context != null)
            {
                msg.SessionId = context.SessionId;
                msg.Caller = context.Caller;
            }

            msg.Headers.To = new Uri(CreateUrl(option.Address));
            msg.Headers.MessageId = new System.Xml.UniqueId(Guid.NewGuid());
            return msg;
        }

        protected override IWcfMessageEncoderStrategy GetMessageEncoderStrategy()
        {
            return SocketWcfMessageEncoderStrategy.Instance;
        }
    }
}

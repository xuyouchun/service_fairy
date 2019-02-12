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
using System.IO;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Communication.Wcf.Common;

namespace Common.Communication.Wcf.Strategies.WcfConnectionStrategies
{
    /// <summary>
    /// TCP通信方式
    /// </summary>
    [WcfCommunicationStrategy(CommunicationType.WTcp)]
    class TcpWcfCommunicationStrategy : WcfCommunicationStrategyBase
    {
        public override WcfConnectionBase CreateConnection(WcfListener listener, CommunicationOption option)
        {
            return new TcpWcfConnection(this, listener, option);
        }

        protected override Binding CreateBinding(CommunicationOption option)
        {
            var transportBindingElement = new TcpTransportBindingElementEx() {
                MaxBufferPoolSize = WcfSettings.MaxBufferPoolSize,
                MaxReceivedMessageSize = WcfSettings.MaxMessageSize,
                MaxBufferSize = WcfSettings.MaxBufferSize,
                MaxPendingConnections = 128,
                MaxPendingAccepts = 32,
                TransferMode = TransferMode.Buffered,
            };

            CustomBinding binding = new CustomBinding(new BindingElement[]{
                CreateMessageEncodingBindingElement(option),
                transportBindingElement,
            });

            return binding;
        }

        #region Class TcpTransportBindingElementEx ...

        class TcpTransportBindingElementEx : TcpTransportBindingElement
        {
            public TcpTransportBindingElementEx()
            {

            }

            protected TcpTransportBindingElementEx(TcpTransportBindingElementEx elementToBeCloned)
                : base(elementToBeCloned)
            {

            }

            public override string Scheme
            {
                get
                {
                    return "net.wtcp";
                }
            }

            public override BindingElement Clone()
            {
                return new TcpTransportBindingElementEx(this);
            }
        }

        #endregion

        /// <summary>
        /// 创建通信参数
        /// </summary>
        /// <param name="option">通信设置</param>
        /// <param name="context">上下文环境</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
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
            msg.Headers.MessageId = new System.Xml.UniqueId(Guid.NewGuid());
            return msg;
        }

        protected override string CreateUrl(ServiceAddress address)
        {
            return "net.tcp://" + address + "/s";
        }
    }
}

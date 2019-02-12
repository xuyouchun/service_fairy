using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using Common.Communication.Wcf.Bindings;
using Common.Communication.Wcf.Common;
using Common.Communication.Wcf.Encoders;
using Common.Communication.Wcf.Service;
using Common.Contracts;
using Common.Contracts.Service;

namespace Common.Communication.Wcf.Strategies.WcfConnectionStrategies
{
    abstract class WcfCommunicationStrategyBase : IWcfCommunicationStrategy
    {
        public virtual IWcfServiceInterface CreateServiceInterface(WcfConnection owner, CommunicationOption option)
        {
            Binding binding = CreateBinding(option);
            return CreateServiceInterface(owner, option, binding, option.Duplex);
        }

        #region Class WcfServiceInterfaceClient ...

        class WcfServiceInterfaceClient : ClientBase<IWcfServiceInterface>, IWcfServiceInterface
        {
            public WcfServiceInterfaceClient(CustomBinding binding, string url)
                : base(binding, new EndpointAddress(url))
            {

            }

            public Message Request(Message input)
            {
                return base.Channel.Request(input);
            }

            public void OneWay(Message input)
            {
                base.Channel.OneWay(input);
            }

            public Stream OpenStream(Stream input)
            {
                return base.Channel.OpenStream(input);
            }
        }

        #endregion

        public virtual ServiceHost CreateServiceHost(WcfListener listener, CommunicationOption option)
        {
            ServiceHost serviceHost = new ServiceHost(GetServiceImplementType(option, listener));
            Binding binding = CreateBinding(option);
            serviceHost.AddServiceEndpoint(GetServiceType(option), binding, CreateUrl(option.Address));
            return serviceHost;
        }

        protected virtual IWcfServiceInterface CreateServiceInterface(WcfConnection owner, CommunicationOption option, Binding binding, bool? supportDuplex)
        {
            if (supportDuplex != null)
                return CreateServiceInterface(owner, option, binding, (bool)supportDuplex);

            try
            {
                return CreateServiceInterface(owner, option, binding, true);
            }
            catch
            {
                return CreateServiceInterface(owner, option, binding, false);
            }
        }

        protected virtual IWcfServiceInterface CreateServiceInterface(WcfConnection owner, CommunicationOption option, Binding binding, bool supportDuplex)
        {
            if (supportDuplex == true)
            {
                InstanceContext instanceContext = new InstanceContext(new WcfServiceCallback(owner));
                DuplexChannelFactory<IDuplexWcfServiceInterface> factory = new DuplexChannelFactory<IDuplexWcfServiceInterface>(instanceContext,
                    binding, new EndpointAddress(CreateUrl(option.Address)));

                return new WcfServiceInterfaceProxy(factory.CreateChannel(), factory);
            }
            else
            {
                return ChannelFactory<IWcfServiceInterface>.CreateChannel(binding, new EndpointAddress(CreateUrl(option.Address)));
            }
        }

        /// <summary>
        /// 创建服务的Url
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        protected abstract string CreateUrl(ServiceAddress address);

        /// <summary>
        /// 创建通信的通道
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        protected abstract Binding CreateBinding(CommunicationOption option);

        /// <summary>
        /// 创建服务的实体
        /// </summary>
        /// <param name="option"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        protected virtual Type GetServiceImplementType(CommunicationOption option, WcfListener listener)
        {
            if (option.Duplex == true)
                return typeof(DuplexWcfServiceImplement);

            return typeof(WcfServiceImplement);
        }

        protected virtual Type GetServiceType(CommunicationOption option)
        {
            if (option.Duplex == true)
                return typeof(IDuplexWcfServiceInterface);

            return typeof(IWcfServiceInterface);
        }

        public virtual EntityMessage Invoke(WcfListener listener, EntityMessage input)
        {
            WcfConnectionBase connection = listener.GetCurrentConnection();

            CommunicateData data = connection.RaiseDataReceivedEvent(input);
            EntityMessage msg = connection.Reply(input, data);
            if (msg != null && msg.Data == null)
                msg.Data = new CommunicateData(null, input.Data.DataFormat, ServiceStatusCode.Ok);

            return msg;
        }

        private EntityMessage _CreateReplyMessage(CommunicateData data, EntityMessage input, WcfListener listener)
        {
            EntityMessage msg = CreateMessage(listener.Option);
            msg.Method = input.Method;
            msg.Headers.Action = input.Headers.Action;
            msg.Data = data;

            if (input.Headers.MessageId != null)
            {
                msg.Headers.RelatesTo = input.Headers.MessageId;
                input.Headers.MessageId = null;
            }

            return msg;
        }

        /// <summary>
        /// 创建Message
        /// </summary>
        /// <param name="option">通信设置</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public abstract EntityMessage CreateMessage(CommunicationOption option, CommunicateContext context = null, CallingSettings settings = null);

        protected virtual MessageEncodingBindingElement CreateMessageEncodingBindingElement(CommunicationOption option)
        {
            return new CommonMessageEncodingBindingElement(option, GetMessageEncoderStrategy(), WcfSettings.MessageVersion, WcfSettings.ContentType, WcfSettings.MediaType);
        }

        protected virtual IWcfMessageEncoderStrategy GetMessageEncoderStrategy()
        {
            return EmptyWcfMessageEncoderStrategy.Instance;
        }

        #region Class EmptyWcfMessageEncoderStrategy ...

        class EmptyWcfMessageEncoderStrategy : IWcfMessageEncoderStrategy
        {
            public bool Require(EntityMessageHeader header)
            {
                return true;
            }

            public bool OnewayWhenNoMessageId()
            {
                return false;
            }

            public static readonly EmptyWcfMessageEncoderStrategy Instance = new EmptyWcfMessageEncoderStrategy();
        }

        #endregion

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public abstract WcfConnectionBase CreateConnection(WcfListener listener, CommunicationOption option);

        #region Class WcfServiceInterfaceProxy ...

        protected class WcfServiceInterfaceProxy : CommunicationObjectAdapterBase, IDuplexWcfServiceInterface, IDisposable
        {
            private readonly IDuplexWcfServiceInterface _wcfServiceInterface;
            private readonly DuplexChannelFactory<IDuplexWcfServiceInterface> _factory;

            public WcfServiceInterfaceProxy(IDuplexWcfServiceInterface wcfServiceInterface, DuplexChannelFactory<IDuplexWcfServiceInterface> factory)
                : base((ICommunicationObject)wcfServiceInterface)
            {
                _wcfServiceInterface = wcfServiceInterface;
                _factory = factory;
            }

            public Message Request(Message input)
            {
                return _wcfServiceInterface.Request(input);
            }

            public void OneWay(Message input)
            {
                _wcfServiceInterface.OneWay(input);
            }

            public Stream OpenStream(Stream input)
            {
                return _wcfServiceInterface.OpenStream(input);
            }

            /*
            public void Duplex(Message input)
            {
                _wcfServiceInterface.Duplex(input);
            }

            public Message DuplexRequest(Message input)
            {
                return _wcfServiceInterface.DuplexRequest(input);
            }*/

            public void Dispose()
            {
                GC.SuppressFinalize(this);
                ((IDisposable)_factory).Dispose();
            }

            ~WcfServiceInterfaceProxy()
            {
                Dispose();
            }
        }

        #endregion
    }
}

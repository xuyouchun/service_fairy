using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Communication.Wcf.Service;
using System.ServiceModel;

namespace Common.Communication.Wcf.Strategies.WcfConnectionStrategies
{
    abstract class WcfConnectionBase : WcfConnection
    {
        public WcfConnectionBase(WcfCommunicationStrategyBase owner, WcfListener listener, CommunicationOption info)
        {
            _owner = owner;
            _info = info;
            Listener = listener;

            State = ConnectionState.Opened;

            IServiceChannel channel = OperationContext.Current.GetCallbackChannel<IServiceChannel>();
            _callback = channel as IWcfServiceCallback;
        }

        private readonly WcfCommunicationStrategyBase _owner;

        private readonly IWcfServiceCallback _callback;

        protected void ValidateCallback()
        {
            if (_callback == null)
                throw new NotSupportedException("不支持双向通信");
        }

        /// <summary>
        /// 回调
        /// </summary>
        public IWcfServiceCallback Callback
        {
            get { return _callback; }
        }

        public WcfCommunicationStrategyBase Owner
        {
            get { return _owner; }
        }

        private readonly CommunicationOption _info;

        public WcfListener Listener { get; private set; }

        public object Response { get; protected set; }

        public override CommunicationOption Option
        {
            get { return _info; }
        }

        protected override void OnOpen()
        {
            throw new NotSupportedException();
        }

        protected override CommunicateData OnSend(CommunicateContext context, string method, CommunicateData data, CallingSettings settings)
        {
            ValidateCallback();

            if (settings == null)
                settings = CallingSettings.RequestReply;

            EntityMessage input = Owner.CreateMessage(Option, context, settings);
            input.Method = method;
            input.Data = data;
            input.Headers.Action = WcfRequestActions.GetAction(settings.InvokeType);
            input.Settings = settings;

            EntityMessage output;

            switch (settings.GetDirection())
            {
                case CommunicateInvokeType.RequestReply:
                    output = (EntityMessage)Callback.Request(input);
                    break;

                case CommunicateInvokeType.OneWay:
                    Callback.OneWay(input);
                    output = null;
                    break;

                default:
                    throw new NotSupportedException("不支持的通信方式");
            }
            
            return output == null ? null : output.Data;
        }

        protected override void OnClose()
        {

        }

        protected override void OnDispose()
        {

        }

        protected override WcfConnectionOperation GetSupportedOperations()
        {
            return (WcfConnectionOperation)0;
        }

        protected override System.IO.Stream OnOpenStream()
        {
            throw new NotSupportedException();
        }
    }
}

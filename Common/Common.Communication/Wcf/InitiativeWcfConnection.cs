using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf.Service;
using Common.Communication.Wcf.Strategies;
using Common.Contracts;
using Common.Contracts.Service;
using System.Xml;
using Common.Package;
using Common.Package.Cache;
using System.Threading;
using Common.Package.Service;
using System.ServiceModel;
using System.Net.Sockets;
using System.IO;
using Common.Utility;

namespace Common.Communication.Wcf
{
    /// <summary>
    /// 主动的WCF连接
    /// </summary>
    class InitiativeWcfConnection : WcfConnection
    {
        public InitiativeWcfConnection(WcfService wcfService, CommunicationOption option)
        {
            _wcfService = wcfService;
            _option = option;
        }

        private readonly WcfService _wcfService;
        private readonly CommunicationOption _option;
        private readonly object _thisLock = new object();

        public override CommunicationOption Option
        {
            get { return _option; }
        }

        private volatile IWcfServiceInterface _interface;

        protected override void OnOpen()
        {
            lock (_thisLock)
            {
                _CreateNewWcfServiceInterface();
            }
        }

        private void _CreateNewWcfServiceInterface()
        {
            ICommunicationObject comObj = _interface as ICommunicationObject;
            if (comObj != null)
                _Close(comObj);

            _interface = WcfFactory.CreateConnectionStrategy(Option.Type).CreateServiceInterface(this, Option);
            comObj = (_interface as ICommunicationObject);
            if (comObj != null)
                comObj.Open();
        }

        private static readonly SessionStateManager _sessionStateManager = new SessionStateManager();

        private IWcfServiceInterface _GetWcfServiceInterface()
        {
            lock (_thisLock)
            {
                ICommunicationObject comObj;
                if (_interface == null || ((comObj = _interface as ICommunicationObject) != null && comObj.State != CommunicationState.Opened))
                    _CreateNewWcfServiceInterface();

                return _interface;
            }
        }

        protected override CommunicateData OnSend(CommunicateContext context, string method, CommunicateData data, CallingSettings settings)
        {
            settings = settings ?? CallingSettings.RequestReply;

            EntityMessage input = WcfUtility.CreateMessage(Option, context, settings), output = null;
            input.Method = method;
            input.Data = data;

            for (int tryTimes = 1; ; tryTimes++)
            {
                IWcfServiceInterface wcfInterface = _GetWcfServiceInterface();
                input.Headers.Action = WcfRequestActions.GetAction(settings.InvokeType);

                try
                {
                    switch (settings.GetDirection())
                    {
                        case CommunicateInvokeType.RequestReply:  // 单向请求应答
                            output = (EntityMessage)wcfInterface.Request(input);
                            break;

                        case CommunicateInvokeType.OneWay:  // 单向无应答
                            ((IWcfServiceInterface)wcfInterface).OneWay(input);
                            break;

                        case CommunicateInvokeType.OpenStream:  // 流连接
                            throw new NotSupportedException("应使用OpenStream方法打开流连接");
                    }

                    break;
                }
                catch (Exception ex)
                {
                    ICommunicationObject comObj = wcfInterface as ICommunicationObject;
                    if (tryTimes < settings.TryTimes &&
                        ((ex is CommunicationException)) || (ex is ObjectDisposedException)
                        || (comObj != null && comObj.State == CommunicationState.Faulted) || ex.IsCauseBy<SocketException>())
                    {
                        lock (_thisLock)
                        {
                            if (object.ReferenceEquals(_interface, wcfInterface))
                                _CreateNewWcfServiceInterface();

                            continue;
                        }
                    }

                    throw new ServiceException((int)ClientErrorCode.NetworkError, ex.Message, ex);
                }
            }

            return output == null ? null : output.Data;
        }

        /// <summary>
        /// 打开流连接
        /// </summary>
        /// <returns></returns>
        protected override Stream OnOpenStream()
        {
            Stream input = new DuplexStream();
            Stream output = _GetWcfServiceInterface().OpenStream(input);

            return new WcfStream(output, input);
        }

        /// <summary>
        /// 对请求的应答，用于Duplex双向通信中
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <param name="rspData"></param>
        internal protected override EntityMessage Reply(EntityMessage reqMsg, CommunicateData rspData)
        {
            CallingSettings settings = CallingSettings.OneWay;
            EntityMessage rspMsg = WcfUtility.CreateMessage(Option);
            rspMsg.Data = rspData;
            rspMsg.Headers.Action = WcfRequestActions.GetAction(settings.InvokeType);
            rspMsg.Headers.RelatesTo = reqMsg.Headers.MessageId;
            rspMsg.Settings = settings;
            _interface.OneWay(rspMsg);

            return null;
        }

        protected override void OnClose()
        {
            ICommunicationObject comObj = _interface as ICommunicationObject;
            if (comObj != null)
                _Close(comObj);

            _interface = null;
        }

        private void _Close(ICommunicationObject comObj)
        {
            try
            {
                comObj.Close();
            }
            catch (Exception)
            {
                CommonUtility.TryInvoke(comObj.Abort);
            }
        }

        protected override void OnDispose()
        {
            if (_wcfService != null)
                _wcfService.RemoveConnection(this);

            OnClose();
        }

        protected override WcfConnectionOperation GetSupportedOperations()
        {
            return WcfConnectionOperation.Open | WcfConnectionOperation.Close | WcfConnectionOperation.Send;
        }
    }
}

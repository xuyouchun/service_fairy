using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Diagnostics.Contracts;
using Common.Communication.Wcf.Service;
using System.ServiceModel;
using Common.Communication.Wcf.Strategies;
using Common.Contracts;
using Common.Contracts.Service;
using System.Runtime.Remoting;
using System.Xml;

namespace Common.Communication.Wcf
{
    /// <summary>
    /// WCF连接
    /// </summary>
    public abstract class WcfConnection : MarshalByRefObjectEx, IConnection, ICommunicate, IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal WcfConnection()
        {
            
        }
        
        private volatile bool _disposed = false;
        private readonly object _thisLock = new object();
        private volatile ConnectionState _state = ConnectionState.Created;

        public abstract CommunicationOption Option { get; }

        /// <summary>
        /// 连接的状态
        /// </summary>
        public ConnectionState State
        {
            get { return _state; }
            internal protected set
            {
                if (_state != value)
                {
                    _state = value;
                    _RaiseStateChangedEvent();
                }
            }
        }

        private Exception _CreateObjectedDisposedException()
        {
            return new ObjectDisposedException(this.GetType().ToString());
        }

        /// <summary>
        /// 打开连接
        /// </summary>
        public void Open()
        {
            lock (_thisLock)
            {
                if (_disposed)
                    throw _CreateObjectedDisposedException();

                if (_state == ConnectionState.Opened)
                    return;

                OnOpen();

                State = ConnectionState.Opened;
            }
        }

        protected abstract void OnOpen();

        private void _ThrowIfDisposedOrNotConnect()
        {
            if (_disposed)
                throw _CreateObjectedDisposedException();

            if (_state != ConnectionState.Opened)
            {
                throw new InvalidOperationException(State == ConnectionState.Created ? "连接尚未开启" :
                    State == ConnectionState.Closed ? "连接已关闭" : "连接目前不是开启状态");
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="context">上下文环境</param>
        /// <param name="method">方法</param>
        /// <param name="data">通信数据</param>
        /// <param name="waitForReply">请求类型</param>
        public CommunicateData Send(CommunicateContext context, string method, CommunicateData data, CallingSettings settings = null)
        {
            Contract.Requires(method != null && data != null);

            _ThrowIfDisposedOrNotConnect();
            return OnSend(context, method, data, settings);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="context">上下文环境</param>
        /// <param name="method">方法</param>
        /// <param name="data">通信数据</param>
        /// <param name="format">数据序列化方式</param>
        /// <param name="settings">通信设置</param>
        /// <returns></returns>
        public CommunicateData Send(CommunicateContext context, string method, byte[] data, DataFormat format = DataFormat.Unknown, CallingSettings settings = null)
        {
            return Send(context, method, new CommunicateData(data, format), settings);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="method">方法</param>
        /// <param name="data">通信数据</param>
        /// <param name="format">数据序列化方式</param>
        /// <param name="settings">通信设置</param>
        /// <returns></returns>
        public CommunicateData Send(string method, CommunicateData data, CallingSettings settings = null)
        {
            return Send(null, method, data, settings);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="method">方法</param>
        /// <param name="data">通信数据</param>
        /// <param name="format">数据序列化方式</param>
        /// <param name="settings">通信设置</param>
        /// <returns></returns>
        public CommunicateData Send(string method, byte[] data, DataFormat format = DataFormat.Unknown, CallingSettings settings = null)
        {
            return Send(null, method, data, format, settings);
        }

        protected abstract CommunicateData OnSend(CommunicateContext context, string method, CommunicateData data, CallingSettings settings);

        /// <summary>
        /// 打开流
        /// </summary>
        /// <returns></returns>
        public Stream OpenStream()
        {
            return OnOpenStream();
        }

        /// <summary>
        /// 打开流
        /// </summary>
        /// <returns></returns>
        protected abstract Stream OnOpenStream();

        /// <summary>
        /// 数据的应答
        /// </summary>
        /// <param name="reqMsg"></param>
        /// <param name="rspData"></param>
        internal protected abstract EntityMessage Reply(EntityMessage reqMsg, CommunicateData rspData);

        /// <summary>
        /// 收到消息
        /// </summary>
        public event ConnectionDataReceivedEventHandler Received;

        CommunicateData ICommunicate.Call(CommunicateContext context, string method, CommunicateData data, CallingSettings settings)
        {
            return Send(context, method, data, settings);
        }

        internal virtual CommunicateData RaiseDataReceivedEvent(EntityMessage message)
        {
            CommunicateData replyData = null;
            var eh = Received;
            if (eh != null)
            {
                ServiceAddress from = WcfUtility.GetCurrentServiceAddress();
                CommunicateContext context = new CommunicateContext(from, message.Caller, message.SessionId);
                ConnectionDataReceivedEventArgs e = new ConnectionDataReceivedEventArgs(this, context, message.Method, message.Data, message.Settings);

                eh(this, e);
                replyData = e.ReplyData;
            }

            if (replyData == null && message.Settings != null && message.Settings.NeedReply())
                replyData = new CommunicateData(null, message.Data.DataFormat, (int)ServerErrorCode.NoData);

            return replyData;
        }

        /// <summary>
        /// 连接关闭
        /// </summary>
        public event EventHandler StateChanged;

        private void _RaiseStateChangedEvent()
        {
            EventHandler eh = StateChanged;
            if (eh != null)
                eh(this, EventArgs.Empty);
        }

        /// <summary>
        /// 连接关闭
        /// </summary>
        public void Close()
        {
            lock (_thisLock)
            {
                if (_disposed)
                    throw new ObjectDisposedException(this.GetType().ToString());

                if (State == ConnectionState.Closed)
                    return;

                OnClose();
                State = ConnectionState.Closed;
            }
        }

        protected abstract void OnClose();

        /// <summary>
        /// 是否为双向连接
        /// </summary>
        public bool Duplex
        {
            get { return Option.Duplex; }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);

            lock (_thisLock)
            {
                if (_disposed)
                    return;

                Close();
                OnDispose();

                _disposed = true;
            }
        }

        protected abstract void OnDispose();

        /// <summary>
        /// 查询该连接对象支持的操作
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public bool IsSupport(WcfConnectionOperation operation)
        {
            return (GetSupportedOperations() & operation) == operation;
        }

        protected abstract WcfConnectionOperation GetSupportedOperations();        

        ~WcfConnection()
        {
            if (!_disposed)
                Dispose();
        }
    }

    /// <summary>
    /// WcfConnection支持的操作
    /// </summary>
    [Flags]
    public enum WcfConnectionOperation
    {
        None = 0x00,

        /// <summary>
        /// 打开
        /// </summary>
        Open = 0x01,

        /// <summary>
        /// 关闭
        /// </summary>
        Close = 0x02,

        /// <summary>
        /// 发送
        /// </summary>
        Send = 0x04,
    }
}

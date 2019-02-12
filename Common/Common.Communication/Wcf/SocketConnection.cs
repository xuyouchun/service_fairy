using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Net.Sockets;
using Common.Communication.Wcf.Service;
using System.Diagnostics.Contracts;
using Common.Communication.Wcf.Encoders;
using Common.Utility;
using Common.Package;
using System.Threading;
using Common.Communication.Wcf.Bindings.SocketTransport;
using Common.Communication.Wcf.Bindings;
using System.ServiceModel.Channels;
using Common.Package.Service;
using Common.Communication.Wcf.Strategies.WcfConnectionStrategies;

namespace Common.Communication.Wcf
{
    /// <summary>
    /// 用TCP自定义格式保持的长连接
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class SocketConnection : IConnection, ICommunicate
    {
        public SocketConnection(CommunicationOption option)
        {
            Contract.Requires(option != null);

            Option = option;
        }

        public CommunicationOption Option { get; private set; }
        private readonly object _syncLocker = new object();
        private volatile bool _disposed = false;

        /// <summary>
        /// 打开连接
        /// </summary>
        public void Open()
        {
            lock (_syncLocker)
            {
                _ValidateDisposed();

                if (_tcpClient == null)
                {
                    _tcpClient = _CreateTcpClient();
                    State = ConnectionState.Opened;
                }
            }
        }

        private void _ValidateDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().ToString());
        }

        private TcpClient _tcpClient;

        private TcpClient _GetTcpClient()
        {
            lock (_syncLocker)
            {
                _ValidateDisposed();

                if (State != ConnectionState.Opened)
                {
                    throw new InvalidOperationException(State == ConnectionState.Created ? "连接尚未开启" :
                        State == ConnectionState.Closed ? "连接已关闭" : "连接目前不是开启状态");
                }

                return _tcpClient;
            }
        }

        private TcpClient _CreateTcpClient()
        {
            TcpClient tc = new TcpClient();
            ServiceAddress sa = Option.Address;
            tc.Connect(sa.Address, sa.Port);

            ReceiveState state = new ReceiveState(tc, _bufferManager) { Adapter = new WcfConnectionAdapter(this, tc) };
            tc.Client.BeginReceive(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, _ReceiveCallback, state);

            return tc;
        }

        private readonly BufferManager _bufferManager = SocketTransportUtility.CreateBufferManager();

        #region Class ReceiveState ...

        class ReceiveState
        {
            public ReceiveState(TcpClient tc, BufferManager bufferManager)
            {
                CommunicationOption option = new CommunicationOption(
                    ServiceAddress.Parse(tc.Client.RemoteEndPoint.ToString()), CommunicationType.Tcp, true);

                AnalyzeQueue = new MessageAnalyzeQueue(new CommonMessageEncodingBindingElement(option, SocketWcfMessageEncoderStrategy.Instance), bufferManager);
                Client = tc;
            }

            public readonly TcpClient Client;
            public readonly byte[] Buffer = new byte[1024];
            public readonly MessageAnalyzeQueue AnalyzeQueue;
            public WcfConnectionAdapter Adapter;
        }

        #endregion

        private void _ReceiveCallback(IAsyncResult ar)
        {
            ReceiveState state = (ReceiveState)ar.AsyncState;
            TcpClient tc = state.Client;
            if (!tc.Connected)
                return;

            try
            {
                int length = tc.Client.EndReceive(ar);
                do
                {
                    state.AnalyzeQueue.Enqueue(state.Buffer, 0, length);
                } while (tc.Available > 0 && (length = tc.Client.Receive(state.Buffer)) > 0);

                EntityMessage msg;
                while ((msg = state.AnalyzeQueue.Dequeue() as EntityMessage) != null)
                {
                    _TryAccept(state, msg);
                }

                if (state.Client.Connected)
                    tc.Client.BeginReceive(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, _ReceiveCallback, state);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
            }
        }

        private bool _TryAccept(ReceiveState state, EntityMessage msg)
        {
            try
            {
                _Accept(state, msg);
                return true;
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return false;
            }
        }

        private void _Accept(ReceiveState state, EntityMessage msg)
        {
            if (msg.Headers.RelatesTo != null)  // 应答数据
            {
                _sessionStateManager.ApplyResponse(msg);
            }
            else
            {
                _RaiseDataReceivedEvent(state, msg);
            }
        }

        /// <summary>
        /// 接收到数据
        /// </summary>
        public event ConnectionDataReceivedEventHandler Received;

        private void _RaiseDataReceivedEvent(ReceiveState state, EntityMessage msg)
        {
            var eh = Received;
            if (eh != null)
            {
                ConnectionDataReceivedEventArgs e = new ConnectionDataReceivedEventArgs(state.Adapter,
                    new CommunicateContext(Option.Address, msg.Caller, msg.SessionId), msg.Method, msg.Data, msg.Settings);

                eh(this, e);
            }
        }

        #region Class WcfConnectionAdapter ...

        class WcfConnectionAdapter : WcfConnection
        {
            public WcfConnectionAdapter(SocketConnection owner, TcpClient tc)
            {
                _owner = owner;
                _tc = tc;
            }

            private readonly SocketConnection _owner;
            private readonly TcpClient _tc;

            public override CommunicationOption Option
            {
                get { return _owner.Option; }
            }

            protected override void OnOpen()
            {
                
            }

            protected override CommunicateData OnSend(CommunicateContext context, string method, CommunicateData data, CallingSettings settings)
            {
                return _owner._Send(_tc, context, method, data, settings);
            }

            protected override System.IO.Stream OnOpenStream()
            {
                throw new NotSupportedException();
            }

            protected internal override EntityMessage Reply(EntityMessage reqMsg, CommunicateData rspData)
            {
                throw new NotSupportedException();
            }

            protected override void OnClose()
            {
                _tc.Close();
            }

            protected override void OnDispose()
            {
                _tc.Dispose();   
            }

            protected override WcfConnectionOperation GetSupportedOperations()
            {
                return WcfConnectionOperation.Send;
            }
        }

        #endregion

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="context"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public CommunicateData Send(CommunicateContext context, string method, CommunicateData data, CallingSettings settings = null)
        {
            return _Send(_GetTcpClient(), context, method, data, settings);
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public CommunicateData Send(string method, CommunicateData data, CallingSettings settings = null)
        {
            return Send(null, method, data, settings);
        }

        /// <summary>
        /// 调用
        /// </summary>
        /// <param name="context"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        CommunicateData ICommunicate.Call(CommunicateContext context, string method, CommunicateData data, CallingSettings settings = null)
        {
            return Send(context, method, data, settings);
        }

        private CommunicateData _Send(TcpClient tc, CommunicateContext context, string method, CommunicateData data, CallingSettings settings = null)
        {
            SessionState sessionState =  null;
            settings = settings ?? CallingSettings.RequestReply;

            EntityMessage input = WcfUtility.CreateMessage(Option, context, settings);
            input.Method = method;
            input.Data = data;

            if (settings.GetDirection() == CommunicateInvokeType.RequestReply)
                sessionState = _sessionStateManager.Create(input);

            ArraySegment<byte> inputTypes = EncoderFactory.CreateEncoder(data.DataFormat)
                .Serialize(input, WcfSettings.MaxMessageSize, _bufferManager, 0, SocketWcfMessageEncoderStrategy.Instance);

            try
            {
                NetworkStream ns = tc.GetStream();
                lock (ns)
                {
                    ns.Write(inputTypes.Count);
                    ns.Write(inputTypes);
                    ns.Flush();
                }
            }
            catch (Exception)
            {
                if (sessionState != null)
                    sessionState.Dispose();

                State = ConnectionState.Closed;
                throw;
            }
            finally
            {
                _bufferManager.ReturnBuffer(inputTypes.Array);
            }

            // 如果是请求应答方式，则在此等待
            if (sessionState != null)
            {
                EntityMessage outputMsg = sessionState.WaitWithException();
                return outputMsg == null ? null : outputMsg.Data;
            }

            return null;
        }

        /// <summary>
        /// 双向连接
        /// </summary>
        public bool Duplex
        {
            get { return true; }
        }

        private volatile ConnectionState _state;

        /// <summary>
        /// 目前连接的状态
        /// </summary>
        public ConnectionState State
        {
            get { return _state; }
            private set
            {
                _state = value;
            }
        }

        private readonly SessionStateManager _sessionStateManager = new SessionStateManager();

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            lock (_syncLocker)
            {
                _disposed = true;

                if (_tcpClient != null)
                    _tcpClient.Close();

                _sessionStateManager.Dispose();

                State = ConnectionState.Closed;
            }
        }

        ~SocketConnection()
        {
            Dispose();
        }
    }
}

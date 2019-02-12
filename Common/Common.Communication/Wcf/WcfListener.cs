using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ServiceModel;
using Common.Communication.Wcf.Service;
using System.ServiceModel.Description;
using Common.Communication.Wcf.Strategies;
using Common.Communication.Wcf.Bindings;
using Common.Communication.Wcf.Behaviors;
using Common.Package;
using Common.Package.Cache;
using Common.Utility;
using Common.Contracts.Service;
using System.Collections.Concurrent;
using System.Threading;
using Common.Communication.Wcf.Strategies.WcfConnectionStrategies;
using System.ServiceModel.Channels;

namespace Common.Communication.Wcf
{
    /// <summary>
    /// WCF侦听器
    /// </summary>
    public class WcfListener : IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">所属service</param>
        /// <param name="option">终端信息</param>
        /// <exception cref="ArgumentNullException">endpointInfo为空</exception>
        private WcfListener(WcfService service, CommunicationOption option)
        {
            Contract.Requires(option != null);

            Service = service;
            Option = option;
        }

        private readonly object _thisLock = new object();

        /// <summary>
        /// 终端基础信息
        /// </summary>
        public CommunicationOption Option { get; private set; }

        /// <summary>
        /// 服务
        /// </summary>
        public WcfService Service { get; private set; }

        private volatile ServiceHost _serviceHost;
        private volatile bool _disposed = false;

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            if (_disposed)
                throw new ObjectDisposedException("WcfListener");

            lock (_thisLock)
            {
                if (_status == WcfHostServiceStatus.Running)
                    return;

                _Start();
                _status = WcfHostServiceStatus.Running;
            }
        }

        private IWcfCommunicationStrategy _strategy;

        private IWcfCommunicationStrategy _GetStrategy()
        {
            return _strategy ?? (_strategy = WcfFactory.CreateConnectionStrategy(Option.Type));
        }

        private void _Start()
        {
            try
            {
                if (_serviceHost == null)
                {
                    _serviceHost = _GetStrategy().CreateServiceHost(this, Option);

                    // 并发控制
                    ServiceThrottlingBehavior throttlingBehavior = _serviceHost.Description.Behaviors.Find<ServiceThrottlingBehavior>();
                    if (throttlingBehavior == null)
                    {
                        throttlingBehavior = new ServiceThrottlingBehavior();
                        _serviceHost.Description.Behaviors.Add(throttlingBehavior);
                    }

                    throttlingBehavior.MaxConcurrentCalls = 512;
                    throttlingBehavior.MaxConcurrentInstances = 1024;
                    throttlingBehavior.MaxConcurrentSessions = 8192;

                    // 其它行为的控制
                    foreach (ServiceEndpoint endpoint in _serviceHost.Description.Endpoints)
                    {
                        endpoint.Behaviors.Add(new EndpointBehavior(this));
                        foreach (OperationDescription op in endpoint.Contract.Operations)
                        {
                            op.Behaviors.Add(new OperationBehavior(this));
                            //op.Behaviors.Remove<DataContractSerializerOperationBehavior>();
                            //DelegatingFormatterBehavior
                        }
                    }
                }

                LogManager.LogMessage("正在启动侦听 " + Option + " ...");
                _serviceHost.Open();
            }
            catch (Exception)
            {
                _serviceHost = null;
                throw;
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            lock (_thisLock)
            {
                if (_status == WcfHostServiceStatus.Stop)
                    return;

                _Stop();
                _status = WcfHostServiceStatus.Stop;
            }
        }

        private void _Stop()
        {
            if (_serviceHost != null)
            {
                LogManager.LogMessage("正在停止侦听" + Option + " ...");

                _serviceHost.Close();
                ((IDisposable)_serviceHost).Dispose();
                _serviceHost = null;
            }
        }

        private volatile WcfHostServiceStatus _status = WcfHostServiceStatus.Stop;

        /// <summary>
        /// 状态
        /// </summary>
        public WcfHostServiceStatus Status
        {
            get { return _status; }
        }

        /// <summary>
        /// 新连接建立
        /// </summary>
        public event EventHandler<WcfListenerConnectedEventArgs> Connected;

        /// <summary>
        /// 连接断开
        /// </summary>
        public event EventHandler<WcfListenerDisconnectedEventArgs> Disconnected;

        private void _RaiseConnectedEvent(IConnection[] connections)
        {
            var eh = Connected;
            if (eh != null)
                eh(this, new WcfListenerConnectedEventArgs(connections));
        }

        /// <summary>
        /// 添加一个连接
        /// </summary>
        /// <param name="connection"></param>
        private void _AddConnection(IConnection connection)
        {
            ConnectionItem connectionItem = new ConnectionItem(connection);
            if (_connectionDict.TryAdd(connection, connectionItem))
            {
                _RaiseConnectedEvent(new[] { connection });
            }
        }

        /// <summary>
        /// 删除一个连接
        /// </summary>
        /// <param name="connection"></param>
        private void _RemoveConnection(IConnection connection)
        {
            ConnectionItem connectionItem;
            if (_connectionDict.TryRemove(connection, out connectionItem))
            {
                connection.Dispose();
                _RaiseDisconnectedEvent(new[] { connection });
            }
        }

        /// <summary>
        /// 获取当前的连接对象
        /// </summary>
        /// <returns></returns>
        internal WcfConnectionBase GetCurrentConnection()
        {
            MyExtension extension = _GetExtension();
            return extension.Connection;
        }

        private CommunicationOption _GetCurrentCommunicationOption()
        {
            OperationContext opCtx = OperationContext.Current;
            if (!opCtx.IncomingMessageProperties.ContainsKey(RemoteEndpointMessageProperty.Name))
                return null;

            RemoteEndpointMessageProperty property = opCtx.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            return new CommunicationOption(new ServiceAddress(property.Address, property.Port), Option.Type, Option.Duplex);
        }

        private MyExtension _GetExtension()
        {
            OperationContext opCtx = OperationContext.Current;
            MyExtension extension = opCtx.Channel.Extensions.Find<MyExtension>();
            if (extension == null)
            {
                WcfConnectionBase connection = _GetStrategy().CreateConnection(this, _GetCurrentCommunicationOption());
                extension = new MyExtension(this, connection);
                opCtx.Channel.Extensions.Add(extension);
                opCtx.Channel.Closed += (sender, args) => this._RemoveConnection(connection);

                _AddConnection(connection);
            }

            return extension;
        }

        #region Class MyExtension ...

        class MyExtension : IExtension<IContextChannel>
        {
            public MyExtension(WcfListener listener, WcfConnectionBase connection)
            {
                Connection = connection;
            }

            public WcfConnectionBase Connection { get; private set; }

            public void Attach(IContextChannel owner)
            {

            }

            public void Detach(IContextChannel owner)
            {

            }
        }

        #endregion

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            lock (_thisLock)
            {
                if (!_disposed)
                {
                    _disposed = true;

                    _Stop();
                    Service.RemoveListener(this);
                }
            }
        }

        /// <summary>
        /// 创建监听器
        /// </summary>
        /// <param name="service"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        internal static WcfListener Create(WcfService service, CommunicationOption info)
        {
            return new WcfListener(service, info);
        }

        private readonly ConcurrentDictionary<IConnection, ConnectionItem> _connectionDict
             = new ConcurrentDictionary<IConnection, ConnectionItem>();

        #region Class ConnectionItem ...

        class ConnectionItem
        {
            public ConnectionItem(IConnection connection)
            {
                Connection = connection;
                ConnectedTime = DateTime.UtcNow;

                Index = Interlocked.Increment(ref _index);
            }

            private static long _index;

            public long Index { get; private set; }

            public IConnection Connection { get; private set; }

            public DateTime ConnectedTime { get; private set; }

            public override string ToString()
            {
                return string.Format("标识:{0} 连接时间(UTC)：{1}", Index, ConnectedTime);
            }
        }

        #endregion

        /// <summary>
        /// 断开连接的通知
        /// </summary>
        /// <param name="connection"></param>
        internal void Disconnect(WcfConnection connection)
        {
            
        }

        private void _RaiseDisconnectedEvent(IConnection[] connections)
        {
            var eh = Disconnected;
            if (eh != null)
                eh(this, new WcfListenerDisconnectedEventArgs(connections));
        }
    }

    /// <summary>
    /// WCF通信服务的状态
    /// </summary>
    public enum WcfHostServiceStatus
    {
        /// <summary>
        /// 正在运行
        /// </summary>
        Running,

        /// <summary>
        /// 暂停
        /// </summary>
        Pause,

        /// <summary>
        /// 停止
        /// </summary>
        Stop,
    }

    /// <summary>
    /// 终端连接到本服务事件参数
    /// </summary>
    public class WcfListenerConnectedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connections"></param>
        internal WcfListenerConnectedEventArgs(IConnection[] connections)
        {
            Connections = connections;
        }

        /// <summary>
        /// 连接对象
        /// </summary>
        public IConnection[] Connections { get; private set; }
    }

    /// <summary>
    /// 终端断开连接的事件参数
    /// </summary>
    public class WcfListenerDisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connections"></param>
        internal WcfListenerDisconnectedEventArgs(IConnection[] connections)
        {
            Connections = connections;
        }

        /// <summary>
        /// 连接对象
        /// </summary>
        public IConnection[] Connections { get; private set; }
    }
}

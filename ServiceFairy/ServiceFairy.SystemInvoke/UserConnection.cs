using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using Common.Communication.Wcf;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Package;
using Common.Package.Service;
using Common.Utility;
using ServiceFairy.Entities.Navigation;
using ServiceFairy.Entities.Sys;
using ServiceFairy.Entities.User;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 用户连接的接口
    /// </summary>
    public interface IUserConnection : IServiceClient, IDisposable
    {
        /// <summary>
        /// 当前是否为连接状态
        /// </summary>
        bool Connected { get; set; }

        /// <summary>
        /// 安全码
        /// </summary>
        Sid Sid { get; }

        /// <summary>
        /// 当前用户的信息
        /// </summary>
        UserInfo UserInfo { get; }
    }

    /// <summary>
    /// 支持用户会话状态的长连接，并发送心跳保持会话状态
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class UserConnection : IUserConnection, IServiceClient, IDisposable
    {
        static UserConnection()
        {
            GlobalTimer<ITask>.Default.Add(TimeSpan.FromSeconds(1), new TaskFuncAdapter(_KeepConnection), false);
        }

        public UserConnection(Sid sid, CommunicationOption proxy,
            DataFormat dataFormat = DataFormat.Unknown)
            : this((sc) => sid, () => proxy, dataFormat)
        {

        }

        public UserConnection(string username, string password, CommunicationOption proxy,
            DataFormat dataFormat = DataFormat.Unknown)
            : this((sc) => _Login(sc, username, password), () => proxy, dataFormat)
        {

        }

        internal UserConnection(Func<IServiceClient, Sid> sidGetter, Func<CommunicationOption> proxyGetter, DataFormat dataFormat)
        {
            DataFormat = dataFormat;
            _proxyGetter = proxyGetter;
            _sidGetter = sidGetter;

            _serviceClient = new Lazy<IServiceClient>(_CreateServiceClient, true);
            _Register(this);

            _invoker = new Lazy<SystemInvoker>(() => {
                _serviceClient.EnsureLoaded();
                return SystemInvoker.FromServiceClient(this, eventSender: this);
            }, true);
        }

        /// <summary>
        /// 是否处于连接状态
        /// </summary>
        public bool Connected
        {
            get { return _serviceClient.IsValueCreated && _conProxy.State == ConnectionState.Opened; }
            set
            {
                if (value != Connected)
                {
                    if (value)
                        Connect();
                    else
                        Disconnect();
                }
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            lock (_syncLocker)
            {
                if (!_serviceClient.IsValueCreated)
                    return;

                _conProxy.Dispose();
            }
        }

        /// <summary>
        /// 连接
        /// </summary>
        public void Connect()
        {
            lock (_syncLocker)
            {
                _serviceClient.EnsureLoaded();
                if (_conProxy.State != ConnectionState.Opened)
                {
                    _conProxy.SetConnection(_CreateConnection());
                    _Login(_serviceClient.Value);
                }
            }
        }

        private static readonly WcfService _wcfService = new WcfService();
        private readonly Func<IServiceClient, Sid> _sidGetter;
        private ConnectionProxy _conProxy;

        internal UserConnectionManager Container { get; set; }

        private static Sid _Login(IServiceClient sc, string username, string password)
        {
            SystemInvoker invoker = SystemInvoker.FromServiceClient(sc);
            return invoker.User.LoginSr(username, password).Validate().Sid;
        }

        /// <summary>
        /// 安全码
        /// </summary>
        public Sid Sid
        {
            get
            {
                if (!_sid.IsEmpty())
                    return _sid;

                _ValidateConnected();
                _serviceClient.EnsureLoaded();
                return _sid;
            }
            private set
            {
                _sid = value;
            }
        }

        private Sid _sid;

        /// <summary>
        /// 当前用户的信息
        /// </summary>
        public UserInfo UserInfo
        {
            get
            {
                if (_userInfo != null)
                    return _userInfo;

                _ValidateConnected();
                _serviceClient.EnsureLoaded();
                return _userInfo;
            }
            private set
            {
                _userInfo = value;
            }
        }

        private void _ValidateConnected()
        {
            if (!Connected)
                throw new InvalidOperationException("尚未与服务器建立连接");
        }

        private UserInfo _userInfo;

        /// <summary>
        /// 系统功能调用
        /// </summary>
        public SystemInvoker Invoker
        {
            get
            {
                return _invoker.Value;
            }
        }

        private readonly Lazy<SystemInvoker> _invoker;
        private readonly Func<CommunicationOption> _proxyGetter;

        /// <summary>
        /// 数据格式
        /// </summary>
        public DataFormat DataFormat { get; private set; }

        private readonly Lazy<IServiceClient> _serviceClient;
        private readonly object _syncLocker = new object();

        private IConnection _CreateConnection()
        {
            CommunicationOption proxy = _proxyGetter();
            IConnection con;
            switch (proxy.Type)
            {
                case CommunicationType.Tcp:
                    con = new SocketConnection(proxy);
                    break;

                default:
                    con = _wcfService.Connect(proxy);
                    break;
            }

            con.Open();
            return con;
        }

        private void _Login(IServiceClient sc)
        {
            try
            {
                _sid = _sidGetter(sc);
                UserInfo = _GetMyInfo(sc, _sid);
            }
            catch (Exception)
            {
                _conProxy.Dispose();
                throw;
            }
        }

        private IServiceClient _CreateServiceClient()
        {
            _conProxy = new ConnectionProxy(this, null);
            IServiceClient sc = new ServiceFairyClient(_conProxy, DataFormat, disposeIt: true);
            _conProxy.Received += new ConnectionDataReceivedEventHandler(con_Received);

            return sc;
        }

        private UserInfo _GetMyInfo(IServiceClient sc, Sid sid)
        {
            using (SystemInvoker invoker = SystemInvoker.FromServiceClient(sc, false))
            {
                return invoker.User.GetMyInfo(CallingSettings.RequestReplyWithSid(sid));
            }
        }

        private void con_Received(object sender, ConnectionDataReceivedEventArgs e)
        {
            ConnectionDataReceivedEventHandler eh = DataReceived;
            if (eh != null)
                eh(this, e);
        }

        /// <summary>
        /// 接收到数据
        /// </summary>
        public event ConnectionDataReceivedEventHandler DataReceived;

        /// <summary>
        /// 释放
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// 发起调用请求
        /// </summary>
        /// <param name="method"></param>
        /// <param name="input"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public ServiceResult Call(string method, object input, CallingSettings settings = null)
        {
            _lastAccess = QuickTime.UtcNow;
            return _serviceClient.Value.Call(method, input, _Revise(settings));
        }

        /// <summary>
        /// 注册事件接收器
        /// </summary>
        /// <typeparam name="entityType"></typeparam>
        /// <param name="method"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public IServiceClientReceiverHandler RegisterReceiver(string method, Type entityType, IServiceClientReceiver receiver)
        {
            return _serviceClient.Value.RegisterReceiver(method, entityType, receiver);
        }

        private CallingSettings _Revise(CallingSettings settings)
        {
            return CallingSettings.FromPrototype(settings, Sid);
        }

        /// <summary>
        /// 发起调用请求
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="method"></param>
        /// <param name="input"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public ServiceResult<object> Call(string method, object input, Type entityType, CallingSettings settings = null)
        {
            _lastAccess = QuickTime.UtcNow;
            return _serviceClient.Value.Call(method, input, entityType, _Revise(settings));
        }

        #region Class ConnectionProxy ...

        [System.Diagnostics.DebuggerStepThrough]
        class ConnectionProxy : IConnection
        {
            public ConnectionProxy(UserConnection owner, IConnection con)
            {
                _owner = owner;
                SetConnection(con);
            }

            private readonly UserConnection _owner;

            public void SetConnection(IConnection con)
            {
                if (_con != null)
                {
                    _con.Received -= _con_Received;
                }

                _con = con;
                if (_con != null)
                {
                    _con.Received += _con_Received;
                }
            }

            private IConnection _con;

            private void _con_Received(object sender, ConnectionDataReceivedEventArgs e)
            {
                var eh = Received;
                if (eh != null)
                    eh(sender, e);
            }

            public event ConnectionDataReceivedEventHandler Received;

            public bool Duplex
            {
                get { return _con.Duplex; }
            }

            public ConnectionState State
            {
                get
                {
                    IConnection con = _con;
                    return con == null ? ConnectionState.Closed : con.State;
                }
            }

            public void Open()
            {
                IConnection con = _con;
                if (con != null)
                    con.Open();
            }

            public CommunicateData Call(CommunicateContext context, string method, CommunicateData data, CallingSettings settings = null)
            {
                _owner._ValidateConnected();
                IConnection con = _con;
                if (con != null)
                    return con.Call(context, method, data, settings);

                return null;
            }

            public void Dispose()
            {
                IConnection con = _con;
                if (con != null)
                    con.Dispose();
            }
        }

        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _Unregister(this);

            UserConnectionManager container = Container;
            if(container!=null)
                container.Remove(Sid);

            if (_serviceClient.IsValueCreated)
                _serviceClient.Value.Dispose();

            Disposed.RaiseEvent(this);
        }

        ~UserConnection()
        {
            Dispose();
        }

        #region FromNavigation ...

        /// <summary>
        /// 通过导航创建
        /// </summary>
        /// <param name="navigation">导航</param>
        /// <param name="sid">安全码</param>
        /// <param name="navDataFormat">导航通信编码</param>
        /// <param name="proxyDataFormat">代理通信编码</param>
        /// <param name="proxyCommunicationType">代理通信类型</param>
        /// <returns></returns>
        public static UserConnection FromNavigation(Sid sid, CommunicationOption navigation, DataFormat navDataFormat = DataFormat.Unknown,
            CommunicationType proxyCommunicationType = CommunicationType.WTcp, DataFormat proxyDataFormat = DataFormat.Unknown)
        {
            Contract.Requires(sid != null && navigation != null);
            return new UserConnection(sc => sid,
                () => InvokerBase.GetProxyList(navigation, proxyCommunicationType, CommunicationDirection.Bidirectional, proxyDataFormat, 1, throwErrorWhenEmpty: true).FirstOrDefault(),
                dataFormat: proxyDataFormat
            );
        }

        /// <summary>
        /// 通过导航创建
        /// </summary>
        /// <param name="sid">安全码</param>
        /// <param name="navigation">导航</param>
        /// <param name="navCommunicationType">导航通信类型</param>
        /// <param name="navDataFormat">导航通信编码</param>
        /// <param name="proxyCommunicationType">代理通信类型</param>
        /// <param name="proxyDataFormat">代理通信编码</param>
        /// <returns></returns>
        public static UserConnection FromNavigation(Sid sid, ServiceAddress navigation, CommunicationType navCommunicationType = CommunicationType.Http,
            DataFormat navDataFormat = DataFormat.Unknown, CommunicationType proxyCommunicationType = CommunicationType.WTcp, DataFormat proxyDataFormat = DataFormat.Unknown)
        {
            return FromNavigation(sid, new CommunicationOption(navigation, navCommunicationType), navDataFormat, proxyCommunicationType, proxyDataFormat);
        }

        /// <summary>
        /// 通过导航创建
        /// </summary>
        /// <param name="sid">安全码</param>
        /// <param name="navigation">导航</param>
        /// <param name="navCommunicationType">导航通信类型</param>
        /// <param name="navDataFormat">导航通信编码</param>
        /// <param name="proxyCommunicationType">代理通信类型</param>
        /// <param name="proxyDataFormat">代理通信编码</param>
        /// <returns></returns>
        public static UserConnection FromNavigation(Sid sid, string navigation, CommunicationType navCommunicationType = CommunicationType.Http,
            DataFormat navDataFormat = DataFormat.Unknown, CommunicationType proxyCommunicationType = CommunicationType.WTcp, DataFormat proxyDataFormat = DataFormat.Unknown)
        {
            return FromNavigation(sid, new CommunicationOption(ServiceAddress.Parse(navigation), navCommunicationType), navDataFormat, proxyCommunicationType, proxyDataFormat);
        }

        /// <summary>
        /// 通过导航创建
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="navDataFormat">导航通信编码</param>
        /// <param name="proxyDataFormat">代理通信编码</param>
        /// <param name="proxyCommunicationType">代理通信方式</param>
        /// <returns></returns>
        public static UserConnection FromNavigation(string username, string password, CommunicationOption navigation, DataFormat navDataFormat = DataFormat.Unknown,
            CommunicationType proxyCommunicationType = CommunicationType.WTcp, DataFormat proxyDataFormat = DataFormat.Unknown)
        {
            Contract.Requires(username != null && navigation != null);
            return new UserConnection(sc => _Login(sc, username, password),
                () => InvokerBase.GetProxyList(navigation, proxyCommunicationType, CommunicationDirection.Bidirectional, proxyDataFormat, 1, throwErrorWhenEmpty: true).FirstOrDefault(),
                dataFormat: proxyDataFormat
            );
        }

        /// <summary>
        /// 通过导航创建
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="navigation">导航</param>
        /// <param name="navCommunicationType">导航通信方式</param>
        /// <param name="navDataFormat">导航通信编码</param>
        /// <param name="proxyCommunicationType">代理通信方式</param>
        /// <param name="proxyDataFormat">代理通信编码</param>
        /// <returns></returns>
        public static UserConnection FromNavigation(string username, string password, ServiceAddress navigation, CommunicationType navCommunicationType = CommunicationType.Http,
            DataFormat navDataFormat = DataFormat.Unknown, CommunicationType proxyCommunicationType = CommunicationType.WTcp, DataFormat proxyDataFormat = DataFormat.Unknown)
        {
            return FromNavigation(username, password, new CommunicationOption(navigation, navCommunicationType), navDataFormat, proxyCommunicationType, proxyDataFormat);
        }

        /// <summary>
        /// 通过导航创建
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="navigation">导航</param>
        /// <param name="navCommunicationType">导航通信方式</param>
        /// <param name="navDataFormat">导航通信编码</param>
        /// <param name="proxyCommunicationType">代理通信方式</param>
        /// <param name="proxyDataFormat">代理通信编码</param>
        /// <returns></returns>
        public static UserConnection FromNavigation(string username, string password, string navigation, CommunicationType navCommunicationType = CommunicationType.Http,
            DataFormat navDataFormat = DataFormat.Unknown, CommunicationType proxyCommunicationType = CommunicationType.WTcp, DataFormat proxyDataFormat = DataFormat.Unknown)
        {
            return FromNavigation(username, password, new CommunicationOption(ServiceAddress.Parse(navigation), navCommunicationType), navDataFormat, proxyCommunicationType, proxyDataFormat);
        }

        #endregion

        #region KeepConnection ...

        private static readonly HashSet<UserConnection> _instances = new HashSet<UserConnection>();

        private static void _Register(UserConnection con)
        {
            lock (_instances)
            {
                _instances.Add(con);
            }
        }

        private static void _Unregister(UserConnection con)
        {
            lock (_instances)
            {
                _instances.Remove(con);
            }
        }

        private static void _KeepConnection()
        {
            UserConnection[] cons;
            lock (_instances)
            {
                cons = _instances.ToArray();
            }

            foreach (UserConnection con in cons)
            {
                con._PrivateKeepConnection();
            }
        }

        private DateTime _lastAccess;

        private void _PrivateKeepConnection(object state = null)
        {
            if (QuickTime.UtcNow - _lastAccess < TimeSpan.FromSeconds(60))
                return;

            _lastAccess = QuickTime.UtcNow;


            try
            {
                lock (_syncLocker)
                {
                    if (!Connected)
                        return;

                    SysServices.Hello(_serviceClient.Value, 
                        CallingSettings.FromTarget(SFNames.ServiceNames.Tray, CommunicateInvokeType.OneWay, Sid, ServiceTargetModel.Auto)
                    );
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
            }
        }

        #endregion
    }
}

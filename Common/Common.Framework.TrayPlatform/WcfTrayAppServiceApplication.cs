using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf;
using System.Net;
using Common.Contracts.Service;
using Common.Contracts;
using Common.Package;
using Common.Package.Cache;
using Common.Utility;
using System.ServiceModel;
using System.Net.Sockets;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Collections.Concurrent;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// 基于WCF控制的AppService应用
    /// </summary>
    public partial class WcfTrayAppServiceApplication : TrayAppServiceApplicationBase
    {
        public WcfTrayAppServiceApplication()
        {
            WcfService = new WcfService();

            MemoryStorageStrategy<CommunicationOption, WcfConnection> storage = new MemoryStorageStrategy<CommunicationOption, WcfConnection>();
            storage.Expired += new EventHandler<MemoryStorageExpiredEventArgs<CommunicationOption, WcfConnection>>(storage_Expired);
            _communicateOption2ConnectionsCache = new Cache<CommunicationOption, WcfConnection>(storage);

            _serviceCommunicationSearcherProxy = new Lazy<ServiceCommunicationSearcherProxy>(() => new ServiceCommunicationSearcherProxy(this));
            _defaultCallbackCommunicateFactory = new Lazy<CallbackCommunicateFactory>(() => new CallbackCommunicateFactory(this, _serviceCommunicationSearcherProxy.Value));
            _securityProviderProxy = new Lazy<SecurityProviderProxy>(() => new SecurityProviderProxy(this));
            _traySessionStateManager = new Lazy<TraySessionStateManager>(() => new TraySessionStateManager(this, _securityProviderProxy.Value));
            _userConnectionManager = new Lazy<UserConnectionManager>(() => new UserConnectionManager(ServiceManager.ClientID));
        }

        public WcfService WcfService { get; private set; }

        protected override void OnStop()
        {
            base.OnStop();
            WcfService.Dispose();
        }

        public override CommunicateData Callback(CommunicateContext context, string method, CommunicateData data, CommunicationOption option, CallingSettings settings)
        {
            if (_IsLocalCall(option) != false)
            {
                CommunicateData rspData = base.Callback(context, method, data, option, settings);

                // 返回值为空时，是不需要应答结果的情况；返回值不为空时，若状态码不为NotFound，则返回结果，否则尝试远程调用
                if ((rspData == null && (settings != null && !settings.NeedReply()))
                    || (rspData != null && (rspData.StatusCode != TrayUtility.ServiceNotFoundStatusCode_SelfNotFound)))
                    return rspData;
            }

            return _CallRemote(context, method, data, option, settings);
        }

        private bool? _IsLocalCall(CommunicationOption option)
        {
            if (option == null)
                return null;

            return GetAllCommunicateOptions().Contains(option);
        }

        private CommunicateData _CallRemote(CommunicateContext context, string method, CommunicateData data, CommunicationOption option, CallingSettings settings)
        {
            // 调用外部的服务
            MethodParser mp = new MethodParser(method);
            WcfConnection con = _CreateConnection(mp.Service, mp.ServiceVersion, option);
            if (con == null)
                return TrayUtility.CreateServiceNotFoundCommunicateData(method, data.DataFormat);

            try
            {
                return con.Send(context, method, data, settings);
            }
            catch (Exception ex)
            {
                if (ex is CommunicationException)
                {
                    _service2connectionsCache.Remove(new Tuple<string, SVersion>(mp.Service, mp.ServiceVersion));
                    con.Dispose();
                }

                throw new ServiceException(ClientErrorCode.NetworkError, null, ex.ToString());
            }
        }

        private readonly object _thisLock = new object();

        private WcfConnection _CreateConnection(string serviceName, SVersion version, CommunicationOption option)
        {
            if (option != null)  // 定向调用（指定了服务的地址）
            {
                return _communicateOption2ConnectionsCache.GetOrAddOfRelative(option, TimeSpan.FromMinutes(5), delegate(CommunicationOption op) {
                    WcfConnection con = WcfService.Connect(option.Address, op.Type, op.Duplex);
                    con.Open();
                    return con;
                });
            }
            else  // 不定向调用，自动寻找该服务的地址
            {
                var cacheKey = new Tuple<string, SVersion>(serviceName, version);
                WcfConnection con = _service2connectionsCache.Get(cacheKey);
                if (con == null)
                {
                    lock (_thisLock)
                    {
                        if ((con = _service2connectionsCache.Get(cacheKey)) == null)
                        {
                            con = OnCreateConnection(serviceName, version);
                            if (con == null)
                                return null;

                            con.Open();
                            _service2connectionsCache.Add(new Tuple<string, SVersion>(serviceName, version),
                                con, new CacheExpiredDependency(this, serviceName, version, con));
                        }
                    }
                }

                return con;
            }
        }

        /// <summary>
        /// 创建WCF连接
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        protected virtual WcfConnection OnCreateConnection(string serviceName, SVersion version)
        {
            CommunicationOption option = _GetServiceCommunicateOption(serviceName, version);
            if (option == null)
                return null;

            return WcfService.Connect(option.Address, option.Type, option.Duplex);
        }

        private CommunicationOption _GetServiceCommunicateOption(string serviceName, SVersion version)
        {
            AppInvokeInfo serverInfo = AppInvokeManager.GetServerInfoByServiceName(serviceName, version);
            if (serverInfo != null)
                return TrayUtility.PickCommunicationOption(serverInfo.CommunicateOptions);

            return null;
        }

        /// <summary>
        /// 开启侦听
        /// </summary>
        /// <param name="option"></param>
        public override void StartListener(CommunicationOption option)
        {
            Contract.Requires(option != null && option.Address != null);

            WcfListener listener = WcfService.CreateListener(option.Address, option.Type, option.Duplex);
            listener.Connected += new EventHandler<WcfListenerConnectedEventArgs>(listener_Connected);
            listener.Disconnected += new EventHandler<WcfListenerDisconnectedEventArgs>(listener_Disconnected);

            try
            {
                listener.Start();
            }
            catch
            {
                listener.Dispose();
                throw;
            }
        }

        /// <summary>
        /// 停止侦听
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public override bool StopListener(ServiceAddress address)
        {
            Contract.Requires(address != null);
            WcfListener listener = WcfService.GetListener(address);
            if (listener == null)
                return false;

            listener.Dispose();
            return true;
        }

        // 连接建立
        private void listener_Connected(object sender, WcfListenerConnectedEventArgs e)
        {
            e.Connections.ForEach(con => con.Received += new ConnectionDataReceivedEventHandler(connection_Received));
        }

        // 连接断开
        private void listener_Disconnected(object sender, WcfListenerDisconnectedEventArgs e)
        {
            UserConnectionManager ucMgr = _userConnectionManager.Value;
            e.Connections.ForEach(con => {
                con.Received -= new ConnectionDataReceivedEventHandler(connection_Received);
                ucMgr.RemoveByConnection(con);
            });
        }

        // 接收到数据
        private void connection_Received(object sender, ConnectionDataReceivedEventArgs e)
        {
            ServiceAddress from = (e.Context == null) ? null : e.Context.From;

            // 保持长连接的双向通信
            if (e.Settings != null && !e.Settings.Sid.IsEmpty())
            {
                e.Settings.Sid = SidUtility.DecryptSid(e.Settings.Sid, from);
                if (ProxyEnabled && e.Connection.Duplex)
                    _RecordConnection(e.Settings.Sid, e.Connection);
            }

            CommunicateData replyData = this.Call(e.Context, e.Method, e.RequestData, e.Settings);
            if (replyData != null && !replyData.Sid.IsEmpty())
            {
                if (ProxyEnabled && e.Connection.Duplex)  // 安全码改变
                    _RecordConnection(replyData.Sid, e.Connection);

                replyData.Sid = SidUtility.EncryptSid(replyData.Sid, from);
            }

            e.ReplyData = replyData;
        }

        // 保持双向连接
        private void _RecordConnection(Sid sid, IConnection connection)
        {
            UserSessionState uss = _traySessionStateManager.Value.GetSessionState(sid);
            UserConnectionManager ucMgr = _userConnectionManager.Value;
            if (uss != null && uss.BasicInfo != null)
                ucMgr.Add(uss, connection);
            else
                ucMgr.RemoveBySid(sid);
        }

        private readonly Lazy<CallbackCommunicateFactory> _defaultCallbackCommunicateFactory;

        protected override CommunicateData Call(CommunicateContext context, string method, CommunicateData data, CallingSettings settings)
        {
            if (settings != null && !(settings.Target == null || settings.Target.EndPoints.IsNullOrEmpty()))  // 直接调用或多播
            {
                ICommunicate communicate = _defaultCallbackCommunicateFactory.Value.CreateCommunicate(settings.Target);
                return communicate.Call(null, method, data, settings.ToNormal());
            }

            // 自动调用
            CommunicateData result = base.Call(context, method, data, settings);
            if (result != null && result.StatusCode == TrayUtility.ServiceNotFoundStatusCode_SelfNotFound)
            {
                if (ProxyEnabled)  // 作为代理调用其它的服务
                    return _CallRemote(null, method, data, null, settings);
            }

            return result;
        }

        private readonly Cache<Tuple<string, SVersion>, WcfConnection> _service2connectionsCache = new Cache<Tuple<string, SVersion>, WcfConnection>();
        private readonly Cache<CommunicationOption, WcfConnection> _communicateOption2ConnectionsCache;

        void storage_Expired(object sender, MemoryStorageExpiredEventArgs<CommunicationOption, WcfConnection> e)
        {
            e.Items.ForEach(item => item.Value.Dispose());
        }

        #region Class CacheExpiredDependency ...

        class CacheExpiredDependency : ICacheExpireDependency
        {
            public CacheExpiredDependency(WcfTrayAppServiceApplication owner, string serviceName, SVersion version, WcfConnection con)
            {
                _owner = owner;
                _serviceName = serviceName;
                _con = con;
                _version = version;

                _lastOption = _owner._GetServiceCommunicateOption(serviceName, version);
            }

            private readonly WcfTrayAppServiceApplication _owner;
            private readonly string _serviceName;
            private readonly SVersion _version;
            private readonly WcfConnection _con;

            private CommunicationOption _lastOption;

            public void Reset()
            {
                
            }

            public void AccessNotify()
            {
                
            }

            public bool HasExpired()
            {
                CommunicationOption curOption = _owner._GetServiceCommunicateOption(_serviceName, _version);
                bool expired = (curOption != _lastOption);
                if (expired)
                    _con.Dispose();

                return expired;
            }
        }

        #endregion

        public override CommunicationOption[] GetAllCommunicateOptions()
        {
            return WcfService.GetAllListeners().Select(
                lis => new CommunicationOption(lis.Option.Address, lis.Option.Type, lis.Option.Duplex)
            ).ToArray();
        }

        public override ICommunicateFactory CreateCommunicateFactory()
        {
            return new CallbackCommunicateFactory(this, _serviceCommunicationSearcherProxy.Value);
        }

        public override ITraySessionStateManager CreateTraySessionStateManager()
        {
            return _traySessionStateManager.Value;
        }

        private readonly Lazy<ServiceCommunicationSearcherProxy> _serviceCommunicationSearcherProxy;
        private readonly Lazy<SecurityProviderProxy> _securityProviderProxy;
        private readonly Lazy<TraySessionStateManager> _traySessionStateManager;

        #region Class ServiceCommunicationSearchProxy ...

        class ServiceCommunicationSearcherProxy : IServiceCommunicationSearcher
        {
            public ServiceCommunicationSearcherProxy(WcfTrayAppServiceApplication app)
            {
                _app = app;
            }

            private readonly WcfTrayAppServiceApplication _app;
            private ServiceCommunicationSearcherWrapper _searchWrapper;

            private IServiceCommunicationSearcher _GetSearch()
            {
                IServiceCommunicationSearcher searcher = _app.ServiceManager.GetHandler<IServiceCommunicationSearcher>()
                    ?? EmptyServiceCommunicationSearcher.Empty;

                return (_searchWrapper != null && _searchWrapper.InnerSearcher == searcher) ? _searchWrapper
                    : (_searchWrapper = new ServiceCommunicationSearcherWrapper(searcher));
            }

            #region Class ServiceCommunicationSearcherWrapper ...

            class ServiceCommunicationSearcherWrapper : IServiceCommunicationSearcher
            {
                public ServiceCommunicationSearcherWrapper(IServiceCommunicationSearcher searcher)
                {
                    InnerSearcher = searcher;
                }

                public readonly IServiceCommunicationSearcher InnerSearcher;
                private readonly Dictionary<ServiceDesc, AppInvokeInfo[]> _serviceDescDict = new Dictionary<ServiceDesc, AppInvokeInfo[]>();
                private readonly Dictionary<Guid, AppInvokeInfo[]> _clientIdDict = new Dictionary<Guid, AppInvokeInfo[]>();
                private volatile AppInvokeInfo[] _all;

                private AppInvokeInfo[] _TrySearch(Func<AppInvokeInfo[]> func)
                {
                    try
                    {
                        return func();
                    }
                    catch (Exception ex)
                    {
                        LogManager.LogError(ex);
                        return null;
                    }
                }

                public AppInvokeInfo[] Search(ServiceDesc serviceDesc)
                {
                    AppInvokeInfo[] infos = _TrySearch(() => InnerSearcher.Search(serviceDesc));

                    lock (_serviceDescDict)
                    {
                        if (infos == null)
                            return _serviceDescDict.GetOrDefault(serviceDesc);

                        _serviceDescDict[serviceDesc] = infos;
                        return infos;
                    }
                }

                public AppInvokeInfo[] Search(Guid[] clientIds)
                {
                    AppInvokeInfo[] infos = _TrySearch(() => InnerSearcher.Search(clientIds));

                    lock (_clientIdDict)
                    {
                        if (infos == null)
                        {
                            List<AppInvokeInfo> list = new List<AppInvokeInfo>();
                            foreach (Guid clientId in clientIds)
                            {
                                AppInvokeInfo[] v;
                                if (_clientIdDict.TryGetValue(clientId, out v))
                                    list.AddRange(v);
                            }

                            return list.ToArray();
                        }

                        foreach (var g in infos.GroupBy(info => info.ClientID))
                        {
                            _clientIdDict[g.Key] = g.ToArray();
                        }

                        return infos;
                    }
                }

                public AppInvokeInfo[] SearchAll()
                {
                    AppInvokeInfo[] infos = _TrySearch(() => InnerSearcher.SearchAll());
                    return (infos == null) ? _all : (_all = infos);
                }
            }

            #endregion

            public AppInvokeInfo[] Search(ServiceDesc serviceDesc)
            {
                return _GetSearch().Search(serviceDesc);
            }

            public AppInvokeInfo[] Search(Guid[] clientIds)
            {
                return _GetSearch().Search(clientIds);
            }

            public AppInvokeInfo[] SearchAll()
            {
                return _GetSearch().SearchAll();
            }

            class EmptyServiceCommunicationSearcher : IServiceCommunicationSearcher
            {
                public AppInvokeInfo[] Search(ServiceDesc serviceDesc)
                {
                    return Array<AppInvokeInfo>.Empty;
                }

                public AppInvokeInfo[] Search(Guid[] clientIds)
                {
                    return Array<AppInvokeInfo>.Empty;
                }

                public AppInvokeInfo[] SearchAll()
                {
                    return Array<AppInvokeInfo>.Empty;
                }

                public static readonly EmptyServiceCommunicationSearcher Empty = new EmptyServiceCommunicationSearcher();
            }
        }

        #endregion

        #region Class SecurityProviderProxy ...

        class SecurityProviderProxy : ISecurityProvider
        {
            public SecurityProviderProxy(WcfTrayAppServiceApplication app)
            {
                _app = app;
            }

            private readonly WcfTrayAppServiceApplication _app;
            private volatile ISecurityProvider _sp;

            private ISecurityProvider _GetSp()
            {
                return _app.ServiceManager.GetHandler<ISecurityProvider>();
            }

            public UserSessionState GetUserSessionState(Sid sid)
            {
                return _GetSp().GetUserSessionState(sid);
            }

            public UserBasicInfo GetUserBasicInfo(string username)
            {
                return _GetSp().GetUserBasicInfo(username);
            }

            #region Class EmptySecurityProvider ...

            class EmptySecurityProvider : ISecurityProvider
            {
                public UserSessionState GetUserSessionState(Sid sid)
                {
                    return null;
                }

                public UserBasicInfo GetUserBasicInfo(string username)
                {
                    return null;
                }

                public void UpdateSessionState(UserSessionState uss)
                {

                }

                public static readonly EmptySecurityProvider Empty = new EmptySecurityProvider();
            }

            #endregion
        }

        #endregion

        private readonly Lazy<UserConnectionManager> _userConnectionManager;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using Common.Contracts.Service;
using Common.Package.Service;
using System.Diagnostics.Contracts;
using Common.Communication.Wcf;
using Common.Package;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// 平台上下文
    /// </summary>
    public class TrayContext : IServiceProvider, IDisposable
    {
        internal TrayContext(IAppService service, IServiceProvider sp, AppServiceInfo info)
        {
            Service = service;
            _sp = sp;

            CommunicateFactory = sp.GetService<ICommunicateFactory>(true);
            Platform = sp.GetService<ITrayPlatform>(true);
            Configuration = sp.GetService<ITrayConfiguration>(true);
            ConfigReader = new ConfigurationReader(Configuration);
            ProxyManager = sp.GetService<ITrayProxyManager>(true);
            CookieManager = sp.GetService<ITrayCookieManager>(true);
            SessionStateManager = sp.GetService<ITraySessionStateManager>(true);
            CacheManager = sp.GetService<ITrayCacheManager>(true);
            Communicate = CommunicateFactory.CreateDefaultCommunicate();
            Synchronizer = sp.GetService<ITraySynchronizer>(true);

            LogManager = sp.GetService<ITrayLogManager>(true);
            Common.Package.LogManager.RegisterLogWritter(_logWriter = new TrayLogWriter(LogManager));

            Guid.TryParse(Platform.GetData(SystemDataKeys.PLATFORM_DEPLOY_ID).ToStringIgnoreNull(), out _platformDeployId);
            ServiceInfo = info;
            ServiceDesc = info.ServiceDesc;
            ClientID = _GetClientID(Platform);
            ServiceEndPoint = new ServiceEndPoint(ClientID, ServiceDesc);
            RunningPath = Platform.GetRunningPath();
        }

        private readonly IServiceProvider _sp;

        public object GetService(Type serviceType)
        {
            return _sp.GetService(serviceType);
        }

        /// <summary>
        /// 服务
        /// </summary>
        public IAppService Service { get; private set; }

        /// <summary>
        /// 与平台的通信
        /// </summary>
        public ITrayPlatform Platform { get; private set; }

        /// <summary>
        /// Cookie管理器
        /// </summary>
        public ITrayCookieManager CookieManager { get; private set; }

        /// <summary>
        /// 日志管理器
        /// </summary>
        public ITrayLogManager LogManager { get; private set; }

        /// <summary>
        /// 回调请求通信的创建工厂
        /// </summary>
        public ICommunicateFactory CommunicateFactory { get; private set; }

        /// <summary>
        /// 回调请求通信方式
        /// </summary>
        public ICommunicate Communicate { get; private set; }

        /// <summary>
        /// 读取配置信息的接口
        /// </summary>
        public ITrayConfiguration Configuration { get; private set; }

        /// <summary>
        /// 配置信息读取器
        /// </summary>
        public ConfigurationReader ConfigReader { get; private set; }

        /// <summary>
        /// 代理管理器
        /// </summary>
        public ITrayProxyManager ProxyManager { get; private set; }

        /// <summary>
        /// 用户会话状态管理器
        /// </summary>
        public ITraySessionStateManager SessionStateManager { get; private set; }

        /// <summary>
        /// 缓存管理器
        /// </summary>
        public ITrayCacheManager CacheManager { get; private set; }

        /// <summary>
        /// 线程同步器
        /// </summary>
        public ITraySynchronizer Synchronizer { get; private set; }

        /// <summary>
        /// 服务信息
        /// </summary>
        public AppServiceInfo ServiceInfo { get; private set; }

        /// <summary>
        /// 服务名称与版本号
        /// </summary>
        public ServiceDesc ServiceDesc { get; private set; }

        /// <summary>
        /// 客户端唯一标识
        /// </summary>
        public Guid ClientID { get; private set; }

        /// <summary>
        /// 平台版本
        /// </summary>
        public Guid PlatformDeployId { get { return _platformDeployId; } }

        /// <summary>
        /// 服务终端描述
        /// </summary>
        public ServiceEndPoint ServiceEndPoint { get; private set; }

        /// <summary>
        /// 运行路径
        /// </summary>
        public string RunningPath { get; private set; }

        private Guid _platformDeployId;
        private TrayLogWriter _logWriter;

        private Guid _GetClientID(ITrayPlatform platform)
        {
            object clientIdData = platform.GetData(SystemDataKeys.CLIENT_ID);
            Guid clientId;
            if (clientIdData == null || !Guid.TryParse(clientIdData.ToString(), out clientId))
                throw new ServiceException(ServiceStatusCode.ServerError, "未配置CLIENT_ID或格式错误");

            return clientId;
        }

        private volatile IServiceClient _serviceClient;
        private readonly object _thisLock = new object();

        /// <summary>
        /// 默认的服务通道
        /// </summary>
        public IServiceClient ServiceClient
        {
            get
            {
                if (_serviceClient != null)
                    return _serviceClient;

                lock (_thisLock)
                {
                    if (_serviceClient == null)
                        _serviceClient = CreateServiceClient(Communicate);
                }

                return _serviceClient;
            }
        }

        /// <summary>
        /// 创建服务通道
        /// </summary>
        /// <param name="communicate"></param>
        /// <returns></returns>
        public IServiceClient CreateServiceClient(ICommunicate communicate)
        {
            Contract.Requires(communicate != null);

            AppServiceInfo sInfo = ServiceInfo;
            return new ServiceClient(communicate, sInfo.DefaultDataFormat);
        }

        /// <summary>
        /// 创建指定终端的通道
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public IServiceClient CreateServiceClient(Guid clientId)
        {
            return CreateServiceClient(CommunicateFactory.CreateCommunicate(clientId));
        }

        /// <summary>
        /// 创建指定服务终端的通道
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public IServiceClient CreateServiceClient(ServiceEndPoint endpoint)
        {
            Contract.Requires(endpoint != null);
            return CreateServiceClient(CommunicateFactory.CreateCommunicate(ServiceTarget.FromServiceEndPoint(endpoint)));
        }

        /// <summary>
        /// 创建指定服务终端的通道
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public IServiceClient CreateServiceClient(Guid clientId, ServiceDesc serviceDesc)
        {
            return CreateServiceClient(new ServiceEndPoint(clientId, serviceDesc));
        }

        /// <summary>
        /// 创建指定用户的通道
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public IServiceClient CreateServiceClient(int userId, bool throwError = false)
        {
            ICommunicate communicate = SessionStateManager.CreateCommunicate(userId);
            if (communicate == null)
            {
                if (throwError)
                    throw new ServiceException(ServerErrorCode.InvalidOperation, "用户" + userId + "未在该终端上连接");

                return null;
            }

            return CreateServiceClient(communicate);
        }

        /// <summary>
        /// 创建指定插件的通道
        /// </summary>
        /// <param name="addin"></param>
        /// <returns></returns>
        public IServiceClient CreateServiceClient(IAppServiceAddin addin)
        {
            Contract.Requires(addin != null);

            return CreateServiceClient(addin.Communicate);
        }

        /// <summary>
        /// 创建指定服务的广播通道
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <param name="includeMySelf"></param>
        /// <param name="async"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IServiceClient CreateBroadcastServiceClient(ServiceDesc serviceDesc, bool includeMySelf = false, bool async = true, ICommunicateCallback callback = null)
        {
            Contract.Requires(serviceDesc != null);

            ICommunicate communicate = CommunicateFactory.CreateBroadcastCommunicate(serviceDesc, includeMySelf: includeMySelf, async: async, callback: callback);
            return CreateServiceClient(communicate);
        }

        /// <summary>
        /// 创建指定终端的广播通信器
        /// </summary>
        /// <param name="endpoints"></param>
        /// <param name="async"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IServiceClient CreateBroadcastServiceClient(ServiceEndPoint[] endpoints, bool async = true, ICommunicateCallback callback = null)
        {
            Contract.Requires(endpoints != null);

            ICommunicate communicate = CommunicateFactory.CreateBroadcastCommunicate(endpoints, async: async, callback: callback);
            return CreateServiceClient(communicate);
        }

        /// <summary>
        /// 创建自己的服务的广播通信器
        /// </summary>
        /// <param name="includeMySelf"></param>
        /// <param name="async"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IServiceClient CreateBoradcastServiceClientOfMyself(bool includeMySelf = false, bool async = true, ICommunicateCallback callback = null)
        {
            return CreateBroadcastServiceClient(ServiceDesc, includeMySelf: includeMySelf, async: async, callback: callback);
        }

        private readonly Cache<CommunicationOption, ICommunicate> _communicateCache = new Cache<CommunicationOption, ICommunicate>();

        /// <summary>
        /// 创建指下通信方式的通信器
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public ICommunicate CreateCommunicate(CommunicationOption option)
        {
            if (option == null)
                return Communicate;

            return _communicateCache.GetOrAddOfRelative(option, TimeSpan.FromMinutes(5), delegate(CommunicationOption op) {
                return CommunicateFactory.CreateCommunicate(new[] { op });
            });
        }

        /// <summary>
        /// 根据服务名称创建通信器的代理
        /// </summary>
        /// <param name="communicate"></param>
        /// <param name="defaultServiceDesc"></param>
        /// <returns></returns>
        public ICommunicate CreateServiceDescCommunicateDecorate(ServiceDesc defaultServiceDesc, ICommunicate communicate = null)
        {
            Contract.Requires(defaultServiceDesc != null);

            return new ServiceDescCommunicateDecorate(communicate ?? Communicate, defaultServiceDesc);
        }

        #region Class ServiceDescCommunicateDecorate ...

        // 能够自动添加服务名称的装饰器
        class ServiceDescCommunicateDecorate : MarshalByRefObjectEx, ICommunicate
        {
            public ServiceDescCommunicateDecorate(ICommunicate communicate, ServiceDesc serviceDesc)
            {
                _communicate = communicate;
                _serviceDesc = serviceDesc;
            }

            private readonly ICommunicate _communicate;
            private readonly ServiceDesc _serviceDesc;

            public CommunicateData Call(CommunicateContext context, string method, CommunicateData data, CallingSettings settings = null)
            {
                Contract.Requires(method != null);

                if (method.IndexOf('/') < 0)  // 未指定服务名称
                    method = _serviceDesc + "/" + method;

                return _communicate.Call(context, method, data, settings);
            }

            public void Dispose()
            {
                _communicate.Dispose();
            }
        }

        #endregion

        /// <summary>
        /// 创建指定通信配置的通信器
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public IServiceClient CreateServiceClient(CommunicationOption option)
        {
            if (option == null)
                return ServiceClient;

            return CreateServiceClient(CreateCommunicate(option));
        }

        void IDisposable.Dispose()
        {
            if (ConfigReader != null)
                ConfigReader.Dispose();

            if (_logWriter != null)
                _logWriter.Dispose();
        }
    }
}

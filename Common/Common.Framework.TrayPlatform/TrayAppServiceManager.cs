using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using System.IO;
using Common.Collection;
using System.Reflection;
using System.Runtime.Remoting;
using Common.Utility;
using Common.Contracts;
using Common.Package;
using System.Configuration;
using System.Net;
using Common.Communication.Wcf;
using System.Threading;
using Common.Contracts.Log;
using Common.Package.Log;
using Common.Package.GlobalTimer;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// AppService管理器
    /// </summary>
    public partial class TrayAppServiceManager : IDisposable
    {
        static TrayAppServiceManager()
        {
            AppDomain.MonitoringIsEnabled = true;  // 允许跟踪运行信息
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TrayAppServiceManager(IServiceManagerCallback callback, ITrayLogManager logManager)
        {
            Contract.Requires(callback != null);

            _callback = callback;
            _trayLogManager = logManager;

            _collectAppServiceRuntimeInfoHandle = GlobalTimer<ITask>.Default.Add(TimeSpan.FromSeconds(10), _CollectRuntimeInfo, false);
        }

        private readonly ThreadSafeDictionaryWrapper<string, TrayAppServiceInfoCollection> _dict
            = new ThreadSafeDictionaryWrapper<string, TrayAppServiceInfoCollection>(new Dictionary<string, TrayAppServiceInfoCollection>());

        private readonly IGlobalTimerTaskHandle _collectAppServiceRuntimeInfoHandle;

        private readonly IServiceManagerCallback _callback;
        private readonly object _thisLock = new object();

        /// <summary>
        /// 获取指定名称的Service的信息
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public TrayAppServiceInfo GetServiceInfo(string serviceName, SVersion version = default(SVersion))
        {
            Contract.Requires(serviceName != null);

            TrayAppServiceInfoCollection infos = _dict.GetOrDefault(serviceName);
            return infos == null ? null : (version == default(SVersion) ? infos.Current : infos.Get(version));
        }

        /// <summary>
        /// 获取指定名称与版本号的Service的信息
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public TrayAppServiceInfo GetServiceInfo(ServiceDesc serviceDesc)
        {
            Contract.Requires(serviceDesc != null);

            return GetServiceInfo(serviceDesc.Name, serviceDesc.Version);
        }

        /// <summary>
        /// 获取指定名称的Service信息
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public TrayAppServiceInfo[] GetServiceInfos(string serviceName)
        {
            Contract.Requires(serviceName != null);

            TrayAppServiceInfoCollection infos = _dict.GetOrDefault(serviceName);
            return infos == null ? null : infos.GetAll();
        }

        /// <summary>
        /// 获取指定名称的Service
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public IAppService GetService(string serviceName, SVersion version = default(SVersion))
        {
            TrayAppServiceInfo info = GetServiceInfo(serviceName, version);
            return info == null ? null : info.Service;
        }

        /// <summary>
        /// 获取指定名称的Service
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public IAppService GetService(ServiceDesc serviceDesc)
        {
            Contract.Requires(serviceDesc != null);

            return GetService(serviceDesc.Name, serviceDesc.Version);
        }

        /// <summary>
        /// 是否包含指定服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public bool ContainsService(string serviceName, SVersion version = default(SVersion))
        {
            return GetService(serviceName, version) != null;
        }

        /// <summary>
        /// 是否包含指定的服务
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public bool ContainsService(ServiceDesc serviceDesc)
        {
            return GetService(serviceDesc) != null;
        }

        /// <summary>
        /// 调用服务，若不存在则返回ServiceNotFound
        /// </summary>
        /// <param name="context"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <param name="settings"></param>
        /// <param name="notFound"></param>
        /// <returns>如果不存在该服务，则返回null，否则返回调用结果</returns>
        public CommunicateData CallService(CommunicateContext context, string method, CommunicateData data, CallingSettings settings)
        {
            MethodParser parser = new MethodParser(method);
            IAppService service = GetService(parser.Service, parser.ServiceVersion);
            if (service == null)
                return new CommunicateData(null, data.DataFormat, TrayUtility.ServiceNotFoundStatusCode_SelfNotFound);

            return service.Communicate.Call(context, method, data, settings);
        }

        /// <summary>
        /// 获取所有服务信息
        /// </summary>
        /// <returns></returns>
        public ServiceInfo[] GetAllServiceInfos()
        {
            lock (_thisLock)
            {
                return _dict.Values.SelectMany(v => v.GetAll()).Select(v => v.ToServiceInfo()).ToArray();
            }
        }

        /// <summary>
        /// 从程序集中加载AppService
        /// </summary>
        /// <param name="assemblyFile">程序集路径</param>
        /// <param name="configuration">配置信息</param>
        /// <returns></returns>
        public TrayAppServiceInfo LoadService(string assemblyFile, string configuration = null)
        {
            Contract.Requires(assemblyFile != null);

            if (!File.Exists(assemblyFile))
                throw new FileNotFoundException("指定的程序集不存在: " + assemblyFile);

            lock (_thisLock)
            {
                return _LoadService(assemblyFile, configuration);
            }
        }

        private void _StartAppService(TrayAppServiceInfo serviceInfo)
        {
            try
            {
                serviceInfo.Service.Start();
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
            }
        }

        private TrayAppServiceInfo _LoadService(string assemblyFile, string configuration)
        {
            AppDomainSetup adSetup = new AppDomainSetup();
            string appName = Path.GetFileNameWithoutExtension(assemblyFile);
            adSetup.ApplicationName = appName;
            adSetup.ApplicationBase = Path.GetDirectoryName(assemblyFile);
            adSetup.PrivateBinPath = AppDomain.CurrentDomain.BaseDirectory;
            adSetup.ConfigurationFile = assemblyFile + ".config";
            adSetup.ShadowCopyFiles = "true";
            adSetup.ShadowCopyDirectories = Path.GetTempPath();
            AppDomain domain = AppDomain.CreateDomain(appName, AppDomain.CurrentDomain.Evidence, adSetup);

            IAppService svr = null;
            try
            {
                IAppServiceLoader loader = domain.CreateInstanceAndUnwrap(typeof(TrayAppServiceManager).Assembly.FullName,
                    typeof(AssemblyAppServiceLoader).FullName) as IAppServiceLoader;

                svr = loader.LoadService(assemblyFile);
                TrayAppServiceInfo info = new TrayAppServiceInfo(domain, assemblyFile);
                info.Configuration = new TrayConfiguration(this, configuration);
                info.CommunicateFactory = _callback.CreateCommunicateFactory();
                info.Platform = new TrayPlatform(this, info);
                info.ProxyManager = new TrayProxyManager(this);
                info.LogManager = new TrayLogManager(info, _trayLogManager);
                info.CookieManager = new TrayCookieManager(this, info);
                info.SessionStateManager = _callback.CreateTraySessionStateManager();
                info.TrayCacheManager = new TrayCacheManager(_globalTrayCacheManager);
                info.Synchronizer = new TraySynchronizer(this, info);

                AppServiceInfo appServiceInfo;
                IServiceProvider sp = _CreatePlatformServiceProvider(info);

                if (svr == null || (appServiceInfo = svr.Init(sp, AppServiceInitModel.Execute)) == null)
                    throw new ServiceException(ServerErrorCode.InvalidOperation, "服务" + appName + "不允许运行");

                info.AppServiceInfo = appServiceInfo;
                info.CommunicateFactory.Owner = appServiceInfo.ServiceDesc;
                TrayAppServiceInfoCollection container = _dict.GetOrSet(appServiceInfo.ServiceDesc.Name);
                info.Service = new AppServiceWrapper(this, svr, container, info);

                container.Add(info);
                return info;
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);

                if (svr != null)
                    svr.Dispose();

                throw;
            }
        }

        private IServiceProvider _CreatePlatformServiceProvider(TrayAppServiceInfo info)
        {
            PlatformServiceProvider sp = new PlatformServiceProvider();
            sp.AddService(typeof(ICommunicateFactory), info.CommunicateFactory);
            sp.AddService(typeof(ITrayConfiguration), info.Configuration);
            sp.AddService(typeof(ITrayPlatform), info.Platform);
            sp.AddService(typeof(ITrayLogManager), info.LogManager);
            sp.AddService(typeof(ITrayProxyManager), info.ProxyManager);
            sp.AddService(typeof(ITrayCookieManager), info.CookieManager);
            sp.AddService(typeof(ITraySessionStateManager), info.SessionStateManager);
            sp.AddService(typeof(ITrayCacheManager), info.TrayCacheManager);
            sp.AddService(typeof(ITraySynchronizer), info.Synchronizer);

            return sp;
        }

        private TrayAppServiceInfo[] _GetAllTrayServiceInfos()
        {
            lock (_thisLock)
            {
                return _dict.Values.SelectMany(v => v.GetAll()).ToArray();
            }
        }

        private readonly ITrayLogManager _trayLogManager;

        /// <summary>
        /// 卸载服务
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public bool UnloadService(ServiceDesc serviceDesc)
        {
            Contract.Requires(serviceDesc != null);

            return UnloadService(serviceDesc.Name, serviceDesc.Version);
        }

        /// <summary>
        /// 卸载服务
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        public bool UnloadService(string name, SVersion version)
        {
            Contract.Requires(name != null);

            lock (_thisLock)
            {
                IAppService service = GetService(name, version);
                if (service != null)
                {
                    service.Dispose();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 收集运行时信息
        /// </summary>
        private void _CollectRuntimeInfo()
        {
            GC.Collect();
            GC.WaitForFullGCComplete();

            foreach (TrayAppServiceInfo si in _GetAllTrayServiceInfos())
            {
                try
                {
                    si.RuntimeInfo = _GetRuntimeInfo(si.AppDomain);
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
            }
        }

        // 获取指定AppDomain的运行时信息
        private AppServiceRuntimeInfo _GetRuntimeInfo(AppDomain domain)
        {
            return new AppServiceRuntimeInfo() { Memory = domain.MonitoringSurvivedMemorySize };
        }

        private volatile bool _disposed = false;

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);

            _collectAppServiceRuntimeInfoHandle.Dispose();
            _cookieManager.Dispose();

            lock (_thisLock)
            {
                if (_disposed)
                    return;

                foreach (TrayAppServiceInfo info in _GetAllTrayServiceInfos())
                {
                    try
                    {
                        info.Service.Dispose();
                    }
                    catch (Exception ex)
                    {
                        LogManager.LogError(ex);
                    }
                }

                _disposed = true;
            }
        }

        ~TrayAppServiceManager()
        {
            Dispose();
        }

        /// <summary>
        /// 服务终端的唯一标识
        /// </summary>
        public Guid ClientID
        {
            get
            {
                return _GetClientID();
            }
        }

        private readonly ThreadSafeDictionaryWrapper<string, object> _data = new ThreadSafeDictionaryWrapper<string, object>();

        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetData(string key, object value)
        {
            Contract.Requires(key != null);

            if (value == null)
                _data.Remove(key);
            else
                _data[key] = value;
        }

        /// <summary>
        /// 获取客户端唯一标识
        /// </summary>
        /// <returns></returns>
        private Guid _GetClientID()
        {
            object id = GetData(SystemDataKeys.CLIENT_ID);
            if (id is Guid)
                return (Guid)id;

            throw new InvalidOperationException("尚未设置终端唯一标识");
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetData(string key)
        {
            Contract.Requires(key != null);
            return _data.GetOrDefault(key);
        }

        private readonly HandlerManager _handlerManager = new HandlerManager();

        /// <summary>
        /// 获取注册的句柄
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] GetHandlers<T>() where T : class
        {
            return _handlerManager.GetHandlers<T>();
        }

        /// <summary>
        /// 获取注册的句柄
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetHandler<T>() where T : class
        {
            return _handlerManager.GetHandler<T>();
        }

        #region Class PlatformServiceProvider ...

        class PlatformServiceProvider : ServiceProvider, IServiceProvider
        {
            public PlatformServiceProvider()
            {
                
            }
        }

        #endregion
    }

}

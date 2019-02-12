using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Contracts;
using System.Threading;
using System.Net;
using Common.Package;
using Common.Package.TaskDispatcher;
using Common.Contracts.Log;
using System.Data.Common;
using System.Data;
using System.Configuration;
using System.Security;
using System.Net.Sockets;
using Common.Communication.Wcf;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// Tray App Service 应用程序实体
    /// </summary>
    public abstract class TrayAppServiceApplicationBase : ApplicationBase, IServiceManagerCallback
    {
        public TrayAppServiceApplicationBase()
        {
            _trayLogManager = new Lazy<InnerTrayLogManager>(() => new InnerTrayLogManager(CreateLogWriter(), CreateLogReader()));
            _serviceManager = new Lazy<TrayAppServiceManager>(delegate {
                return new TrayAppServiceManager(this, _trayLogManager.Value);
            });

            AppInvokeManager = new AppInvokeManager();
        }

        private Lazy<InnerTrayLogManager> _trayLogManager;

        private Lazy<TrayAppServiceManager> _serviceManager;

        public TrayAppServiceManager ServiceManager { get { return _serviceManager.Value; } }

        public AppInvokeManager AppInvokeManager { get; private set; }

        private Action<string, string[]> _assemblyHostCallback;

        /// <summary>
        /// 日志读写器
        /// </summary>
        public ITrayLogManager TrayLogManager
        {
            get { return _trayLogManager.Value; }
        }

        /// <summary>
        /// 运行该应用程序
        /// </summary>
        public sealed override void Run(Action<string, string[]> callback, WaitHandle waitHandle)
        {
            _assemblyHostCallback = callback;

            OnLoadServices();

            OnStart();

            if (waitHandle != null)
                waitHandle.WaitOne();

            OnStop();
        }

        /// <summary>
        /// 加载服务
        /// </summary>
        protected virtual void OnLoadServices()
        {
            
        }

        private void _RunningFunc()
        {
            
        }

        /// <summary>
        /// 启动
        /// </summary>
        protected virtual void OnStart()
        {
            LogManager.LogMessage("系统启动成功");
        }

        /// <summary>
        /// 停止
        /// </summary>
        protected virtual void OnStop()
        {
            _callServiceTasks.Dispose();
            ServiceManager.Dispose();
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="context"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <param name="settings"></param>
        protected virtual CommunicateData Call(CommunicateContext context, string method, CommunicateData data, CallingSettings settings)
        {
            return ServiceManager.CallService(context, method, data, settings);
        }

        public override void Dispose()
        {
            OnStop();
            _proxyLifeManager.Dispose();
            ServiceManager.Dispose();
        }

        /// <summary>
        /// 回调方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="serviceInfo"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <param name="option"></param>
        /// <param name="settings"></param>
        /// <param name="serviceExists"></param>
        /// <returns></returns>
        public virtual CommunicateData Callback(CommunicateContext context, string method, CommunicateData data, CommunicationOption option, CallingSettings settings)
        {
            string serviceName = MethodParser.GetServiceName(method);
            settings = settings ?? CallingSettings.RequestReply;

            if (settings.NeedReply())
            {
                // 尝试内部调用
                return _CallInternal(context, method, data, settings);
            }
            else
            {
                // 不需要应答
                MethodParser mp = new MethodParser(method);
                if (ServiceManager.ContainsService(mp.Service, mp.ServiceVersion))
                {
                    _AddToCallDispatcher(context, method, data, settings);
                    return null;
                }

                return new CommunicateData(null, data.DataFormat, TrayUtility.ServiceNotFoundStatusCode_SelfNotFound);
            }
        }

        // 内部调用
        private CommunicateData _CallInternal(CommunicateContext context, string method, CommunicateData data, CallingSettings settings)
        {
            try
            {
                return ServiceManager.CallService(context, method, data, settings);
            }
            catch (ServiceException ex)
            {
                return new CommunicateData(null, data.DataFormat, ex.StatusCode, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return new CommunicateData(null, data.DataFormat, ServerErrorCode.ArgumentError, ex.Message);
            }
            catch (NotSupportedException ex)
            {
                return new CommunicateData(null, data.DataFormat, ServerErrorCode.NotSupported, ex.Message);
            }
            catch (DataException ex)
            {
                return new CommunicateData(null, data.DataFormat, ServerErrorCode.DataError, ex.Message);
            }
            catch (DbException ex)
            {
                return new CommunicateData(null, data.DataFormat, ServerErrorCode.DataError, ex.Message);
            }
            catch (ConfigurationException ex)
            {
                return new CommunicateData(null, data.DataFormat, ServerErrorCode.ConfigurationError, ex.Message);
            }
            catch (SecurityException ex)
            {
                return new CommunicateData(null, data.DataFormat, ServerErrorCode.SecurityError, ex.Message);
            }
            catch (SocketException ex)
            {
                return new CommunicateData(null, data.DataFormat, ClientErrorCode.NetworkError, ex.Message);
            }
            catch (Exception ex)
            {
                return new CommunicateData(null, data.DataFormat, ServerErrorCode.ServerError, ex.Message);
            }
        }

        private readonly TaskDispatcher<CallServiceTask> _callServiceTasks = new TaskDispatcher<CallServiceTask>(10);

        private void _AddToCallDispatcher(CommunicateContext context, string method, CommunicateData data, CallingSettings settings)
        {
            _callServiceTasks.Add(new CallServiceTask(this, context, method, data, settings));
        }

        #region Class TrayLogManager ...

        class InnerTrayLogManager : MarshalByRefObjectEx, ITrayLogManager
        {
            public InnerTrayLogManager(ILogWriter<LogItem> logWriter, ILogReader<LogItem> logReader)
            {
                Writer = logWriter;
                Reader = logReader;
            }

            public ILogWriter<LogItem> Writer { get; private set; }

            public ILogReader<LogItem> Reader { get; private set; }
        }

        #endregion

        #region Class CallServiceTask ...

        class CallServiceTask : ITask
        {
            public CallServiceTask(TrayAppServiceApplicationBase owner, CommunicateContext context, string method, CommunicateData data, CallingSettings settings)
            {
                _owner = owner;
                Context = context;
                Method = method;
                Data = data;
                Settings = settings;
            }

            public string Method { get; private set; }

            public CommunicateData Data { get; private set; }

            public CallingSettings Settings { get; private set; }

            public CommunicateContext Context { get; private set; }

            private readonly TrayAppServiceApplicationBase _owner;

            public void Execute()
            {
                try
                {
                    _owner.ServiceManager.CallService(Context, Method, Data, Settings);
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
            }
        }

        #endregion

        /// <summary>
        /// 启动侦听
        /// </summary>
        /// <param name="option"></param>
        public abstract void StartListener(CommunicationOption option);

        /// <summary>
        /// 停止侦听
        /// </summary>
        /// <param name="address"></param>
        public abstract bool StopListener(ServiceAddress address);

        /// <summary>
        /// 创建通道工厂
        /// </summary>
        /// <returns></returns>
        public abstract ICommunicateFactory CreateCommunicateFactory();

        /// <summary>
        /// 创建会话状态管理器
        /// </summary>
        /// <returns></returns>
        public abstract ITraySessionStateManager CreateTraySessionStateManager();

        /// <summary>
        /// 更新调用列表
        /// </summary>
        /// <param name="serviceInfos"></param>
        public void UpdateInvokeInfos(AppInvokeInfo[] invokeInfos)
        {
            AppInvokeManager.Update(invokeInfos);
        }

        /// <summary>
        /// 获取调用列表
        /// </summary>
        /// <returns></returns>
        public AppInvokeInfo[] GetInvokeInfos()
        {
            return AppInvokeManager.Get();
        }

        /// <summary>
        /// 获取所有的通信方式
        /// </summary>
        /// <returns></returns>
        public abstract CommunicationOption[] GetAllCommunicateOptions();

        private readonly TrayAppServiceApplicationProxyLifeManager _proxyLifeManager = new TrayAppServiceApplicationProxyLifeManager();

        /// <summary>
        /// 是否启用代理
        /// </summary>
        public virtual bool ProxyEnabled
        {
            get { return _proxyLifeManager.Enabled; }
        }

        /// <summary>
        /// 确保代理处于开启状态
        /// </summary>
        /// <param name="owner"></param>
        public void KeepProxyEnable(object owner)
        {
            _proxyLifeManager.KeepEnable(owner);
        }

        /// <summary>
        /// 禁用代理
        /// </summary>
        /// <param name="owner"></param>
        public void DisableProxy(object owner)
        {
            _proxyLifeManager.Disable(owner);
        }

        /// <summary>
        /// 重新启动平台
        /// </summary>
        /// <param name="args"></param>
        /// <param name="commandName"></param>
        public void DoCommand(string commandName, string[] args)
        {
            if (commandName == "LiveUpdate" || commandName == "Restart" || commandName == "Exit")
            {
                if (_assemblyHostCallback != null)
                    _assemblyHostCallback(commandName, args);
            }
        }
    }
}

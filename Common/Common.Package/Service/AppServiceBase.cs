using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Contracts;
using Common.Utility;
using System.Threading;
using Common.Collection;
using System.Diagnostics.Contracts;

namespace Common.Package.Service
{
    /// <summary>
    /// AppService的基类
    /// </summary>
    public abstract class AppServiceBase : MarshalByRefObjectEx, IAppService
    {
        public AppServiceBase()
        {
            _communicate = new AppServiceCommunicate(this);
            _appServiceInfo = new Lazy<AppServiceInfo>(GetServiceInfo);
            _objectPropertyLoader = new ObjectPropertyLoader(this);

            AppComponentManager = new AppComponentManager(this);
        }

        /// <summary>
        /// 组件管理器
        /// </summary>
        public AppComponentManager AppComponentManager { get; private set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="initModel"></param>
        /// <returns></returns>
        AppServiceInfo IAppService.Init(IServiceProvider sp, AppServiceInitModel initModel)
        {
            if (sp == null)
                throw new ArgumentNullException("sp");

            try
            {
                AppServiceInfo info = AppServiceInfo;
                ServiceProvider = sp;
                if (initModel == AppServiceInitModel.Execute)
                    OnInit(info);

                return info;
            }
            finally
            {
                _waitForInitCompleted.Set();
            }
        }

        /// <summary>
        /// 等待服务到指定的状态
        /// </summary>
        /// <param name="waitType">等待类型</param>
        /// <param name="millsecondsTimeout">超时时间（毫秒数）</param>
        /// <returns>如果超时，则返回false</returns>
        public bool Wait(AppServiceWaitType waitType, int millsecondsTimeout)
        {
            switch (waitType)
            {
                case AppServiceWaitType.InitCompleted:
                    return WaitForInitCompleted(millsecondsTimeout);

                case AppServiceWaitType.Running:
                    return WaitForRunning(millsecondsTimeout);
            }

            return false;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="info">服务信息</param>
        protected virtual void OnInit(AppServiceInfo info)
        {

        }

        /// <summary>
        /// 获取服务的信息
        /// </summary>
        /// <returns></returns>
        protected abstract AppServiceInfo GetServiceInfo();

        private readonly AppCommandCollection _appCommands = new AppCommandCollection();

        /// <summary>
        /// 获取全部的指令
        /// </summary>
        /// <returns></returns>
        public virtual IAppCommand[] GetAllCommands()
        {
            return _appCommands.GetAppCommands();
        }

        /// <summary>
        /// 获取指令
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public virtual IAppCommand GetCommand(string name, SVersion version = default(SVersion))
        {
            Contract.Requires(name != null);

            return _appCommands.GetAppCommand(name, version);
        }

        /// <summary>
        /// 获取指令
        /// </summary>
        /// <param name="commandDesc"></param>
        /// <returns></returns>
        public IAppCommand GetCommand(CommandDesc commandDesc)
        {
            Contract.Requires(commandDesc != null);

            return GetCommand(commandDesc.Name, commandDesc.Version);
        }
        
        /// <summary>
        /// 添加一个指令
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="command"></param>
        public virtual void AddCommand(IAppCommand command)
        {
            Contract.Requires(command != null);

            _appCommands.Add(command);
        }

        /// <summary>
        /// 批量添加指令
        /// </summary>
        /// <param name="commands"></param>
        public void AddCommands(IEnumerable<IAppCommand> commands)
        {
            Contract.Requires(commands != null);

            foreach (IAppCommand command in commands)
            {
                AddCommand(command);
            }
        }

        /// <summary>
        /// 移除一个指令
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        public virtual void RemoveCommand(string name, SVersion version = default(SVersion))
        {
            Contract.Requires(name != null);

            _appCommands.RemoveCommand(name, version);
        }

        /// <summary>
        /// 移除一个指令
        /// </summary>
        /// <param name="commandDesc"></param>
        public void RemoveCommand(CommandDesc commandDesc)
        {
            Contract.Requires(commandDesc != null);

            RemoveCommand(commandDesc.Name, commandDesc.Version);
        }

        /// <summary>
        /// 批量移除指令
        /// </summary>
        /// <param name="commandDescs"></param>
        public void RemoveCommands(IEnumerable<CommandDesc> commandDescs)
        {
            Contract.Requires(commandDescs != null);

            foreach (CommandDesc cd in commandDescs)
            {
                RemoveCommand(cd);
            }
        }

        /// <summary>
        /// 移除一个指令
        /// </summary>
        /// <param name="command"></param>
        public void RemoveCommand(IAppCommand command)
        {
            Contract.Requires(command != null);

            AppCommandInfo info = command.GetInfo();
            RemoveCommand(info.CommandDesc);
        }

        /// <summary>
        /// 批量移除指令
        /// </summary>
        /// <param name="commands"></param>
        public void RemoveCommands(IEnumerable<IAppCommand> commands)
        {
            Contract.Requires(commands != null);

            foreach (IAppCommand command in commands)
            {
                RemoveCommand(command);
            }
        }

        private readonly object _thisLock = new object();

        /// <summary>
        /// 启动
        /// </summary>
        void IAppService.Start()
        {
            lock (_thisLock)
            {
                _ValidateDisposed();
                if (_status == AppServiceStatus.Running)
                    return;

                OnStart();
                Status = AppServiceStatus.Running;
                OnStarted();
            }
        }

        /// <summary>
        /// 启动
        /// </summary>
        protected virtual void OnStart()
        {
            AppComponentManager.Start();
        }

        /// <summary>
        /// 完成启动
        /// </summary>
        protected virtual void OnStarted()
        {

        }

        /// <summary>
        /// 停止
        /// </summary>
        void IAppService.Stop()
        {
            lock (_thisLock)
            {
                _ValidateDisposed();
                if (_status != AppServiceStatus.Running)
                    return;

                OnStop();
                Status = AppServiceStatus.Stopped;
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        protected virtual void OnStop()
        {
            AppComponentManager.Stop();
        }

        /// <summary>
        /// 等待程序启动
        /// </summary>
        /// <param name="millsecondsTimeout"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public bool WaitForRunning(int millsecondsTimeout = 0, bool throwError = false)
        {
            if (Status == AppServiceStatus.Running)
                return true;

            bool r = WaitHandle.WaitAny(new WaitHandle[] { _waitForStartEvent, _waitForDispose }, millsecondsTimeout) == 0;
            if (!r && throwError)
                throw new ServiceException(ServerErrorCode.ServiceInvalid, "服务尚未运行");

            return r;
        }

        private readonly ManualResetEvent _waitForInitCompleted = new ManualResetEvent(false);

        /// <summary>
        /// 等待初始化完毕
        /// </summary>
        /// <param name="millsecondsTimeout"></param>
        /// <returns></returns>
        public bool WaitForInitCompleted(int millsecondsTimeout = 15 * 1000)
        {
            return _waitForInitCompleted.WaitOne(millsecondsTimeout);
        }

        /// <summary>
        /// 等待初始化完毕
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool WaitForInitCompleted(TimeSpan timeout)
        {
            if (Status != AppServiceStatus.Init)
                return true;

            return WaitForInitCompleted((int)timeout.TotalMilliseconds);
        }

        /// <summary>
        /// 创建AppCommand的执行环境
        /// </summary>
        /// <param name="context"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public virtual AppCommandExecuteContext CreateAppCommandExecuteContext(CommunicateContext context, CallingSettings settings)
        {
            AppServiceInfo sInfo = AppServiceInfo;
            return new AppCommandExecuteContext(this, this.ServiceProvider, sInfo.DefaultDataFormat, context, settings, (sid) => GetSessionState(sid, false));
        }

        /// <summary>
        /// 获取用户的会话状态
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public UserSessionState GetSessionState(Sid sid, bool throwError = true)
        {
            UserSessionState uss = OnGetSessionState(sid);
            if (uss == null && throwError)
                throw new ServiceException(ServerErrorCode.InvalidUser, "用户未登录");

            return uss;
        }

        /// <summary>
        /// 获取用户的会话状态
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        protected abstract UserSessionState OnGetSessionState(Sid sid);

        /// <summary>
        /// 执行调用
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="method">指令</param>
        /// <param name="data">数据</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        protected virtual CommunicateData OnCall(CommunicateContext context, string method, CommunicateData data, CallingSettings settings)
        {
            MethodParser mp = new MethodParser(method);
            IAppCommand cmd = GetCommand(mp.CommandDesc);

            AppCommandInfo cmdInfo = null;
            if (cmd != null && (cmdInfo = cmd.GetInfo()) != null && cmdInfo.Usable == UsableType.Disabled)
                throw new ServiceException(ServerErrorCode.Disabled, string.Format("接口“{0}”已禁用", method));

            WaitForRunning(15 * 1000, true);

            try
            {
                CommunicateData replyData = _appCommands.Call(CreateAppCommandExecuteContext(context, settings), mp.Command, data, mp.CommandVersion);
                if (cmdInfo != null && cmdInfo.Usable == UsableType.Obsolete && replyData != null && replyData.StatusCode == (int)ServiceStatusCode.Ok)
                {
                    replyData.StatusCode = (int)SuccessCode.Obsolete;
                    replyData.StatusDesc = string.Format("接口“{0}”已不推荐使用", method);
                }

                return replyData;
            }
            catch (ServiceException ex)
            {
                if (ex.StatusCode == (int)ServerErrorCode.NotFound)
                    throw new ServiceException(ServerErrorCode.NotFound, string.Format("接口“{0}”在服务“{1}”中不存在", mp.Command, mp.Service));

                throw;
            }
        }

        private volatile AppServiceStatus _status;

        /// <summary>
        /// 当前状态
        /// </summary>
        public virtual AppServiceStatus Status
        {
            get { return _status; }
            protected set
            {
                _status = value;
                if (_status == AppServiceStatus.Running)
                    _waitForStartEvent.Set();
                else
                    _waitForStartEvent.Reset();
            }
        }

        private readonly ManualResetEvent _waitForStartEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _waitForDispose = new ManualResetEvent(false);

        private ICommunicate _communicate;

        /// <summary>
        /// 通信方式
        /// </summary>
        ICommunicate IAppService.Communicate
        {
            get { return _communicate; }
        }

        /// <summary>
        /// 与平台通信的接口
        /// </summary>
        public IServiceProvider ServiceProvider { get; private set; }

        private readonly Lazy<AppServiceInfo> _appServiceInfo;

        /// <summary>
        /// 服务的详细信息
        /// </summary>
        public AppServiceInfo AppServiceInfo
        {
            get { return _appServiceInfo.Value; }
        }

        [ObjectProperty(false)]
        private readonly ObjectPropertyLoader _objectPropertyLoader;

        /// <summary>
        /// 获取所有属性的描述
        /// </summary>
        /// <returns></returns>
        public virtual ObjectProperty[] GetAllProperties()
        {
            return _objectPropertyLoader.GetAllProperties();
        }

        /// <summary>
        /// 获取指定名称的属性值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public virtual ObjectPropertyValue GetPropertyValue(string propertyName)
        {
            return _objectPropertyLoader.GetPropertyValue(propertyName);
        }

        /// <summary>
        /// 设置指定名称的属性值
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetPropertyValue(ObjectPropertyValue value)
        {
            _objectPropertyLoader.SetPropertyValue(value);
        }

        private void _ValidateDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(this.GetType().ToString());
        }

        private volatile bool _disposed = false;

        /// <summary>
        /// Dispose
        /// </summary>
        void IDisposable.Dispose()
        {
            lock (_thisLock)
            {
                if (_disposed)
                    return;

                ((IAppService)this).Stop();
                OnDispose();

                _disposed = true;
                _waitForDispose.Set();
            }
        }

        protected virtual void OnDispose()
        {
            AppComponentManager.Dispose();
        }

        #region Class AppServiceCommunicate ...

        class AppServiceCommunicate : MarshalByRefObjectEx, ICommunicate
        {
            public AppServiceCommunicate(AppServiceBase owner)
            {
                _owner = owner;
            }

            private readonly AppServiceBase _owner;

            public CommunicateData Call(CommunicateContext context, string method, CommunicateData data, CallingSettings settings)
            {
                return _owner.OnCall(context, method, data, settings);
            }

            public void Dispose()
            {
                
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package;
using System.Diagnostics.Contracts;
using Common.Utility;
using System.Threading;
using Common.Contracts;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Common.Package.Service
{
    /// <summary>
    /// 最简单的组件
    /// </summary>
    public class AppComponent : MarshalByRefObjectEx, IAppComponent
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner"></param>
        public AppComponent(object owner)
        {
            Owner = owner;
            _appComponentInfo = new Lazy<AppComponentInfo>(_LoadAppComponentInfo);
            _objectPropertyLoader = new ObjectPropertyLoader(this);
        }

        /// <summary>
        /// 所有者
        /// </summary>
        public virtual object Owner { get; private set; }

        private volatile bool _running;
        private readonly object _thisLocker = new object();

        /// <summary>
        /// 状态
        /// </summary>
        public virtual bool Running
        {
            get { return _running; }
            set
            {
                lock (_thisLocker)
                {
                    if (_running == value)
                        return;

                    if (value)
                        _Start();
                    else
                        _Stop();

                    _running = value;
                }
            }
        }

        private void _Start()
        {
            OnStart();
        }

        private void _Stop()
        {
            OnStop();
        }

        /// <summary>
        /// 启动
        /// </summary>
        protected virtual void OnStart()
        {

        }

        /// <summary>
        /// 停止
        /// </summary>
        protected virtual void OnStop()
        {

        }

        /// <summary>
        /// 获取相关属性
        /// </summary>
        /// <param name="onlyKeys"></param>
        /// <returns></returns>
        public virtual IDictionary<string, object> GetProperties(bool onlyKeys)
        {
            return new Dictionary<string, object>();
        }

        /// <summary>
        /// 自动重试
        /// </summary>
        /// <param name="action"></param>
        /// <param name="interval">周期</param>
        /// <param name="maxTryTimes">重试次数</param>
        /// <returns>是否执行成功</returns>
        protected bool AutoRetry(Action action, TimeSpan interval, int maxTryTimes = int.MaxValue)
        {
            Contract.Requires(action != null);

            return CommonUtility.AutoTry(action, delegate(int tryTimes, Exception error) {
                LogManager.LogError(error);
                if (tryTimes >= maxTryTimes || Disposed)
                    return false;

                Thread.Sleep(interval);
                return true;
            });
        }

        /// <summary>
        /// 自动重试
        /// </summary>
        /// <param name="func"></param>
        /// <param name="interval">周期</param>
        /// <param name="maxTryTimes">重试次数</param>
        /// <returns>是否执行成功</returns>
        protected bool AutoRetry(Func<bool> func, TimeSpan interval, int maxTryTimes = int.MaxValue)
        {
            Contract.Requires(func != null);

            return CommonUtility.AutoTry(func, delegate(int tryTimes, Exception error) {
                LogManager.LogError(error);
                if (tryTimes >= maxTryTimes || Disposed)
                    return false;

                Thread.Sleep(interval);
                return true;
            });
        }


        /// <summary>
        /// 自动重试
        /// </summary>
        /// <param name="action"></param>
        /// <param name="init">起始间隔</param>
        /// <param name="step">递增步长</param>
        /// <param name="max">最大间隔</param>
        /// <param name="maxTryTimes">最大重试次数</param>
        /// <returns>是否执行成功</returns>
        protected bool AutoRetry(Action action, TimeSpan init, TimeSpan step, TimeSpan max, int maxTryTimes = int.MaxValue)
        {
            Contract.Requires(action != null);

            TimeSpan interval = init;
            return CommonUtility.AutoTry(action, delegate(int tryTimes, Exception error) {
                LogManager.LogError(error);
                if (tryTimes >= maxTryTimes || Disposed)
                    return false;

                interval = MathUtility.Min(interval + step, max);
                Thread.Sleep(interval);
                return true;
            });
        }

        /// <summary>
        /// 自动重试
        /// </summary>
        /// <param name="action"></param>
        /// <param name="maxTryTimes">最大重试次数</param>
        /// <returns>是否执行成功</returns>
        protected bool AutoRetry(Action action, int maxTryTimes)
        {
            return AutoRetry(action, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30), maxTryTimes);
        }

        /// <summary>
        /// 自动重试
        /// </summary>
        /// <param name="action"></param>
        /// <returns>是否执行成功</returns>
        protected bool AutoRetry(Action action)
        {
            return AutoRetry(action, int.MaxValue);
        }

        /// <summary>
        /// 自动重试
        /// </summary>
        /// <param name="func"></param>
        /// <param name="init">起始间隔</param>
        /// <param name="step">递增步长</param>
        /// <param name="max">最大间隔</param>
        /// <param name="maxTryTimes">最大重试次数</param>
        /// <returns>是否执行成功</returns>
        protected bool AutoRetry(Func<bool> func, TimeSpan init, TimeSpan step, TimeSpan max, int maxTryTimes = int.MaxValue)
        {
            Contract.Requires(func != null);

            TimeSpan interval = init;
            return CommonUtility.AutoTry(func, delegate(int tryTimes, Exception error) {
                LogManager.LogError(error);
                if (tryTimes >= maxTryTimes || Disposed)
                    return false;

                interval = MathUtility.Min(interval + step, max);
                Thread.Sleep(interval);
                return true;
            });
        }

        /// <summary>
        /// 自动重试
        /// </summary>
        /// <param name="func"></param>
        /// <param name="init">起始间隔</param>
        /// <param name="step">递增步长</param>
        /// <param name="max">最大间隔</param>
        /// <param name="maxTryTimes">最大重试次数</param>
        /// <returns>是否执行成功</returns>
        protected bool AutoRetry(Func<bool> func)
        {
            return AutoRetry(func, int.MaxValue);
        }

        /// <summary>
        /// 自动重试
        /// </summary>
        /// <param name="func"></param>
        /// <param name="maxTryTimes">最大重试次数</param>
        /// <returns>是否执行成功</returns>
        protected bool AutoRetry(Func<bool> func, int maxTryTimes)
        {
            return AutoRetry(func, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30), maxTryTimes);
        }

        /// <summary>
        /// 执行方法并记录异常
        /// </summary>
        /// <param name="action"></param>
        /// <returns>异常</returns>
        protected Exception InvokeNoThrow(Action action)
        {
            Exception error = action.TryInvoke();
            if (error != null)
                LogManager.LogError(error);

            return error;
        }

        [ObjectProperty(false)]
        private readonly ObjectPropertyLoader _objectPropertyLoader;

        /// <summary>
        /// 获取所有属性
        /// </summary>
        /// <returns></returns>
        public virtual ObjectProperty[] GetAllProperties()
        {
            return _objectPropertyLoader.GetAllProperties();
        }

        /// <summary>
        /// 获取指定名称的属性值
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        public virtual ObjectPropertyValue GetPropertyValue(string propertyName)
        {
            return _objectPropertyLoader.GetPropertyValue(propertyName);
        }

        /// <summary>
        /// 设置指定名称的属性值
        /// </summary>
        /// <param name="value">值</param>
        public virtual void SetPropertyValue(Contracts.ObjectPropertyValue value)
        {
            _objectPropertyLoader.SetPropertyValue(value);
        }

        private ServiceResult _avaliableState;

        /// <summary>
        /// 设置有效状态
        /// </summary>
        /// <param name="state">状态</param>
        public void SetAvaliableState(ServiceResult state)
        {
            _avaliableState = state;
        }

        /// <summary>
        /// 检查有效状态
        /// </summary>
        public virtual void ValidateAvaliableState()
        {
            if (_avaliableState != null && !_avaliableState.Succeed)
                throw _avaliableState.CreateException();
        }

        private readonly Lazy<AppComponentInfo> _appComponentInfo;

        private AppComponentInfo _LoadAppComponentInfo()
        {
            Type t = GetType();
            string defaultName = t.Name.TrimEnd("AppComponent");
            AppComponentAttribute attr = t.GetAttribute<AppComponentAttribute>();
            if (attr != null)
            {
                return new AppComponentInfo(
                    StringUtility.GetFirstNotNullOrWhiteSpaceString(attr.Name, defaultName), attr.Title, attr.Desc, attr.Category
                );
            }

            return new AppComponentInfo(defaultName);
        }

        /// <summary>
        /// 获取组件详细信息
        /// </summary>
        /// <returns></returns>
        public virtual AppComponentInfo GetInfo()
        {
            return _appComponentInfo.Value;
        }

        private volatile bool _disposed = false;

        /// <summary>
        /// 是否已经Dispose
        /// </summary>
        public bool Disposed
        {
            get { return _disposed; }
            protected set { _disposed = value; }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            lock (_thisLocker)
            {
                if (!Disposed)
                {
                    _exitWaitHandle.Set();
                    OnDispose();
                    Disposed = true;
                }
            }
        }

        private readonly ManualResetEvent _exitWaitHandle = new ManualResetEvent(false);

        /// <summary>
        /// 等待退出的信号
        /// </summary>
        public WaitHandle ExitWaitHandle
        {
            get { return _exitWaitHandle; }
        }

        /// <summary>
        /// 等待信号，如果遇到组件的Dispose，则返回false
        /// </summary>
        /// <param name="eh"></param>
        /// <returns></returns>
        public bool Wait(WaitHandle eh)
        {
            Contract.Requires(eh != null);

            return WaitHandle.WaitAny(new WaitHandle[] { eh, _exitWaitHandle }) == 0;
        }

        /// <summary>
        /// 等待信号，正常返回0，如果遇到组件的Dispose，则返回1，超时则返回WaitHandle.WaitTimeout
        /// </summary>
        /// <param name="eh"></param>
        /// <param name="timeoutMillseconds"></param>
        /// <returns></returns>
        public int Wait(WaitHandle eh, int timeoutMillseconds)
        {
            Contract.Requires(eh != null);

            return WaitHandle.WaitAny(new WaitHandle[] { eh, _exitWaitHandle });
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected virtual void OnDispose()
        {
            
        }
    }
}

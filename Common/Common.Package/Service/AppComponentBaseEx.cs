using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Utility;
using System.Threading;
using Common.Contracts.Service;

namespace Common.Package.Service
{
    /// <summary>
    /// 具有定时任务、状态控制等功能的组件
    /// </summary>
    public abstract class AppComponentBaseEx : AppComponentBase
    {
        public AppComponentBaseEx(object owner)
            : base(owner)
        {
            _controllerContext = new AppComponentControllerContext(this);
        }

        protected override IAppComponentControllerContext CreateControllerContext()
        {
            return _controllerContext;
        }

        private readonly AppComponentControllerContext _controllerContext;

        protected override IServiceProvider CreateServiceProvider()
        {
            ServiceProvider sp = new ServiceProvider();
            sp.AddService(typeof(IAppComponentStatusFunction), new AppComponentStatusService(this));
            sp.AddService(typeof(IAppComponentTaskFunction), new AppComponentTaskService(this));
            return sp;
        }

        #region Class AppComponentControllerContext ...

        class AppComponentControllerContext : IAppComponentControllerContext
        {
            public AppComponentControllerContext(AppComponentBaseEx component)
            {
                _component = component;
                _serviceProvider = _component.CreateServiceProvider();
            }

            private readonly AppComponentBaseEx _component;
            private readonly IServiceProvider _serviceProvider;
            private readonly object _thisLocker = new object();

            public IServiceProvider ServiceProvider
            {
                get { return _serviceProvider; }
            }
        }

        #endregion

        #region Class AppComponentTaskService ...

        class AppComponentTaskService : IAppComponentTaskFunction
        {
            public AppComponentTaskService(AppComponentBaseEx component)
            {
                _component = component;
            }

            private readonly AppComponentBaseEx _component;

            public void Execute(string taskName)
            {
                _component._ExecuteTask(taskName);
            }
        }

        #endregion

        #region Class AppComponentStatusService ...

        class AppComponentStatusService : IAppComponentStatusFunction
        {
            public AppComponentStatusService(AppComponentBaseEx component)
            {
                _component = component;
            }

            private readonly AppComponentBaseEx _component;
            private volatile AppComponentStatus _status;
            private readonly object _thisLocker = new object();

            public AppComponentStatus Status
            {
                get
                {
                    return _status;
                }
                set
                {
                    lock(_thisLocker)
                    {
                        if (value != _status)
                        {
                            _status = value;
                            _component._OnStatusChanged(value);
                            _RaiseStatusChangedEvent(value);
                        }
                    }
                }
            }

            private void _RaiseStatusChangedEvent(AppComponentStatus status)
            {
                var eh = StatusChanged;
                if (eh != null)
                    eh(this, new AppComponentStatusChangedEventArgs(status));
            }

            /// <summary>
            /// 状态变化事件
            /// </summary>
            public event EventHandler<AppComponentStatusChangedEventArgs> StatusChanged;
        }

        #endregion

        /// <summary>
        /// 状态发生变化
        /// </summary>
        /// <param name="status"></param>
        protected virtual void OnStatusChanged(AppComponentStatus status)
        {

        }

        private void _OnStatusChanged(AppComponentStatus status)
        {
            _status = status;
            OnStatusChanged(status);
        }

        private AppComponentStatus _status = AppComponentStatus.Disable;

        /// <summary>
        /// 状态
        /// </summary>
        public AppComponentStatus Status
        {
            get { return _status; }
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="taskName"></param>
        private void _ExecuteTask(string taskName)
        {
            if (_status != AppComponentStatus.Enable)
                return;

            try
            {
                OnExecuteTask(taskName);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
            }
        }

        public override void ValidateAvaliableState()
        {
            if (Status != AppComponentStatus.Enable)
                throw new ServiceException(ServerErrorCode.ServiceInvalid, string.Format("组件“{0}”尚未启动", GetInfo().Name));

            base.ValidateAvaliableState();
        }

        /// <summary>
        /// 需要执行的定时任务
        /// </summary>
        /// <param name="taskName"></param>
        protected virtual void OnExecuteTask(string taskName)
        {

        }
    }
}

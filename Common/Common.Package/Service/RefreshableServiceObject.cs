using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Package.GlobalTimer;
using Common.Utility;

namespace Common.Package.Service
{
    /// <summary>
    /// 支持刷新逻辑的ServiceObject
    /// </summary>
    public class RefreshableServiceObject : MarshalByRefObjectEx, IRefreshableServiceObject
    {
        private RefreshableServiceObject(ServiceObject serviceObject)
        {
            _serviceObject = serviceObject;
            _timerHandle = GlobalTimer<ITask>.Default.Add(TimeSpan.FromSeconds(1), new TaskFuncAdapter(_RefreshFunc), false, false);
        }

        private readonly HashSet<IServiceObjectRefresher> _refreshers = new HashSet<IServiceObjectRefresher>();
        private readonly IGlobalTimerTaskHandle _timerHandle;
        private volatile bool _refreshable_refreshing = false, _refreshable_disposed = false;

        private void _RefreshFunc()
        {
            try
            {
                IServiceObjectRefresher[] rs;
                lock (_refreshers) rs = _refreshers.ToArray();

                foreach (IServiceObjectRefresher refresher in rs)
                {
                    ServiceObjectRefreshResult r = refresher.Refresh();

                    if (!_ApplyRefreshResult(r, refresher))
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                Error.RaiseEvent(this, ex);
            }
        }

        private bool _ApplyRefreshResult(ServiceObjectRefreshResult r, IServiceObjectRefresher refresher)
        {
            if ((r & ServiceObjectRefreshResult.Dispose) != 0)
            {
                _refreshers.Clear();
                _refreshable_disposed = true;
            }

            if ((r & (ServiceObjectRefreshResult.Refresh | ServiceObjectRefreshResult.Dispose)) != 0)
                Refresh.RaiseEvent(this);

            if ((r & ServiceObjectRefreshResult.Completed) != 0)
            {
                _RemoveRefresher(refresher);
            }

            if (_refreshable_disposed || _disposed)
            {
                Dispose();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 开始刷新
        /// </summary>
        /// <param name="refresher"></param>
        public void StartRefresh(IServiceObjectRefresher refresher)
        {
            Contract.Requires(refresher != null);

            lock (_refreshers)
            {
                _refreshers.Add(refresher);
                _refreshable_refreshing = true;
                _timerHandle.Start();
            }
        }

        /// <summary>
        /// 开始刷新
        /// </summary>
        /// <param name="func"></param>
        public void StartRefresh(ServiceObjectRefreshFunc func)
        {
            Contract.Requires(func != null);

            StartRefresh(new ServiceObjectRefreshFuncAdapter(func));
        }

        /// <summary>
        /// 停止刷新
        /// </summary>
        /// <param name="r"></param>
        public void StopAllRefresh(ServiceObjectRefreshResult r = ServiceObjectRefreshResult.Completed)
        {
            lock (_refreshers)
            {
                foreach (IServiceObjectRefresher refresher in _refreshers.ToArray())
                {
                    _ApplyRefreshResult(r, refresher);
                }
            }
        }

        /// <summary>
        /// 停止刷新
        /// </summary>
        /// <param name="refresher"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public void StopRefresh(IServiceObjectRefresher refresher, ServiceObjectRefreshResult r = ServiceObjectRefreshResult.Completed)
        {
            Contract.Requires(refresher != null);
            lock (_refreshers)
            {
                _ApplyRefreshResult(r, refresher);
            }
        }

        private void _RemoveRefresher(IServiceObjectRefresher refresher)
        {
            lock (_refreshers)
            {
                _refreshers.Remove(refresher);
                if (_refreshers.Count == 0)
                    _timerHandle.Stop();
            }
        }

        private readonly ServiceObject _serviceObject;

        /// <summary>
        /// 出现错误
        /// </summary>
        public event ErrorEventHandler Error;

        /// <summary>
        /// 刷新
        /// </summary>
        public event EventHandler Refresh;

        /// <summary>
        /// 是否正在刷新
        /// </summary>
        public bool IsRefreshing
        {
            get { return _refreshable_refreshing; }
        }

        /// <summary>
        /// 是否已经销毁
        /// </summary>
        public bool IsDisposed
        {
            get { return _refreshable_disposed; }
        }

        public ServiceObjectGroup[] GetGroups()
        {
            return _serviceObject.GetGroups();
        }

        public ServiceObjectAction[] GetActions(ServiceObjectActionType actionType)
        {
            return _serviceObject.GetActions(actionType);
        }

        public object DoAction(string actionName, IDictionary<string, object> parameters)
        {
            return _serviceObject.DoAction(actionName, parameters);
        }

        public ServiceObjectProperty[] GetProperties()
        {
            return _serviceObject.GetProperties();
        }

        public object GetPropertyValue(string propertyName)
        {
            return _serviceObject.GetPropertyValue(propertyName);
        }

        public ServiceObjectInfo Info
        {
            get { return _serviceObject.Info; }
        }

        public IDictionary<object, object> Items
        {
            get { return _serviceObject.Items; }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            if (!_disposed)
            {
                _disposed = true;

                _timerHandle.Dispose();
            }
        }

        ~RefreshableServiceObject()
        {
            Dispose();
        }

        private volatile bool _disposed;

        /// <summary>
        /// 从对象中加载
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static RefreshableServiceObject FromObject(object obj, ServiceObjectInfo info = null)
        {
            Contract.Requires(obj != null);
            ServiceObject so = ServiceObject.FromObject(obj);

            return new RefreshableServiceObject(so);
        }
    }
}

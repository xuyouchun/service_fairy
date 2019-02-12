using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Framework.TrayPlatform;
using Common.Collection;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Utility;
using ServiceKey = System.Tuple<System.Guid, Common.Contracts.Service.ServiceDesc>;
using Common;
using ServiceFairy.SystemInvoke;
using Common.Package;

namespace ServiceFairy.Service.Station.Components
{
    /// <summary>
    /// 发布订阅管理器
    /// </summary>
    [AppComponent("发布订阅管理器", "在服务之间实现发布订阅的交互方式", AppComponentCategory.Application, name: "ServiceEventManager")]
    class ServiceEventManagerAppComponent : TimerAppComponentBase
    {
        public ServiceEventManagerAppComponent(TrayAppServiceBase service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;
        }

        private readonly TrayAppServiceBase _service;

        private readonly Dictionary<string, HashSet<EventTarget>> _eventsByNames = new Dictionary<string, HashSet<EventTarget>>();
        private readonly Dictionary<ServiceKey, EventTargetGroup> _eventsByServiceKey = new Dictionary<ServiceKey, EventTargetGroup>();

        private readonly RwLocker _locker = new RwLocker();

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="eventNames"></param>
        public void Register(ServiceEndPoint endpoint, string[] eventNames)
        {
            Contract.Requires(endpoint != null && eventNames != null);
            Register(endpoint.ClientId, endpoint.ServiceDesc, eventNames);
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <param name="clientId"></param>
        /// <param name="eventNames"></param>
        public void Register(Guid clientId, ServiceDesc serviceDesc, string[] eventNames)
        {
            Contract.Requires(eventNames != null && serviceDesc != null);

            using (_locker.Write())
            {
                ServiceKey key = new ServiceKey(clientId, serviceDesc);
                EventTargetGroup g = _eventsByServiceKey.GetOrDefault(key);
                if (g != null)
                    _RemoveRange(g.EventTargets);

                g = new EventTargetGroup();
                foreach (string eventName in eventNames.WhereNotNull())
                {
                    EventTarget et = new EventTarget(eventName, clientId, serviceDesc);
                    _eventsByNames.GetOrSet(eventName).Add(et);
                    g.EventTargets.Add(et);
                }

                if (g.EventTargets.Count == 0)
                    _eventsByServiceKey.Remove(key);
                else
                    _eventsByServiceKey[key] = g;
            }
        }

        private void _RemoveRange(IEnumerable<EventTarget> ets)
        {
            foreach (EventTarget et in ets)
            {
                _Remove(et);
            }
        }

        private void _Remove(EventTarget et)
        {
            HashSet<EventTarget> hs;
            if (_eventsByNames.TryGetValue(et.EventName, out hs))
            {
                hs.Remove(et);
            }
        }

        /// <summary>
        /// 取消注册事件
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="serviceDesc"></param>
        public void Unregister(Guid clientId, ServiceDesc serviceDesc)
        {
            Contract.Requires(serviceDesc != null);

            using (_locker.Write())
            {
                ServiceKey key = new ServiceKey(clientId, serviceDesc);
                EventTargetGroup g;
                if (_eventsByServiceKey.TryGetValue(key, out g))
                {
                    _eventsByServiceKey.Remove(key);
                    _RemoveRange(g.EventTargets);
                }
            }
        }

        /// <summary>
        /// 取消注册事件
        /// </summary>
        /// <param name="endpoint"></param>
        public void Unregister(ServiceEndPoint endpoint)
        {
            Contract.Requires(endpoint != null);

            Unregister(endpoint.ClientId, endpoint.ServiceDesc);
        }

        /// <summary>
        /// 激发事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="eventName"></param>
        /// <param name="eventArgs"></param>
        /// <param name="enableRoute">是否通知其它的Register服务继续分发该事件</param>
        public void Raise(ServiceEndPoint source, string eventName, byte[] eventArgs, bool enableRoute = true)
        {
            Contract.Requires(source != null && eventName != null);

            EventTarget[] ets;
            using (_locker.Read())
            {
                ets = _eventsByNames.GetOrDefault(eventName, new HashSet<EventTarget>()).ToArray();
            }

            // 通知各个订阅者
            if (ets.Length > 0)
            {
                try
                {
                    IServiceClient serviceClient = _service.Context.CreateBroadcastServiceClient(
                        ets.ToArray(et => new ServiceEndPoint(et.ClientId, et.ServiceDesc)));

                    CoreInvoker invoker = CoreInvoker.FromServiceClient(serviceClient);
                    invoker.Sys.OnEvent(source, eventName, eventArgs, CallingSettings.OneWay);
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
            }

            // 通知各个Station服务，继续分发该事件
            if (enableRoute)
            {
                try
                {
                    IServiceClient serviceClient = _service.Context.CreateBoradcastServiceClientOfMyself();
                    CoreInvoker invoker = CoreInvoker.FromServiceClient(serviceClient);
                    invoker.Station.RaiseEvent(source, eventName, eventArgs, false);
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
            }
        }

        #region Class EventTarget ...

        class EventTarget
        {
            public EventTarget(string eventName, Guid clientId, ServiceDesc serviceDesc)
            {
                Contract.Requires(eventName != null && serviceDesc != null);

                EventName = eventName;
                ClientId = clientId;
                ServiceDesc = serviceDesc;
            }

            /// <summary>
            /// 事件类型
            /// </summary>
            public string EventName { get; private set; }

            /// <summary>
            /// 服务终端
            /// </summary>
            public Guid ClientId { get; private set; }

            /// <summary>
            /// 服务
            /// </summary>
            public ServiceDesc ServiceDesc { get; private set; }

            public override bool Equals(object obj)
            {
                if (obj == null || obj.GetType() != typeof(EventTarget))
                    return false;

                EventTarget e = (EventTarget)obj;
                return e.ClientId == ClientId && e.EventName == EventName && e.ServiceDesc == ServiceDesc;
            }

            public override int GetHashCode()
            {
                return ClientId.GetHashCode() ^ EventName.GetHashCode() ^ ServiceDesc.GetHashCode();
            }

            public static bool operator ==(EventTarget e1, EventTarget e2)
            {
                return object.Equals(e1, e2);
            }

            public static bool operator !=(EventTarget e1, EventTarget e2)
            {
                return !object.Equals(e1, e2);
            }
        }

        #endregion

        #region Class EventTargetGroup ...

        class EventTargetGroup
        {
            public EventTargetGroup()
            {
                EventTargets = new HashSet<EventTarget>();
                LastUpdate = DateTime.UtcNow;
            }

            public HashSet<EventTarget> EventTargets { get; private set; }

            public DateTime LastUpdate { get; private set; }
        }

        #endregion

        // 超过60秒，将被视为过期
        protected override void OnExecuteTask(string taskName)
        {
            using (_locker.Write())
            {
                DateTime now = DateTime.UtcNow;
                var items = _eventsByServiceKey.RemoveWhereWithReturn(item => now - item.Value.LastUpdate > TimeSpan.FromMinutes(1));
                _RemoveRange(items.SelectMany(item => item.Value.EventTargets));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Collection;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Framework.TrayPlatform;
using Common.Package;
using Common.Package.Serializer;
using Common.Package.Service;
using Common.Package.TaskDispatcher;
using Common.Utility;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Sys;
using ServiceFairy.SystemInvoke;
using System.Diagnostics;

namespace ServiceFairy.Components
{
    /// <summary>
    /// 服务事件的客户端管理器，负责事件注册请求的心跳逻辑
    /// </summary>
    [AppComponent("发布订阅终端", "向" + SFNames.ServiceNames.Station + "服务注册事件、接收事件及引发事件", category: AppComponentCategory.System, name: "Sys_ServiceEvent")]
    public class ServiceEventAppComponent : TimerAppComponentBase
    {
        public ServiceEventAppComponent(TrayAppServiceBase service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;

            service.AddCommand(_cmd = new OnEventAppCommand(this));
        }

        private readonly TrayAppServiceBase _service;
        private readonly OnEventAppCommand _cmd;
        private readonly TaskDispatcher<ITask> _onEventTaskDispatcher = TaskDispatcher<ITask>.Default;

        private readonly ThreadSafeDictionaryWrapper<string, HashSet<EventItemBase>> _events
            = new ThreadSafeDictionaryWrapper<string, HashSet<EventItemBase>>();

        #region Class EventItem ...

        abstract class EventItemBase
        {
            public EventItemBase(ServiceEventAppComponent owner, string eventName, Delegate eh)
            {
                Owner = owner;
                EventName = eventName;
                _eh = eh;
            }

            public ServiceEventAppComponent Owner { get; private set; }

            public string EventName { get; private set; }

            private readonly Delegate _eh;

            public abstract void Raise(ServiceEventArgs e);

            public override int GetHashCode()
            {
                return EventName.GetHashCode() ^ _eh.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj == null || obj.GetType() != this.GetType() || !(obj is EventItemBase))
                    return false;

                EventItemBase ei = (EventItemBase)obj;
                return ei.EventName == EventName && object.Equals(ei._eh, _eh);
            }
        }

        class EventItem : EventItemBase
        {
            public EventItem(ServiceEventAppComponent owner, string eventName, ServiceEventHandler eventHandler)
                : base(owner, eventName, eventHandler)
            {
                EventHandler = eventHandler;
            }

            /// <summary>
            /// 事件句柄
            /// </summary>
            public ServiceEventHandler EventHandler { get; private set; }

            public override void Raise(ServiceEventArgs e)
            {
                EventHandler(Owner, e);
            }
        }

        class EventItem<TEntity> : EventItemBase where TEntity : EventEntity
        {
            public EventItem(ServiceEventAppComponent owner, string eventName, ServiceEventHandler<TEntity> eventHandler)
                : base(owner, eventName, eventHandler)
            {
                EventHandler = eventHandler;
            }

            private readonly ServiceEventHandler<TEntity> EventHandler;

            public override void Raise(ServiceEventArgs e)
            {
                EventHandler(Owner, new ServiceEventArgs<TEntity>(e));
            }
        }

        #endregion

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventHandler"></param>
        public void Register(string eventName, ServiceEventHandler eventHandler)
        {
            Contract.Requires(eventName != null && eventHandler != null);

            lock (_events)
            {
                HashSet<EventItemBase> hs = _events.GetOrSet(eventName);
                if (hs.Add(new EventItem(this, eventName, eventHandler)))
                    ExecuteImmediately();
            }
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventHandler"></param>
        public void Register<TEntity>(ServiceEventHandler<TEntity> eventHandler) where TEntity : EventEntity
        {
            Contract.Requires(eventHandler != null);

            string eventName = EventEntity.GetEventName<TEntity>();
            HashSet<EventItemBase> hs = _events.GetOrSet(eventName);
            lock (hs)
            {
                if (hs.Add(new EventItem<TEntity>(this, eventName, eventHandler)))
                    ExecuteImmediately();
            }
        }

        /// <summary>
        /// 取消注册事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventHandler"></param>
        public void Unregister(string eventName, ServiceEventHandler eventHandler)
        {
            Contract.Requires(eventName != null && eventHandler != null);

            lock (_events)
            {
                HashSet<EventItemBase> hs;
                if (_events.TryGetValue(eventName, out hs))
                {
                    if (hs.Remove(new EventItem(this, eventName, eventHandler)))
                        ExecuteImmediately();
                }
            }
        }

        /// <summary>
        /// 取消注册事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventHandler"></param>
        public void Unregister<TEntity>(ServiceEventHandler<TEntity> eventHandler) where TEntity : EventEntity
        {
            Contract.Requires(eventHandler != null);

            lock (_events)
            {
                string eventName = EventEntity.GetEventName<TEntity>();
                HashSet<EventItemBase> hs;
                if (_events.TryGetValue(eventName, out hs))
                {
                    if (hs.Remove(new EventItem<TEntity>(this, eventName, eventHandler)))
                        ExecuteImmediately();
                }
            }
        }

        /// <summary>
        /// 判断指定的事件是否已注册
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventHandler"></param>
        /// <returns></returns>
        public bool IsRegisted(string eventName, ServiceEventHandler eventHandler)
        {
            Contract.Requires(eventName != null && eventName != null);

            lock (_events)
            {
                HashSet<EventItemBase> hs;
                if (_events.TryGetValue(eventName, out hs))
                {
                    return hs.Contains(new EventItem(this, eventName, eventHandler));
                }
            }

            return false;
        }

        /// <summary>
        /// 判断指定的事件是否已注册
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventHandler"></param>
        /// <returns></returns>
        public bool IsRegisted<TEntity>(ServiceEventHandler<TEntity> eventHandler) where TEntity : EventEntity
        {
            Contract.Requires(eventHandler != null);

            lock (_events)
            {
                string eventName = EventEntity.GetEventName<TEntity>();
                HashSet<EventItemBase> hs;
                if (_events.TryGetValue(eventName, out hs))
                {
                    return hs.Contains(new EventItem<TEntity>(this, eventName, eventHandler));
                }
            }

            return false;
        }

        /// <summary>
        /// 切换事件的注册状态
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventHandler"></param>
        /// <param name="register"></param>
        public bool Switch(string eventName, ServiceEventHandler eventHandler, bool? register = null)
        {
            if (register == null)
                register = !IsRegisted(eventName, eventHandler);

            if (register == true)
                Register(eventName, eventHandler);
            else
                Unregister(eventName, eventHandler);

            return (bool)register;
        }

        /// <summary>
        /// 切换事件的注册状态
        /// </summary>
        /// <param name="eventHandler"></param>
        /// <param name="register"></param>
        public bool Switch<TEntity>(ServiceEventHandler<TEntity> eventHandler, bool? register = null) where TEntity : EventEntity
        {
            Contract.Requires(eventHandler != null);

            string eventName = EventEntity.GetEventName<TEntity>();
            if (register == null)
                register = !IsRegisted<TEntity>(eventHandler);

            if (register == true)
                Register<TEntity>(eventHandler);
            else
                Unregister<TEntity>(eventHandler);

            return (bool)register;
        }

        /// <summary>
        /// 切换事件的注册状态
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventHandler"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool Switch(string eventName, ServiceEventHandler eventHandler, AppComponentStatus status)
        {
            return Switch(eventName, eventHandler, (status == AppComponentStatus.Enable));
        }

        /// <summary>
        /// 切换事件的注册状态
        /// </summary>
        /// <param name="eventHandler"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool Switch<TEntity>(ServiceEventHandler<TEntity> eventHandler, AppComponentStatus status) where TEntity : EventEntity
        {
            return Switch<TEntity>(eventHandler, (status == AppComponentStatus.Enable));
        }

        /// <summary>
        /// 服务事件
        /// </summary>
        public event ServiceEventHandler OnEvent;

        private void _RaiseOnEventEvent(ServiceEndPoint source, string eventName, byte[] eventArgs)
        {
            ServiceEventArgs e = new ServiceEventArgs(source, eventName, eventArgs);
            var eh = OnEvent;

            EventItemBase[] eis = null;
            HashSet<EventItemBase> hs = _events.GetOrDefault(eventName);
            if (hs != null)
            {
                lock (hs) eis = hs.ToArray();
            }

            if (eh == null && hs == null)
                return;

            _onEventTaskDispatcher.Add(new TaskFuncAdapter(delegate {

                if (eh != null)
                    _TryInvoke(() => eh(this, e));

                if (eis != null)
                {
                    _onEventTaskDispatcher.AddRange(eis.Select(ei => new TaskFuncAdapter(delegate {
                        _TryInvoke(() => ei.Raise(e));
                    })));
                }
            }));
        }

        private void _TryInvoke(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
            }
        }

        /// <summary>
        /// 将事件的数据添加到Cookie，将会通过心跳的逻辑发送到Register服务器
        /// </summary>
        protected override void OnExecuteTask(string taskName)
        {
            RegisterEventCookieData cookie = new RegisterEventCookieData() { Items = _GetAllEventItems() };
            _service.Context.CookieManager.AddCookie(ServiceCookieNames.EVENT_REGISTER, SerializerUtility.SerializeToBytes(DataFormat.Unknown, cookie));
        }

        private RegisterEventCookieItem[] _GetAllEventItems()
        {
            lock (_events)
            {
                return _events.Values.SelectMany(hs => _GetEventNames(hs))
                    .ToArray(name => new RegisterEventCookieItem() { EventName = name });
            }
        }

        private static string[] _GetEventNames(HashSet<EventItemBase> hs)
        {
            return hs.Select(ei => ei.EventName).ToArray();
        }

        #region Class OnEventAppCommand ...

        [AppCommand("Sys_OnEvent", "在发布订阅模型中激发事件", AppCommandCategory.System, SecurityLevel = SecurityLevel.Public)]
        class OnEventAppCommand : ACS<TrayAppServiceBase>.Action<Sys_OnEvent_Request>
        {
            public OnEventAppCommand(ServiceEventAppComponent owner)
            {
                _owner = owner;
            }

            private readonly ServiceEventAppComponent _owner;

            protected override void OnExecute(AppCommandExecuteContext<TrayAppServiceBase> context, Sys_OnEvent_Request req, ref ServiceResult sr)
            {
                try
                {
                    _owner._RaiseOnEventEvent(req.Source, req.EventName, req.EventArgs);
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
            }
        }

        #endregion

        /// <summary>
        /// 激发事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="eventArgs">事件参数</param>
        /// <param name="settings">调用设置</param>
        public void Raise(string eventName, byte[] eventArgs, CallingSettings settings)
        {
            Contract.Requires(eventName != null);

            SystemInvoker invoker = SystemInvoker.FromServiceClient(_service.Context.ServiceClient);
            invoker.Station.RaiseEvent(_service.Context.ServiceEndPoint, eventName, eventArgs, true, settings);
        }

        /// <summary>
        /// 激发事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="eventArgs">事件参数</param>
        /// <param name="sync">是否为同步调用，默认为异步</param>
        public void Raise(string eventName, byte[] eventArgs, bool sync = false)
        {
            Raise(eventName, eventArgs, sync ? CallingSettings.RequestReply : CallingSettings.OneWay);
        }

        /// <summary>
        /// 激发事件
        /// </summary>
        /// <typeparam name="TEntity">参数类型</typeparam>
        /// <param name="eventArgs">事件参数</param>
        /// <param name="sync">是否为同步调用，默认为异步</param>
        public void Raise<TEntity>(TEntity eventArgs, bool sync = false)
            where TEntity : EventEntity
        {
            Raise(EventEntity.GetEventName<TEntity>(),
                eventArgs == null ? null : SerializerUtility.OptimalBinarySerialize(eventArgs),
                sync);
        }

        /// <summary>
        /// 激发事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="sync">是否为同步调用</param>
        public void Raise(string eventName, bool sync)
        {
            Raise(eventName, null, sync);
        }

        /// <summary>
        /// 同步激发事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventArgs"></param>
        public void SyncRaise(string eventName, byte[] eventArgs = null)
        {
            Raise(eventName, eventArgs, true);
        }

        /// <summary>
        /// 同步激发事件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="eventName"></param>
        /// <param name="eventArgs"></param>
        public void SyncRaise<TEntity>(string eventName, TEntity eventArgs)
            where TEntity : EventEntity
        {
            Raise<TEntity>(eventArgs, true);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            _onEventTaskDispatcher.Dispose();
            _service.RemoveCommand(_cmd);
        }
    }

    /// <summary>
    /// 服务事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ServiceEventHandler(object sender, ServiceEventArgs e);

    /// <summary>
    /// 服务事件委托
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ServiceEventHandler<TEntity>(object sender, ServiceEventArgs<TEntity> e) where TEntity : EventEntity;

    #region Class ServiceEventArgs ...

    /// <summary>
    /// 服务事件参数
    /// </summary>
    public class ServiceEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="eventName">事件名称</param>
        /// <param name="eventArgs">事件参数</param>
        public ServiceEventArgs(ServiceEndPoint source, string eventName, byte[] eventArgs)
        {
            Contract.Requires(source != null);

            Source = source;
            EventName = eventName;
            EventArgs = eventArgs;
        }

        /// <summary>
        /// 服务终端标识
        /// </summary>
        public ServiceEndPoint Source { get; private set; }

        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName { get; private set; }

        /// <summary>
        /// 服务参数
        /// </summary>
        public byte[] EventArgs { get; private set; }
    }

    #endregion

    #region Class ServiceEventArgs<TEntity> ...

    /// <summary>
    /// 服务事件参数
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class ServiceEventArgs<TEntity> : ServiceEventArgs where TEntity : EventEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="eventName">事件名称</param>
        /// <param name="eventArgs">事件参数</param>
        public ServiceEventArgs(ServiceEndPoint source, string eventName, byte[] eventArgs)
            : base(source, eventName, eventArgs)
        {
            _entity = new Lazy<TEntity>(_LoadEntity);
        }

        public ServiceEventArgs(ServiceEventArgs e)
            : this(e.Source, e.EventName, e.EventArgs)
        {

        }

        private readonly Lazy<TEntity> _entity;

        private TEntity _LoadEntity()
        {
            if (EventArgs.IsNullOrEmpty())
                return null;

            return SerializerUtility.OptimalBinaryDeserialize<TEntity>(EventArgs);
        }

        /// <summary>
        /// 参数实体
        /// </summary>
        public TEntity Entity
        {
            get
            {
                return _entity.Value;
            }
        }
    }

    #endregion

    /// <summary>
    /// 注册事件的Cookie
    /// </summary>
    [Serializable, DataContract]
    public class RegisterEventCookieData
    {
        [DataMember]
        public RegisterEventCookieItem[] Items { get; set; }
    }

    /// <summary>
    /// 注册事件的Cookie
    /// </summary>
    [Serializable, DataContract]
    public class RegisterEventCookieItem
    {
        [DataMember]
        public string EventName { get; set; }
    }
}

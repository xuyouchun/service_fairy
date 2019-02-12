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
using Common.Utility;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Sys;
using ServiceFairy.SystemInvoke;
using System.Diagnostics;
using ServiceFairy.Entities.Station;

namespace ServiceFairy.Components
{
    /// <summary>
    /// 插件管理器
    /// </summary>
    [AppComponent("插件管理终端", "向" + SFNames.ServiceNames.Station + "服务注册插件", AppComponentCategory.System, "Sys_ServiceAddin")]
    public class ServiceAddinAppComponent : TimerAppComponentBase
    {
        public ServiceAddinAppComponent(CoreAppServiceBase service)
            : base(service, TimeSpan.FromSeconds(1))
        {
            _service = service;
            _commands = ServiceFairyUtility.LoadAppCommandsFromInstance(this);
        }

        private readonly CoreAppServiceBase _service;
        private readonly AppCommandCollection _commands;
        private readonly Dictionary<AddinDesc, HashSet<AppServiceAddinItem>> _addins = new Dictionary<AddinDesc, HashSet<AppServiceAddinItem>>();

        /// <summary>
        /// 注册一个插件
        /// </summary>
        /// <param name="target"></param>
        /// <param name="addin"></param>
        public ServiceAddinHandle Register(ServiceDesc target, IAppServiceAddin addin)
        {
            Contract.Requires(target != null && addin != null);

            AppServiceAddinInfo info = addin.GetInfo();

            lock (_addins)
            {
                AppServiceAddinItem item = new AppServiceAddinItem(target, _service.Context.ServiceEndPoint, addin);
                _addins.GetOrSet(info.AddinDesc).Add(item);

                return new ServiceAddinHandle(this, target, addin);
            }
        }

        /// <summary>
        /// 注册一个插件
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="addin"></param>
        public ServiceAddinHandle Register(string serviceName, IAppServiceAddin addin)
        {
            Contract.Requires(serviceName != null && addin != null);

            return Register(new ServiceDesc(serviceName, SVersion.Empty), addin);
        }

        /// <summary>
        /// 取消注册一个插件
        /// </summary>
        /// <param name="target"></param>
        /// <param name="addin"></param>
        internal void Unregister(ServiceDesc target, IAppServiceAddin addin)
        {
            Contract.Requires(target != null && addin != null);

            lock (_addins)
            {
                HashSet<AppServiceAddinItem> hs;
                if (_addins.TryGetValue(addin.GetInfo().AddinDesc, out hs))
                    hs.Remove(new AppServiceAddinItem(target, _service.Context.ServiceEndPoint, addin));
            }
        }

        /// <summary>
        /// 取消注册一个插件
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="addin"></param>
        internal void Unregister(string serviceName, IAppServiceAddin addin)
        {
            Contract.Requires(serviceName != null && addin != null);

            Unregister(new ServiceDesc(serviceName, SVersion.Empty), addin);
        }

        /// <summary>
        /// 获取所有已注册的插件
        /// </summary>
        /// <returns></returns>
        internal AppServiceAddinItem[] GetAllRegisteredAddins()
        {
            lock (_addins)
            {
                return _addins.SelectMany(addin => addin.Value).ToArray();
            }
        }

        protected override void OnExecuteTask(string taskName)
        {
            ServiceAddinCookieData data;
            lock (_addins)
            {
                data = new ServiceAddinCookieData() {
                    Relations = _addins.SelectMany(item => item.Value.Select(
                        v => new ServiceAddinRelation { AddinInfo = v.Info, Source = _service.Context.ServiceEndPoint, Target = v.Target })
                    ).ToArray()
                };
            }

            _service.Context.CookieManager.AddCookie(ServiceCookieNames.ADDIN_REGISTER,
                SerializerUtility.SerializeToBytes(DataFormat.Unknown, data)
            );
        }

        /// <summary>
        /// 获取注册到当前服务的指定名称的插件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AppServiceAddinItem[] GetAddins(string name)
        {
            return _cache.GetOrAddOfRelative(name ?? _EMPTY_ADDIN_NAME, TimeSpan.FromSeconds(5), (key) => {
                AppServiceCookie[] cookies = _service.Context.CookieManager.GetReplyCookies(ServiceCookieNames.ADDIN_LIST);
                return cookies.SelectMany(cookie => SerializerUtility.Deserialize<ServiceAddinRelation[]>(DataFormat.Unknown, cookie.Data))
                    .Where(v => name == null || v.AddinInfo.AddinDesc.Name == name)
                    .ToArray(r => new AppServiceAddinItem(r.Target, r.Source, new AppServiceAddinRemoteProxy(this, r))
                );
            });
        }

        /// <summary>
        /// 获取注册到当前服务的所有插件
        /// </summary>
        /// <returns></returns>
        public AppServiceAddinItem[] GetAllAddins()
        {
            return GetAddins(null);
        }

        private const string _EMPTY_ADDIN_NAME = "CA33DFC7-28E2-4B53-883F-7589976A95BA";
        private readonly Cache<string, AppServiceAddinItem[]> _cache = new Cache<string, AppServiceAddinItem[]>();

        /// <summary>
        /// 获取注册到当前服务的指定名称的插件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AppServiceAddinItem GetAddin(string name)
        {
            return GetAddins(name).FirstOrDefault();
        }

        #region Class AppServiceAddinRemoteProxy ...

        class AppServiceAddinRemoteProxy : MarshalByRefObjectEx, IAppServiceAddin
        {
            public AppServiceAddinRemoteProxy(ServiceAddinAppComponent component, ServiceAddinRelation relation)
            {
                _relation = relation;
                _component = component;
                _service = component._service;

                _communicate = new CommunicateProxy(this);
            }

            private readonly ServiceAddinAppComponent _component;
            private readonly TrayAppServiceBaseEx _service;
            private readonly ServiceAddinRelation _relation;
            private readonly CommunicateProxy _communicate;

            public AppServiceAddinInfo GetInfo()
            {
                return _relation.AddinInfo;
            }

            public ICommunicate Communicate
            {
                get { return _communicate; }
            }

            public void Dispose()
            {

            }

            #region Class CommunicateProxy ...

            class CommunicateProxy : ICommunicate
            {
                public CommunicateProxy(AppServiceAddinRemoteProxy owner)
                {
                    _owner = owner;
                    _addinDesc = _owner._relation.AddinInfo.AddinDesc;
                    TrayAppServiceBaseEx service = _owner._service;
                    ServiceEndPoint source = _owner._relation.Source;

                    ICommunicate communicate = (source.ClientId == Guid.Empty) ?
                        _owner._service.Context.Communicate : service.Context.CommunicateFactory.CreateCommunicate(source.ClientId);
                    communicate = service.Context.CreateServiceDescCommunicateDecorate(source.ServiceDesc, communicate);
                    _invoker = CoreInvoker.FromServiceClient(service.Context.CreateServiceClient(communicate));
                }

                private readonly AppServiceAddinRemoteProxy _owner;
                private readonly CoreInvoker _invoker;
                private readonly AddinDesc _addinDesc;

                public CommunicateData Call(CommunicateContext context, string method, CommunicateData data, CallingSettings settings = null)
                {
                    return _invoker.Sys.AddinCall(_addinDesc, method, data, settings);
                }

                public void Dispose()
                {

                }
            }

            #endregion
        }

        #endregion

        protected override void OnStart()
        {
            base.OnStart();

            _service.AddCommands(_commands);
        }

        protected override void OnStop()
        {
            base.OnStop();

            _service.RemoveCommands(_commands);
        }

        #region Class AddinCallAppCommand ...

        /// <summary>
        /// 插件调用
        /// </summary>
        [AppCommand("Sys_AddinCall", title: "插件调用", category: AppCommandCategory.System)]
        class AddinCallAppCommand : AppCommandBase<Sys_OnAddinCall_Request, Sys_OnAddinCall_Reply>
        {
            public AddinCallAppCommand(ServiceAddinAppComponent owner)
            {
                _owner = owner;
            }

            private readonly ServiceAddinAppComponent _owner;

            protected override Sys_OnAddinCall_Reply OnExecute(AppCommandExecuteContext context, Sys_OnAddinCall_Request req, ref ServiceResult sr)
            {
                HashSet<AppServiceAddinItem> hs;
                AppServiceAddinItem[] items;
                if (!_owner._addins.TryGetValue(req.AddinDesc, out hs) || (items = _ToArray(hs)).Length == 0)
                    throw new ServiceException(ServerErrorCode.NotFound, "插件" + req.AddinDesc + "不存在");

                IAppServiceAddin addin = items[0].Addin;
                CommunicateData returnValue = addin.Communicate.Call(context.CommunicateContext, req.Method, req.Argument, context.Settings);

                return new Sys_OnAddinCall_Reply() { ReturnValue = returnValue };
            }

            private AppServiceAddinItem[] _ToArray(HashSet<AppServiceAddinItem> hs)
            {
                lock (hs)
                {
                    return hs.ToArray();
                }
            }
        }

        #endregion

        #region Class GetAddinsInfosAppCommand ...

        /// <summary>
        /// 获取插件信息
        /// </summary>
        [AppCommand("Sys_GetAddinsInfos", "获取插件信息", AppCommandCategory.System)]
        class GetAddinsInfosAppCommand : ACS<CoreAppServiceBase>.Func<Sys_GetAddinsInfos_Reply>
        {
            public GetAddinsInfosAppCommand(ServiceAddinAppComponent component)
            {
                _component = component;
            }

            private readonly ServiceAddinAppComponent _component;

            protected override Sys_GetAddinsInfos_Reply OnExecute(AppCommandExecuteContext<CoreAppServiceBase> context, ref ServiceResult sr)
            {
                return new Sys_GetAddinsInfos_Reply() {
                    AddinInfos = _component.GetAllAddins().Select(
                        addin => new AppServiceAddinInfoItem { Info = addin.Info, Source = addin.Source, Target = addin.Target, AddinType = AppServiceAddinType.In }
                    ).Union(_component.GetAllRegisteredAddins().Select(
                        addin => new AppServiceAddinInfoItem { Info = addin.Info, Source = addin.Source, Target = addin.Target, AddinType = AppServiceAddinType.Out }
                    )).ToArray()
                };
            }
        }

        #endregion

    }

    #region Class ServiceAddinCookieData ...

    /// <summary>
    /// 服务插件的Cookie数据
    /// </summary>
    [Serializable, DataContract]
    public class ServiceAddinCookieData
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        [DataMember]
        public ServiceAddinRelation[] Relations { get; set; }
    }

    #endregion

    #region Class ServiceAddinHandle ...

    /// <summary>
    /// 服务插件的注册句柄
    /// </summary>
    public class ServiceAddinHandle : IDisposable
    {
        internal ServiceAddinHandle(ServiceAddinAppComponent component, ServiceDesc target, IAppServiceAddin addin)
        {
            _component = component;
            _target = target;
            _addin = addin;
        }

        private readonly ServiceAddinAppComponent _component;
        private readonly ServiceDesc _target;
        private readonly IAppServiceAddin _addin;

        public void Dispose()
        {
            _component.Unregister(_target, _addin);
        }
    }

    #endregion

    #region Class AppServiceAddinItem ...

    public class AppServiceAddinItem
    {
        internal AppServiceAddinItem(ServiceDesc target, ServiceEndPoint source, IAppServiceAddin addin)
        {
            Target = target;
            Source = source;
            Addin = addin;
        }

        /// <summary>
        /// 注册目标
        /// </summary>
        public ServiceDesc Target { get; private set; }

        /// <summary>
        /// 所在位置
        /// </summary>
        public ServiceEndPoint Source { get; private set; }

        /// <summary>
        /// 插件
        /// </summary>
        public IAppServiceAddin Addin { get; private set; }

        private AppServiceAddinInfo _info;

        /// <summary>
        /// 插件信息
        /// </summary>
        public AppServiceAddinInfo Info { get { return _info ?? (_info = Addin.GetInfo()); } }

        public override int GetHashCode()
        {
            return Source.GetHashCode() ^ Target.GetHashCode() ^ Addin.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(AppServiceAddinItem))
                return false;

            AppServiceAddinItem item = (AppServiceAddinItem)obj;
            return object.Equals(item.Source, Source) && object.Equals(item.Addin, Addin) && item.Target == Target;
        }

        public static bool operator ==(AppServiceAddinItem obj1, AppServiceAddinItem obj2)
        {
            return object.Equals(obj1, obj2);
        }

        public static bool operator !=(AppServiceAddinItem obj1, AppServiceAddinItem obj2)
        {
            return !object.Equals(obj1, obj2);
        }
    }

    #endregion
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using Common.Utility;
using Common.Contracts;
using Common.Contracts.Log;
using Common.Package;
using System.Diagnostics.Contracts;
using Common.Communication.Wcf;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// 带有安全功能的AppService基类
    /// </summary>
    public abstract class TrayAppServiceBase : AssemblyAppServiceBase
    {
        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            IServiceProvider sp = ServiceProvider;
            Context = new TrayContext(this, new ServiceProviderDecorate(this, sp), info);
        }

        /// <summary>
        /// 平台环境
        /// </summary>
        public TrayContext Context { get; private set; }

        #region Class ServiceProviderDecorate ...

        class ServiceProviderDecorate : IServiceProvider
        {
            public ServiceProviderDecorate(TrayAppServiceBase owner, IServiceProvider sp)
            {
                _owner = owner;
                _sp = sp;
            }

            private readonly TrayAppServiceBase _owner;
            private readonly IServiceProvider _sp;

            public object GetService(Type serviceType)
            {
                return _owner.OnGetPlatformService(_sp, serviceType);
            }
        }

        #endregion

        /// <summary>
        /// 获取平台服务
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual object OnGetPlatformService(IServiceProvider sp, Type type)
        {
            object srv = sp.GetService(type);

            if (type == typeof(ICommunicateFactory))
            {
                srv = new SecurityCommunicateFactoryDecorate(this, (ICommunicateFactory)srv);
            }

            return srv;
        }

        /// <summary>
        /// 获取用户的会话状态
        /// </summary>
        /// <param name="sid">安全码</param>
        /// <returns></returns>
        protected override UserSessionState OnGetSessionState(Sid sid)
        {
            if (sid.IsEmpty())
                return null;

            return _sessionState = Context.SessionStateManager.GetSessionState(sid);
        }

        [ThreadStatic]
        private static UserSessionState _sessionState;

        [ThreadStatic]
        private static Sid _currentSid;

        /// <summary>
        /// 根据安全码获取安全信息
        /// </summary>
        /// <param name="sids">安全码</param>
        /// <returns></returns>
        protected abstract SecurityInfo[] GetSecurityInfos(Sid[] sids);

        /// <summary>
        /// 根据安全码获取安全信息
        /// </summary>
        /// <param name="sid">安全码</param>
        /// <returns></returns>
        protected SecurityInfo GetSecurityInfo(Sid sid)
        {
            SecurityInfo[] sis = GetSecurityInfos(new[] { sid });
            return sis.IsNullOrEmpty() ? null : sis[0];
        }

        /// <summary>
        /// 获取服务的运行安全信息
        /// </summary>
        /// <param name="context">当前执行环境</param>
        /// <param name="method">接口名称</param>
        /// <returns></returns>
        protected abstract SecurityInfo GetServiceSecurityInfo(CommunicateContext context, string method);

        #region Class SecurityCommunicateFactoryDecorate ...

        class SecurityCommunicateFactoryDecorate : ICommunicateFactory
        {
            public SecurityCommunicateFactoryDecorate(TrayAppServiceBase srv, ICommunicateFactory factory)
            {
                _srv = srv;
                _factory = factory;
            }

            private readonly TrayAppServiceBase _srv;
            private readonly ICommunicateFactory _factory;

            public ICommunicate CreateCommunicate(ServiceTarget target, bool includeMySelf = false, bool async = true, ICommunicateCallback callback = null)
            {
                return new SecurityCommunicateDecorate(_srv, _factory.CreateCommunicate(target, includeMySelf, async, callback));
            }

            public ICommunicate CreateCommunicate(CommunicationOption[] options, bool async = true, ICommunicateCallback callback = null)
            {
                return new SecurityCommunicateDecorate(_srv, _factory.CreateCommunicate(options, async, callback));
            }

            public ServiceDesc Owner
            {
                get
                {
                    return _factory.Owner;
                }
                set
                {
                    _factory.Owner = value;
                }
            }

            #region Class SecurityCommunicateDecorate ...

            // 添加安全选项的装饰器
            class SecurityCommunicateDecorate : ICommunicate
            {
                public SecurityCommunicateDecorate(TrayAppServiceBase srv, ICommunicate communicate)
                {
                    _srv = srv;
                    _communicate = communicate;
                }

                private readonly TrayAppServiceBase _srv;
                private readonly ICommunicate _communicate;

                public CommunicateData Call(CommunicateContext context, string method, CommunicateData data, CallingSettings settings = null)
                {
                    if (settings == null || !settings.InvokeType.HasFlag(CommunicateInvokeType.UseOriginalSid))
                    {
                        // 从用户权限、通过settings明确指定的权限与服务运行权限中寻找一个最高权限
                        if (settings == null)
                            settings = new CallingSettings();

                        SecurityInfo[] sis = _srv.GetSecurityInfos(new[] { settings.Sid, _currentSid });
                        SecurityInfo si = sis[0];
                        SecurityInfo si1 = sis[1];
                        SecurityInfo si2 = _srv.GetServiceSecurityInfo(context, method);

                        if (si == null || (si1 != null && si1.SecurityLevel > si.SecurityLevel))
                            si = si1;

                        if (si == null || (si2 != null && si2.SecurityLevel > si.SecurityLevel))
                            si = si2;

                        if (si != null)
                            settings.Sid = si.Sid;
                    }

                    return _communicate.Call(context, method, data, settings);
                }

                public void Dispose()
                {
                    _communicate.Dispose();
                }
            }

            #endregion
        }

        #endregion

        /// <summary>
        /// 调用指定的接口
        /// </summary>
        /// <param name="context">调用上下文</param>
        /// <param name="method">接口名称</param>
        /// <param name="data">数据</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        protected override CommunicateData OnCall(CommunicateContext context, string method, CommunicateData data, CallingSettings settings)
        {
            _currentSid = _ValidateSecurityLevel(context, method, data, settings);
            return base.OnCall(context, method, data, settings);
        }

        // 验证权限
        private Sid _ValidateSecurityLevel(CommunicateContext context, string method, CommunicateData data, CallingSettings settings)
        {
            Sid sid = (settings == null) ? Sid.Empty : settings.Sid;

            MethodParser mp = new MethodParser(method);
            IAppCommand cmd = GetCommand(mp.CommandDesc);
            AppCommandInfo cmdInfo;
            if (cmd != null && (cmdInfo = cmd.GetInfo()) != null && cmdInfo.SecurityLevel > SecurityLevel.Public)
            {
                SecurityInfo si = GetSecurityInfo(sid);
                SecurityLevel sl = (si == null) ? SecurityLevel.Undefined : si.SecurityLevel;
                if (sl < cmdInfo.SecurityLevel)
                {
                    string errMsg = string.Format("无权限调用接口“{0}”，调用者权限:{1}，接口权限:{2}",
                        method, sl.GetSecurityLevelDesc(), cmdInfo.SecurityLevel.GetSecurityLevelDesc());
                    throw new ServiceException(ServerErrorCode.InvalidUser, errMsg);
                }
            }

            return sid;
        }

        /// <summary>
        /// 服务状态
        /// </summary>
        public override AppServiceStatus Status
        {
            get
            {
                return base.Status;
            }
            protected set
            {
                if (base.Status != value)
                {
                    base.Status = value;
                    Context.Platform.StatusChangedNotify(value);
                }
            }
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            Context.Dispose();
        }
    }
}

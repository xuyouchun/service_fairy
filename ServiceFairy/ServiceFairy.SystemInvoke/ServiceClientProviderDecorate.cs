using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 能够自动保存安全码的ServiceClientProvider装饰器
    /// </summary>
    public abstract class ServiceClientProviderDecorate : IServiceClientProvider
    {
        public ServiceClientProviderDecorate(IServiceClientProvider scProvider)
        {
            Contract.Requires(scProvider != null);
            ScProvider = scProvider;
        }

        public IServiceClientProvider ScProvider { get; private set; }

        /// <summary>
        /// 自动记录安全码的装饰器
        /// </summary>
        /// <param name="scp"></param>
        /// <returns></returns>
        public static IServiceClientProvider ToSecurityDecorate(IServiceClientProvider scp)
        {
            return new SecurityServiceClientProviderDecorate(scp);
        }

        #region Class SecurityServiceClientProviderDecorate ...

        class SecurityServiceClientProviderDecorate : ServiceClientProviderDecorate
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="scp"></param>
            public SecurityServiceClientProviderDecorate(IServiceClientProvider scp)
                : base(scp)
            {
                
            }

            /// <summary>
            /// 安全码
            /// </summary>
            public Sid Sid { get; private set; }

            /// <summary>
            /// 创建ServiceClient
            /// </summary>
            /// <returns></returns>
            public override IServiceClient Get()
            {
                return new SecurityServiceClientDecorate(this, ScProvider.Get());
            }

            #region Class SecurityServiceClientDecorate ...

            class SecurityServiceClientDecorate : ServiceClientBase
            {
                public SecurityServiceClientDecorate(SecurityServiceClientProviderDecorate owner, IServiceClient sc)
                    : base(sc)
                {
                    _owner = owner;
                }

                private readonly SecurityServiceClientProviderDecorate _owner;

                protected override void BeforeCall(ref CallingSettings settings)
                {
                    if (!_owner.Sid.IsEmpty())
                    {
                        if (settings == null)
                            settings = new CallingSettings();

                        if (settings.Sid.IsEmpty())
                            settings.Sid = _owner.Sid;
                    }
                }

                protected override void AfterCall(ref ServiceResult result)
                {
                    _Check(result);   
                }

                protected override void AfterCall(ref ServiceResult<object> result)
                {
                    _Check(result);
                }

                private ServiceResult _Check(ServiceResult sr)
                {
                    if (sr != null && !sr.Sid.IsEmpty())
                        _owner.Sid = sr.Sid;

                    return sr;
                }
            }

            #endregion
        }

        #endregion

        #region Class AsyncServiceClientProviderDecorate ...

        class AsyncServiceClientProviderDecorate
        {

        }

        #endregion


        #region IObjectProvider<IServiceClient> Members

        public virtual IServiceClient Get()
        {
            return ScProvider.Get();
        }

        #endregion

        #region Class ServiceClientBase ...

        public abstract class ServiceClientBase : IServiceClient
        {
            public ServiceClientBase(IServiceClient sc)
            {
                Contract.Requires(sc != null);
                Sc = sc;
            }

            protected readonly IServiceClient Sc;

            /// <summary>
            /// 开始调用
            /// </summary>
            /// <param name="settings"></param>
            protected virtual void BeforeCall(ref CallingSettings settings)
            {

            }

            /// <summary>
            /// 结束调用
            /// </summary>
            /// <param name="result"></param>
            protected virtual void AfterCall(ref ServiceResult result)
            {

            }

            /// <summary>
            /// 结束调用
            /// </summary>
            /// <param name="result"></param>
            protected virtual void AfterCall(ref ServiceResult<object> result)
            {

            }

            #region IServiceClient Members

            public virtual ServiceResult Call(string method, object input, CallingSettings settings = null)
            {
                BeforeCall(ref settings);
                ServiceResult sr = Sc.Call(method, input, settings);
                AfterCall(ref sr);
                return sr;
            }

            public virtual ServiceResult<object> Call(string method, object input, Type replyType, CallingSettings settings = null)
            {
                BeforeCall(ref settings);
                ServiceResult<object> sr = Sc.Call(method, input, replyType, settings);
                AfterCall(ref sr);
                return sr;
            }

            public virtual IServiceClientReceiverHandler RegisterReceiver(string method, Type entityType, IServiceClientReceiver receiver)
            {
                return Sc.RegisterReceiver(method, entityType, receiver);
            }

            #endregion

            #region IDisposable Members

            public virtual void Dispose()
            {
                Sc.Dispose();
            }

            #endregion
        }

        #endregion

    }
}


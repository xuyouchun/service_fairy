using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using ServiceFairy.Client;
using Common.Contracts;
using Common.Communication.Wcf;
using Common.Utility;
using ServiceFairy.Entities.Navigation;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;

namespace ServiceFairy.SystemInvoke
{
    [System.Diagnostics.DebuggerStepThrough]
    public abstract class InvokerBase : IDisposable
    {
        protected InvokerBase(InvokerBase innerInvoker, object eventSender = null)
            : this(new InnerInvokerBase(innerInvoker), false)
        {
            
        }

        protected InvokerBase(IServiceClientProvider scProvider, bool disposeIt, object eventSender = null)
        {
            Contract.Requires(scProvider != null);

            _scProvider = scProvider;
            _disposeIt = disposeIt;
            _eventSender = eventSender ?? this;
        }

        private readonly object _eventSender;

        /// <summary>
        /// 默认的安全码
        /// </summary>
        public Sid Sid { get; set; }

        protected static IServiceClientProvider CreateProviderByServiceClient(IServiceClient serviceClient)
        {
            return new ServiceClientProvider(serviceClient);
        }

        private static readonly WcfService _wcfService = new WcfService();

        /// <summary>
        /// 通过代理创建
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        protected static IServiceClientProvider CreateProviderByProxy(CommunicationOption proxy, DataFormat format)
        {
            ServiceFairyClient sfc = new ServiceFairyClient(_wcfService.Connect(proxy), format, true);
            return CreateProviderByServiceClient(sfc);
        }

        /// <summary>
        /// 通过通道创建
        /// </summary>
        /// <param name="communicate"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        protected static IServiceClientProvider CreateProviderByCommunicate(ICommunicate communicate, DataFormat format)
        {
            ServiceFairyClient sfc = new ServiceFairyClient(communicate, format);
            return CreateProviderByServiceClient(sfc);
        }

        private readonly IServiceClientProvider _scProvider;

        protected IServiceClient GetServiceClient()
        {
            return _scProvider.Get();
        }

        protected static void Validate(ServiceResult sr)
        {
            if (sr == null)
                return;

            if (!sr.Succeed)
                throw sr.CreateException();
        }

        protected static ServiceResult<TResult> CreateSr<TResult>(ServiceResult sr, TResult result)
        {
            if (sr == null)
                return null;

            return new ServiceResult<TResult>(result, sr.StatusCode, sr.StatusDesc, sr.Sid);
        }

        protected static ServiceResult<TResult> CreateSr<TResult>(ServiceResult sr, Func<ServiceResult, TResult> convert)
        {
            if (sr == null)
                return null;

            return CreateSr<TResult>(sr, convert(sr));
        }

        protected static ServiceResult<TResult> CreateSr<TSource, TResult>(ServiceResult<TSource> sr, Func<TSource, TResult> converter, TResult defaultValue = default(TResult))
        {
            if (sr == null)
                return null;

            if (sr.Result == null)
                return new ServiceResult<TResult>(defaultValue, sr.StatusCode, sr.StatusDesc, sr.Sid);

            try
            {
                return new ServiceResult<TResult>(converter(sr.Result), sr.StatusCode, sr.StatusDesc, sr.Sid);
            }
            catch (ServiceException ex)
            {
                return new ServiceResult<TResult>(defaultValue, ex.StatusCode, sr.StatusDesc, sr.Sid);
            }
        }

        protected static void InvokeWithCheck(ServiceResult serviceResult)
        {
            Validate(serviceResult);
        }

        protected static TResult InvokeWithCheck<TResult>(ServiceResult<TResult> result)
        {
            if (result == null)
                return default(TResult);

            Validate(result);
            return result.Result;
        }

        protected static void RaiseEvent<TMessage>(ServiceClientReceiveEventHandler<TMessage> eh, object sender, ServiceClientReceiveEventArgs<TMessage> e)
        {
            if (eh != null)
            {
                ThreadPool.QueueUserWorkItem(delegate { eh(sender, e); });
            }
        }

        #region Class InnerInvokerBase ...

        class InnerInvokerBase : IServiceClientProvider
        {
            public InnerInvokerBase(InvokerBase invoker)
            {
                _invoker = invoker;
            }

            private readonly InvokerBase _invoker;

            public IServiceClient Get()
            {
                return _invoker.GetServiceClient();
            }
        }

        #endregion

        /// <summary>
        /// 获取代理列表
        /// </summary>
        /// <param name="navigation"></param>
        /// <param name="communicationType"></param>
        /// <param name="dataFormat"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public static CommunicationOption[] GetProxyList(CommunicationOption navigation, CommunicationType communicationType = CommunicationType.Unknown,
            CommunicationDirection direction = CommunicationDirection.Unidirectional,
            DataFormat dataFormat = DataFormat.Unknown, int maxCount = 0, bool throwErrorWhenEmpty = false)
        {
            using (WcfService wcfService = new WcfService())
            {
                using (WcfConnection con = wcfService.Connect(navigation))
                {
                    ServiceFairyClient sf = new ServiceFairyClient(con, dataFormat);
                    con.Open();

                    if (communicationType != CommunicationType.Unknown)
                    {
                        CommunicationOption[] options = _GetProxyList(sf, communicationType, direction);
                        if (options.IsNullOrEmpty() && throwErrorWhenEmpty)
                            throw new ServiceException(ServerErrorCode.NoData, string.Format("导航服务器未提供协议为“{0}”的{1}的代理列表", communicationType, direction.GetDesc()));

                        return options;
                    }
                    else
                    {
                        CommunicationOption[] options = null;
                        foreach (CommunicationType communicationType0 in new[] { CommunicationType.WTcp, CommunicationType.Http, CommunicationType.Unknown })
                        {
                            options = _GetProxyList(sf, communicationType0, direction);
                            if (!options.IsNullOrEmpty() && throwErrorWhenEmpty)
                                break;
                        }

                        if (options.IsNullOrEmpty())
                            throw new ServiceException(ServerErrorCode.NoData, string.Format("导航服务器未提供{0}代理列表",
                                (direction == CommunicationDirection.None ? "" : direction.GetDesc())));

                        return options;
                    }
                }
            }
        }

        private static CommunicationOption[] _GetProxyList(IServiceClient sf, CommunicationType type, CommunicationDirection direction)
        {
            ServiceResult<Navigation_GetProxyList_Reply> sr = NavigationService.GetProxyList(sf, new Navigation_GetProxyList_Request() {
                CommunicationType = type, Direction = direction
            });

            sr.Validate();
            return sr.Result.CommunicationOptions;
        }

        public virtual void Dispose()
        {
            foreach (Invoker invoker in _invokers)
            {
                invoker.Close();
            }

            IDisposable dis = _scProvider as IDisposable;
            if (dis != null && _disposeIt)
                dis.Dispose();
        }

        private bool _disposeIt;
        private ConcurrentBag<Invoker> _invokers = new ConcurrentBag<Invoker>();

        #region Class Invoker ...

        public class Invoker
        {
            public Invoker(InvokerBase owner)
            {
                Owner = owner;
                owner._invokers.Add(this);
            }

            protected InvokerBase Owner { get; private set; }

            private volatile IServiceClient _sc;
            private readonly object _syncLocker = new object();
            private readonly List<ReceiverItem> _receiverItems = new List<ReceiverItem>();

            #region Class ReceiverItem ...

            class ReceiverItem
            {
                public ReceiverItem(string method, Type entityType, IServiceClientReceiver receiver)
                {
                    Method = method;
                    EntityType = entityType;
                    Receiver = receiver;
                }

                public readonly string Method;
                public readonly Type EntityType;
                public readonly IServiceClientReceiver Receiver;
                public IServiceClientReceiverHandler Handler;
            }

            #endregion

            protected IServiceClient Sc
            {
                get
                {
                    lock (_syncLocker)
                    {
                        IServiceClient sc = Owner.GetServiceClient();
                        if (sc != _sc)
                        {
                            _UnregisterReceivers(_sc, _receiverItems);
                            _RegisterReceivers(_sc = sc, _receiverItems);
                        }

                        return _sc;
                    }
                }
            }

            /// <summary>
            /// 注册一个事件接收器
            /// </summary>
            /// <typeparam name="TEntity"></typeparam>
            /// <param name="method"></param>
            /// <param name="receiver"></param>
            protected void RegisterReceiver<TEntity>(string method, IServiceClientReceiver<TEntity> receiver)
            {
                RegisterReceiver(method, typeof(TEntity), new ServiceClientReceiverAdapter<TEntity>(receiver));
            }

            /// <summary>
            /// 注册一个事件接收器
            /// </summary>
            /// <param name="method"></param>
            /// <param name="entityType"></param>
            /// <param name="receiver"></param>
            protected void RegisterReceiver(string method, Type entityType, IServiceClientReceiver receiver)
            {
                lock (_syncLocker)
                {
                    ReceiverItem item = new ReceiverItem(method, entityType, receiver);
                    _RegisterReceiver(Sc, item);
                    _receiverItems.Add(item);
                }
            }

            private void _RegisterReceiver(IServiceClient sc, ReceiverItem item)
            {
                if (sc != null)
                {
                    item.Handler = sc.RegisterReceiver(item.Method, item.EntityType, item.Receiver);
                }
            }

            private void _RegisterReceivers(IServiceClient sc, IList<ReceiverItem> items)
            {
                foreach (ReceiverItem item in items)
                {
                    _RegisterReceiver(sc, item);
                }
            }

            private void _UnregisterReceivers(IServiceClient sc, IList<ReceiverItem> items)
            {
                foreach (ReceiverItem item in items)
                {
                    if (item.Handler != null)
                        item.Handler.Unregister();
                }
            }

            /// <summary>
            /// 注册数据接收器
            /// </summary>
            /// <typeparam name="TMessage"></typeparam>
            /// <param name="eh"></param>
            public void RegisterEventHandler<TMessage>(string method, ServiceClientReceiveEventHandler<TMessage> eh)
            {
                RegisterReceiver(method, new ServiceClientReceiverEhAdapter<TMessage>(Owner._eventSender, eh));
            }

            /// <summary>
            /// 注册数据接收器
            /// </summary>
            /// <typeparam name="TMessage"></typeparam>
            /// <param name="eh"></param>
            public void RegisterEventHandler<TMessage>(ServiceClientReceiveEventHandler<TMessage> eh) where TMessage : MessageEntity
            {
                string method = MessageAttribute.GetMethod(typeof(TMessage));
                RegisterEventHandler<TMessage>(method, eh);
            }

            internal void Close()
            {
                lock (_syncLocker)
                {
                    _UnregisterReceivers(_sc, _receiverItems);
                }
            }
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package;
using Common.Package.TaskDispatcher;
using Common.Utility;
using Common.Communication.Wcf;

namespace Common.Framework.TrayPlatform
{
    partial class WcfTrayAppServiceApplication
    {
        class CallbackCommunicateFactory : MarshalByRefObjectEx, ICommunicateFactory, IDisposable
        {
            public CallbackCommunicateFactory(WcfTrayAppServiceApplication app, IServiceCommunicationSearcher searcher)
            {
                _app = app;
                _searcher = searcher;

                _clientId = _app.ServiceManager.ClientID;
            }

            private readonly TaskDispatcher<CallTask> _taskDispatcher = new TaskDispatcher<CallTask>(5);
            private readonly WcfTrayAppServiceApplication _app;
            private readonly IServiceCommunicationSearcher _searcher;
            private readonly Guid _clientId;
            private ServiceDesc _ownerServiceDesc;

            ServiceDesc ICommunicateFactory.Owner
            {
                get { return _ownerServiceDesc; }
                set { _ownerServiceDesc = value; }
            }

            private CommunicationOption[] _ExceptLocal(IEnumerable<CommunicationOption> ops)
            {
                if (ops == null)
                    return Array<CommunicationOption>.Empty;

                return ops.WhereToArray(op => !op.IsLocalHost());
            }

            private CommunicationOption _GetCommunicationOption(Guid clientId)
            {
                AppInvokeInfo[] infos = _searcher.Search(new[] { clientId });
                if (infos.IsNullOrEmpty())
                    return null;

                return infos[0].PickCommunicationOption();
            }

            private CommunicationOption[] _GetCommunicationOptions(ServiceDesc serviceDesc)
            {
                return _GetAppInvokeInfos(serviceDesc).ToArray(invokeInfo => invokeInfo.PickCommunicationOption());
            }

            private AppInvokeInfo[] _GetAppInvokeInfos(ServiceDesc serviceDesc)
            {
                return _searcher.Search(serviceDesc) ?? Array<AppInvokeInfo>.Empty;
            }

            private CommunicationOptionPair[] _GetCommunicationOptions(ServiceTarget target, bool includeMySelf)
            {
                if (target.EndPoints.IsNullOrEmpty())
                    return null;

                List<CommunicationOptionPair> pairs = new List<CommunicationOptionPair>();
                foreach (ServiceEndPoint endpoint in target.EndPoints)
                {
                    if (endpoint.ClientId == Guid.Empty)
                    {
                        if (endpoint.ServiceDesc != null)
                        {
                            pairs.AddRange(_GetAppInvokeInfos(endpoint.ServiceDesc).Select(invokeInfo =>
                                new CommunicationOptionPair { ClientId = invokeInfo.ClientID,
                                    CommunicationOption = invokeInfo.PickCommunicationOption(), ServiceDesc = endpoint.ServiceDesc }
                                )
                            );
                        }
                    }
                    else
                    {
                        if (endpoint.ClientId == _clientId)
                        {
                            pairs.Add(new CommunicationOptionPair { ClientId = endpoint.ClientId, ServiceDesc = endpoint.ServiceDesc });
                        }
                        else
                        {
                            CommunicationOption op = _GetCommunicationOption(endpoint.ClientId);
                            if (op != null)
                                pairs.Add(new CommunicationOptionPair { ClientId = endpoint.ClientId, CommunicationOption = op, ServiceDesc = endpoint.ServiceDesc });
                        }
                    }
                }

                var list = pairs.Distinct(pair => new ServiceEndPoint(pair.ClientId, pair.ServiceDesc));
                if (!includeMySelf)
                    list = list.Where(p => !(p.ClientId == _clientId && p.ServiceDesc == _ownerServiceDesc));

                return list.ToArray();
            }

            /// <summary>
            /// 根据目标创建多播通道
            /// </summary>
            /// <param name="target"></param>
            /// <param name="includeMySelf"></param>
            /// <param name="async"></param>
            /// <param name="callback"></param>
            /// <returns></returns>
            public ICommunicate CreateCommunicate(ServiceTarget target, bool includeMySelf = false, bool async = true, ICommunicateCallback callback = null)
            {
                // 默认调用
                if (target == null || target.Model == ServiceTargetModel.Auto)
                {
                    ServiceEndPoint[] endpoints = (target == null || target.EndPoints == null) ? null : target.EndPoints;
                    if (endpoints.IsNullOrEmpty())
                        return _Wrap(new DefaultCommunicate(this, callback));
                    else if (endpoints.Length == 1 && endpoints[0].ServiceDesc != null)
                    {
                        if (endpoints[0].ClientId != Guid.Empty)
                            throw new ServiceException(ServerErrorCode.ArgumentError, "在默认调用方式中，不能指定ClientId");

                        return _Wrap(new DirectCommunicate(this, null, endpoints[0].ServiceDesc, callback));
                    }
                    else
                        throw new ServiceException(ServerErrorCode.ArgumentError, "在默认调用方式中，调用目标或者不指定，或者只指定ServiceDesc");
                }

                ServiceEndPoint[] eps = target.EndPoints;
                if (eps.IsNullOrEmpty())
                    throw new ServiceException(ServerErrorCode.ArgumentError, "未指定调用目标");

                // 直接调用
                if (target.Model == ServiceTargetModel.Direct)
                {
                    if (eps.Length != 1)
                        throw new ServiceException(ServerErrorCode.ArgumentError, "只能指定一个直接调用目标");

                    ServiceEndPoint ep = eps[0];
                    if (ep.ClientId == Guid.Empty)
                        throw new ServiceException(ServerErrorCode.ArgumentError, "未指定调用目标");

                    if (ep.ClientId == _clientId)  // 本地调用
                        return _Wrap(new DirectCommunicate(this, null, ep.ServiceDesc, callback));

                    // 远程调用
                    CommunicationOption op = _GetCommunicationOption(ep.ClientId);
                    if (op == null)
                        throw new ServiceException(ServerErrorCode.ArgumentError, "调用目标不存在: ClientID=" + ep.ClientId);

                    return _Wrap(new DirectCommunicate(this, op, ep.ServiceDesc, callback));
                }

                // 多播调用
                if (target.Model == ServiceTargetModel.Broadcast)
                {
                    CommunicationOptionPair[] pairs = _GetCommunicationOptions(target, includeMySelf);
                    if (pairs.IsNullOrEmpty())
                        return EmptyCommunicate.Instance;

                    return _Wrap(new BroadcastCallbackCommunicate(this, pairs, async, callback));
                }

                throw new ServiceException(ServerErrorCode.NotSupported, "不支持调用方式：" + target);
            }

            /// <summary>
            /// 创建指定信道的通道
            /// </summary>
            /// <param name="options">信道</param>
            /// <param name="async">是否为异步调用</param>
            /// <param name="callback">回调</param>
            /// <returns></returns>
            public ICommunicate CreateCommunicate(CommunicationOption[] options, bool async = true, ICommunicateCallback callback = null)
            {
                if (options.IsNullOrEmpty())
                    return EmptyCommunicate.Instance;

                if (options.Length == 1)
                    return _Wrap(new DirectCommunicate(this, options[0], null, callback));

                return _Wrap(new BroadcastCallbackCommunicate(this,
                    options.ToArray(op => new CommunicationOptionPair { CommunicationOption = op }), async, callback
                ));
            }

            class CommunicationOptionPair
            {
                public Guid ClientId;
                public CommunicationOption CommunicationOption;
                public ServiceDesc ServiceDesc;
            }

            private ServiceEndPoint _serviceEndPoint;

            private CommunicateContext _CreateCommunicateContext(CommunicateContext context)
            {
                ServiceEndPoint caller = _serviceEndPoint ?? (_serviceEndPoint = new ServiceEndPoint(_clientId, _ownerServiceDesc));
                Guid sessionId = (context == null || context.SessionId == Guid.Empty) ? Guid.NewGuid() : context.SessionId;
                return new CommunicateContext(null, caller, sessionId);
            }

            private ICommunicate _Wrap(ICommunicate communicate)
            {
                if (communicate is CommunicateProxy)
                    return communicate;

                return new CommunicateProxy(this, communicate);
            }

            #region Class CommunicateProxy ...

            class CommunicateProxy : MarshalByRefObjectEx, ICommunicate
            {
                public CommunicateProxy(CallbackCommunicateFactory owner, ICommunicate communicate)
                {
                    _owner = owner;
                    _communicate = communicate;
                }

                private readonly CallbackCommunicateFactory _owner;
                private readonly ICommunicate _communicate;

                public CommunicateData Call(CommunicateContext context, string method, CommunicateData data, CallingSettings settings = null)
                {
                    return _communicate.Call(_owner._CreateCommunicateContext(context), method, data, settings);
                }

                public void Dispose()
                {
                    _communicate.Dispose();
                }
            }

            #endregion

            #region Class DefaultCommunicate ...

            class DefaultCommunicate : ICommunicate
            {
                public DefaultCommunicate(CallbackCommunicateFactory owner, ICommunicateCallback callback)
                {
                    _owner = owner;
                    _callback = callback;
                }

                private readonly CallbackCommunicateFactory _owner;
                private readonly ICommunicateCallback _callback;

                public CommunicateData Call(CommunicateContext context, string method, CommunicateData data, CallingSettings settings)
                {
                    CommunicateData replyData = _owner._app.Callback(context, method, data, null, settings);
                    if (_callback != null)
                        _callback.Callback(new CommunicateCallbackArgs(replyData));

                    return replyData;
                }

                public void Dispose()
                {

                }
            }

            #endregion

            #region Class EmptyCommunicate ...

            class EmptyCommunicate : MarshalByRefObjectEx, ICommunicate
            {
                public CommunicateData Call(CommunicateContext context, string method, CommunicateData data, CallingSettings settings = null)
                {
                    return null;
                }

                public void Dispose()
                {
                    
                }

                public static readonly EmptyCommunicate Instance = new EmptyCommunicate();
            }

            #endregion

            #region Class DirectCommunicate ...

            class DirectCommunicate : ICommunicate
            {
                public DirectCommunicate(CallbackCommunicateFactory owner, CommunicationOption option, ServiceDesc serviceDesc, ICommunicateCallback callback)
                {
                    _owner = owner;
                    _option = option;
                    _serviceDesc = serviceDesc;
                    _callback = callback;
                }

                private readonly CallbackCommunicateFactory _owner;
                private readonly CommunicationOption _option;
                private readonly ServiceDesc _serviceDesc;
                private readonly ICommunicateCallback _callback;

                /// <summary>
                /// 是否已被取消
                /// </summary>
                public bool Canceled { get; private set; }

                public CommunicateData Call(CommunicateContext context, string method, CommunicateData data, CallingSettings settings = null)
                {
                    CommunicateData replyData = _owner._app.Callback(context, _ReviseMethod(_serviceDesc, method), data, _option, settings);
                    if (_callback != null)
                        Canceled = _callback.Callback(new CommunicateCallbackArgs(replyData));

                    return replyData;
                }

                public void Dispose()
                {
                    
                }
            }

            #endregion

            #region Class BroadcastCallbackCommunicate ...

            class BroadcastCallbackCommunicate : ICommunicate
            {
                public BroadcastCallbackCommunicate(CallbackCommunicateFactory factory, CommunicationOptionPair[] pairs, bool async, ICommunicateCallback callback)
                {
                    _factory = factory;
                    _pairs = pairs;
                    _async = async;
                    _callback = callback;
                }

                private readonly CallbackCommunicateFactory _factory;
                private readonly CommunicationOptionPair[] _pairs;
                private readonly bool _async;
                private readonly ICommunicateCallback _callback;

                public CommunicateData Call(CommunicateContext context, string method, CommunicateData data, CallingSettings settings = null)
                {
                    CallTask[] tasks = _pairs.Select(pair =>
                        new CallTask(_factory, method, data, pair.ServiceDesc, context, pair.CommunicationOption, settings, _callback)
                    ).ToArray();

                    if (_async)  // 异步执行，添加到任务队列中
                    {
                        _factory._taskDispatcher.AddRange(tasks);
                    }
                    else  // 同步执行
                    {
                        foreach (CallTask task in tasks)
                        {
                            try
                            {
                                task.Execute();
                                if (task.Canceled)
                                    break;
                            }
                            catch (Exception ex)
                            {
                                LogManager.LogError(ex);
                            }
                        }
                    }

                    return null;
                }

                public void Dispose()
                {
                    
                }
            }

            #endregion

            #region Class CallTask ...

            class CallTask : ITask
            {
                public CallTask(CallbackCommunicateFactory factory, string method, CommunicateData requestData, ServiceDesc serviceDesc,
                    CommunicateContext context, CommunicationOption option, CallingSettings settings, ICommunicateCallback callback)
                {
                    _context = context;
                    _factory = factory;
                    _requestData = requestData;
                    _method = method;
                    _option = option;
                    _serviceDesc = serviceDesc;
                    _settings = settings;
                    _callback = callback;
                }

                private readonly CommunicateContext _context;
                private readonly CallbackCommunicateFactory _factory;
                private readonly CommunicateData _requestData;
                private readonly string _method;
                private readonly CommunicationOption _option;
                private readonly ServiceDesc _serviceDesc;
                private readonly CallingSettings _settings;
                private readonly ICommunicateCallback _callback;

                /// <summary>
                /// 是否已被取消
                /// </summary>
                public bool Canceled { get; private set; }

                public void Execute()
                {
                    try
                    {
                        using (DirectCommunicate communicate = new DirectCommunicate(_factory, _option, _serviceDesc, _callback))
                        {
                            communicate.Call(_context, _method, _requestData, _settings);
                            Canceled = communicate.Canceled;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.LogError(ex);
                    }
                }
            }

            #endregion

            private static string _ReviseMethod(ServiceDesc serviceDesc, string method)
            {
                Contract.Requires(method != null);

                ServiceDesc sd;
                CommandDesc cd;
                _ParseMethod(method, out sd, out cd);

                if (serviceDesc == null && sd == null)
                    throw new ServiceException(ServerErrorCode.ArgumentError, "调用目标不明确");

                return (sd ?? serviceDesc) + "/" + cd;
            }

            private static void _ParseMethod(string method, out ServiceDesc sd, out CommandDesc cd)
            {
                if (method.IndexOf('/') >= 0)  // 指定了服务名称
                {
                    MethodParser mp = new MethodParser(method);
                    sd = mp.ServiceDesc;
                    cd = mp.CommandDesc;
                }
                else
                {
                    sd = null;
                    cd = CommandDesc.Parse(method);
                }
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);
                _taskDispatcher.Dispose();
            }

            ~CallbackCommunicateFactory()
            {
                Dispose();
            }
        }
    }
}

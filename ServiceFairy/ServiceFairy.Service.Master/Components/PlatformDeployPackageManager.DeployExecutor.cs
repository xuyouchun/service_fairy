using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Utility;
using ServiceFairy.Entities.Master;
using Common.Package;
using Common.Package.GlobalTimer;
using ServiceFairy.Client;
using System.Threading;
using Common;
using ServiceFairy.Entities;


namespace ServiceFairy.Service.Master.Components
{
    partial class PlatformDeployPackageManager
    {
        /// <summary>
        /// 执行器
        /// </summary>
        class DeployExecutor : IDisposable
        {
            public DeployExecutor(Service service, DeployPackageInfo deployPackageInfo, Guid[] clientIds)
            {
                _service = service;
                _deployPackageInfo = deployPackageInfo;
                _clientIds = clientIds.ToHashSet();
            }

            private readonly Service _service;
            private readonly DeployPackageInfo _deployPackageInfo;
            private readonly HashSet<Guid> _clientIds;
            private Guid _masterClientId;
            private volatile bool _completed = false, _disposed = false;
            private readonly ManualResetEvent _waitForDispose = new ManualResetEvent(false);
            private readonly object _thisLocker = new object();

            // 描述服务状态
            class ServiceDescState
            {
                public ServiceDesc ServiceDesc;
                public int Count, TotalCount;
            }

            // 描述终端状态
            class ClientState
            {
                public Guid ClientId;
                public bool Avaliable;
                public PlatformDeployStatus Status;
                public DateTime StartDeployTime;
                public DateTime ConnectedTime;

                public bool HasStarted()
                {
                    return StartDeployTime != default(DateTime);
                }
            }

            private readonly Dictionary<Guid, ClientState> _clientStates = new Dictionary<Guid, ClientState>();

            // 执行更新
            public void Execute(Action callback)
            {
                IGlobalTimerTaskHandle handle = null;
                ThreadUtility.StartNew(delegate {
                    try
                    {
                        _InitClientStatDataDict();
                        handle = GlobalTimer<ITask>.Default.Add(TimeSpan.FromMilliseconds(500), new TaskFuncAdapter(_UpdateClientStatus), false);
                        _Execute();
                    }
                    catch (Exception ex)
                    {
                        LogManager.LogError(ex);
                    }
                    finally
                    {
                        _completed = true;
                        if (handle != null)
                            handle.Dispose();

                        callback();
                    }
                });
            }

            // 收集终端的状态
            private void _InitClientStatDataDict()
            {
                foreach (AppClientDeployInfo deployInfo in _service.DeployMapManager.GetAllDeployInfos())
                {
                    AppClientInfo clientInfo = _service.AppClientManager.GetClientInfo(deployInfo.ClientId);
                    if (clientInfo != null)
                    {
                        _clientStates[deployInfo.ClientId] = new ClientState() {
                            ClientId = deployInfo.ClientId, Avaliable = true, Status = PlatformDeployStatus.Waiting,
                            ConnectedTime = clientInfo.ConnectedTime,
                        };
                    }
                }
            }

            // 更新终端状态
            private void _UpdateClientStatus()
            {
                lock (_thisLocker)
                {
                    foreach (ClientState clientState in _clientStates.Values)
                    {
                        AppClientInfo clientInfo = _service.AppClientManager.GetClientInfo(clientState.ClientId);
                        clientState.Avaliable = _service.AppClientManager.IsAvaliable(clientState.ClientId);

                        if (clientState.Status == PlatformDeployStatus.Deploying)
                        {
                            if (DateTime.UtcNow - clientState.StartDeployTime > TimeSpan.FromMinutes(2))
                            {
                                clientState.Status = PlatformDeployStatus.Timeout;
                            }
                            else if (clientState.Avaliable)
                            {
                                if (clientInfo.PlatformDeployId == _deployPackageInfo.Id)
                                    clientState.Status = PlatformDeployStatus.Completed;
                                else if (clientState.ConnectedTime != clientInfo.ConnectedTime)  // 连接时间不同，说明是已经重新启动
                                    clientState.Status = PlatformDeployStatus.Error;
                            }
                        }
                    }
                }
            }

            // 是否全部完成
            private bool _AllCompleted()
            {
                lock (_thisLocker)
                {
                    return _clientStates.Values.Where(v => _clientIds.Contains(v.ClientId))
                        .All(cs => cs.Status == PlatformDeployStatus.Completed || cs.Status == PlatformDeployStatus.Timeout || cs.Status == PlatformDeployStatus.Error);
                }
            }

            // 是否有仍在运行的部署
            private bool _AnyRunning()
            {
                lock (_thisLocker)
                {
                    return _clientStates.Values.Where(v => _clientIds.Contains(v.ClientId)).Any(cs => cs.Status == PlatformDeployStatus.Deploying);
                }
            }

            private void _Execute()
            {
                int remain = 9;
                Guid[] clientIds;

                while (!_disposed)
                {
                    if (_AnyRunning())
                    {
                        if (_waitForDispose.WaitOne(TimeSpan.FromSeconds(1)))
                            return;

                        continue;
                    }

                    while ((clientIds = _GetClientIdsNeedToDeploy((float)remain / 10)).Length == 0)
                    {
                        if (_disposed || remain < 0 || _AllCompleted())
                            return;

                        remain--;
                    }

                    if (clientIds.Length > 0)
                    {
                        clientIds.Select(id => _clientStates[id]).ForEach(cs => {
                            cs.Status = PlatformDeployStatus.Deploying; cs.StartDeployTime = DateTime.UtcNow;
                        });
                        _service.DeployMapManager.UpdatePlatformDeployId(clientIds, _deployPackageInfo.Id);
                    }
                }
            }

            // 搜索可以执行部署的终端
            private Guid[] _GetClientIdsNeedToDeploy(float remain)
            {
                if (remain < 0)
                {
                    if (_masterClientId == default(Guid) || _clientStates[_masterClientId].Status != PlatformDeployStatus.Waiting)
                        return Array<Guid>.Empty;

                    return new[] { _masterClientId };
                }

                lock (_thisLocker)
                {
                    // 服务数量统计
                    Dictionary<Guid, List<ServiceDesc>> sdCounts = new Dictionary<Guid, List<ServiceDesc>>();
                    Dictionary<ServiceDesc, ServiceDescState> serviceDescs = new Dictionary<ServiceDesc, ServiceDescState>();
                    foreach (AppClientInfo clientInfo in _service.AppClientManager.GetAllClientInfos())
                    {
                        if (clientInfo != null)
                        {
                            List<ServiceDesc> sCounts = sdCounts.GetOrSet(clientInfo.ClientId);
                            foreach (ServiceDesc sd in clientInfo.ServiceInfos.Select(si => si.ServiceDesc))
                            {
                                ServiceDescState sCount = serviceDescs.GetOrSet(sd, (sd0) => new ServiceDescState() { ServiceDesc = sd0 });
                                sCount.Count++;
                                sCount.TotalCount++;
                                sCounts.Add(sd);

                                if (sd.IsMasterService())
                                    _masterClientId = clientInfo.ClientId;
                            }
                        }
                    }

                    List<Guid> clientIds = new List<Guid>();
                    // 尝试逐个裁剪掉AppClient
                    foreach (Guid clientId in _clientIds.Except(_masterClientId))
                    {
                        ClientState clientState;
                        if (!_clientStates.TryGetValue(clientId, out clientState) || clientState.Status != PlatformDeployStatus.Waiting)
                            continue;

                        List<ServiceDesc> sds = sdCounts.GetOrDefault(clientId);
                        if (sds.All(sd => { ServiceDescState st = serviceDescs[sd]; return ((float)(st.Count - 1) / st.Count) >= remain; }))
                        {
                            clientIds.Add(clientId);
                            sds.ForEach(sd => serviceDescs[sd].Count--);
                        }
                    }

                    return clientIds.ToArray();
                }
            }

            /// <summary>
            /// 获取进度
            /// </summary>
            /// <returns></returns>
            public PlatformDeployProgress[] GetProgress()
            {
                lock (_thisLocker)
                {
                    return _clientStates.Values.Where(v => _clientIds.Contains(v.ClientId)).ToArray(cs =>
                        new PlatformDeployProgress() { ClientID = cs.ClientId, StartTime = cs.StartDeployTime, Status = cs.Status, Connected = cs.Avaliable });
                }
            }

            /// <summary>
            /// 是否已经完成
            /// </summary>
            /// <returns></returns>
            public bool IsCompleted()
            {
                return _completed;
            }

            ~DeployExecutor()
            {
                Dispose();
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);

                _disposed = true;
                _waitForDispose.Set();
            }
        }
    }
}

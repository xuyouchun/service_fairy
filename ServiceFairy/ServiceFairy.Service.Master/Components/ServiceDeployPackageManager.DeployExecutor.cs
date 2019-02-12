using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Master;
using Common.Utility;
using Common.Package;
using Common.Package.GlobalTimer;
using Common.Contracts.Service;
using System.Threading;
using ServiceFairy.Client;
using Common.Framework.TrayPlatform;
using Common;
using ServiceFairy.Entities;

namespace ServiceFairy.Service.Master.Components
{
    partial class ServiceDeployPackageManager
    {
        /// <summary>
        /// 执行服务的部署任务
        /// </summary>
        class DeployExecutor : IDisposable
        {
            public DeployExecutor(Service service, ServiceDeployPackageInfo packageInfo, Guid[] clientIds)
            {
                _service = service;
                _serviceDesc=packageInfo.ServiceDesc;
                _packageInfo = packageInfo;
                _clientIds = clientIds.ToHashSet();
            }

            private readonly Service _service;
            private readonly ServiceDesc _serviceDesc;
            private readonly ServiceDeployPackageInfo _packageInfo;
            private readonly HashSet<Guid> _clientIds;
            private volatile bool _completed, _disposed;
            private readonly ManualResetEvent _waitForDispose = new ManualResetEvent(false);
            private readonly object _thisLocker = new object();
            private readonly Dictionary<Guid, ServiceDescStatus> _serviceStatus = new Dictionary<Guid, ServiceDescStatus>();

            class ServiceDescStatus
            {
                public Guid ClientId;
                public DateTime StartDeployTime;
                public bool Connected, Restarting;
                public ServiceDeployStatus Status;
            }

            /// <summary>
            /// 开始执行部署任务
            /// </summary>
            /// <param name="callback"></param>
            public void Execute(Action callback)
            {
                IGlobalTimerTaskHandle handle = null;
                ThreadUtility.StartNew(delegate {
                    try
                    {
                        _InitServiceStatDataDict();
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

            private void _Execute()
            {
                Guid[] clientIds;

                while (!_disposed)
                {
                    if (_AnyRunning())
                    {
                        if (_waitForDispose.WaitOne(TimeSpan.FromSeconds(1)))
                            return;

                        continue;
                    }

                    if ((clientIds = _GetClientIdsNeedToDeploy()).Length == 0)
                        return;

                    clientIds.Select(id => _serviceStatus[id]).ForEach(cs => {
                        cs.Status = ServiceDeployStatus.Deploying; cs.StartDeployTime = DateTime.UtcNow;
                    });

                    _service.DeployMapManager.Adjust(clientIds.Select(clientId =>
                        new AppClientAdjustInfo() { ClientID = clientId, ServicesToStop = new[] { _serviceDesc } }).ToArray()
                    );
                }
            }

            // 逐渐选取需要启动该服务的终端
            private Guid[] _GetClientIdsNeedToDeploy()
            {
                lock (_thisLocker)
                {
                    // 统计该服务正在运行的数量
                    int count = _service.AppClientManager.GetAllClientInfos()
                        .Count(ci => ci.ServiceInfos.Any(si => si.ServiceDesc == _serviceDesc));
                    int needToDeployCount = Math.Max(1, count / 3);

                    return _service.AppClientManager.GetAllClientInfos()
                        .Where(ci => _clientIds.Contains(ci.ClientId) && _serviceStatus[ci.ClientId].Status == ServiceDeployStatus.Waiting)
                        .Select(ci => ci.ClientId).Take(needToDeployCount).ToArray();
                }
            }

            // 是否有任务在运行
            private bool _AnyRunning()
            {
                lock (_thisLocker)
                {
                    return _serviceStatus.Values.Any(s => s.Status == ServiceDeployStatus.Deploying);
                }
            }

            private bool _AllCompleted()
            {
                lock (_thisLocker)
                {
                    return _serviceStatus.Values.All(s =>
                        s.Status == ServiceDeployStatus.Completed || s.Status == ServiceDeployStatus.Error || s.Status == ServiceDeployStatus.Timeout);
                }
            }

            // 初始化服务的状态
            private void _InitServiceStatDataDict()
            {
                lock (_thisLocker)
                {
                    foreach (AppClientDeployInfo deployInfo in _service.DeployMapManager.GetAllDeployInfos())
                    {
                        AppClientInfo clientInfo = _service.AppClientManager.GetClientInfo(deployInfo.ClientId);
                        if (clientInfo != null && clientInfo.ServiceInfos != null && clientInfo.ServiceInfos.Any(si => si.ServiceDesc == _packageInfo.ServiceDesc))
                        {
                            _serviceStatus.Add(clientInfo.ClientId, new ServiceDescStatus() {
                                ClientId = clientInfo.ClientId, Connected = true, Status = ServiceDeployStatus.Waiting,
                            });
                        }
                    }
                }
            }

            private void _UpdateClientStatus()
            {
                lock (_thisLocker)
                {
                    foreach (ServiceDescStatus status in _serviceStatus.Values)
                    {
                        AppClientInfo clientInfo = _service.AppClientManager.GetClientInfo(status.ClientId);
                        ServiceInfo serviceInfo = null;
                        status.Connected = (clientInfo != null
                            && (serviceInfo = clientInfo.ServiceInfos.FirstOrDefault(si => si.ServiceDesc == _packageInfo.ServiceDesc)) != null);

                        if (status.Status == ServiceDeployStatus.Deploying)
                        {
                            if (status.Connected)
                            {
                                if (status.Restarting)
                                    //if (serviceInfo.DeployId == _packageInfo.Id)
                                    status.Status = ServiceDeployStatus.Completed;
                                /*else
                                    status.Status = ServiceDeployStatus.Error;*/
                            }
                            else
                            {
                                if (!status.Restarting)
                                {
                                    status.Restarting = true;
                                    _service.DeployMapManager.Adjust(new AppClientAdjustInfo() {
                                        ClientID = status.ClientId, ServicesToStart = new[] { _serviceDesc }
                                    });
                                }
                                else
                                {
                                    if (DateTime.UtcNow - status.StartDeployTime > TimeSpan.FromMinutes(2))
                                        status.Status = ServiceDeployStatus.Timeout;
                                }
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// 获取服务的部署进度
            /// </summary>
            /// <returns></returns>
            public ServiceDeployProgress[] GetProgress()
            {
                lock (_thisLocker)
                {
                    return _serviceStatus.Values.Select(s => new ServiceDeployProgress() {
                        ClientID = s.ClientId, Connected = s.Connected, ServiceDesc = _serviceDesc,
                        StartTime = s.StartDeployTime, Status = s.Status,
                    }).ToArray();
                }
            }

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

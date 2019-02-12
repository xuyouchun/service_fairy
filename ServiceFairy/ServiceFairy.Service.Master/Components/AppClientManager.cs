using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common;
using Common.Package;
using Common.Package.Cache;
using Common.Utility;
using Common.Contracts.Service;
using ServiceFairy.Client;
using Common.Package.Service;
using Common.Framework.TrayPlatform;
using Common.Collection;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Master;
using Common.Communication.Wcf;
using ServiceFairy.Deploy;
using System.Threading;

namespace ServiceFairy.Service.Master.Components
{
    /// <summary>
    /// 终端管理器
    /// </summary>
    [AppComponent("终端管理器", "记录终端的状态，包括服务、信道等状态")]
    partial class AppClientManager : TimerAppComponentBase
    {
        public AppClientManager(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;
            _serviceDeployInfos = new AutoLoad<Dictionary<ServiceDesc, ServiceDeployInfo>>(_LoadServiceDeployInfos, TimeSpan.FromSeconds(1));
        }

        private readonly Service _service;
        private readonly Dictionary<Guid, Wrapper> _clients = new Dictionary<Guid, Wrapper>();

        class Wrapper
        {
            public DateTime LastUpdate { get; set; }

            public AppClientInfo AppClientInfo { get; set; }
        }

        /// <summary>
        /// 更新终端信息
        /// </summary>
        /// <param name="info"></param>
        public void UpdateClientInfo(AppClientInfo info)
        {
            Contract.Requires(info != null);

            UpdateClientInfos(new[] { info });
        }

        /// <summary>
        /// 批量更新终端信息
        /// </summary>
        /// <param name="infos"></param>
        public void UpdateClientInfos(AppClientInfo[] infos)
        {
            Contract.Requires(infos != null);

            List<Guid> avaliableClientIds = new List<Guid>(), invalidClientIds = new List<Guid>();

            lock (_clients)
            {
                List<AppClientInfo> newClients = null;
                List<DeployChangedInfo> changedInfos = new List<DeployChangedInfo>();
                foreach (AppClientInfo info in infos)
                {
                    Wrapper w;
                    if (!_clients.TryGetValue(info.ClientId, out w))  // 新启动的服务终端
                    {
                        _clients.Add(info.ClientId, w = new Wrapper() { AppClientInfo = info, LastUpdate = DateTime.UtcNow });
                        (newClients ?? (newClients = new List<AppClientInfo>())).Add(w.AppClientInfo);

                        changedInfos.Add(new DeployChangedInfo(info.ClientId,
                            info.ServiceInfos == null ? null : info.ServiceInfos.Where(si => si.Status == AppServiceStatus.Running).Select(si => si.ServiceDesc).ToArray(),
                            null,
                            info.Communications,
                            null));
                    }
                    else
                    {
                        AppClientInfo oldInfo = w.AppClientInfo;
                        w.LastUpdate = DateTime.UtcNow;
                        w.AppClientInfo = info;
                        info.ConnectedTime = w.AppClientInfo.ConnectedTime;

                        DeployChangedInfo changedInfo = _GetDeployChangedInfo(oldInfo, info);
                        if (changedInfo.HasChanged())
                            changedInfos.Add(changedInfo);
                    }

                    (info.Avaliable ? avaliableClientIds : invalidClientIds).Add(info.ClientId);
                }

                if (!newClients.IsNullOrEmpty())
                    _RaiseConnectedEvent(newClients.ToArray());

                if (changedInfos.Count > 0)
                    _RaiseChangedEvent(changedInfos.ToArray());
            }

            _service.DeployMapManager.SetAvaliable(avaliableClientIds.ToArray(), AppClientAvaliable.ByClient, true);
            _service.DeployMapManager.SetAvaliable(invalidClientIds.ToArray(), AppClientAvaliable.ByClient, false);
            _service.DeployMapManager.SetConnected(infos.ToArray(info => info.ClientId), true);
        }

        /// <summary>
        /// 终端断开的通知
        /// </summary>
        /// <param name="clientIds"></param>
        public void DisconnectedNotify(Guid[] clientIds)
        {
            Contract.Requires(clientIds != null);

            lock (_clients)
            {
                HashSet<Guid> ids = clientIds.ToHashSet();
                IDictionary<Guid, Wrapper> dict = _clients.RemoveWhereWithReturn(item => ids.Contains(item.Key));
                if (dict.Count > 0)
                    _RaiseDisconnectedEvent(dict.Values.Select(v => v.AppClientInfo).ToArray());
            }
        }

        // 对比出哪些服务状态出现变化
        private DeployChangedInfo _GetDeployChangedInfo(AppClientInfo oldInfo, AppClientInfo info)
        {
            var oldSds = (oldInfo.ServiceInfos ?? new ServiceInfo[0]).ToDictionary(v => v.ServiceDesc);
            var newSds = (info.ServiceInfos ?? new ServiceInfo[0]).ToDictionary(v => v.ServiceDesc);

            List<ServiceDesc> startedSds = new List<ServiceDesc>(), stoppedSds = new List<ServiceDesc>();
            oldSds.CompareTo(newSds, delegate(ServiceDesc sd, ServiceInfo oldSi, ServiceInfo newSi, CollectionChangedType t) {
                if (t == CollectionChangedType.Add)
                {
                    if (newSi.Status == AppServiceStatus.Running)
                        startedSds.Add(sd);
                }
                else if (t == CollectionChangedType.Remove)
                {
                    if (oldSi.Status == AppServiceStatus.Running)
                        stoppedSds.Add(sd);
                }
                else if (t == CollectionChangedType.Update)
                {
                    (newSi.Status == AppServiceStatus.Running ? startedSds : stoppedSds).Add(sd);
                }
            });

            var oldComs = oldInfo.Communications ?? new CommunicationOption[0];
            var newComs = info.Communications ?? new CommunicationOption[0];
            List<CommunicationOption> openedComs = new List<CommunicationOption>(), closedComs = new List<CommunicationOption>();
            oldComs.CompareTo(newComs, delegate(CommunicationOption op, CollectionChangedType t) {
                if (t == CollectionChangedType.Add) openedComs.Add(op);
                else if (t == CollectionChangedType.Remove) closedComs.Add(op);
            });

            return new DeployChangedInfo(info.ClientId, startedSds.ToArray(), stoppedSds.ToArray(), openedComs.ToArray(), closedComs.ToArray());
        }

        /// <summary>
        /// 获取客户端列表
        /// </summary>
        /// <param name="clientIds"></param>
        public AppClientInfo[] GetClientInfos(Guid[] clientIds)
        {
            Contract.Requires(clientIds != null);

            lock (_clients)
            {
                return clientIds.Select(clientId => _clients.GetOrDefault(clientId)).WhereNotNull().Select(w => w.AppClientInfo).ToArray();
            }
        }

        /// <summary>
        /// 获取客户端列表
        /// </summary>
        /// <returns></returns>
        public AppClientInfo[] GetAllClientInfos()
        {
            lock (_clients)
            {
                return _clients.Values.Select(w => w.AppClientInfo).ToArray();
            }
        }

        /// <summary>
        /// 获取当前的终端数量
        /// </summary>
        /// <returns></returns>
        public int GetClientCount()
        {
            return _clients.Count;
        }

        /// <summary>
        /// 获取终端的状态
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public AppClientInfo GetClientInfo(Guid clientId)
        {
            lock (_clients)
            {
                Wrapper w = _clients.GetOrDefault(clientId);
                return w == null ? null : w.AppClientInfo;
            }
        }

        /// <summary>
        /// 获取所有服务终端的描述信息
        /// </summary>
        /// <returns></returns>
        public ClientDesc[] GetAllClientDescs()
        {
            lock (_clients)
            {
                return _clients.Values.Select(w => new ClientDesc() {
                    ClientID = w.AppClientInfo.ClientId, Title = w.AppClientInfo.Title,
                    ConnectedTime = w.AppClientInfo.ConnectedTime, Desc = w.AppClientInfo.Desc, HostName = w.AppClientInfo.HostName,
                    ServiceCount = w.AppClientInfo.ServiceInfos.CountOrDefault(), CommunicationCount = w.AppClientInfo.Communications.CountOrDefault(),
                    IPs = w.AppClientInfo.IPs,
                }).ToArray();
            }
        }

        /// <summary>
        /// 寻找指定地址的AppClient
        /// </summary>
        /// <param name="address"></param>
        public AppClientInfo[] SearchClientByIP(string address)
        {
            Contract.Requires(address != null);

            lock (_clients)
            {
                List<AppClientInfo> infos = new List<AppClientInfo>();
                foreach (Wrapper w in _clients.Values)
                {
                    if (!w.AppClientInfo.Communications.IsNullOrEmpty()
                        && w.AppClientInfo.Communications.Any(op => op.Address.Address == address))
                    {
                        infos.Add(w.AppClientInfo);
                    }
                }

                return infos.ToArray();
            }
        }

        public int[] SearchUsedPortByIP(string address)
        {
            Contract.Requires(address != null);
            lock (_clients)
            {
                List<int> ports = new List<int>();
                foreach (Wrapper w in _clients.Values)
                {
                    if (w.AppClientInfo.Communications.IsNullOrEmpty())
                        continue;

                    foreach (CommunicationOption op in w.AppClientInfo.Communications)
                    {
                        if (op.Address.Address == address)
                            ports.Add(op.Address.Port);
                    }
                }

                return ports.ToArray();
            }
        }

        private static TimeSpan _GetOfflineExpiredTime()
        {
#if DEBUG
                return TimeSpan.FromSeconds(300);
#else
                return TimeSpan.FromSeconds(10);
#endif
        }

        protected override void OnExecuteTask(string taskName)
        {
            Wrapper[] ws;
            lock (_clients)
            {
                DateTime dt = DateTime.UtcNow - _GetOfflineExpiredTime();
                ws = _clients.RemoveWhereWithReturn(w => w.Value.LastUpdate < dt).Values.ToArray();
            }

            if (!ws.IsNullOrEmpty())
            {
                _service.DeployMapManager.SetConnected(ws.ToArray(w => w.AppClientInfo.ClientId), false);
                _RaiseDisconnectedEvent(ws.ToArray(w => w.AppClientInfo));
            }
        }

        /// <summary>
        /// 是否为有效状态
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public bool IsAvaliable(Guid clientId)
        {
            AppClientDeployInfo deployInfo = _service.DeployMapManager.GetDeployInfo(clientId);
            AppClientInfo clientInfo = _service.AppClientManager.GetClientInfo(clientId);

            if (deployInfo == null || clientInfo == null)
                return false;

            return deployInfo.IsAvaliable() && clientInfo.Avaliable && !clientInfo.Deploying;
        }

        /// <summary>
        /// 新客户端连接事件
        /// </summary>
        public event EventHandler<AppClientInfoManagerClientConnectedEventArgs> Connected;

        private void _RaiseConnectedEvent(AppClientInfo[] clientInfos)
        {
            var eh = Connected;
            if (eh != null)
            {
                _AsyncRaise(() => eh(this, new AppClientInfoManagerClientConnectedEventArgs(clientInfos)));
            }
        }

        /// <summary>
        /// 客户端断开连接事件
        /// </summary>
        public event EventHandler<AppClientInfoManagerClientDisconnectedEventArgs> Disconnected;

        private void _RaiseDisconnectedEvent(AppClientInfo[] clientInfos)
        {
            var eh = Disconnected;
            if (eh != null)
            {
                _AsyncRaise(() => eh(this, new AppClientInfoManagerClientDisconnectedEventArgs(clientInfos)));
            }
        }

        /// <summary>
        /// 服务启动或停止事件
        /// </summary>
        public event EventHandler<AppClientInfoManagerClientInfoChangedEventArgs> Changed;

        private void _RaiseChangedEvent(DeployChangedInfo[] infos)
        {
            var eh = Changed;
            if (eh != null)
            {
                _AsyncRaise(() => eh(this, new AppClientInfoManagerClientInfoChangedEventArgs(infos)));
            }
        }

        private void _AsyncRaise(Action action)
        {
            ThreadPool.QueueUserWorkItem(delegate {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
            });
        }

        private readonly AutoLoad<Dictionary<ServiceDesc, ServiceDeployInfo>> _serviceDeployInfos;

        private Dictionary<ServiceDesc, ServiceDeployInfo> _LoadServiceDeployInfos()
        {
            Dictionary<ServiceDesc, List<Guid>> dict = new Dictionary<ServiceDesc, List<Guid>>();
            lock (_clients)
            {
                foreach (KeyValuePair<Guid, Wrapper> item in _clients)
                {
                    Wrapper w = item.Value;
                    foreach (ServiceInfo si in w.AppClientInfo.ServiceInfos)
                    {
                        dict.GetOrSet(si.ServiceDesc).Add(item.Key);
                    }
                }
            }

            return dict.ToDictionary(v => v.Key,
                v => new ServiceDeployInfo() { ServiceDesc = v.Key, ClientIDs = v.Value.ToArray() }
            );
        }

        /// <summary>
        /// 获取服务在终端的部署信息
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public ServiceDeployInfo[] GetServiceDeployInfos(ServiceDesc[] serviceDescs)
        {
            var dict = _serviceDeployInfos.Value;
            if (serviceDescs == null)
                return dict.Values.ToArray();

            return serviceDescs.Select(sd => dict.GetOrDefault(sd,
                new ServiceDeployInfo() { ServiceDesc = sd, ClientIDs = Array<Guid>.Empty })
            ).ToArray();
        }

        /// <summary>
        /// 获取服务在终端的部署信息
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public ServiceDeployInfo GetServiceDeployInfo(ServiceDesc serviceDesc)
        {
            return GetServiceDeployInfos(new[]{ serviceDesc}).FirstOrDefault();
        }

        /// <summary>
        /// 获取服务在终端上部署的数量
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public int GetClientCountOfService(ServiceDesc serviceDesc)
        {
            Contract.Requires(serviceDesc != null);
            ServiceDeployInfo info = GetServiceDeployInfo(serviceDesc);
            return (info != null) ? info.ClientIDs.Length : 0;
        }

        /// <summary>
        /// 判断指定的服务是否在运行
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public bool IsRunning(ServiceDesc serviceDesc)
        {
            Contract.Requires(serviceDesc != null);
            return GetClientCountOfService(serviceDesc) > 0;
        }

        /// <summary>
        /// 重启指定的终端
        /// </summary>
        /// <param name="clientIds"></param>
        public void RestartClients(Guid[] clientIds)
        {
            Contract.Requires(clientIds != null);

            RestartExecutor executor = new RestartExecutor(clientIds);
            executor.Execute(() => {

            });
        }

        /// <summary>
        /// 重启指定的终端
        /// </summary>
        /// <param name="clientId"></param>
        public void RestartClient(Guid clientId)
        {
            RestartClients(new[] { clientId });
        }
    }

    /// <summary>
    /// 新终端事件
    /// </summary>
    public class AppClientInfoManagerClientConnectedEventArgs : EventArgs
    {
        internal AppClientInfoManagerClientConnectedEventArgs(AppClientInfo[] clients)
        {
            Clients = clients;
        }

        /// <summary>
        ///　终端
        /// </summary>
        public AppClientInfo[] Clients { get; private set; }
    }

    /// <summary>
    /// 终端断开连接事件
    /// </summary>
    public class AppClientInfoManagerClientDisconnectedEventArgs : EventArgs
    {
        internal AppClientInfoManagerClientDisconnectedEventArgs(AppClientInfo[] clients)
        {
            Clients = clients;
        }

        /// <summary>
        ///　终端
        /// </summary>
        public AppClientInfo[] Clients { get; private set; }
    }

    /// <summary>
    /// 服务启动或停止
    /// </summary>
    public class AppClientInfoManagerClientInfoChangedEventArgs : EventArgs
    {
        internal AppClientInfoManagerClientInfoChangedEventArgs(DeployChangedInfo[] changedInfos)
        {
            DeployChangedInfos = changedInfos;
        }

        /// <summary>
        /// 变化的详细信息
        /// </summary>
        public DeployChangedInfo[] DeployChangedInfos { get; private set; }
    }
}

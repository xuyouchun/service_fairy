using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Client;
using Common.Package;
using System.Diagnostics.Contracts;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities;
using Common;
using Common.Utility;
using Common.Package.Service;
using ServiceFairy.Components;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Station.Components
{
    /// <summary>
    /// 终端实时信息管理器
    /// </summary>
    [AppComponent("终端实时信息管理器", "记录终端的实时信息，如内存、CPU占用情况等")]
    class AppClientInfoManagerAppComponent : TimerAppComponentBase
    {
        public AppClientInfoManagerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;
        }

        private readonly Service _service;
        private readonly Dictionary<Guid, Wrapper> _clients = new Dictionary<Guid, Wrapper>();
        private readonly Dictionary<Guid, Wrapper> _disconnectClients = new Dictionary<Guid, Wrapper>();
        private readonly object _thisLocker = new object();

        #region Class Wrapper ...

        class Wrapper
        {
            public DateTime LastUpdate { get; set; }

            public DateTime ConnectedTime { get; set; }

            public AppClientInfo AppClientInfo { get; set; }
        }

        #endregion

        /// <summary>
        /// 更新客户端信息
        /// </summary>
        /// <param name="clientInfo"></param>
        public void UpdateClientInfo(AppClientInfo clientInfo)
        {
            Contract.Requires(clientInfo != null);

            UpdateClientInfo(new[] { clientInfo });
        }

        /// <summary>
        /// 更新服务终端信息
        /// </summary>
        /// <param name="clientInfos"></param>
        public void UpdateClientInfo(AppClientInfo[] clientInfos)
        {
            Contract.Requires(clientInfos != null);

            lock (_thisLocker)
            {
                bool hasNewClient = false;
                foreach (AppClientInfo info in clientInfos)
                {
                    Wrapper w0;
                    if (_clients.TryGetValue(info.ClientId, out w0))
                    {
                        w0.AppClientInfo = info;
                        w0.LastUpdate = DateTime.UtcNow;
                        w0.AppClientInfo.ConnectedTime = w0.ConnectedTime;
                    }
                    else
                    {
                        Wrapper w = new Wrapper() { LastUpdate = DateTime.UtcNow, AppClientInfo = info, ConnectedTime = DateTime.UtcNow };
                        _clients[info.ClientId] = w;
                        info.ConnectedTime = w.ConnectedTime;
                        hasNewClient = true;
                    }

                    _disconnectClients.Remove(info.ClientId);
                }

                if(hasNewClient)
                    _HeartBeatImmediately();
            }
        }

        /// <summary>
        /// 通知连接断开
        /// </summary>
        /// <param name="clientId"></param>
        public void DisconnectNotify(Guid clientId)
        {
            lock (_thisLocker)
            {
                Wrapper r;
                if (_clients.TryGetValue(clientId, out r))
                {
                    if (_clients.Remove(clientId))
                        _disconnectClients.Add(clientId, r);
                }
            }
        }

        /// <summary>
        /// 获取全部已连接的终端
        /// </summary>
        /// <returns></returns>
        public AppClientInfo[] GetAllConnected()
        {
            lock (_thisLocker)
            {
                return _clients.Values.Select(w => w.AppClientInfo).ToArray();
            }
        }

        protected override void OnExecuteTask(string taskName)
        {
            lock (_thisLocker)
            {
                DateTime dt = DateTime.UtcNow - TimeSpan.FromSeconds(15);
                _clients.RemoveWhere(item => item.Value.LastUpdate < dt);
            }
        }

        /// <summary>
        /// 获取全部已断开的终端
        /// </summary>
        /// <param name="clear"></param>
        /// <returns></returns>
        public AppClientInfo[] GetAllDisconnected(bool clear = false)
        {
            lock (_thisLocker)
            {
                AppClientInfo[] infos = _disconnectClients.Values.Select(w => w.AppClientInfo).ToArray();
                _disconnectClients.Clear();
                return infos;
            }
        }

        /// <summary>
        /// 清空已断开的终端
        /// </summary>
        public void ClearAllDisconnected()
        {
            lock (_thisLocker)
            {
                _disconnectClients.Clear();
            }
        }

        /// <summary>
        /// 是否有断开的终端
        /// </summary>
        /// <returns></returns>
        public bool ContainsDisconnected()
        {
            return _disconnectClients.Count > 0;
        }

        protected override void OnStatusChanged(AppComponentStatus status)
        {
            _service.ServiceEvent.Switch(ServiceEventNames.EVENT_CLIENT_DISCONNECTED, _ClientDisconnectedNotify, status);

            base.OnStatusChanged(status);
        }

        private void _HeartBeatImmediately()
        {
            _service.HeartBeatController.DoImmediately();
        }

        // 服务终端断开连接
        private void _ClientDisconnectedNotify(object sender, ServiceEventArgs e)
        {
            DisconnectNotify(e.Source.ClientId);
            _HeartBeatImmediately();
        }
    }
}

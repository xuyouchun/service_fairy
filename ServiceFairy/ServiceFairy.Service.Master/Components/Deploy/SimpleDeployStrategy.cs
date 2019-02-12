using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.IO;
using Common;
using ServiceFairy.Entities;
using Common.Utility;
using Common.Framework.TrayPlatform;
using Common.Contracts.Service;
using Common.Package.GlobalTimer;
using Common.Package;
using ServiceFairy.Entities.Master;
using Common.Communication.Wcf;
using ServiceFairy.Client;
using ServiceFairy.Deploy;

namespace ServiceFairy.Service.Master.Components.Deploy
{
    /// <summary>
    /// 部署地图策略
    /// </summary>
    class SimpleDeployStrategy : IDeployStrategy
    {
        public SimpleDeployStrategy(Service service, AppClientDeployMap map)
        {
            _map = map ?? new AppClientDeployMap();
            _service = service;
        }

        private readonly Service _service;
        private IGlobalTimerTaskHandle _handle;
        private AppClientDeployMap _map;
        private readonly object _thisLock = new object();

        public void Init()
        {
            _handle = GlobalTimer<ITask>.Default.Add(TimeSpan.FromMinutes(5), new TaskFuncAdapter(_ReviseForBalanceFunc), false);
            DeployUtility.ReviseForBalance(_service, _map);
            _map.UpdateVersion();
        }

        public AppClientDeployMap GetDeployMap()
        {
            return _map;
        }

        public void OnChanged(DeployChangedInfo[] changedInfos)
        {
            if (changedInfos.IsNullOrEmpty())
                return;

            lock (_thisLock)
            {
                foreach (DeployChangedInfo changedInfo in changedInfos)
                {
                    _OnChanged(changedInfo);
                }

                _ReviseForBalance();
            }
        }

        private void _OnChanged(DeployChangedInfo changedInfo)
        {
            AppClientDeployInfo dInfo = _map.GetDeployInfo(changedInfo.ClientId);
            if (dInfo == null)
                return;

            if (!changedInfo.ServicesStarted.IsNullOrEmpty())
            {
                foreach (ServiceDesc serviceDesc in changedInfo.ServicesStarted)
                {
                    _OnServiceStarted(dInfo, serviceDesc);
                }
            }

            if (!changedInfo.ServicesStopped.IsNullOrEmpty())
            {
                foreach (ServiceDesc serviceDesc in changedInfo.ServicesStopped)
                {
                    _OnServiceStopped(dInfo, serviceDesc);
                }
            }
        }

        private void _OnServiceStarted(AppClientDeployInfo dInfo, ServiceDesc serviceDesc)
        {
            return;
        }

        private void _OnServiceStopped(AppClientDeployInfo dInfo, ServiceDesc serviceDesc)
        {
            return;
        }

        public void OnNewClient(AppClientDeployInfo[] deployInfos)
        {
            if (deployInfos.IsNullOrEmpty())
                return;

            lock (_thisLock)
            {
                _map.AddDeployInfos(deployInfos, true);
            }
        }

        public bool Adjust(AppClientAdjustInfo[] adjustInfos)
        {
            if (adjustInfos.IsNullOrEmpty())
                return false;

            bool modified = false;
            lock (_thisLock)
            {
                foreach (AppClientAdjustInfo info in adjustInfos)
                {
                    if (_Adjust(info))
                        modified = true;
                }

                if (modified)
                    _map.UpdateVersion();
            }

            return modified;
        }

        private bool _Adjust(AppClientAdjustInfo info)
        {
            AppClientDeployInfo dInfo = _map.GetDeployInfo(info.ClientID);
            if (dInfo == null)
                return false;

            return dInfo.Adjust(info, true);
        }

        public AppClientDeployInfo GetDeployInfo(Guid clientId)
        {
            lock (_thisLock)
            {
                return _map.GetDeployInfo(clientId);
            }
        }

        /// <summary>
        /// 获取全部的部署信息
        /// </summary>
        /// <param name="lastUpdate"></param>
        /// <returns></returns>
        public AppClientDeployInfo[] GetAllDeployInfos(out DateTime lastUpdate)
        {
            lock (_thisLock)
            {
                lastUpdate = _map.LastUpdate;
                return _map.GetAll();
            }
        }

        /// <summary>
        /// 设置启用状态
        /// </summary>
        /// <param name="clientIds"></param>
        /// <param name="type"></param>
        /// <param name="avaliable"></param>
        public void SetAvaliable(Guid[] clientIds, AppClientAvaliable type, bool avaliable)
        {
            if (clientIds.IsNullOrEmpty())
                return;

            lock (_thisLock)
            {
                bool modified = false;
                foreach (Guid clientId in clientIds)
                {
                    AppClientDeployInfo dInfo = _map.GetDeployInfo(clientId);
                    if (dInfo != null && dInfo.SetAvaliable(type, avaliable, true))
                        modified = true;
                }

                if (modified)
                    _ReviseForBalance();
            }
        }

        /// <summary>
        /// 设置连接状态
        /// </summary>
        /// <param name="clientIds"></param>
        /// <param name="connected"></param>
        public void SetConnected(Guid[] clientIds, bool connected)
        {
            if (clientIds.IsNullOrEmpty())
                return;

            lock (_thisLock)
            {
                bool modified = false;
                foreach (Guid clientId in clientIds)
                {
                    AppClientDeployInfo dInfo = _map.GetDeployInfo(clientId);
                    if (dInfo != null && dInfo.SetConnected(connected, true))
                        modified = true;
                }

                if (modified)
                    _ReviseForBalance();
            }
        }

        private void _ReviseForBalance()
        {
            _handle.ExecuteImmediately();
        }

        private void _ReviseForBalanceFunc()
        {
            lock (_thisLock)
            {
                DeployUtility.ReviseForBalance(_service, _map);
                _map.UpdateVersion();
                _RaiseModifiedEvent();
            }
        }

        /// <summary>
        /// 获取最后更新时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetLastUpdate()
        {
            lock (_thisLock)
            {
                return _map.LastUpdate;
            }
        }

        /// <summary>
        /// 更新平台版本号
        /// </summary>
        /// <param name="clientIds"></param>
        /// <param name="deployPackageId"></param>
        public void UpdatePlatformDeployId(Guid[] clientIds, Guid deployPackageId)
        {
            Contract.Requires(clientIds != null);

            bool modified = false;
            foreach (Guid clientId in clientIds)
            {
                AppClientDeployInfo deployInfo = GetDeployInfo(clientId);
                if (deployInfo != null && deployInfo.PlatformDeployPackageId != deployPackageId)
                {
                    deployInfo.UpdateVersion();
                    deployInfo.PlatformDeployPackageId = deployPackageId;

                    AppClientInfo clientInfo = _service.AppClientManager.GetClientInfo(clientId);
                    if (clientInfo != null)
                        clientInfo.Deploying = true;

                    modified = true;
                }
            }

            if (modified)
            {
                _ReviseForBalance();
            }
        }

        /// <summary>
        /// 发生变化
        /// </summary>
        public event EventHandler Modified;

        private void _RaiseModifiedEvent()
        {
            var eh = Modified;
            if (eh != null)
                eh(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            if (_handle != null)
                _handle.Dispose();
        }

        public object SyncLocker
        {
            get { return _thisLock; }
        }
    }
}

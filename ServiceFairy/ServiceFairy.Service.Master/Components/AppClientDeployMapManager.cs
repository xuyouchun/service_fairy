using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;
using Common.Package;
using Common.Package.GlobalTimer;
using Common.Framework.TrayPlatform;
using Common;
using Common.Utility;
using Common.Package.Service;
using ServiceFairy.Entities;
using System.IO;
using System.Threading;
using ServiceFairy.Components;
using ServiceFairy.Entities.Master;
using Common.Communication.Wcf;
using ServiceFairy.Service.Master.Components.Deploy;
using ServiceFairy.Deploy;

namespace ServiceFairy.Service.Master.Components
{
    [AppComponent("部署地图管理器", "记录终端启动的服务、开启的端口、及交互方式，对部署地图的修改将直接影响终端的布局")]
    class AppClientDeployMapManager : TimerAppComponentBase
    {
        public AppClientDeployMapManager(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;
        }

        private readonly Service _service;
        private IDeployStrategy _deployStrategy;
        private DateTime _deployMapLastUpdate;

        protected override void OnExecuteTask(string taskName)
        {
            if (_deployStrategy == null)
                return;

            // 保存配置文件表
            try
            {
                DateTime dt = _deployStrategy.GetLastUpdate();
                if (dt != _deployMapLastUpdate && _deployMapLastUpdate != default(DateTime))
                {
                    lock (_deployStrategy.SyncLocker)
                    {
                        _SaveDeployMap(_deployStrategy.GetDeployMap());
                    }
                }

                _deployMapLastUpdate = dt;
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
            }
        }

        public bool IsAvaliable()
        {
            return _deployStrategy != null;
        }

        private string _GetDeployMapPath()
        {
            return Path.Combine(_service.ServiceDataPath, "DeployMap.xml");
        }

        private AppClientDeployMap _LoadDeployMap()
        {
            string file = _GetDeployMapPath();
            return DeployUtility.LoadDeployMapFromXmlFile(file) ?? new AppClientDeployMap();
        }

        private void _SaveDeployMap(AppClientDeployMap deployMap)
        {
            DeployUtility.SaveDeployMapToXmlFile(_GetDeployMapPath(), deployMap);
        }

        /// <summary>
        /// 获取部署地图
        /// </summary>
        /// <param name="lastUpdate">版本号</param>
        /// <returns></returns>
        public AppClientDeployInfo[] GetAllDeployInfos(out DateTime lastUpdate)
        {
            _ValidateAvaliable();

            return _deployStrategy.GetAllDeployInfos(out lastUpdate);
        }

        private void _ValidateAvaliable()
        {
            if (_deployStrategy == null)
                throw new ServiceException(ServerErrorCode.DataNotReady, "部署地图尚未准备好");
        }

        /// <summary>
        /// 获取部署地图
        /// </summary>
        /// <returns></returns>
        public AppClientDeployInfo[] GetAllDeployInfos()
        {
            DateTime dt;
            return GetAllDeployInfos(out dt);
        }

        /// <summary>
        /// 根据ClientId获取部署信息
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="lastUpdate"></param>
        /// <returns></returns>
        public AppClientDeployInfo GetDeployInfo(Guid clientId)
        {
            _ValidateAvaliable();

            return _deployStrategy.GetDeployInfo(clientId);
        }

        /// <summary>
        /// 添加部署信息
        /// </summary>
        /// <param name="deployInfo"></param>
        public void AddDeployInfo(AppClientDeployInfo deployInfo)
        {
            Contract.Requires(deployInfo != null);

            _ValidateAvaliable();

            _deployStrategy.OnNewClient(new[] { deployInfo });
        }

        /// <summary>
        /// 修改部署地图
        /// </summary>
        /// <param name="info"></param>
        public bool Adjust(AppClientAdjustInfo info)
        {
            Contract.Requires(info != null);

            return Adjust(new[] { info });
        }

        /// <summary>
        /// 修改部署地图
        /// </summary>
        /// <param name="infos"></param>
        public bool Adjust(AppClientAdjustInfo[] infos)
        {
            Contract.Requires(infos != null);

            _ValidateAvaliable();

            if (infos.Length == 0)
                return false;

            return _deployStrategy.Adjust(infos);
        }

        /// <summary>
        /// 更新指定服务终端的平台版本号
        /// </summary>
        /// <param name="clientIds"></param>
        /// <param name="deployPackageId"></param>
        public void UpdatePlatformDeployId(Guid[] clientIds, Guid deployPackageId)
        {
            if (clientIds.IsNullOrEmpty())
                return;

            _deployStrategy.UpdatePlatformDeployId(clientIds, deployPackageId);
        }

        /// <summary>
        /// 设置终端的有效状态
        /// </summary>
        /// <param name="clientId">终端ID</param>
        /// <param name="avaliable">有效状态</param>
        /// <param name="type">类型</param>
        public void SetAvaliable(Guid clientId, AppClientAvaliable type, bool avaliable)
        {
            _ValidateAvaliable();

            _deployStrategy.SetAvaliable(new[] { clientId }, type, avaliable);
        }

        /// <summary>
        /// 设置终端的有效千姿百态
        /// </summary>
        /// <param name="clientIds">终端ID</param>
        /// <param name="type">类型</param>
        /// <param name="avaliable">有效状态</param>
        public void SetAvaliable(Guid[] clientIds, AppClientAvaliable type, bool avaliable)
        {
            _ValidateAvaliable();

            if (!clientIds.IsNullOrEmpty())
                _deployStrategy.SetAvaliable(clientIds, type, avaliable);
        }

        /// <summary>
        /// 设置终端的连接状态
        /// </summary>
        /// <param name="clientIds"></param>
        /// <param name="connected"></param>
        public void SetConnected(Guid[] clientIds, bool connected)
        {
            _ValidateAvaliable();

            if (!clientIds.IsNullOrEmpty())
                _deployStrategy.SetConnected(clientIds, connected);
        }

        /// <summary>
        /// 设置终端的连接状态
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="connected"></param>
        public void SetConnected(Guid clientId, bool connected)
        {
            _ValidateAvaliable();

            _deployStrategy.SetConnected(new[] { clientId }, connected);
        }

        /// <summary>
        /// 获取已经使用的端口号
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int[] SearchPortsByIp(string ip)
        {
            Contract.Requires(ip != null);
            _ValidateAvaliable();

            List<int> ports = new List<int>();
            foreach (AppClientDeployInfo dInfo in GetAllDeployInfos())
            {
                foreach (CommunicationOption op in dInfo.CommunicateOptions ?? new CommunicationOption[0])
                {
                    if (op.Address.Address == ip)
                        ports.Add(op.Address.Port);
                }
            }

            return ports.ToArray();
        }

        protected override void OnStatusChanged(AppComponentStatus status)
        {
            if (status == AppComponentStatus.Enable)
            {
                AutoRetry((Action)_TryLoadDeployInfo);
                _service.AppClientManager.Changed += new EventHandler<AppClientInfoManagerClientInfoChangedEventArgs>(AppClientInfoManager_Changed);
                _service.AppClientManager.Connected += new EventHandler<AppClientInfoManagerClientConnectedEventArgs>(AppClientInfoManager_Connected);
                _service.AppClientManager.Disconnected += new EventHandler<AppClientInfoManagerClientDisconnectedEventArgs>(AppClientInfoManager_Disconnected);
            }
            else
            {
                _service.AppClientManager.Changed -= new EventHandler<AppClientInfoManagerClientInfoChangedEventArgs>(AppClientInfoManager_Changed);
                _service.AppClientManager.Connected -= new EventHandler<AppClientInfoManagerClientConnectedEventArgs>(AppClientInfoManager_Connected);
                _service.AppClientManager.Disconnected -= new EventHandler<AppClientInfoManagerClientDisconnectedEventArgs>(AppClientInfoManager_Disconnected);

                if (_deployStrategy != null)
                    _deployStrategy.Dispose();
            }

            base.OnStatusChanged(status);
        }

        // 新终端连接
        void AppClientInfoManager_Connected(object sender, AppClientInfoManagerClientConnectedEventArgs e)
        {
            
        }

        // 终端断开
        void AppClientInfoManager_Disconnected(object sender, AppClientInfoManagerClientDisconnectedEventArgs e)
        {
            
        }

        // 服务状态发生变化
        void AppClientInfoManager_Changed(object sender, AppClientInfoManagerClientInfoChangedEventArgs e)
        {
            _deployStrategy.OnChanged(e.DeployChangedInfos);
        }

        private void _TryLoadDeployInfo()
        {
            AppClientDeployMap map = _LoadDeployMap();
            IDeployStrategy strategy = new SimpleDeployStrategy(_service, map);

            // 将自身的部署信息添加上去
            AppClientDeployInfo selfDeployInfo = new AppClientDeployInfo() {
                ClientId = _service.Context.ClientID,
                CommunicateOptions = _service.Context.Platform.GetAllCommunicationOptions(),
                Services = _service.Context.Platform.GetAllServiceInfos().ToArray(si => new AppServiceDeployInfo(si.ServiceDesc))
            };

            _deployStrategy = strategy;
            strategy.OnNewClient(new[] { selfDeployInfo });
            strategy.Init();

            _deployStrategy.Modified += new EventHandler(_deployStrategy_Modified);
        }

        // 部署地图变化通知
        private void _deployStrategy_Modified(object sender, EventArgs e)
        {
            _service.ServiceEvent.Raise(ServiceEventNames.EVENT_DEPLOY_MAPMODIFIED, null);
        }
    }
}

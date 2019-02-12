using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using Common;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Framework.TrayPlatform;
using Common.Package;
using Common.Package.GlobalTimer;
using Common.Package.Service;
using Common.Utility;
using ServiceFairy.Components;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Master;
using ServiceFairy.SystemInvoke;

namespace ServiceFairy.Service.Deploy.Components
{
    /// <summary>
    /// 部署地图管理器
    /// </summary>
    [AppComponent("部署地图管理器", "管理服务的部署地图，并提供下载")]
    class DeployMapManager : TimerAppComponentBase
    {
        public DeployMapManager(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;
        }

        #region Class Wrapper ...

        class Wrapper
        {
            public DateTime LastUpdate { get; set; }

            public AppClientDeployMap DeployMap { get; set; }

            public Dictionary<string, Dictionary<SVersion, List<AppInvokeInfo>>> Dict { get; set; }
        }

        #endregion

        private readonly Service _service;
        private Wrapper _wrapper = new Wrapper() { LastUpdate = DateTime.MinValue, DeployMap = new AppClientDeployMap() };
        private readonly object _thisLocker = new object();

        protected override void OnExecuteTask(string taskName)
        {
            _Update();
        }

        /// <summary>
        /// 更新部署信息
        /// </summary>
        /// <param name="deployInfos"></param>
        /// <param name="lastUpdate"></param>
        private void _UpdateDeployMap(AppClientDeployInfo[] deployInfos, DateTime lastUpdate)
        {
            AppClientDeployMap map = new AppClientDeployMap();
            map.AddDeployInfos(deployInfos, false);

            Dictionary<string, Dictionary<SVersion, List<AppInvokeInfo>>> dict = new Dictionary<string, Dictionary<SVersion, List<AppInvokeInfo>>>();
            foreach (AppClientDeployInfo dInfo in (deployInfos ?? new AppClientDeployInfo[0]).Where(info => info.IsAvaliable()))
            {
                foreach (ServiceDesc serviceDesc in (dInfo.Services ?? Array<AppServiceDeployInfo>.Empty).Select(v => v.ServiceDesc))
                {
                    dict.GetOrSet(serviceDesc.Name).GetOrSet(serviceDesc.Version).Add(
                        new AppInvokeInfo() { ServiceDescs = new[] { serviceDesc }, CommunicateOptions = dInfo.CommunicateOptions, ClientID = dInfo.ClientId }
                    );
                }
            }

            _wrapper = new Wrapper() { LastUpdate = lastUpdate, DeployMap = map, Dict = dict };
            _avaliable = true;
            _waitForAvaliable.Set();
        }

        /// <summary>
        /// 获取部署信息
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <returns></returns>
        public AppClientDeployInfo GetDeployInfo(Guid clientId)
        {
            return GetDeployInfos(new[] { clientId }).FirstOrDefault();
        }

        /// <summary>
        /// 批量获取部署信息
        /// </summary>
        /// <param name="clientIds"></param>
        /// <returns></returns>
        public AppClientDeployInfo[] GetDeployInfos(Guid[] clientIds)
        {
            Contract.Requires(clientIds != null);

            AppClientDeployMap dMap = _wrapper.DeployMap;
            return clientIds.Distinct().Select(clientId => dMap.GetDeployInfo(clientId)).WhereNotNull().ToArray();
        }

        /// <summary>
        /// 获取全部部署信息
        /// </summary>
        /// <returns></returns>
        public AppClientDeployInfo[] GetAllDeployInfos()
        {
            return _wrapper.DeployMap.GetAll();
        }

        /// <summary>
        /// 获取所有的服务终端唯一标识
        /// </summary>
        /// <returns></returns>
        public Guid[] GetAllClientIds()
        {
            AppClientDeployInfo[] infos = GetAllDeployInfos();
            return infos.Where(info => info.IsAvaliable()).ToArray(info => info.ClientId);
        }

        private volatile bool _avaliable;

        /// <summary>
        /// 数据是否可用
        /// </summary>
        public bool Avaliable
        {
            get { return _avaliable; }
        }

        public void _ValidateAvaliable()
        {
            if (!_avaliable)
                throw new ServiceException(ServerErrorCode.DataNotReady);
        }

        private readonly ManualResetEvent _waitForAvaliable = new ManualResetEvent(false);
        private readonly ManualResetEvent _waitForExit = new ManualResetEvent(false);

        /// <summary>
        /// 等待数据可用
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool WaitForAvaliable(TimeSpan timeout)
        {
            if (_avaliable)
                return true;

            ExecuteImmediately();
            return WaitHandle.WaitAny(new[] { _waitForAvaliable, _waitForExit }, timeout) == 0;
        }

        private void _Update()
        {
            AutoRetry((Func<bool>)_TryUpdate);
        }

        private bool _TryUpdate()
        {
            ServiceResult<Master_GetDeployMap_Reply> rsp = MasterService.DownloadDeployMap(_service.Context.ServiceClient,
                new Master_GetDeployMap_Request() {
                    ClientID = _service.Context.ClientID, Caller = _service.Context.ServiceDesc, LastUpdate = _wrapper.LastUpdate
            });

            if (rsp.Succeed)
            {
                if (rsp.Result.DeployInfos != null)
                {
                    _UpdateDeployMap(rsp.Result.DeployInfos, rsp.Result.LastUpdate);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 根据服务信息获取服务位置
        /// </summary>
        /// <param name="serviceDesc"></param>
        public AppInvokeInfo[] SearchServices(ServiceDesc serviceDesc)
        {
            Contract.Requires(serviceDesc != null);

            _ValidateAvaliable();

            Dictionary<string, Dictionary<SVersion, List<AppInvokeInfo>>> dict = _wrapper.Dict;
            Dictionary<SVersion, List<AppInvokeInfo>> verDict;
            if (!dict.TryGetValue(serviceDesc.Name, out verDict))
                return new AppInvokeInfo[0];

            if (serviceDesc.Version.IsEmpty)
                return verDict.Values.SelectMany(v => v).ToArray();

            List<AppInvokeInfo> invokeInfos;
            verDict.TryGetValue(serviceDesc.Version, out invokeInfos);
            return invokeInfos == null ? new AppInvokeInfo[0] : invokeInfos.ToArray();
        }

        protected override void OnStatusChanged(AppComponentStatus status)
        {
            _service.ServiceEvent.Switch(ServiceEventNames.EVENT_DEPLOY_MAPMODIFIED, _DeployMapModifiedNotify, status);

            base.OnStatusChanged(status);
        }

        // 部署地图发生变化
        private void _DeployMapModifiedNotify(object sender, ServiceEventArgs e)
        {
            ExecuteImmediately();
        }

        protected override void OnStart()
        {
            base.OnStart();

            _waitForAvaliable.Reset();
            _waitForExit.Reset();
        }

        protected override void OnDispose()
        {
            _waitForExit.Set();
            base.OnDispose();
        }
    }
}

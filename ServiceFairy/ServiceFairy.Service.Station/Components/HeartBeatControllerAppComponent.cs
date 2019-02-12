using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Client;
using Common.Package;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities;
using System.Diagnostics.Contracts;
using Common.Package.Service;
using Common.Utility;
using System.Threading;
using ServiceFairy.Components;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Station.Components
{
    /// <summary>
    /// 对Master的心跳
    /// </summary>
    [AppComponent("对中心服务的心跳控制器", "向中心服务器报告各终端的情况，维持各终端与中心服务的连接")]
    class HeartBeatControllerAppComponent : TimerAppComponentBase
    {
        public HeartBeatControllerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(2))
        {
            _service = service;
        }

        private readonly Service _service;
        private readonly object _thisLocker = new object();

        /// <summary>
        /// 立即执行心跳逻辑
        /// </summary>
        public void DoImmediately()
        {
            ExecuteImmediately();
        }

        protected override void OnExecuteTask(string taskName)
        {
            try
            {
                SystemInvoker invoker = SystemInvoker.FromServiceClient(_service.Context.ServiceClient);
                invoker.Master.StationHeartBeat(
                    _service.AppClientInfoManager.GetAllConnected(),
                    _service.AppClientInfoManager.GetAllDisconnected(true).ToArray(c => c.ClientId)
                );
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
            }
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
    }
}

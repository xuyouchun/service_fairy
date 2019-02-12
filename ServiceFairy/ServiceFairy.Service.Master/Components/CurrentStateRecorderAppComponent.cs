using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Client;
using Common.Contracts;

namespace ServiceFairy.Service.Master.Components
{
    /// <summary>
    /// 当前状态记录器
    /// </summary>
    [AppComponent("当前状态记录器", "记录集群的当前状态")]
    class CurrentStateRecorderAppComponent : TimerAppComponentBase
    {
        public CurrentStateRecorderAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;
        }

        private readonly Service _service;

        [ObjectProperty("当前在线用户数")]
        private int _onlineUserCount = 0;

        [ObjectProperty("最大在线用户数")]
        private int _maxOnlineUserCount = 0;

        [ObjectProperty("最大在线用户数发生的时间（UTC）")]
        private DateTime _maxOnlineUserCountTime = DateTime.UtcNow;

        [ObjectProperty("最大在线用户数发生的时间（服务器所在地）")]
        private DateTime MaxOnlineUserCountTime
        {
            get { return _maxOnlineUserCountTime.ToLocalTime(); }
        }

        private bool _hasDowngrade = false;

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnExecuteTask(string taskName)
        {
            // 统计在线用户的数量
            AppClientInfo[] clientInfos = _service.AppClientManager.GetAllClientInfos();
            _onlineUserCount = clientInfos.Sum(ci => ci.GetOnlineUserCount());
            if (_onlineUserCount > _maxOnlineUserCount || (_onlineUserCount == _maxOnlineUserCount && _hasDowngrade))
            {
                _maxOnlineUserCount = _onlineUserCount;
                _maxOnlineUserCountTime = DateTime.UtcNow;
                _hasDowngrade = false;
            }
            else if (_onlineUserCount < _maxOnlineUserCount)
            {
                _hasDowngrade = true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities.Sys;
using System.Runtime.Serialization;
using Common;
using Common.Package;
using Common.Utility;

namespace ServiceFairy.Components
{
    /// <summary>
    /// 服务监控器的终端
    /// </summary>
    [AppComponent("服务监控器的终端", "将服务的运转状况提供给监控服务", category: AppComponentCategory.System, name: "Sys_ServiceWatch")]
    public class ServiceWatchAppComponent : TimerAppComponentBase
    {
        public ServiceWatchAppComponent(CoreAppServiceBase service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;
            _appCommands = AppCommandCollection.LoadFromType(GetType(), new object[] { this });
        }

        private readonly CoreAppServiceBase _service;
        private readonly AppCommandCollection _appCommands;

        private WatchInfo[] _curWatchInfos = Array<WatchInfo>.Empty;

        protected override void OnExecuteTask(string taskName)
        {
            _curWatchInfos = _GetWatchInfos();
        }

        protected override void OnStart()
        {
            base.OnStart();

            _service.AddCommands(_appCommands);
        }

        protected override void OnStop()
        {
            base.OnStop();

            _service.RemoveCommands(_appCommands);
        }

        /// <summary>
        /// 获取监控信息
        /// </summary>
        public event ServiceWatchClientQueryInfoEventHandler QueryWatchInfo;

        private WatchInfo[] _GetWatchInfos()
        {
            var eh = QueryWatchInfo;
            if (eh == null)
                return Array<WatchInfo>.Empty;

            List<WatchInfo> infos = new List<WatchInfo>();
            foreach (ServiceWatchClientQueryInfoEventHandler eh0 in eh.GetInvocationList())
            {
                try
                {
                    ServiceWatchClientQueryInfoEventArgs e = new ServiceWatchClientQueryInfoEventArgs();
                    eh0(this, e);
                    if (!e.Infos.IsNullOrEmpty())
                        infos.AddRange(e.Infos);
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
            }

            return infos.ToArray();
        }

        /// <summary>
        /// 获取服务的监控信息
        /// </summary>
        [AppCommand("Sys_GetWatchInfo", title: "获取服务的监控信息", category: AppCommandCategory.System, SecurityLevel = SecurityLevel.SysRunningLevel)]
        class GetWatchInfoAppCommand : ACS<CoreAppServiceBase>.Func<Sys_GetWatchInfo_Reply>
        {
            public GetWatchInfoAppCommand(ServiceWatchAppComponent owner)
            {
                _owner = owner;
            }

            private readonly ServiceWatchAppComponent _owner;

            protected override Sys_GetWatchInfo_Reply OnExecute(AppCommandExecuteContext<CoreAppServiceBase> context, ref ServiceResult sr)
            {
                return new Sys_GetWatchInfo_Reply() {
                    Infos = _owner._curWatchInfos,
                };
            }
        }
    }

    /// <summary>
    /// 获取监控信息的事件参数
    /// </summary>
    [Serializable, DataContract]
    public class ServiceWatchClientQueryInfoEventArgs : EventArgs
    {
        /// <summary>
        /// 监控信息
        /// </summary>
        [DataMember]
        public WatchInfo[] Infos { get; set; }
    }

    /// <summary>
    /// 获取服务监控信息的事件委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ServiceWatchClientQueryInfoEventHandler(object sender, ServiceWatchClientQueryInfoEventArgs e);
}

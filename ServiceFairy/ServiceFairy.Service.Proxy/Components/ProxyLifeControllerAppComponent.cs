using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using Common.Package;

namespace ServiceFairy.Service.Proxy.Components
{
    /// <summary>
    /// 用于控制代理服务的开启与关闭
    /// </summary>
    [AppComponent("代理生命周期控制器", "维持代理的生命周期，控制代理服务的开启与关闭")]
    class ProxyLifeControllerAppComponent : TimerAppComponentBase
    {
        public ProxyLifeControllerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;
        }

        private readonly Service _service;

        /// <summary>
        /// 代理的状态
        /// </summary>
        public bool ProxyEnabled
        {
            get { return _service.Context.ProxyManager.Enabled; }
        }

        protected override void OnStatusChanged(AppComponentStatus status)
        {
            if (status == AppComponentStatus.Enable)
            {
                _service.Context.ProxyManager.EnsureEnable();
                LogManager.LogMessage("代理已开启");
            }
            else
            {
                _service.Context.ProxyManager.Disable();
                LogManager.LogMessage("代理已关闭");
            }

            base.OnStatusChanged(status);
        }

        protected override void OnExecuteTask(string taskName)
        {
            _service.Context.ProxyManager.EnsureEnable();
        }
    }
}

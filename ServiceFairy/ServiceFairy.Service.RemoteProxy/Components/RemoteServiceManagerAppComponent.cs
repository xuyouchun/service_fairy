using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Contracts.Service;
using Common.Package.Service;

namespace ServiceFairy.Service.RemoteProxy.Components
{
    /// <summary>
    /// 远程服务管理器
    /// </summary>
    [AppComponent("远程服务管理器", "维护远程服务的信息")]
    class RemoteServiceManagerAppComponent : TimerAppComponentBase
    {
        public RemoteServiceManagerAppComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;

        protected override void OnExecuteTask(string taskName)
        {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Contracts.Service;
using Common.Package.Service;

namespace ServiceFairy.Service.Session.Components
{
    /// <summary>
    /// 会话缓存管理器
    /// </summary>
    [AppComponent("会话缓存管理器", "存储会话数据")]
    class SessionCacheManagerAppComponent : TimerAppComponentBase
    {
        public SessionCacheManagerAppComponent(Service service)
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

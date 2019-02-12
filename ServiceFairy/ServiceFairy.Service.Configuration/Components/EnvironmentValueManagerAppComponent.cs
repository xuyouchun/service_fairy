using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities;

namespace ServiceFairy.Service.Configuration.Components
{
    /// <summary>
    /// 环境变量管理器
    /// </summary>
    [AppComponent("环境变量管理器", "下载并缓存环境变量")]
    class EnvironmentValueManagerAppComponent : TimerAppComponentBase
    {
        public EnvironmentValueManagerAppComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;

        /// <summary>
        /// 获取所有的环境变量
        /// </summary>
        /// <param name="lastUpdate">最后更新时间</param>
        /// <returns></returns>
        public EnvironmentValue[] GetAll(out DateTime lastUpdate)
        {
            lastUpdate = default(DateTime);
            return Array<EnvironmentValue>.Empty;
        }

        protected override void OnExecuteTask(string taskName)
        {
            
        }
    }
}

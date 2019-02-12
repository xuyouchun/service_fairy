using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;

namespace ServiceFairy.Service.Log.Components
{
    /// <summary>
    /// 日志分析器
    /// </summary>
    [AppComponent("日志分析器", "聚合并分析海量日志，提供有价值的信息")]
    class LogAnalyzerAppComponent : AppComponent
    {
        public LogAnalyzerAppComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;
    }
}

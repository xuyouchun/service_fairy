using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.File.UnionFile;

namespace ServiceFairy.Service.File.Components
{
    /// <summary>
    /// 文件扫描器
    /// </summary>
    [AppComponent("文件扫描器", "将文件同步到其它的服务器作为备份，并删除已标记为删除状态的文件")]
    class FileScanerAppComponent : TimerAppComponentBase
    {
        public FileScanerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;
            _cluster = service.FileSystemManager.FileCluster;
        }

        private readonly Service _service;
        private readonly IUnionFileCluster _cluster;

        protected override void OnExecuteTask(string taskName)
        {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Contracts.Service;
using Common.Package.Service;

namespace ServiceFairy.Service.FileCenter.Components
{
    /// <summary>
    /// 文件信息管理器
    /// </summary>
    [AppComponent("文件信息管理器", "记录文件的信息及存储位置")]
    class FileInfoManagerAppComponent : TimerAppComponentBase
    {
        public FileInfoManagerAppComponent(Service service)
            : base(service)
        {

        }

        protected override void OnExecuteTask(string taskName)
        {
            
        }
    }
}

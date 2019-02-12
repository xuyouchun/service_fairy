using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Database;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Database
{
    /// <summary>
    /// 初始化表的元数据
    /// </summary>
    [AppCommand("InitMetadata", "初始化表的元数据", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class InitMetadataAppCommand : ACS<Service>.Action<Database_InitMetadata_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Database_InitMetadata_Request req, ref ServiceResult sr)
        {
            context.Service.DbManager.InitMetadata(req.ReviseInfos);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.File;

namespace ServiceFairy.Service.File
{
    /// <summary>
    /// 删除文件
    /// </summary>
    [AppCommand("Delete", title: "删除文件", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class DeleteAppCommand : ACS<Service>.Action<File_Delete_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, File_Delete_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            srv.FileSystemManager.Delete(req.Paths);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.File;

namespace ServiceFairy.Service.File
{
    /// <summary>
    /// 结束下载
    /// </summary>
    [AppCommand("End", title: "结束上传或下载的事务", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class EndAppCommand : ACS<Service>.Action<File_End_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, File_End_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            srv.FileSystemManager.End(req.Tokens);
        }
    }
}

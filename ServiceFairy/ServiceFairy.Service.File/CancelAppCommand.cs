using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.File;

namespace ServiceFairy.Service.File
{
    /// <summary>
    /// 取消上传或下载
    /// </summary>
    [AppCommand("Cancel", title: "取消上传或下载", desc: "如果是下载，则其效果与End方法相同", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class CancelAppCommand : ACS<Service>.Action<File_Cancel_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, File_Cancel_Request req, ref ServiceResult sr)
        {
            Service srv = context.Service;
            srv.FileSystemManager.Cancel(req.Tokens);
        }
    }
}

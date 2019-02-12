using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.File;

namespace ServiceFairy.Service.File
{
    /// <summary>
    /// 一次上传全部文件内容
    /// </summary>
    [AppCommand("UploadAll", "一次性上传全部文件内容", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class UploadAllAppCommand : ACS<Service>.Action<File_UploadAll_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, File_UploadAll_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;

            srv.FileSystemManager.UploadAll(req.Path, req.Buffer);
        }
    }
}

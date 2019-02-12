using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.File;

namespace ServiceFairy.Service.File
{
    /// <summary>
    /// 开始上传
    /// </summary>
    [AppCommand("BeginUpload", title: "开始上传文件", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class BeginUploadAppCommand : ACS<Service>.Func<File_BeginUpload_Request, File_BeginUpload_Reply>
    {
        protected override File_BeginUpload_Reply OnExecute(AppCommandExecuteContext<Service> context, File_BeginUpload_Request req, ref ServiceResult sr)
        {
            Service srv = context.Service;
            string token = srv.FileSystemManager.BeginUpload(req.Path);
            return new File_BeginUpload_Reply() { Token = token };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.File;

namespace ServiceFairy.Service.File
{
    /// <summary>
    /// 直接上传文件
    /// </summary>
    [AppCommand("UploadDirect", title: "直接上传文件", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class UploadDirectAppCommand : ACS<Service>.Func<File_UploadDirect_Request, File_UploadDirect_Reply>
    {
        protected override File_UploadDirect_Reply OnExecute(AppCommandExecuteContext<Service> context, File_UploadDirect_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            var fs = srv.FileSystemManager;

            string token = fs.BeginUpload(req.Path);
            fs.Upload(token, req.Buffer);

            if (req.AtEnd)
                fs.End(token);

            return new File_UploadDirect_Reply() { Token = token };
        }
    }
}

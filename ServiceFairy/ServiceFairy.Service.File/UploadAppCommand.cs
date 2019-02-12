using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.File;

namespace ServiceFairy.Service.File
{
    /// <summary>
    /// 上传文件
    /// </summary>
    [AppCommand("Upload", title: "上传文件", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class UploadAppCommand : ACS<Service>.Func<File_Upload_Request, File_Upload_Reply>
    {
        protected override File_Upload_Reply OnExecute(AppCommandExecuteContext<Service> context, File_Upload_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            srv.FileSystemManager.Upload(req.Token, req.Buffer);

            if (req.AtEnd)
                srv.FileSystemManager.End(req.Token);

            return new File_Upload_Reply();
        }
    }
}

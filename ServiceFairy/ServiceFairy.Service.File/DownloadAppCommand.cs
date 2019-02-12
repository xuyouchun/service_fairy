using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.File;
using Common.Package;

namespace ServiceFairy.Service.File
{
    /// <summary>
    /// 下载文件
    /// </summary>
    [AppCommand("Download", title: "下载文件", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class DownloadAppCommand : ACS<Service>.Func<File_Download_Request, File_Download_Reply>
    {
        protected override File_Download_Reply OnExecute(AppCommandExecuteContext<Service> context, File_Download_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            bool atEnd;
            byte[] buffer = srv.FileSystemManager.Download(req.Token, req.MaxSize, out atEnd);

            if (!atEnd)
                sr = new ServiceResult(SuccessCode.Continue);

            return new File_Download_Reply() { Buffer = buffer };
        }
    }
}

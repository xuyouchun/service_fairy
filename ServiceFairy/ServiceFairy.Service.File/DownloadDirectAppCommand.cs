using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.File;
using Common.File.UnionFile;

namespace ServiceFairy.Service.File
{
    /// <summary>
    /// 直接下载文件
    /// </summary>
    [AppCommand("DownloadDirect", title: "直接下载文件", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class DownloadDirectAppCommand : ACS<Service>.Func<File_DownloadDirect_Request, File_DownloadDirect_Reply>
    {
        protected override File_DownloadDirect_Reply OnExecute(AppCommandExecuteContext<Service> context, File_DownloadDirect_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            var fs = srv.FileSystemManager;

            UnionFileInfo info;
            string token = fs.BeginDownload(req.Path, out info);

            bool atEnd;
            byte[] buffer = fs.Download(token, req.MaxSize, out atEnd);

            if (!atEnd)
                sr = new ServiceResult(SuccessCode.Continue);

            return new File_DownloadDirect_Reply() { Bytes = buffer, FileInfo = info, Token = token };
        }
    }
}

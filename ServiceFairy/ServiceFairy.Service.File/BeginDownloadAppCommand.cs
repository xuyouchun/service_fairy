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
    /// 开始下载文件
    /// </summary>
    [AppCommand("BeginDownload", title: "开始下载文件", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class BeginDownloadAppCommand : ACS<Service>.Func<File_BeginDownload_Request, File_BeginDownload_Reply>
    {
        protected override File_BeginDownload_Reply OnExecute(AppCommandExecuteContext<Service> context, File_BeginDownload_Request req, ref ServiceResult sr)
        {
            Service srv = context.Service;
            UnionFileInfo fileInfo;
            string token = srv.FileSystemManager.BeginDownload(req.Path, out fileInfo);
            return new File_BeginDownload_Reply() { Token = token, FileInfo = fileInfo };
        }
    }
}

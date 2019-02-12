using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.File;
using Common.File.UnionFile;

namespace ServiceFairy.Service.File
{
    [AppCommand("DownloadAll", title: "下载文件的全部内容", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class DownloadAllAppCommand : ACS<Service>.Func<File_DownloadAll_Request, File_DownloadAll_Reply>
    {
        protected override File_DownloadAll_Reply OnExecute(AppCommandExecuteContext<Service> context, File_DownloadAll_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            UnionFileInfo fInfo;
            byte[] buffer = srv.FileSystemManager.DownloadAll(req.Path, out fInfo);

            return new File_DownloadAll_Reply() { Buffer = buffer, FileInfo = fInfo };
        }
    }
}

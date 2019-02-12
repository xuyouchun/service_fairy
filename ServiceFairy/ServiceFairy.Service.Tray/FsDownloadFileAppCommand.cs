using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Tray;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Tray
{
    /// <summary>
    /// 下载文件系统中的文件
    /// </summary>
    [AppCommand("FsDownloadFile", "下载文件系统中的文件", SecurityLevel = SecurityLevel.Admin)]
    class FsDownloadFileAppCommand : ACS<Service>.Func<Tray_FsDownloadFile_Request, Tray_FsDownloadFile_Reply>
    {
        protected override Tray_FsDownloadFile_Reply OnExecute(AppCommandExecuteContext<Service> context, Tray_FsDownloadFile_Request req, ref ServiceResult sr)
        {
            Service srv = context.Service;
            byte[] content = srv.FileSystem.ReadAllBytes(req.Path);
            FsFileInfo info = srv.FileSystem.GetFileInfo(req.Path);

            return new Tray_FsDownloadFile_Reply() { Content = content, Info = info };
        }
    }
}

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
    /// 开始下载StreamTable
    /// </summary>
    [AppCommand("BeginDownloadStreamTable", title: "开始下载StreamTable", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class BeginDownloadStreamTableAppCommand : ACS<Service>.Func<File_BeginDownloadStreamTable_Request, File_BeginDownloadStreamTable_Reply>
    {
        protected override File_BeginDownloadStreamTable_Reply OnExecute(AppCommandExecuteContext<Service> context, File_BeginDownloadStreamTable_Request req, ref ServiceResult sr)
        {
            Service srv = context.Service;

            StreamTableBasicInfo basicInfo;
            UnionFileInfo fileInfo;
            string token = srv.StreamTableManager.BeginDownload(req.Path, out basicInfo, out fileInfo);
            return new File_BeginDownloadStreamTable_Reply() { Token = token, BasicInfo = basicInfo, FileInfo = fileInfo };
        }
    }
}

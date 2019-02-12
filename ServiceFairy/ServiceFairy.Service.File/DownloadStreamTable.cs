using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.File;

namespace ServiceFairy.Service.File
{
    /// <summary>
    /// 下载StreamTable
    /// </summary>
    [AppCommand("DownloadStreamTable", title: "下载StreamTable", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class DownloadStreamTableAppCommand : ACS<Service>.Func<File_DownloadStreamTable_Request, File_DownloadStreamTable_Reply>
    {
        protected override File_DownloadStreamTable_Reply OnExecute(AppCommandExecuteContext<Service> context, File_DownloadStreamTable_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            StreamTableRowData[] rows = srv.StreamTableManager.Download(req.Token, req.Table, req.Start, req.Count);
            return new File_DownloadStreamTable_Reply() { Rows = rows };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.File;
using Common.Utility;

namespace ServiceFairy.Service.File
{
    /// <summary>
    /// 上传StreamTable
    /// </summary>
    [AppCommand("UploadStreamTable", title: "上传StreamTable", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class UploadStreamTableAppCommand : ACS<Service>.Action<File_UploadStreamTable_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, File_UploadStreamTable_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            var stm = srv.StreamTableManager;

            if (req.TableInfo != null)
                stm.CreateNewTable(req.Token, req.TableInfo);

            if (!req.Rows.IsNullOrEmpty())
                stm.Upload(req.Token, req.Rows);

            if (req.AtEnd)
                stm.EndUpload(req.Token);
        }
    }
}

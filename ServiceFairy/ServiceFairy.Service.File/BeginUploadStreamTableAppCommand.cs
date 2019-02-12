using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.File;

namespace ServiceFairy.Service.File
{
    /// <summary>
    /// 开始上传StreamTable
    /// </summary>
    [AppCommand("BeginUploadStreamTable", title: "开始上传StreamTable", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class BeginUploadStreamTableAppCommand : ACS<Service>.Func<File_BeginUploadStreamTable_Request, File_BeginUploadStreamTable_Reply>
    {
        protected override File_BeginUploadStreamTable_Reply OnExecute(AppCommandExecuteContext<Service> context, File_BeginUploadStreamTable_Request req, ref ServiceResult sr)
        {
            Service srv = context.Service;

            string token = srv.StreamTableManager.BeginUpload(req.Path, req.Name, req.Desc);
            if (req.TableInfo != null)
                srv.StreamTableManager.CreateNewTable(token, req.TableInfo);

            return new File_BeginUploadStreamTable_Reply() { Token = token };
        }
    }
}

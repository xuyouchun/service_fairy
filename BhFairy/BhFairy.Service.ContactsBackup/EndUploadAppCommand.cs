using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using BhFairy.Entities.ContactsBackup;
using ServiceFairy;
using Common.Contracts;

namespace BhFairy.Service.ContactsBackup
{
    /// <summary>
    /// 结束上传通信录
    /// </summary>
    [AppCommand("EndUpload", title: "结束上传通信录数据", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class EndUploadAppCommand : ACS<Service>.Action<ContactsBackup_EndUpload_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, ContactsBackup_EndUpload_Request req, ref ServiceResult sr)
        {
            Service srv = context.Service;
            srv.ContactsBackup.EndUpload(context.GetSessionState(), req.Token);
        }

        const string Remarks = @"上传完毕，需要调用该接口通知服务器，删除与该备份事务相关的数据。";
    }
}

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
    /// 暂停上传
    /// </summary>
    [AppCommand("PauseUpload", title: "暂停上传", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class PauseUploadAppCommand : ACS<Service>.Action<ContactsBackup_PauseUpload_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, ContactsBackup_PauseUpload_Request req, ref ServiceResult sr)
        {
            Service srv = context.Service;
            srv.ContactsBackup.PauseUpload(context.GetSessionState(), req.Token);
        }

        const string Remarks = @"如果中途暂停上传，需要通知服务器暂时不要清理与该上传事务相关的数据。";
    }
}

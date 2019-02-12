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
    /// 结束下载通信录
    /// </summary>
    [AppCommand("EndDownload", "结束下载通信录", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class EndDownloadAppCommand : ACS<Service>.Action<ContactsBackup_EndDownload_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, ContactsBackup_EndDownload_Request req, ref ServiceResult sr)
        {
            Service service = context.Service;
            service.ContactsBackup.EndDownload(context.GetSessionState(), req.Token);
        }

        const string Remarks = @"下载结束时，需要调用该接口通知服务器，删除与该下载事务相关的数据。";
    }
}

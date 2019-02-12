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
    /// 取消下载
    /// </summary>
    [AppCommand("CancelDownload", title: "取消下载", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class CancelDownloadAppCommand : ACS<Service>.Action<ContactsBackup_CancelDownload_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, ContactsBackup_CancelDownload_Request req, ref ServiceResult sr)
        {
            Service srv = context.Service;
            srv.ContactsBackup.CancelDownload(context.GetSessionState(), req.Token);
        }

        private const string Remarks = @"如果中途停止下载，需要调用一下该接口，通知服务器端及时清除与该下载事务相关的数据。";
    }
}

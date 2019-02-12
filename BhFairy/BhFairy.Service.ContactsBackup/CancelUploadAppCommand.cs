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
    /// 取消上传
    /// </summary>
    [AppCommand("CancelUpload", title: "取消上传通信录", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class CancelUploadAppCommand : ACS<Service>.Action<ContactsBackup_CancelUpload_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, ContactsBackup_CancelUpload_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            srv.ContactsBackup.CancelUpload(context.GetSessionState(), req.Token);
        }

        const string Remarks = @"如果中途停止备份通信录，需要调用一下该接口，通知服务器及时清除与该事务相关的数据。";
    }
}

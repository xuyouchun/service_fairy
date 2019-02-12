using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BhFairy.Entities.ContactsBackup;
using ServiceFairy;
using Common.Contracts.Service;
using Common.Contracts;

namespace BhFairy.Service.ContactsBackup
{
    /// <summary>
    /// 开始备份通信录
    /// </summary>
    [AppCommand("BeginUpload", title: "开始备份通信录", desc: "", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class BeginUploadAppCommand : ACS<Service>.Func<ContactsBackup_BeginUpload_Request, ContactsBackup_BeginUpload_Reply>
    {
        protected override ContactsBackup_BeginUpload_Reply OnExecute(AppCommandExecuteContext<Service> context, ContactsBackup_BeginUpload_Request req, ref ServiceResult sr)
        {
            Service srv = context.Service;
            string token = srv.ContactsBackup.BeginUpload(context.GetSessionState(), req.ColumnHeaders);

            return new ContactsBackup_BeginUpload_Reply() { Token = token };
        }

        const string Remarks = @"这是备份通信录的第一步，需要提供列头，返回一个事务标识，随后调用Upload逐步上传时需要携带该事务标识。";
    }
}

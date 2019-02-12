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
    /// 备份通信录
    /// </summary>
    [AppCommand("Upload", title: "备份通信录", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class UploadAppCommand : ACS<Service>.Action<ContactsBackup_Upload_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, ContactsBackup_Upload_Request req, ref ServiceResult sr)
        {
            Service srv = context.Service;
            srv.ContactsBackup.Upload(context.GetSessionState(), req.Token, req.Contacts);
        }

        const string Remarks = @"需要先调用BeginUpload接口获得一个事务标识，然后调用该接口携带事务标识分多次上传通信录。";
    }
}

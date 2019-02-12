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
    /// 下载通信录备份数据
    /// </summary>
    [AppCommand("Download", title: "下载通信录备份数据", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class DownloadAppCommand : ACS<Service>.Func<ContactsBackup_Download_Request, ContactsBackup_Download_Reply>
    {
        protected override ContactsBackup_Download_Reply OnExecute(AppCommandExecuteContext<Service> context, ContactsBackup_Download_Request req, ref ServiceResult sr)
        {
            Service srv = context.Service;
            CbContact[] contacts = srv.ContactsBackup.Download(context.GetSessionState(), req.Token, req.Start, req.Count);

            return new ContactsBackup_Download_Reply() { Contacts = contacts };
        }

        const string Remarks = @"需要先调用BeginDownload接口获取一个事务标识，然后调用该接口分多次下载通信录。";
    }
}

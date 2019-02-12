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
    /// 获取通信录备份列表
    /// </summary>
    [AppCommand("GetList", title: "获取通信录备份列表", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class GetListAppCommand : ACS<Service>.Func<ContactsBackup_GetList_Request, ContactsBackup_GetList_Reply>
    {
        protected override ContactsBackup_GetList_Reply OnExecute(AppCommandExecuteContext<Service> context, ContactsBackup_GetList_Request req, ref ServiceResult sr)
        {
            Service srv = context.Service;
            ContactBackupInfo[] infos = srv.ContactsBackup.GetList(context.GetSessionState());

            return new ContactsBackup_GetList_Reply() { Infos = infos };
        }

        const string Remarks = @"获取已经备份的通信录列表。";
    }
}

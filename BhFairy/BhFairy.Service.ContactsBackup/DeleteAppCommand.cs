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
    /// 删除联系人备份数据
    /// </summary>
    [AppCommand("Delete", title: "删除通信录备份数据", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class DeleteAppCommand : ACS<Service>.Action<ContactsBackup_Delete_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, ContactsBackup_Delete_Request req, ref ServiceResult sr)
        {
            Service srv = context.Service;
            srv.ContactsBackup.Delete(context.GetSessionState(), req.Names);
        }

        const string Remarks = @"删除通信录备份数据，需要指定其名称，通信录的名称可以调用GetList接口获得。";
    }
}

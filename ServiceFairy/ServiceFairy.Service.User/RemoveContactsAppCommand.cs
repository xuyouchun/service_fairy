using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.User;
using Common.Contracts.Service;
using Common.Contracts;

namespace ServiceFairy.Service.User
{
    /// <summary>
    /// 批量删除联系人
    /// </summary>
    [AppCommand("RemoveContacts", "批量删除联系人", SecurityLevel = SecurityLevel.User), Remarks(Remarks), DisabledCommand]
    class RemoveContactsAppCommand : ACS<Service>.Action<User_RemoveContacts_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, User_RemoveContacts_Request req, ref ServiceResult sr)
        {
            context.Service.ContactListManager.RemoveContacts(context.GetSessionState(), req.UserNames);
        }

        const string Remarks = @"删除联系人，将自动取消对其的关注";
    }
}

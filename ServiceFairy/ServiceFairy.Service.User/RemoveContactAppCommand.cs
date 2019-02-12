using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Contracts.Service;
using ServiceFairy.Entities.User;

namespace ServiceFairy.Service.User
{
    /// <summary>
    /// 删除联系人
    /// </summary>
    [AppCommand("RemoveContact", "删除联系人", SecurityLevel = SecurityLevel.User), Remarks(Remarks), DisabledCommand]
    class RemoveContactAppCommand : ACS<Service>.Action<User_RemoveContact_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, User_RemoveContact_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            context.Service.ContactListManager.RemoveContact(context.GetSessionState(), request.UserName);
        }

        private const string Remarks = @"删除联系人，将自动取消对其的关注";
    }
}

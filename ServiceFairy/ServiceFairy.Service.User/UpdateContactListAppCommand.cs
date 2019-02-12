using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.User;
using Common.Contracts;

namespace ServiceFairy.Service.User
{
    /// <summary>
    /// 更新通信录
    /// </summary>
    [AppCommand("UpdateContactList", "更新通信录", SecurityLevel = SecurityLevel.User), Remarks(Remarks), DisabledCommand]
    class UpdateContactListAppCommand : ACS<Service>.Action<User_UpdateContactList_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, User_UpdateContactList_Request req, ref ServiceResult sr)
        {
            context.Service.ContactListManager.UpdateContactList(context.GetSessionState(), req.UserNames);
        }

        private const string Remarks = @"使用指定的通信录替换已经存在的通信录";
    }
}

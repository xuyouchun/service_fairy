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
    /// 添加联系人
    /// </summary>
    [AppCommand("AddContact", "添加联系人", SecurityLevel = SecurityLevel.User), Remarks(Remarks), DisabledCommand]
    class AddContactAppCommand : ACS<Service>.Func<User_AddContact_Request, User_AddContact_Reply>
    {
        protected override User_AddContact_Reply OnExecute(AppCommandExecuteContext<Service> context, User_AddContact_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            int userId = context.Service.ContactListManager.AddContact(context.GetSessionState(), request.UserName);
            return new User_AddContact_Reply { UserId = userId };
        }

        const string Remarks = @"将联系人增量同步到服务器端，所添加的联系人将自动关注，若该联系人尚未注册，则在其注册时，我将收到“System.User/NewUser”通知。
注意：UserId不需要填写";
    }
}

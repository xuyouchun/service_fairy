using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.User;
using Common.Contracts.Service;
using Common.Contracts;
using Common;

namespace ServiceFairy.Service.User
{
    /// <summary>
    /// 获取联系人列表
    /// </summary>
    [AppCommand("GetContactList", "获取联系人列表", SecurityLevel = SecurityLevel.User), Remarks(Remarks), DisabledCommand]
    class GetContactListAppCommand : ACS<Service>.Func<User_GetContactList_Reply>
    {
        protected override User_GetContactList_Reply OnExecute(AppCommandExecuteContext<Service> context, ref ServiceResult sr)
        {
            /*
            Contact[] contacts = context.Service.ContactListManager.SafeGetContactList(context.GetSessionState());
            return new User_GetContactList_Reply { Contacts = contacts };*/

            return null;
        }

        const string Remarks = @"将服务器端所保存的联系人列表下载下来。当本地联系人列表丢失或者换了手机时，需要用于该接口。";
    }
}

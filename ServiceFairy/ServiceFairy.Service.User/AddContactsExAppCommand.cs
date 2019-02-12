using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.User;
using Common.Contracts.Service;
using Common.Utility;
using System.Diagnostics.Contracts;

namespace ServiceFairy.Service.User
{
    /*
    /// <summary>
    /// 添加联系人
    /// </summary>
    [AppCommand("AddContactsEx", "批量添加联系人"), DisabledCommand]
    class AddContactsExAppCommand : ACS<Service>.Func<User_AddContactsEx_Request, User_AddContactsEx_Reply>
    {
        protected override User_AddContactsEx_Reply OnExecute(AppCommandExecuteContext<Service> context, User_AddContactsEx_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            string[] usernames = request.UserNames.Keys.ToArray();
            int[] userIds = context.Service.ContactListManager.AddContacts(context.GetSessionState(), usernames);

            Contract.Assert(usernames.Length == userIds.Length);

            Dictionary<int, int> dict = new Dictionary<int, int>();
            for (int k = 0; k < userIds.Length; k++)
            {
                int userId = userIds[k];
                string username = usernames[k];

                if (userId != 0)
                    dict[userId] = request.UserNames[username];
            }

            return new User_AddContactsEx_Reply { UserIds = dict };
        }
    }*/
}

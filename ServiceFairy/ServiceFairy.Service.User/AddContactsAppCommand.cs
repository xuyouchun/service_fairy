using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.User;
using System.Diagnostics.Contracts;

namespace ServiceFairy.Service.User
{
    /// <summary>
    /// 添加联系人
    /// </summary>
    [AppCommand("AddContacts", "批量添加联系人")]
    class AddContactsAppCommand : ACS<Service>.Func<User_AddContacts_Request, User_AddContacts_Reply>
    {
        protected override User_AddContacts_Reply OnExecute(AppCommandExecuteContext<Service> context, User_AddContacts_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            string[] usernames, ids;
            _Split(request.UserNames, out usernames, out ids);
            int[] userIds = context.Service.ContactListManager.AddContacts(context.GetSessionState(), usernames);

            Contract.Assert(usernames.Length == userIds.Length);

            List<string> r = new List<string>();
            for (int k = 0; k < userIds.Length; k++)
            {
                int userId = userIds[k];
                if (userId != 0)
                {
                    r.Add(userId.ToString());
                    r.Add(ids[k]);
                }
            }

            return new User_AddContacts_Reply { UserIds = r.ToArray() };
        }

        private void _Split(string[] values, out string[] usernames, out string[] ids)
        {
            List<string> unArr = new List<string>(), idArr = new List<string>();
            int length = values.Length & ~0x01;
            for (int k = 0; k < length; k += 2)
            {
                unArr.Add(values[k]);
                idArr.Add(values[k + 1]);
            }

            usernames = unArr.ToArray();
            ids = idArr.ToArray();
        }
    }

    /*
    /// <summary>
    /// 添加联系人
    /// </summary>
    [AppCommand("AddContacts", "批量添加联系人", Remarks, SecurityLevel = SecurityLevel.User)]
    class AddContactsAppCommand : ACS<Service>.Func<User_AddContacts_Request, User_AddContacts_Reply>
    {
        protected override User_AddContacts_Reply OnExecute(AppCommandExecuteContext<Service> context, User_AddContacts_Request req, ref ServiceResult sr)
        {
            int[] userIds = context.Service.ContactListManager.AddContacts(context.GetSessionState(), req.UserNames);
            return new User_AddContacts_Reply { UserIds = userIds };
        }

        const string Remarks = @"将联系人增量同步到服务器端，所添加的联系人将自动关注，若该联系人尚未注册，则在其注册时，我将收到“System.User/NewUser”通知。
注意：UserId不需要填写。";
    }*/
}

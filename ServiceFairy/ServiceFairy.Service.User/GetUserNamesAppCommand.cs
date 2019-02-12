using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.User;
using Common.Contracts.Service;

namespace ServiceFairy.Service.User
{
    /// <summary>
    /// 根据用户ID批量获取用户名
    /// </summary>
    [AppCommand("GetUserNames", "根据用户ID批量获取用户名", SecurityLevel = SecurityLevel.Public), DisabledCommand]
    class GetUserNamesAppCommand : ACS<Service>.Func<User_GetUserNames_Request, User_GetUserNames_Reply>
    {
        protected override User_GetUserNames_Reply OnExecute(AppCommandExecuteContext<Service> context, User_GetUserNames_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            string[] usernames = context.Service.UserParser.ConvertUserIdToNames(request.UserIds);
            return new User_GetUserNames_Reply { UserNames = usernames };
        }
    }
}

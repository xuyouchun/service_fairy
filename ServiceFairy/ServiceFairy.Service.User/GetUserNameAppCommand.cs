using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.User;
using Common.Contracts.Service;

namespace ServiceFairy.Service.User
{
    /// <summary>
    /// 根据用户ID获取用户名
    /// </summary>
    [AppCommand("GetUserName", "根据用户ID获取用户名", SecurityLevel = SecurityLevel.Public), DisabledCommand]
    class GetUserNameAppCommand : ACS<Service>.Func<User_GetUserName_Request, User_GetUserName_Reply>
    {
        protected override User_GetUserName_Reply OnExecute(AppCommandExecuteContext<Service> context, User_GetUserName_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            string username = context.Service.UserParser.ConvertUserIdToName(request.UserId);
            return new User_GetUserName_Reply { UserName = username };
        }
    }
}

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
    /// 将UserName转换为UserId
    /// </summary>
    [AppCommand("GetUserId", "将UserName转换为UserId", SecurityLevel = SecurityLevel.Public), Remarks(Remarks), DisabledCommand]
    class GetUserIdAppCommand : ACS<Service>.Func<User_GetUserId_Request, User_GetUserId_Reply>
    {
        protected override User_GetUserId_Reply OnExecute(AppCommandExecuteContext<Service> context, User_GetUserId_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            int userId = context.Service.UserParser.ConvertUserNameToId(request.UserName);
            return new User_GetUserId_Reply { UserId = userId };
        }

        const string Remarks = @"返回值为一个整型数字，如果该UserName尚未注册，则返回0。
该接口的调用不需要在登录状态下进行。";
    }
}

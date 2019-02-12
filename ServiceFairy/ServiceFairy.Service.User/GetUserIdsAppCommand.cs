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
    /// 将UserName批量转换为UserId
    /// </summary>
    [AppCommand("GetUserIds", "将UserName批量转换为UserId", SecurityLevel = SecurityLevel.Public), Remarks(Remarks), DisabledCommand]
    class GetUserIdsAppCommand : ACS<Service>.Func<User_GetUserIds_Request, User_GetUserIds_Reply>
    {
        protected override User_GetUserIds_Reply OnExecute(AppCommandExecuteContext<Service> context, User_GetUserIds_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            int[] userIds = context.Service.UserParser.ConvertUserNameToIds(request.UserNames);
            return new User_GetUserIds_Reply { UserIds = userIds };
        }

        const string Remarks = @"返回值为一个整型数组，与输入参数中的UserNames数组一一对应，如果该UserName尚未注册，则相应的UserId对应为0。
该接口的调用不需要在登录状态下进行。";
    }
}

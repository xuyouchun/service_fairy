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
    /// 获取当前用户的信息
    /// </summary>
    [AppCommand("GetMyInfo", "获取当前用户的信息", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class GetMyInfoAppCommand : ACS<Service>.Func<User_GetMyInfo_Reply>
    {
        protected override User_GetMyInfo_Reply OnExecute(AppCommandExecuteContext<Service> context, ref ServiceResult sr)
        {
            int userId = context.GetUserId();
            UserInfo info = context.Service.UserAccountManager.GetUserInfo(userId);
            return new User_GetMyInfo_Reply { Info = info };
        }

        const string Remarks = @"获取当前登录用户的详细信息";
    }
}

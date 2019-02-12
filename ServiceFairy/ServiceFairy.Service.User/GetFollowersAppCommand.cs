using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.User;
using Common.Contracts.Service;
using Common.Contracts;

namespace ServiceFairy.Service.User
{
    /// <summary>
    /// 获取粉丝列表
    /// </summary>
    [AppCommand("GetFollowers", "获取粉丝列表", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class GetFollowersAppCommand : ACS<Service>.Func<User_GetFollowers_Reply>
    {
        protected override User_GetFollowers_Reply OnExecute(AppCommandExecuteContext<Service> context, ref ServiceResult sr)
        {
            int[] userIds = context.Service.UserParser.Parse(Users.FromMyFollowers(), context.GetSessionState());
            return new User_GetFollowers_Reply() { UserIds = userIds };
        }

        const string Remarks = @"获取我的所有粉丝，当需要查看我的手机在哪些人的手机通讯录中存在时可使用。";
    }
}

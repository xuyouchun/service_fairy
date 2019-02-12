using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.UserCenter;
using Common.Contracts.Service;

namespace ServiceFairy.Service.UserCenter
{
    /// <summary>
    /// 将用户集合转换为用户ID
    /// </summary>
    [AppCommand("ParseUsers", "将用户集合转换为用户ID", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class ParseUsersAppCommand : ACS<Service>.Func<UserCenter_ParseUsers_Request, UserCenter_ParseUsers_Reply>
    {
        protected override UserCenter_ParseUsers_Reply OnExecute(AppCommandExecuteContext<Service> context, UserCenter_ParseUsers_Request req, ref ServiceResult sr)
        {
            int[] userIds = context.Service.UserCollectionParser.Parse(req.Users);
            return new UserCenter_ParseUsers_Reply() { UserIds = userIds };
        }
    }
}

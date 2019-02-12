using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.UserCenter;

namespace ServiceFairy.Service.UserCenter
{
    /// <summary>
    /// 将用户名转换为用户ID
    /// </summary>
    [AppCommand("ConvertUserNameToIds", "将用户名转换为用户ID", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class ConvertUserNameToIdsAppCommand : ACS<Service>.Func<UserCenter_ConvertUserNameToIds_Request, UserCenter_ConvertUserNameToIds_Reply>
    {
        protected override UserCenter_ConvertUserNameToIds_Reply OnExecute(AppCommandExecuteContext<Service> context, UserCenter_ConvertUserNameToIds_Request req, ref ServiceResult sr)
        {
            int[] userIds = context.Service.UserCollectionParser.ConvertUserNames(req.UserNames);

            return new UserCenter_ConvertUserNameToIds_Reply() { UserIds = userIds };
        }
    }
}

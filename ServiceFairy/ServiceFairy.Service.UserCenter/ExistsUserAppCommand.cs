using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.UserCenter;
using Common.Utility;

namespace ServiceFairy.Service.UserCenter
{
    /// <summary>
    /// 判断指定的用户ID是否在该终端上注册
    /// </summary>
    [AppCommand("ExistsUser", "判断指定的用户ID是否在该终端上注册", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class ExistsUserAppCommand : ACS<Service>.Func<UserCenter_ExistsUser_Request, UserCenter_ExistsUser_Reply>
    {
        protected override UserCenter_ExistsUser_Reply OnExecute(AppCommandExecuteContext<Service> context, UserCenter_ExistsUser_Request req, ref ServiceResult sr)
        {
            int[] existsUserIds = context.Service.UserConnectionManager.GetUserConnectionInfos(req.UserIds).ToArray(info => info.UserId);
            return new UserCenter_ExistsUser_Reply() { ExistsUserIds = existsUserIds };
        }
    }
}

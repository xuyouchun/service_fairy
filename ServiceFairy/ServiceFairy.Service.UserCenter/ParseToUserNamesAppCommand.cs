using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.UserCenter;
using Common.Contracts.Service;

namespace ServiceFairy.Service.UserCenter
{
    /// <summary>
    /// 解析为用户名
    /// </summary>
    [AppCommand("ParseToUserNames", "解析为用户名", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class ParseToUserNamesAppCommand : ACS<Service>.Func<UserCenter_ParseToUserNames_Request, UserCenter_ParseToUserNames_Reply>
    {
        protected override UserCenter_ParseToUserNames_Reply OnExecute(AppCommandExecuteContext<Service> context, UserCenter_ParseToUserNames_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            string[] usernames = context.Service.UserCollectionParser.ParseToUserNames(request.Users);
            return new UserCenter_ParseToUserNames_Reply() { UserNames = usernames };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.User;
using System.Runtime.Serialization;
using Common.Contracts.Service;
using Common.Contracts;
using Common.Utility;

namespace ServiceFairy.Service.User
{
    /// <summary>
    /// 获取关注列表
    /// </summary>
    [AppCommand("GetFollowings", "获取关注列表", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class GetFollowingsAppCommand : ACS<Service>.Func<User_GetFollowings_Request, User_GetFollowings_Reply>
    {
        protected override User_GetFollowings_Reply OnExecute(AppCommandExecuteContext<Service> context, User_GetFollowings_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            var srv = context.Service;
            UserSessionState uss = context.GetSessionState();

            int[] userIds;
            Users users = Users.FromFollowings(uss.BasicInfo.UserId);
            if (request.Since == default(DateTime))
            {
                userIds = srv.UserParser.Parse(users);
            }
            else
            {
                UserBasicInfo[] basicInfos = srv.UserManager.GetUserBasicInfos(users);
                userIds = basicInfos.Where(bi => bi.CreationTime >= request.Since).ToArray(bi => bi.UserId);
            }

            return new User_GetFollowings_Reply() { UserIds = userIds };
        }

        const string Remarks = @"获取我所关注的联系人，该接可用于确定我的手机里哪些联系人已经注册。
在通讯录中存在，但没有注册的联系人将不会被返回。";
    }
}

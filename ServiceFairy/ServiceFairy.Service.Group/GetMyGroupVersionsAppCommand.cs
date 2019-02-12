using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Group;
using Common.Contracts.Service;
using Common;

namespace ServiceFairy.Service.Group
{
    /// <summary>
    /// 获取我的所有群组版本号
    /// </summary>
    [AppCommand("GetMyGroupVersions", "获取我的所有群组版本号", SecurityLevel = SecurityLevel.User), NewCommand]
    class GetMyGroupVersionsAppCommand : ACS<Service>.Func<Group_GetMyGroupVersions_Reply>
    {
        protected override Group_GetMyGroupVersions_Reply OnExecute(AppCommandExecuteContext<Service> context, ref Common.Contracts.Service.ServiceResult sr)
        {
            int userId = context.GetUserId();
            var g = context.Service.GroupManager;
            int[] groupIds = g.GetUserGroups(userId);

            Dictionary<int, long> versions = new Dictionary<int, long>();
            foreach (GroupBasicInfo bi in g.GetGroupBasicInfos(groupIds) ?? Array<GroupBasicInfo>.Empty)
            {
                versions[bi.GroupId] = bi.CreateTime.Ticks;
            }

            return new Group_GetMyGroupVersions_Reply { Versions = versions };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Group;
using Common.Contracts.Service;
using Common;
using Common.Utility;

namespace ServiceFairy.Service.Group
{
    /// <summary>
    /// 获取群组的版本号
    /// </summary>
    [AppCommand("GetGroupVersions", "获取群组的版本号", SecurityLevel = SecurityLevel.User), DisabledCommand]
    class GetGroupVersionsAppCommand : ACS<Service>.Func<Group_GetGroupVersions_Request, Group_GetGroupVersions_Reply>
    {
        protected override Group_GetGroupVersions_Reply OnExecute(AppCommandExecuteContext<Service> context, Group_GetGroupVersions_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            GroupBasicInfo[] bis = context.Service.GroupManager.GetGroupBasicInfos(request.GroupIds);
            Dictionary<int, GroupBasicInfo> dict = (bis ?? Array<GroupBasicInfo>.Empty).ToDictionary(v => v.GroupId, ignoreDupKeys: true);

            long[] versions = request.GroupIds.ToArray(gid => {
                GroupBasicInfo bi = dict.GetOrDefault(gid);
                return (bi == null) ? 0 : bi.ChangedTime.Ticks;
            });

            return new Group_GetGroupVersions_Reply { Versions = versions };
        }
    }
}

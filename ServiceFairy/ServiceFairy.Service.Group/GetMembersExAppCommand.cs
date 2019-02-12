using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Group;
using Common.Contracts.Service;
using Common.Utility;
using ServiceFairy.Service.Group.Components;
using Common.Contracts;

namespace ServiceFairy.Service.Group
{
    /// <summary>
    /// 获取群组成员
    /// </summary>
    [AppCommand("GetGroupsMembers", "批量获取群组成员", SecurityLevel = SecurityLevel.User), Remarks(Remarks), DisabledCommand]
    class GetMembersExAppCommand : ACS<Service>.Func<Group_GetMembersEx_Request, Group_GetMembersEx_Reply>
    {
        protected override Group_GetMembersEx_Reply OnExecute(AppCommandExecuteContext<Service> context, Group_GetMembersEx_Request request, ref ServiceResult sr)
        {
            GroupAccountManagerAppComponent gMgr = context.Service.GroupAccountManager;
            UserSessionState uss = context.GetSessionState();
            HashSet<int> myGroupIds = context.Service.GroupManager.GetUserGroups(uss.BasicInfo.UserId).ToHashSet();

            int[] groupIds = request.GroupIds.Where(gId => myGroupIds.Contains(gId)).ToArray();
            IDictionary<int, int[]> gDict = gMgr.GetMembers(groupIds);

            return new Group_GetMembersEx_Reply() {
                Items = gDict.ToArray(g => new GroupMemberItem { GroupId = g.Key, Members = g.Value })
            };
        }

        const string Remarks = @"获取指定群组的成员。
要求用户必须为该群组的成员，才会有权限进行该操作";
    }
}

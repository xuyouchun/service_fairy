using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Group;
using Common.Contracts.Service;
using Common.Contracts;
using Common;

namespace ServiceFairy.Service.Group
{
    /// <summary>
    /// 获取我的群组信息
    /// </summary>
    [AppCommand("GetMyGroupInfosEx", "获取我的群组信息", SecurityLevel = SecurityLevel.User), Remarks(Remarks), NewCommand]
    class GetMyGroupInfosExAppCommand : ACS<Service>.Func<Group_GetMyGroupInfosEx_Request, Group_GetMyGroupInfosEx_Reply>
    {
        protected override Group_GetMyGroupInfosEx_Reply OnExecute(AppCommandExecuteContext<Service> context, Group_GetMyGroupInfosEx_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            var g = context.Service.GroupAccountManager;
            GroupBasicInfo[] infos = g.SafeGetMyGroupBasicInfos(context.GetSessionState()) ?? Array<GroupBasicInfo>.Empty;
            List<GroupInfo> gInfoList = new List<GroupInfo>();

            // 发生变化的群组信息
            foreach (GroupBasicInfo info in infos)
            {
                long version;
                if (request.LocalVersions == null
                    || !request.LocalVersions.TryGetValue(info.GroupId, out version) || version != info.ChangedTime.Ticks)
                {
                    gInfoList.Add(GroupInfo.FromBasicInfo(info));
                }
            }

            // 已经不存在的群组
            int[] notExistsGroupIds = null;
            if (request.LocalVersions != null)
            {
                notExistsGroupIds = request.LocalVersions.Keys.Except(infos.Select(info => info.GroupId)).ToArray();
            }

            return new Group_GetMyGroupInfosEx_Reply { Infos = gInfoList.ToArray(), NotExistsGroupIds = notExistsGroupIds };
        }

        const string Remarks = @"可以指定本地已经存在的群组的版本号，仅返回已经发生变化的群组信息。";
    }
}

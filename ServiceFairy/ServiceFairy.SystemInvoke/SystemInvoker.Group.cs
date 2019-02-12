using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Group;
using Common;

namespace ServiceFairy.SystemInvoke
{
    partial class SystemInvoker
    {
        private GroupInvoker _group;

        /// <summary>
        /// Group Service
        /// </summary>
        public GroupInvoker Group
        {
            get { return _group ?? (_group = new GroupInvoker(this)); }
        }

        /// <summary>
        /// Group Service
        /// </summary>
        public class GroupInvoker : Invoker
        {
            public GroupInvoker(SystemInvoker owner)
                : base(owner)
            {
                RegisterEventHandler<Group_Message_Message>((sender, e) => RaiseEvent(Message, sender, e));
                RegisterEventHandler<Group_GroupChanged_Message>((sender, e) => RaiseEvent(GroupChanged, sender, e));
            }

            /// <summary>
            /// 接收到群组消息
            /// </summary>
            public event ServiceClientReceiveEventHandler<Group_Message_Message> Message;

            /// <summary>
            /// 群组信息或成员发生变化
            /// </summary>
            public event ServiceClientReceiveEventHandler<Group_GroupChanged_Message> GroupChanged;

            /// <summary>
            /// 创建群组
            /// </summary>
            /// <param name="name">群组名称</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<int> CreateGroupSr(string name, CallingSettings settings = null)
            {
                var sr = GroupService.CreateGroup(Sc,
                    new Group_CreateGroup_Request() { Name = name }, settings);

                return CreateSr(sr, r => r.GroupId);
            }

            /// <summary>
            /// 创建群组
            /// </summary>
            /// <param name="name">群组名称</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public int CreateGroup(string name, CallingSettings settings = null)
            {
                return InvokeWithCheck(CreateGroupSr(name, settings));
            }

            /// <summary>
            /// 批量获取指定群组的成员信息
            /// </summary>
            /// <param name="groupIds"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<GroupMemberItem[]> GetMembersSr(int[] groupIds, CallingSettings settings = null)
            {
                var sr = GroupService.GetMembersEx(Sc, new Group_GetMembersEx_Request { GroupIds = groupIds }, settings);
                return CreateSr(sr, r => r.Items);
            }

            /// <summary>
            /// 批量获取指定群组的成员信息
            /// </summary>
            /// <param name="groupIds">群组ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public GroupMemberItem[] GetMembers(int[] groupIds, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetMembersSr(groupIds, settings));
            }

            /// <summary>
            /// 获取指定群组的成员信息
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="version">版本号</param>
            /// <param name="settings">调用设置</param>
            /// <returns>成员ID</returns>
            public ServiceResult<int[]> GetMembersSr(int groupId, ref long version, CallingSettings settings = null)
            {
                var sr = GroupService.GetMembers(Sc, new Group_GetMembers_Request { GroupId = groupId, Version = version }, settings);
                if (sr.Succeed && sr.Result != null)
                    version = sr.Result.Version;

                return CreateSr(sr, r => r.MemberIds);
            }

            /// <summary>
            /// 获取指定群组的成员信息
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="version">版本号</param>
            /// <param name="settings">调用设置</param>
            /// <returns>成员ID</returns>
            public int[] GetMembers(int groupId, ref long version, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetMembersSr(groupId, ref version, settings));
            }

            /// <summary>
            /// 获取指定群组的成员信息
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns>成员ID</returns>
            public ServiceResult<int[]> GetMembersSr(int groupId, CallingSettings settings = null)
            {
                long version = 0;
                return GetMembersSr(groupId, ref version, settings);
            }

            /// <summary>
            /// 获取指定群组的成员信息
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns>成员ID</returns>
            public int[] GetMembers(int groupId, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetMembersSr(groupId, settings));
            }

            /// <summary>
            /// 添加群组成员
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="members">群组成员</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult AddMembersSr(int groupId, string members, CallingSettings settings = null)
            {
                return GroupService.AddMembers(Sc, new Group_AddMembers_Request { GroupId = groupId, Members = members }, settings);
            }

            /// <summary>
            /// 添加群组成员
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="members">群组成员</param>
            /// <param name="settings">调用设置</param>
            public void AddMembers(int groupId, string members, CallingSettings settings = null)
            {
                InvokeWithCheck(AddMembersSr(groupId, members, settings));
            }

            /// <summary>
            /// 添加群组成员
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="member">群组成员</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult AddMemberSr(int groupId, int member, CallingSettings settings = null)
            {
                return GroupService.AddMember(Sc, new Group_AddMember_Request { GroupId = groupId, Member = member }, settings);
            }

            /// <summary>
            /// 添加群组成员
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="member">群组成员</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void AddMember(int groupId, int member, CallingSettings settings = null)
            {
                InvokeWithCheck(AddMemberSr(groupId, member, settings));
            }

            /// <summary>
            /// 批量删除群组成员
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="members">群组成员</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult RemoveMembersSr(int groupId, string members, CallingSettings settings = null)
            {
                return GroupService.RemoveMembers(Sc, new Group_RemoveMembers_Request { GroupId = groupId, Members = members }, settings);
            }

            /// <summary>
            /// 批量删除群组成员
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="members">群组成员</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void RemoveMembers(int groupId, string members, CallingSettings settings = null)
            {
                InvokeWithCheck(RemoveMembersSr(groupId, members, settings));
            }

            /// <summary>
            /// 删除群组成员
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="memberId">群组成员ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult RemoveMemberSr(int groupId, int memberId, CallingSettings settings = null)
            {
                return GroupService.RemoveMember(Sc, new Group_RemoveMember_Request { GroupId = groupId, MemberId = memberId }, settings);
            }

            /// <summary>
            /// 删除群组成员
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="memberId">群组成员ID</param>
            /// <param name="settings">调用设置</param>
            public void RemoveMember(int groupId, int memberId, CallingSettings settings = null)
            {
                InvokeWithCheck(RemoveMemberSr(groupId, memberId, settings));
            }

            /// <summary>
            /// 用户退出群组
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult ExitGroupSr(int groupId, CallingSettings settings = null)
            {
                return GroupService.ExitGroup(Sc, new Group_ExitGroup_Request { GroupId = groupId }, settings);
            }

            /// <summary>
            /// 用户退出群组
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="settings">调用设置</param>
            public void ExitGroup(int groupId, CallingSettings settings = null)
            {
                InvokeWithCheck(ExitGroupSr(groupId, settings));
            }

            /// <summary>
            /// 删除群组
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult RemoveGroupSr(int groupId, CallingSettings settings = null)
            {
                return GroupService.RemoveGroup(Sc, new Group_RemoveGroup_Request { GroupId = groupId }, settings);
            }

            /// <summary>
            /// 删除群组
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="settings">调用设置</param>
            public void RemoveGroup(int groupId, CallingSettings settings = null)
            {
                InvokeWithCheck(RemoveGroupSr(groupId, settings));
            }

            /// <summary>
            /// 批量删除群组
            /// </summary>
            /// <param name="groupIds">群组ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult RemoveGroupsSr(int[] groupIds, CallingSettings settings = null)
            {
                return GroupService.RemoveGroups(Sc, new Group_RemoveGroups_Request { GroupIds = groupIds }, settings);
            }

            /// <summary>
            /// 批量删除群组
            /// </summary>
            /// <param name="groupIds">群组ID</param>
            /// <param name="settings">调用设置</param>
            public void RemoveGroups(int[] groupIds, CallingSettings settings = null)
            {
                InvokeWithCheck(RemoveGroupsSr(groupIds, settings));
            }

            /// <summary>
            /// 发送群组消息
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="content">消息内容</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult SendMessageSr(int groupId, string content, CallingSettings settings = null)
            {
                return GroupService.SendMessage(Sc, new Group_SendMessage_Request { GroupId = groupId, Content = content }, settings);
            }

            /// <summary>
            /// 发送群组消息
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="content">消息内容</param>
            /// <param name="settings">调用设置</param>
            public void SendMessage(int groupId, string content, CallingSettings settings = null)
            {
                InvokeWithCheck(SendMessageSr(groupId, content, settings));
            }

            /// <summary>
            /// 修改群组信息
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="items">群组信息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult ModifyGroupInfoSr(int groupId, Dictionary<string, string> items, CallingSettings settings = null)
            {
                return GroupService.ModifyGroupInfo(Sc,
                    new Group_ModifyGroupInfo_Request { GroupId = groupId, Items = items }, settings);
            }

            /// <summary>
            /// 修改群组信息
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="items">群组信息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void ModifyGroupInfo(int groupId, Dictionary<string, string> items, CallingSettings settings = null)
            {
                InvokeWithCheck(ModifyGroupInfoSr(groupId, items, settings));
            }

            /// <summary>
            /// 获取群组信息
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns>群组信息</returns>
            public ServiceResult<GroupInfo> GetGroupInfoSr(int groupId, CallingSettings settings = null)
            {
                var sr = GroupService.GetGroupInfo(Sc, new Group_GetGroupInfo_Request { GroupId = groupId }, settings);
                return CreateSr(sr, r => r.Info);
            }

            /// <summary>
            /// 获取群组信息
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns>群组信息</returns>
            public GroupInfo GetGroupInfo(int groupId, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetGroupInfoSr(groupId, settings));
            }

            /// <summary>
            /// 获取群组信息
            /// </summary>
            /// <param name="groupIds">群组ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns>群组信息</returns>
            public ServiceResult<GroupInfo[]> GetGroupInfosSr(int[] groupIds, CallingSettings settings = null)
            {
                var sr = GroupService.GetGroupInfos(Sc, new Group_GetGroupInfos_Request { GroupIds = groupIds }, settings);
                return CreateSr(sr, r => r.Infos);
            }

            /// <summary>
            /// 获取群组信息
            /// </summary>
            /// <param name="groupIds">群组ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns>群组信息</returns>
            public GroupInfo[] GetGroupInfos(int[] groupIds, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetGroupInfosSr(groupIds, settings));
            }

            /// <summary>
            /// 获取我的所有群组信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns>群组信息</returns>
            public ServiceResult<GroupInfo[]> GetMyGroupInfosSr(CallingSettings settings = null)
            {
                var sr = GroupService.GetMyGroupInfos(Sc, settings);
                return CreateSr(sr, r => r.Infos);
            }

            /// <summary>
            /// 获取我的所有群组信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public GroupInfo[] GetMyGroupInfos(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetMyGroupInfosSr(settings));
            }

            /// <summary>
            /// 获取我的所有群组ID
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns>群组ID</returns>
            public ServiceResult<int[]> GetMyGroupsSr(CallingSettings settings = null)
            {
                var sr = GroupService.GetMyGroups(Sc, settings);
                return CreateSr(sr, r => r.GroupIds);
            }

            /// <summary>
            /// 获取我的所有群组ID
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns>群组ID</returns>
            public int[] GetMyGroups(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetMyGroupsSr(settings));
            }

            /// <summary>
            /// 获取群组版本号
            /// </summary>
            /// <param name="groupIds">群组ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns>群组版本号</returns>
            public ServiceResult<long[]> GetGroupVersionsSr(int[] groupIds, CallingSettings settings = null)
            {
                var sr = GroupService.GetGroupVersions(Sc, new Group_GetGroupVersions_Request { GroupIds = groupIds }, settings);
                return CreateSr(sr, r => r.Versions);
            }

            /// <summary>
            /// 获取群组版本号
            /// </summary>
            /// <param name="groupIds">群组ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns>群组版本号</returns>
            public long[] GetGroupVersions(int[] groupIds, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetGroupVersionsSr(groupIds, settings));
            }

            /// <summary>
            /// 获取群组信息
            /// </summary>
            /// <param name="localVersions">本地版本号</param>
            /// <param name="settings">调用设置</param>
            /// <param name="notExistsGroupIds">已经不存在的群组ID</param>
            /// <returns>发生变化的群组信息、已经不存在的群组</returns>
            public ServiceResult<GroupInfo[]> GetMyGroupInfosSr(Dictionary<int, long> localVersions, out int[] notExistsGroupIds, CallingSettings settings = null)
            {
                var sr = GroupService.GetMyGroupInfosEx(Sc, new Group_GetMyGroupInfosEx_Request { LocalVersions = localVersions }, settings);
                notExistsGroupIds = ((sr != null && sr.Result != null) ? null : sr.Result.NotExistsGroupIds) ?? Array<int>.Empty;
                return CreateSr(sr, r => r.Infos);
            }

            /// <summary>
            /// 获取群组信息
            /// </summary>
            /// <param name="localVersions">本地版本号</param>
            /// <param name="notExistsGroupIds">已经不存在的群组ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns>发生变化的群组信息、已经不存在的群组</returns>
            public GroupInfo[] GetMyGroupInfos(Dictionary<int, long> localVersions, out int[] notExistsGroupIds, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetMyGroupInfosSr(localVersions, out notExistsGroupIds, settings));
            }

            /// <summary>
            /// 获取我的所有群组的版本号
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns>群组版本号</returns>
            public ServiceResult<Dictionary<int, long>> GetMyGroupVersionsSr(CallingSettings settings = null)
            {
                var sr = GroupService.GetMyGroupVersions(Sc, settings);
                return CreateSr(sr, r => r.Versions);
            }

            /// <summary>
            /// 获取我的所有群组的版本号
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns>群组版本号</returns>
            public Dictionary<int, long> GetMyGroupVersions(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetMyGroupVersionsSr(settings));
            }
        }
    }
}

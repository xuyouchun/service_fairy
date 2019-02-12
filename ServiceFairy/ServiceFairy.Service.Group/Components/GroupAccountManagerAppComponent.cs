using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using Common.Contracts.Service;
using Common.Data;
using Common.Data.SqlExpressions;
using Common.Data.UnionTable;
using Common.Package;
using Common.Package.Service;
using Common.Utility;
using ServiceFairy.Components;
using ServiceFairy.DbEntities;
using ServiceFairy.DbEntities.Group;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Group;
using ServiceFairy.SystemInvoke;
using Common;
using ServiceFairy.Entities.MessageCenter;
using UmArr = ServiceFairy.Entities.Message.UserMsgArray;

namespace ServiceFairy.Service.Group.Components
{
    /// <summary>
    /// 群组帐号管理器
    /// </summary>
    [AppComponent("群组帐号管理器", "创建群组、删除群组等帐号管理功能")]
    class GroupAccountManagerAppComponent : TimerAppComponentBase
    {
        public GroupAccountManagerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(0.5))
        {
            _service = service;
            _utConProvider = new RemoteUtConnectionProvider(_service.Invoker, DbEntityUtility.LoadReviseInfo());
        }

        private readonly Service _service;
        private readonly IUtConnectionProvider _utConProvider;
        private readonly AutoResetEvent _waitForEffectEvent = new AutoResetEvent(false);
        private readonly HashSet<int> _changedGroupsIds = new HashSet<int>();

        private readonly object _effectSyncLocker = new object();

        /// <summary>
        /// 创建群组
        /// </summary>
        /// <param name="creator">创建者</param>
        /// <param name="name">群组名称</param>
        /// <param name="details">群组信息</param>
        public int CreateGroup(int creator, string name, Dictionary<string, string> details)
        {
            DbGroup dbGroup = new DbGroup() {
                CreateTime = DateTime.UtcNow, Creator = creator,
                Name = name, Enable = true,
                Detail = DbGroup.DetailToString(details),
                ChangedTime = DateTime.UtcNow
            };

            dbGroup.Insert(_utConProvider);

            DbGroupMember dbGroupMember = new DbGroupMember {
                GroupId = dbGroup.GroupId, UserId = creator
            };
            dbGroupMember.Insert(_utConProvider);

            return dbGroup.GroupId;
        }

        /// <summary>
        /// 获取组成员
        /// </summary>
        /// <param name="groupId">组ID</param>
        /// <param name="throwError">是否抛出异常</param>
        /// <returns></returns>
        public int[] GetMembers(int groupId, bool throwError = true)
        {
            GroupMemberInfo[] mInfos = _service.GroupManager.GetGroupMembers(groupId, throwError: throwError);

            return mInfos.ToArray(m => m.UserId);
        }

        /// <summary>
        /// 批量获取群组成员
        /// </summary>
        /// <param name="groupIds">组ID</param>
        public IDictionary<int, int[]> GetMembers(int[] groupIds)
        {
            if (groupIds.IsNullOrEmpty())
                return new Dictionary<int, int[]>();

            IDictionary<int, GroupMemberInfo[]> dict = _service.GroupManager.GetGroupsMembers(groupIds);
            return dict.ToDictionary(item => item.Key, item => item.Value.ToArray(v => v.UserId));
        }

        /// <summary>
        /// 添加群组成员
        /// </summary>
        /// <param name="groupId">组ID</param>
        /// <param name="members">用户ID</param>
        /// <param name="curUserId">当前用户</param>
        public void AddMembers(int groupId, Users members, int curUserId)
        {
            int[] memberIds = _service.UserParser.Parse(members, curUserId);
            if (memberIds.IsNullOrEmpty())
                return;

#warning 没有注册的用户会发送短信...

            AddMembers(groupId, memberIds);
        }

        // 发送群组成员及信息变化消息
        private void _SendGroupChangedMessage(int groupId, Users users, GroupChangedType changedType, string name, bool onlyOnline = true)
        {
            Group_GroupChanged_Message msg = new Group_GroupChanged_Message { GroupId = groupId, Name = name, ChangedType = changedType };
            MsgProperty property = MsgProperty.Override | MsgProperty.NotReliable | (onlyOnline ? MsgProperty.OnlyOnline : MsgProperty.Default);
            _service.MessageSender.Send<Group_GroupChanged_Message>(msg, 0, users, property: property);
        }

        private void _UpdateGroupChangedTime(int[] groupIds)
        {
            _changedGroupsIds.SafeAddRange(groupIds);
        }

        private void _UpdateGroupChangedTime(int groupId)
        {
            _changedGroupsIds.SafeAdd(groupId);
        }

        /// <summary>
        /// 添加群组成员
        /// </summary>
        /// <param name="groupId">群组ID</param>
        /// <param name="memberIds">群组成员</param>
        public void AddMembers(int groupId, int[] memberIds)
        {
#warning 判断一下，如果不是好友，则发送邀请消息（持久化）
            if (memberIds == null || (memberIds = memberIds.DistinctToArray()).Length == 0)
                return;

            DateTime now = DateTime.UtcNow;
            DbGroupMember[] dbGroupMembers = memberIds.ToArray(userId => new DbGroupMember {
                GroupId = groupId, UserId = userId, CreateTime = now,
            });

            DbGroupMember.Merge(_utConProvider, groupId, dbGroupMembers, null, new[] { DbGroupMember.F_UserId });

            _UpdateGroupChangedTime(groupId);

            // 发送消息给被添加的用户
            string name = _service.UserManager.GetUserBasicInfos(memberIds).Select(info => info.Name).JoinBy(",");
            _service.GroupManager.ClearCache(groupId, memberIds);
            _SendGroupChangedMessage(groupId, Users.FromUserIds(memberIds), GroupChangedType.CurrentUserAdded, name);

            // 发送消息给其它用户
            int creator = _service.GroupManager.GetCreator(groupId);
            int[] uids = GetMembers(groupId, throwError: false);
            if (uids != null)
            {
                int[] others = uids.Except(memberIds).Except(creator).ToArray();
                if (others.Length > 0)
                    _SendGroupChangedMessage(groupId, Users.FromUserIds(others), GroupChangedType.MemberAdded, name);
            }
        }

        /// <summary>
        /// 添加群组成员
        /// </summary>
        /// <param name="groupId">组ID</param>
        /// <param name="memberId">成员ID</param>
        public void AddMember(int groupId, int memberId)
        {
            AddMembers(groupId, new[] { memberId });
        }

        /// <summary>
        /// 删除群组成员
        /// </summary>
        /// <param name="groupId">群组ID</param>
        /// <param name="members">群组成员</param>
        public void RemoveMembers(int groupId, Users members)
        {
            int[] memberIds = _service.UserParser.Parse(members);
            RemoveMembers(groupId, memberIds);
        }

        /// <summary>
        /// 删除群组成员
        /// </summary>
        /// <param name="groupId">群组ID</param>
        /// <param name="memberId">群组成员ID</param>
        public void RemoveMember(int groupId, int memberId)
        {
            RemoveMembers(groupId, new[] { memberId });
        }

        /// <summary>
        /// 删除群组成员
        /// </summary>
        /// <param name="groupId">群组ID</param>
        /// <param name="memberIds">群组成员ID</param>
        public void RemoveMembers(int groupId, int[] memberIds)
        {
            int creator = _service.GroupManager.GetCreator(groupId, throwError: true);
            if (memberIds == null || (memberIds = memberIds.Except(creator).ToArray()).Length == 0)
                return;

            DbGroupMember.DeleteIn(_utConProvider, DbGroupMember.F_UserId, memberIds.CastAsObject(), groupId);
            _UpdateGroupChangedTime(groupId);
            _service.GroupManager.ClearCache(groupId, memberIds);

            // 发送消息给被删除的用户
            string name = _service.UserManager.GetUserBasicInfos(memberIds).Select(info => info.Name).JoinBy(",");
            _SendGroupChangedMessage(groupId, Users.FromUserIds(memberIds), GroupChangedType.CurrentUserRemoved, name);

            // 发送消息给其它的成员用户
            int[] others = GetMembers(groupId, throwError: true).Except(memberIds).Except(creator).ToArray();
            if (others.Length > 0)
                _SendGroupChangedMessage(groupId, Users.FromUserIds(others), GroupChangedType.MemberRemoved, name);
        }

        /// <summary>
        /// 用户退出群组
        /// </summary>
        /// <param name="groupId">群组ID</param>
        /// <param name="userId">用户ID</param>
        public void ExitGroup(int groupId, int userId)
        {
            DbGroupMember.DeleteIn(_utConProvider, DbGroupMember.F_UserId, userId, groupId);
            _UpdateGroupChangedTime(groupId);

            // 发送消息给其它的成员
            string groupName = _service.GroupManager.GetGroupName(groupId);
            _service.GroupManager.ClearCache(groupId, new[] { userId });

            int[] members = GetMembers(groupId, throwError: true).Except(userId).ToArray();
            if (members.Length > 0)
                _SendGroupChangedMessage(groupId, Users.FromUserIds(members), GroupChangedType.MemberRemoved, groupName);
        }

        /// <summary>
        /// 获取用户所属的群组
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns>群组信息</returns>
        public GroupBasicInfo[] GetUserGroupBasicInfos(int userId, bool? refresh = null)
        {
            return _service.GroupManager.GetUserGroupBasicInfos(userId, refresh);
        }

        /// <summary>
        /// 获取用户所属的群组
        /// </summary>
        /// <param name="uss">当前登录用户的会话状态</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns>群组信息</returns>
        public GroupBasicInfo[] SafeGetMyGroupBasicInfos(UserSessionState uss, bool? refresh = null)
        {
            return GetUserGroupBasicInfos(uss.BasicInfo.UserId, refresh);
        }

        /// <summary>
        /// 获取指定群组的信息
        /// </summary>
        /// <param name="groupIds">群组ID</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns>群组信息</returns>
        public GroupBasicInfo[] GetGroupBasicInfos(int[] groupIds, bool? refresh = null)
        {
            if (groupIds.IsNullOrEmpty())
                return Array<GroupBasicInfo>.Empty;

            return _service.GroupManager.GetGroupBasicInfos(groupIds, refresh);
        }

        /// <summary>
        /// 获取指定群组的信息
        /// </summary>
        /// <param name="groupId">群组ID</param>
        /// <param name="throwError">是否当该群组不存在时抛出异常</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns>群组信息</returns>
        public GroupBasicInfo GetGroupBasicInfo(int groupId, bool throwError = true, bool? refresh = null)
        {
            return _service.GroupManager.GetGroupBasicInfo(groupId, throwError: throwError, refresh: refresh);
        }

        /// <summary>
        /// 删除群组
        /// </summary>
        /// <param name="groupId">群组ID</param>
        public void RemoveGroup(int groupId)
        {
            RemoveGroups(new[] { groupId });
        }

        /// <summary>
        /// 批量删除群组
        /// </summary>
        /// <param name="groupIds"></param>
        public void RemoveGroups(int[] groupIds)
        {
            if (groupIds.IsNullOrEmpty())
                return;

            IDictionary<int, int[]> membersDict = GetMembers(groupIds);

            // 从数据库删除数据
            DbGroup.UpdateByRouteKey(_utConProvider, new DbGroup { Enable = false }, groupIds, new[] { DbGroup.F_Enable });
            DbGroupMember.DeleteByRouteKey(_utConProvider, groupIds);

            // 发送消息给群组成员
            Dictionary<int, GroupBasicInfo> biDict = _service.GroupManager.GetGroupBasicInfos(groupIds).ToDictionary(g => g.GroupId);

            // 清除缓存
            _service.GroupManager.ClearCache(groupIds, membersDict.SelectMany(v => v.Value).ToArray());

            foreach (var item in membersDict)
            {
                int groupId = item.Key;
                int[] members = item.Value;
                GroupBasicInfo gInfo;

                if (biDict.TryGetValue(groupId, out gInfo))
                {
                    _SendGroupChangedMessage(groupId,
                        Users.FromUserIds(members.Except(gInfo.Creator).ToArray()),
                        GroupChangedType.GroupRemoved, gInfo.Name);
                }
            }
        }

        /// <summary>
        /// 修改群组信息
        /// </summary>
        /// <param name="groupId">群组ID</param>
        /// <param name="items">群组信息</param>
        public void ModifyGroupInfo(int groupId, IDictionary<string, string> items)
        {
            if (items.IsNullOrEmpty())
                return;

            GroupBasicInfo basicInfo = _service.GroupManager.GetGroupBasicInfo(groupId);
            GroupDetailInfo detailInfo = _service.GroupManager.GetGroupDetailInfo(groupId);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (detailInfo != null && detailInfo.Items != null)
                dict.AddRange(detailInfo.Items);

            dict.AddRange(items, ignoreDupKey: true);
            string name = items.GetOrDefault("Name");
            DbGroup dbGroup = new DbGroup {
                Detail = DbGroup.DetailToString(dict),
                GroupId = groupId, ChangedTime = DateTime.UtcNow, Name = name
            };

            dbGroup.Update(_service.DbConnectionManager.Provider,
                name == null ? new[] { DbGroup.F_Detail, DbGroup.F_ChangedTime }
                : new[] { DbGroup.F_Detail, DbGroup.F_ChangedTime, DbGroup.F_Name });
            
            _service.GroupManager.ClearGroupCache(groupId);
            Users users = Users.FromGroupId(groupId) - Users.FromUserId(basicInfo.Creator);
            _SendGroupChangedMessage(groupId, users, GroupChangedType.InfoChanged, basicInfo.Name);
        }

        protected override void OnExecuteTask(string taskName)
        {
            if (_changedGroupsIds.Count > 0)
            {
                int[] groupIds = _changedGroupsIds.SafeToArray(clear: true, trimExcess: true);
                DbGroup.UpdateByRouteKey(_utConProvider,
                    new DbGroup { ChangedTime = DateTime.UtcNow }, groupIds, new[] { DbGroup.F_ChangedTime });
            }
        }
    }
}

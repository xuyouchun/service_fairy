using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Common;
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
using ServiceFairy.Entities.Group;
using ServiceFairy.Entities.UserCenter;
using ServiceFairy.SystemInvoke;

namespace ServiceFairy.Service.UserCenter.Components
{
    /// <summary>
    /// 群组信息管理器
    /// </summary>
    [AppComponent("群组信息管理器", "加载并缓存群组的信息")]
    class GroupInfoManagerAppComponent : TimerAppComponentBase
    {
        public GroupInfoManagerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;
            _utConProvider = new RemoteUtConnectionProvider(_service.Invoker, DbEntityUtility.LoadReviseInfo());

            _groupInfoCacheChain = _service.CacheChainManager.CreateCacheChain<GroupCacheKey, object>(
                enableWeakReference: true,
                memoryCacheExpire: TimeSpan.FromMinutes(1),
                distributedCacheExpire: TimeSpan.FromMinutes(10), distributedCachePrefix: "s_g_",
                loader: _LoadGroupInfos
            );

            _userGroupCacheChain = _service.CacheChainManager.CreateCacheChain<int, int[]>(
                enableWeakReference: true,
                memoryCacheExpire: TimeSpan.FromMinutes(1),
                distributedCacheExpire: TimeSpan.FromMinutes(10), distributedCachePrefix: "s_ug_",
                loader: _LoadUserGroups
            );
        }

        private readonly Service _service;
        private readonly IUtConnectionProvider _utConProvider;
        private readonly CacheChain<GroupCacheKey, object> _groupInfoCacheChain;
        private readonly CacheChain<int, int[]> _userGroupCacheChain;

        protected override void OnStatusChanged(AppComponentStatus status)
        {
            base.OnStatusChanged(status);

            _service.ServiceEvent.Switch<Group_InfoChanged_Event>(_OnInfoChanged, status);
        }

        private void _OnInfoChanged(object sender, ServiceEventArgs<Group_InfoChanged_Event> e)
        {
            if (e.Entity != null)
            {
                int[] groupIds, userIds;
                if (!(groupIds = e.Entity.GroupIds).IsNullOrEmpty())
                    _groupInfoCacheChain.RemoveRange(GroupCacheKey.CreateCacheKeys(groupIds));

                if (!(userIds = e.Entity.EffectUserIds).IsNullOrEmpty())
                    _userGroupCacheChain.RemoveRange(userIds);
            }
        }

        /// <summary>
        /// 获取组信息
        /// </summary>
        /// <param name="groupIds">群组ID</param>
        /// <param name="mask">遮罩</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns>群组信息</returns>
        public GroupInfos[] GetGroupInfos(int[] groupIds, GroupInfoMask mask, bool refresh = false)
        {
            if (groupIds.IsNullOrEmpty())
                return Array<GroupInfos>.Empty;

            KeyValuePair<GroupCacheKey, object>[] values = _groupInfoCacheChain.GetRange(_ToCacheKeys(mask, groupIds), refresh);
            Dictionary<int, GroupInfos> dict = new Dictionary<int, GroupInfos>();

            foreach (KeyValuePair<GroupCacheKey, object> value in values)
            {
                GroupInfos info = dict.GetOrSet(value.Key.GroupId, (groupId) => new GroupInfos() { GroupId = groupId });
                _AssignValue(info, value.Value, value.Key.Mask);
            }

            return dict.Values.ToArray();
        }

        private void _AssignValue(GroupInfos info, object value, GroupInfoMask mask)
        {
            switch (mask)
            {
                case GroupInfoMask.Basic:
                    info.BasicInfo = value as GroupBasicInfo;
                    break;

                case GroupInfoMask.Detail:
                    info.DetailInfo = value as GroupDetailInfo;
                    break;

                case GroupInfoMask.Member:
                    info.MemberInfos = value as GroupMemberInfo[];
                    break;
            }
        }

        /// <summary>
        /// 获取组信息
        /// </summary>
        /// <param name="groupId">群组ID</param>
        /// <param name="mask">遮罩</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns>群组信息</returns>
        public GroupInfos GetGroupInfo(int groupId, GroupInfoMask mask, bool refresh = false)
        {
            return GetGroupInfos(new[] { groupId }, mask, refresh).FirstOrDefault();
        }

        /// <summary>
        /// 获取用户所属的组
        /// </summary>
        /// <param name="userIds">用户ID</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns>群组信息</returns>
        public UserGroupItem[] GetUserGroups(int[] userIds, bool refresh = false)
        {
            return _userGroupCacheChain.GetRange(userIds, refresh).ToArray(
                item => new UserGroupItem() { UserId = item.Key, GroupIds = item.Value }
            );
        }

        private static KeyValuePair<GroupCacheKey, object>[] _GetEntities(DbGroup dbGroup, GroupInfoMask mask)
        {
            List<KeyValuePair<GroupCacheKey, object>> objs = new List<KeyValuePair<GroupCacheKey, object>>();

            foreach (GroupInfoMask m in _masks.Keys.Where(m => mask.HasFlag(m)))
            {
                object obj;
                switch (m)
                {
                    case GroupInfoMask.Basic:
                        obj = new GroupBasicInfo() {
                            Name = dbGroup.Name, GroupId = dbGroup.GroupId, Creator = dbGroup.Creator,
                            CreateTime = dbGroup.CreateTime, Enable = dbGroup.Enable, ChangedTime = dbGroup.ChangedTime,
                        };
                        break;

                    case GroupInfoMask.Detail:
                        obj = new GroupDetailInfo() {
                            GroupId = dbGroup.GroupId, Items = DbGroup.DetailToDict(dbGroup.Detail),
                        };
                        break;

                    default:
                        continue;
                }

                objs.Add(new KeyValuePair<GroupCacheKey, object>(new GroupCacheKey { Mask = m, GroupId = dbGroup.GroupId }, obj));
            }

            return objs.ToArray();
        }

        // 加载群组的信息
        private KeyValuePair<GroupCacheKey, object>[] _LoadGroupInfos(GroupCacheKey[] keys, bool refresh)
        {
            if (keys.IsNullOrEmpty())
                return Array<KeyValuePair<GroupCacheKey, object>>.Empty;

            List<KeyValuePair<GroupCacheKey, object>> list = new List<KeyValuePair<GroupCacheKey, object>>();
            foreach (var g in keys.GroupBy(key => key.Mask))
            {
                GroupInfoMask mask = g.Key;
                if (mask == GroupInfoMask.Basic || mask == GroupInfoMask.Detail)
                {
                    DbGroup[] groups = DbGroup.Select(_utConProvider, g.ToArray(v => v.GroupId), _GetColumnNames(mask));
                    foreach (DbGroup group in groups ?? Array<DbGroup>.Empty)
                    {
                        list.AddRange(_GetEntities(group, mask));
                    }
                }
                else if (mask == GroupInfoMask.Member)
                {
                    DbGroupMember[] gMembers = DbGroupMember.Select(_utConProvider, g.ToArray(v => v.GroupId), _GetColumnNames(mask));
                    var lst = from m in gMembers group m by m.GroupId into gs
                              let key = new GroupCacheKey { GroupId = gs.Key, Mask = GroupInfoMask.Member }
                              let value = gs.ToArray(v => new GroupMemberInfo { UserId = v.UserId, CreateTime = v.CreateTime })
                              select new KeyValuePair<GroupCacheKey, object>(key, value);

                    list.AddRange(lst);
                }
            }

            return list.ToArray();
        }

        private static string[] _GetColumnNames(GroupInfoMask mask)
        {
            HashSet<string> hs = new HashSet<string> { DbGroup.F_GroupId };
            foreach (KeyValuePair<GroupInfoMask, string[]> item in _masks)
            {
                if (mask.HasFlag(item.Key))
                    hs.AddRange(item.Value);
            }

            return hs.ToArray();
        }

        private static readonly Dictionary<GroupInfoMask, string[]> _masks = new Dictionary<GroupInfoMask, string[]> {
            { GroupInfoMask.Basic, new [] { DbGroup.F_Creator, DbGroup.F_Name, DbGroup.F_CreateTime, DbGroup.F_Enable, DbGroup.F_ChangedTime } },
            { GroupInfoMask.Detail, new [] { DbGroup.F_Detail } },
            { GroupInfoMask.Member, new [] { DbGroupMember.F_UserId, DbGroupMember.F_CreateTime } },
        };

        private GroupCacheKey[] _ToCacheKeys(GroupInfoMask mask, int[] groupIds)
        {
            List<GroupCacheKey> keys = new List<GroupCacheKey>();

            foreach (GroupInfoMask m in _masks.Keys)
            {
                if (mask.HasFlag(m))
                    keys.AddRange(groupIds.Select(groupId => new GroupCacheKey() { GroupId = groupId, Mask = mask }));
            }

            return keys.ToArray();
        }

        // 根据用户ID加载用户所属组信息
        private KeyValuePair<int, int[]>[] _LoadUserGroups(int[] userIds, bool refresh)
        {
            DbGroupMember[] members = DbGroupMember.SelectIn(_utConProvider, DbGroupMember.F_UserId,
                userIds.CastAsObject(), null, new[] { DbGroupMember.F_GroupId, DbGroupMember.F_UserId });

            var list = from m in members
                       group m by m.UserId into g
                       select new KeyValuePair<int, int[]>(g.Key, g.ToArray(m => m.GroupId));

            return list.ToArray();
        }
    }
}

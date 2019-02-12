using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Package;
using ServiceFairy.Entities.User;
using ServiceFairy.Entities.Group;
using Common.Utility;
using Common;
using System.Diagnostics.Contracts;
using ServiceFairy.Entities.UserCenter;

namespace ServiceFairy.Components
{
    /// <summary>
    /// 群组管理器
    /// </summary>
    [AppComponent("群组管理器", "加载并缓存用户组的一些信息", AppComponentCategory.System, "Sys_GroupManager")]
    public class GroupManagerAppComponent : TimerAppComponentBase
    {
        public GroupManagerAppComponent(SystemAppServiceBase service)
            : base(service)
        {
            _service = service;

            _groupCache = _service.CacheChainManager.CreateCacheChain(new CacheChainCreationInfo<GroupCacheKey, object> {
                EnableWeakReferenceCache = true,
                TrayCacheExpire = TimeSpan.FromSeconds(30), TrayCacheName = this.GetType() + "_GROUP", TrayCacheGlobal = true,
                Loader = _LoadGroupInfos,
            });

            _userGroupCache = _service.CacheChainManager.CreateCacheChain(new CacheChainCreationInfo<int, int[]> {
                EnableWeakReferenceCache = true,
                TrayCacheExpire = TimeSpan.FromSeconds(30), TrayCacheName = this.GetType() + "_USER_GROUP", TrayCacheGlobal = true,
                Loader = _LoadUserGroup,
            });
        }

        private readonly SystemAppServiceBase _service;
        private readonly CacheChain<GroupCacheKey, object> _groupCache;
        private readonly CacheChain<int, int[]> _userGroupCache;

        // 用于保存信息已变化的群组ID
        private readonly DelayHashSet<int> _infoChangedGroupIds = new DelayHashSet<int>(TimeSpan.FromSeconds(10));

        // 用于保存用户所属群组的信息发生变化的用户ID
        private readonly DelayHashSet<int> _userGroupChangedUserIds = new DelayHashSet<int>(TimeSpan.FromSeconds(10));

        protected override void OnStatusChanged(AppComponentStatus status)
        {
            base.OnStatusChanged(status);

            _service.ServiceEvent.Switch<Group_InfoChanged_Event>(_OnInfoChanged, status);
        }

        private void _OnInfoChanged(object sender, ServiceEventArgs<Group_InfoChanged_Event> e)
        {
            if (e.Entity != null)
            {
                int[] groupIds, effectUserIds;
                if (!(groupIds = e.Entity.GroupIds).IsNullOrEmpty())
                    _ClearGroupCache(groupIds);

                if (!(effectUserIds = e.Entity.EffectUserIds).IsNullOrEmpty())
                    _ClearUserGroupCache(effectUserIds);
            }
        }

        private bool _GetRefresh(int userId, bool? refresh)
        {
            if (refresh != null)
                return (bool)refresh;

            return _infoChangedGroupIds.Contains(userId, includeDeleted: false);
        }

        private bool _GetRefresh(int[] userIds, bool? refresh)
        {
            if (refresh != null)
                return (bool)refresh;

            return userIds.Any(userId => _GetRefresh(userId, refresh));
        }

        /// <summary>
        /// 获取群组的基础信息
        /// </summary>
        /// <param name="groupId">群组ID</param>
        /// <param name="throwError">是否在群组不存在时抛出异常</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns></returns>
        public GroupBasicInfo GetGroupBasicInfo(int groupId, bool throwError = false, bool? refresh = null)
        {
            GroupBasicInfo bi = GetGroupBasicInfos(new[] { groupId }, _GetRefresh(groupId, refresh)).FirstOrDefault();
            if (bi == null && throwError)
                throw _CreateGroupNotExistException();

            return bi;
        }

        /// <summary>
        /// 获取群组名称
        /// </summary>
        /// <param name="groupId">群组ID</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <param name="throwError">是否在群组不存在时抛出异常</param>
        /// <returns>群组名称</returns>
        public string GetGroupName(int groupId, bool throwError = true, bool? refresh = null)
        {
            GroupBasicInfo bi = GetGroupBasicInfo(groupId, throwError: throwError, refresh: refresh);
            return bi == null ? null : bi.Name;
        }

        private Exception _CreateGroupNotExistException()
        {
            return new ServiceException(GroupStatusCode.InvalidGroup);
        }

        /// <summary>
        /// 批量获取群组的基础信息
        /// </summary>
        /// <param name="groupIds">群组ID</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns></returns>
        public GroupBasicInfo[] GetGroupBasicInfos(int[] groupIds, bool? refresh = null)
        {
            if(groupIds.IsNullOrEmpty())
                return Array<GroupBasicInfo>.Empty;

            KeyValuePair<GroupCacheKey, object>[] cacheItems = _groupCache.GetRange(
                groupIds.ToArray(gid => new GroupCacheKey { GroupId = gid, Mask = GroupInfoMask.Basic }), _GetRefresh(groupIds, refresh));

            return cacheItems.ToArrayNotNull(ci => (GroupBasicInfo)ci.Value);
        }

        /// <summary>
        /// 获取群组详细信息
        /// </summary>
        /// <param name="groupId">群组ID</param>
        /// <param name="throwError">是否在未获得详细信息时抛出异常</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns>群组详细信息</returns>
        public GroupDetailInfo GetGroupDetailInfo(int groupId, bool throwError = true, bool? refresh = null)
        {
            GroupDetailInfo di = GetGroupDetailInfos(new[] { groupId }, _GetRefresh(groupId, refresh)).FirstOrDefault();
            if (di == null && throwError)
                throw _CreateGroupNotExistException();

            return di;
        }

        /// <summary>
        /// 获取群组详细信息
        /// </summary>
        /// <param name="groupIds">群组ID</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns>群组详细信息</returns>
        public GroupDetailInfo[] GetGroupDetailInfos(int[] groupIds, bool? refresh = null)
        {
            if (groupIds.IsNullOrEmpty())
                return Array<GroupDetailInfo>.Empty;

            KeyValuePair<GroupCacheKey, object>[] cacheItems = _groupCache.GetRange(
                groupIds.ToArray(gid => new GroupCacheKey { GroupId = gid, Mask = GroupInfoMask.Detail }), _GetRefresh(groupIds, refresh));
            return cacheItems.ToArrayNotNull(ci => (GroupDetailInfo)ci.Value);
        }

        /// <summary>
        /// 获取群组的成员信息
        /// </summary>
        /// <param name="groupId">群组ID</param>
        /// <param name="throwError">是否在不存在该群组时抛出异常</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns>群组成员信息</returns>
        public GroupMemberInfo[] GetGroupMembers(int groupId, bool throwError = true, bool? refresh = null)
        {
            GroupMemberInfo[] mInfos = GetGroupsMembers(new[] { groupId }, refresh).GetOrDefault(groupId);
            if (mInfos == null && throwError)
                throw _CreateGroupNotExistException();

            return mInfos;
        }

        /// <summary>
        /// 批量获取群组的成员信息
        /// </summary>
        /// <param name="groupIds">群组ID</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns></returns>
        public IDictionary<int, GroupMemberInfo[]> GetGroupsMembers(int[] groupIds, bool? refresh = null)
        {
            if (groupIds.IsNullOrEmpty())
                return new Dictionary<int, GroupMemberInfo[]>();

            KeyValuePair<GroupCacheKey, object>[] cacheItems = _groupCache.GetRange(
                groupIds.ToArray(gid => new GroupCacheKey { GroupId = gid, Mask = GroupInfoMask.Member }), _GetRefresh(groupIds, refresh));
            return cacheItems.ToDictionary(ci => ci.Key.GroupId, ci => (GroupMemberInfo[])ci.Value, ignoreDupKeys: true);
        }

        /// <summary>
        /// 获取群组的创建者
        /// </summary>
        /// <param name="groupId">群组ID</param>
        /// <param name="throwError">是否在群组不存在的时候抛出异常</param>
        /// <returns>创建者</returns>
        public int GetCreator(int groupId, bool throwError = true)
        {
            GroupBasicInfo info = GetGroupBasicInfo(groupId, throwError: throwError);
            if (info == null && throwError)
                throw _CreateGroupNotExistException();

            return (info == null) ? 0 : info.Creator;
        }

        // 加载组信息
        private KeyValuePair<GroupCacheKey, object>[] _LoadGroupInfos(GroupCacheKey[] keys, bool refresh)
        {
            GroupCacheKeyGroup[] gs = GroupCacheKey.JoinAndGroup(keys);
            List<KeyValuePair<GroupCacheKey, object>> list = new List<KeyValuePair<GroupCacheKey, object>>();
            foreach (GroupCacheKeyGroup g in gs)
            {
                GroupInfos[] infos = _service.Invoker.UserCenter.GetGroupInfos(g.GroupIds, g.Mask, refresh);

                foreach (GroupInfos item in infos)
                {
                    if (item.BasicInfo != null)
                        list.Add(new KeyValuePair<GroupCacheKey, object>(new GroupCacheKey { GroupId = item.GroupId, Mask = GroupInfoMask.Basic }, item.BasicInfo));

                    if (item.DetailInfo != null)
                        list.Add(new KeyValuePair<GroupCacheKey, object>(new GroupCacheKey { GroupId = item.GroupId, Mask = GroupInfoMask.Detail }, item.DetailInfo));

                    if (item.MemberInfos != null)
                        list.Add(new KeyValuePair<GroupCacheKey, object>(new GroupCacheKey { GroupId = item.GroupId, Mask = GroupInfoMask.Member }, item.MemberInfos));
                }
            }

            return list.ToArray();
        }

        // 加载用户所属的组
        private KeyValuePair<int, int[]>[] _LoadUserGroup(int[] userIds, bool refresh)
        {
            UserGroupItem[] items = _service.Invoker.UserCenter.GetUserGroups(userIds, refresh);
            return items.ToArray(item => new KeyValuePair<int, int[]>(item.UserId, item.GroupIds));
        }

        /// <summary>
        /// 清除指定群组的缓存
        /// </summary>
        /// <param name="groupId">群组ID</param>
        public void ClearGroupCache(int groupId)
        {
            ClearGroupCache(new[] { groupId });
        }

        /// <summary>
        /// 清除指定群组的缓存
        /// </summary>
        /// <param name="groupIds">群组ID</param>
        public void ClearGroupCache(int[] groupIds)
        {
            _ClearGroupCache(groupIds);
            _infoChangedGroupIds.AddRange(groupIds);
        }

        private void _ClearGroupCache(int[] groupIds)
        {
            if (groupIds.IsNullOrEmpty())
                return;

            _groupCache.RemoveRange(GroupCacheKey.CreateCacheKeys(groupIds));
        }

        /// <summary>
        /// 获取用户所属的组
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns></returns>
        public int[] GetUserGroups(int userId, bool? refresh = null)
        {
            return _userGroupCache.Get(userId, refresh == true);
        }

        /// <summary>
        /// 判断用户是否属于某个群组
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="groupId">群组ID</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns></returns>
        public bool IsMemberOfGroup(int userId, int groupId, bool? refresh = null)
        {
            GroupMemberInfo[] mInfos = GetGroupMembers(groupId, throwError:false, refresh: refresh);
            return mInfos != null && mInfos.Any(m => m.UserId == userId);
        }

        /// <summary>
        /// 判断用户是否为某个群组的创建者
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="groupId">群组ID</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns></returns>
        public bool IsCreatorOfGroup(int userId, int groupId, bool? refresh = null)
        {
            GroupBasicInfo basicInfo = GetGroupBasicInfo(groupId, throwError: false, refresh: refresh);
            return basicInfo != null && basicInfo.Creator == userId;
        }

        /// <summary>
        /// 判断用户是否为某个群组的创建者
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="groupIds">群组ID</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns></returns>
        public bool IsCreatorOfGroups(int userId, int[] groupIds, bool? refresh = null)
        {
            if (groupIds.IsNullOrEmpty())
                return true;

            GroupBasicInfo[] infos = GetGroupBasicInfos(groupIds, refresh);
            return infos.All(info => info.Creator == userId);
        }

        /// <summary>
        /// 获取用户所属的组
        /// </summary>
        /// <param name="userIds">用户ID</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns></returns>
        public UserGroupItem[] GetUserGroups(int[] userIds, bool? refresh = null)
        {
            Contract.Requires(userIds != null);

            return _userGroupCache.GetRange(userIds, refresh == true).ToArray(
                item => new UserGroupItem() { UserId = item.Key, GroupIds = item.Value }
            );
        }

        /// <summary>
        /// 获取用户所属的组的基础信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns></returns>
        public GroupBasicInfo[] GetUserGroupBasicInfos(int userId, bool? refresh = null)
        {
            int[] groupIds = GetUserGroups(userId, refresh);
            return GetGroupBasicInfos(groupIds, refresh);
        }

        /// <summary>
        /// 清除用户所属群组的缓存
        /// </summary>
        /// <param name="userId">用户ID</param>
        public void ClearUserGroupCache(int userId)
        {
            ClearUserGroupCache(new[] { userId });
        }

        /// <summary>
        /// 清除用户所属群组的缓存
        /// </summary>
        /// <param name="userIds"></param>
        private void ClearUserGroupCache(int[] userIds)
        {
            _ClearUserGroupCache(userIds);
            _userGroupChangedUserIds.AddRange(userIds);
        }

        private void _ClearUserGroupCache(int[] userIds)
        {
            if (userIds.IsNullOrEmpty())
                return;

            _userGroupCache.RemoveRange(userIds);
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="groupId">群组ID</param>
        /// <param name="effectUserIds">受影响的用户</param>
        public void ClearCache(int groupId, int[] effectUserIds)
        {
            ClearGroupCache(groupId);
            ClearUserGroupCache(effectUserIds);
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="groupIds">群组ID</param>
        /// <param name="effectUserIds">受影响的用户</param>
        public void ClearCache(int[] groupIds, int[] effectUserIds)
        {
            ClearGroupCache(groupIds);
            ClearUserGroupCache(effectUserIds);
        }

        protected override void OnExecuteTask(string taskName)
        {
            if (_infoChangedGroupIds.GetCount(false) > 0 || _userGroupChangedUserIds.GetCount(false) > 0)
            {
                int[] groupIds = _infoChangedGroupIds.ToArray(false, clear: true);
                int[] userIds = _userGroupChangedUserIds.ToArray(false, clear: true);

                if (groupIds.Length > 0 || userIds.Length > 0)
                    _service.ServiceEvent.Raise(new Group_InfoChanged_Event() { GroupIds = groupIds });
            }
        }
    }
}

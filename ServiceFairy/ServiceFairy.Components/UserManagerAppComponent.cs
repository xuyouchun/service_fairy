using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using Common;
using Common.Contracts.Service;
using Common.Framework.TrayPlatform;
using Common.Package;
using Common.Package.Service;
using Common.Utility;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Security;
using ServiceFairy.Entities.User;
using ServiceFairy.Entities.UserCenter;
using ServiceFairy.SystemInvoke;
using CreationCacheKey = System.Tuple<string, string, System.Type, System.Type>;

namespace ServiceFairy.Components
{
    /// <summary>
    /// 用户管理器
    /// </summary>
    [AppComponent("用户管理器", "管理用户的会话状态、身份验证信息等", AppComponentCategory.System, "Sys_UserManager")]
    public class UserManagerAppComponent : TimerAppComponentBase
    {
        public UserManagerAppComponent(SystemAppServiceBase service)
            : base(service, TimeSpan.FromSeconds(0.2))
        {
            _service = service;
            _invoker = service.Invoker;

            _sidCache = new InnerCacheChain<Sid, SidInfo>(this, _GetSidInfos, "SID_INFO", TimeSpan.FromMinutes(10));
            _userInfoCache = new InnerCacheChain<UserCacheKey, object>(this, _LoadUserInfosByUserId, "USER_INFO", TimeSpan.FromHours(1));
        }

        private readonly SystemAppServiceBase _service;
        private readonly SystemInvoker _invoker;

        private readonly InnerCacheChain<Sid, SidInfo> _sidCache;
        private readonly InnerCacheChain<UserCacheKey, object> _userInfoCache;

        // 用于保存信息已变化的用户ID
        private readonly DelayHashSet<int> _infoChangedUserIds = new DelayHashSet<int>(TimeSpan.FromSeconds(10));

        class InnerCacheChain<TKey, TValue> : CacheChain<TKey, TValue> where TValue : class
        {
            public InnerCacheChain(UserManagerAppComponent owner, CacheChainValueLoader<TKey, TValue> loader, string cacheName, TimeSpan trayCacheExpire)
            {
                AddWeakReferenceCache();
                ITrayCacheManager cacheManager = owner._service.Context.CacheManager;
                AddNode(new TrayCacheChainNode<TKey, TValue>(cacheManager.Get<TKey, TValue>(cacheName, true, true), trayCacheExpire));

                if (loader != null)
                    AddLoader(loader);
            }
        }

        // 通过用户ID加载UserBasicInfo
        private KeyValuePair<UserCacheKey, object>[] _LoadUserInfosByUserId(UserCacheKey[] keys, bool refresh)
        {
            UserCacheKeyGroup[] gs = UserCacheKey.JoinAndGroup(keys);

            List<KeyValuePair<UserCacheKey, object>> list = new List<KeyValuePair<UserCacheKey, object>>();
            foreach (UserCacheKeyGroup g in gs)
            {
                UserInfos[] infos = _invoker.UserCenter.GetUserInfos(Users.FromUserIds(g.UserIds), g.Mask, refresh);

                foreach (UserInfos item in infos)
                {
                    if (item.BasicInfo != null)
                        list.Add(new KeyValuePair<UserCacheKey, object>(new UserCacheKey { UserId = item.UserId, Mask = UserInfoMask.Basic }, item.BasicInfo));

                    if (item.DetailInfo != null)
                        list.Add(new KeyValuePair<UserCacheKey, object>(new UserCacheKey { UserId = item.UserId, Mask = UserInfoMask.Detail }, item.DetailInfo));

                    if (item.StatusInfo != null)
                        list.Add(new KeyValuePair<UserCacheKey, object>(new UserCacheKey { UserId = item.UserId, Mask = UserInfoMask.Status }, item.StatusInfo));
                }
            }

            return list.ToArray();
        }

        private bool _GetRefresh(int userId, bool? refresh)
        {
            if (refresh != null)
                return (bool)refresh;

            return _infoChangedUserIds.Contains(userId, includeDeleted: false);
        }

        private bool _GetRefresh(int[] userIds, bool? refresh)
        {
            if (refresh != null)
                return (bool)refresh;

            return userIds.Any(userId => _GetRefresh(userId, refresh));
        }

        private KeyValuePair<Sid, SidInfo>[] _GetSidInfos(Sid[] sids, bool refresh)
        {
            if (sids.Length == 0)
                return Array<KeyValuePair<Sid, SidInfo>>.Empty;

            SidInfo[] sidInfos = _service.Invoker.Security.GetSidInfos(sids);
            List<KeyValuePair<Sid, SidInfo>> list = new List<KeyValuePair<Sid, SidInfo>>();
            for (int k = 0; k < sids.Length; k++)
            {
                list.Add(new KeyValuePair<Sid, SidInfo>(sids[k], sidInfos[k]));
            }

            return list.ToArray();
        }

        /// <summary>
        /// 根据用户ID获取用户基本信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public UserBasicInfo GetUserBasicInfo(int userId, bool throwError = false, bool? refresh = null)
        {
            UserBasicInfo basicInfo = null;

            if (userId != 0)
            {
                UserCacheKey key = new UserCacheKey() { UserId = userId, Mask = UserInfoMask.Basic };
                basicInfo = _userInfoCache.Get(key, _GetRefresh(userId, refresh)) as UserBasicInfo;
            }

            if (basicInfo == null && throwError)
                throw new ServiceException(UserStatusCode.InvalidUser);

            return basicInfo;
        }

        /// <summary>
        /// 根据用户ID批量获取用户基本信息
        /// </summary>
        /// <param name="userIds">用户ID</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns></returns>
        public UserBasicInfo[] GetUserBasicInfos(int[] userIds, bool? refresh = null)
        {
            Contract.Requires(userIds != null);

            UserCacheKey[] keys = userIds.ToArray(userId => new UserCacheKey() { UserId = userId, Mask = UserInfoMask.Basic });
            return _userInfoCache.GetRange(keys, _GetRefresh(userIds, refresh)).ToArrayNotNull(v => v.Value as UserBasicInfo);
        }

        /// <summary>
        /// 获取指定用户的基本信息
        /// </summary>
        /// <param name="users"></param>
        /// <param name="refresh"></param>
        /// <returns></returns>
        public UserBasicInfo[] GetUserBasicInfos(Users users, bool? refresh = null)
        {
            Contract.Requires(users != null);

            int[] userIds = _service.UserParser.Parse(users, refresh == true);
            return GetUserBasicInfos(userIds, _GetRefresh(userIds, refresh));
        }

        /// <summary>
        /// 根据用户名获取用户基本信息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="throwError"></param>
        /// <param name="refresh"></param>
        /// <returns></returns>
        public UserBasicInfo GetUserBasicInfo(string username, bool throwError = false, bool? refresh = null)
        {
            int userId = _service.UserParser.ParseUserNameToId(username);
            return GetUserBasicInfo(userId, throwError: throwError, refresh: refresh);
        }

        /// <summary>
        /// 清除用户信息缓存
        /// </summary>
        /// <param name="userId">用户ID</param>
        public void ClearCache(int userId)
        {
            ClearCache(new[] { userId });
        }

        /// <summary>
        /// 批量清除用户信息缓存
        /// </summary>
        /// <param name="userIds">用户ID</param>
        public void ClearCache(int[] userIds)
        {
            _ClearCache(userIds);

            _infoChangedUserIds.AddRange(userIds);
        }

        private void _ClearCache(int[] userIds)
        {
            if (userIds.IsNullOrEmpty())
                return;

            UserCacheKey[] keys = UserCacheKey.CreateCacheKeys(userIds);
            _userInfoCache.RemoveRange(keys);
        }

        /// <summary>
        /// 是否包含指定的用户
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool ExistsUser(string username)
        {
            return GetUserBasicInfo(username) != null;
        }

        /// <summary>
        /// 根据安全码获取用户会话状态
        /// </summary>
        /// <param name="sid">安全码</param>
        /// <param name="throwError">是否在用户未登录时抛出异常</param>
        /// <returns></returns>
        public UserSessionState GetSessionState(Sid sid, bool throwError = false)
        {
            if (!sid.IsEmpty())
            {
                SidInfo si = _sidCache.Get(sid);
                if (si != null)
                {
                    UserBasicInfo basicInfo = GetUserBasicInfo(si.UserId, throwError);
                    return new UserSessionState(sid, si.SecurityLevel, basicInfo);
                }
            }

            if (throwError)
                throw new ServiceException(ServerErrorCode.InvalidUser, "用户未登录");

            return null;
        }

        /// <summary>
        /// 清除用户的会话状态缓存
        /// </summary>
        /// <param name="sid">安全码</param>
        public void ClearSessionStateCache(Sid sid)
        {
            _sidCache.Remove(sid);

#warning 需要从数据库中将该sid删除
        }

        /// <summary>
        /// 清除用户会话状态
        /// </summary>
        /// <param name="sid"></param>
        public void RemoveSessionState(Sid sid)
        {
            //_invoker.UserCenter.RemoveSid(sid);
#warning 需要从数据库中将该sid删除
        }

        protected override void OnStatusChanged(AppComponentStatus status)
        {
            base.OnStatusChanged(status);

            _service.ServiceEvent.Switch<User_InfoChanged_Event>(_OnUser_InfoChanged, status);
        }

        // 用户信息变化
        private void _OnUser_InfoChanged(object sender, ServiceEventArgs<User_InfoChanged_Event> e)
        {
            int[] userIds;
            if (e.Entity != null && !(userIds = e.Entity.UserIds).IsNullOrEmpty())
            {
                _ClearCache(e.Entity.UserIds);
            }
        }

        /// <summary>
        /// 申请安全码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="securityLevel"></param>
        /// <returns></returns>
        public Sid AcquireSid(int userId, SecurityLevel securityLevel)
        {
            Sid sid = _service.Invoker.Security.AcquireSid(userId, securityLevel);
            _sidCache.Add(sid, new SidInfo { UserId = userId, SecurityLevel = securityLevel });
            return sid;
        }

        /// <summary>
        /// 获取用户的详细信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="throwError">当该用户不存在时，是否抛出异常</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns></returns>
        public UserDetailInfo GetUserDetailInfo(int userId, bool throwError = false, bool? refresh = null)
        {
            UserDetailInfo info = _userInfoCache.Get(new UserCacheKey { Mask = UserInfoMask.Detail, UserId = userId },
                _GetRefresh(userId, refresh)) as UserDetailInfo;

            if (info == null && throwError)
                throw new ServiceException(UserStatusCode.InvalidUser);

            return info;
        }

        /// <summary>
        /// 获取用户的详细信息
        /// </summary>
        /// <param name="userIds">用户ID</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns></returns>
        public UserDetailInfo[] GetUserDetailInfos(int[] userIds, bool? refresh = null)
        {
            if (userIds.IsNullOrEmpty())
                return Array<UserDetailInfo>.Empty;

            var keys = userIds.ToArray(userId => new UserCacheKey { Mask = UserInfoMask.Detail, UserId = userId });
            KeyValuePair<UserCacheKey, object>[] infos = _userInfoCache.GetRange(keys, _GetRefresh(userIds, refresh));

            return infos.ToArray(info => info.Value as UserDetailInfo);
        }

        protected override void OnExecuteTask(string taskName)
        {
            if (_infoChangedUserIds.Count > 0)
            {
                int[] userIds = _infoChangedUserIds.ToArray(includeDeleted: false, clear: true);
                if (userIds.Length > 0)
                    _service.ServiceEvent.Raise(new User_InfoChanged_Event() { UserIds = userIds });
            }
        }
    }
}

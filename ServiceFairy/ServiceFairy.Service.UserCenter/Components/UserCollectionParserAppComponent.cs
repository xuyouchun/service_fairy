using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Utility;
using Common;
using ServiceFairy.Entities;
using System.Diagnostics.Contracts;
using Common.Package;
using CacheChain = Common.Package.CacheChain<string, object>;
using ICacheChainNode = Common.Package.ICacheChainNode<string, object>;
using DistribuedCacheChainNode = ServiceFairy.SystemInvoke.DistribuedCacheChainNode<string, object>;
using Common.Data;
using ServiceFairy.SystemInvoke;
using ServiceFairy.DbEntities.User;
using Common.Data.SqlExpressions;
using Common.Data.UnionTable;
using ServiceFairy.DbEntities;
using ServiceFairy.Entities.UserCenter;

namespace ServiceFairy.Service.UserCenter.Components
{
    /// <summary>
    /// 用户集合解析器
    /// </summary>
    [AppComponent("用户集合解析器", "将用户组解析为用户ID")]
    class UserCollectionParserAppComponent : AppComponent
    {
        public UserCollectionParserAppComponent(Service service)
            : base(service)
        {
            _service = service;
            _utConProvider = new RemoteUtConnectionProvider(_service.Invoker, DbEntityUtility.LoadReviseInfo());

            _usernameCache = new CacheChain(new ICacheChainNode[] {
                CacheChain.CreateMemoryCacheNode(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(5)),
                new DistribuedCacheChainNode(_service.Invoker, TimeSpan.FromHours(1), "s_un_"),
                CacheChain.CreateLoaderNode(_LoadUserIds)
            });

            _allOnlineUsers = new SingleMemoryCache<int[]>(TimeSpan.FromSeconds(5),
                () =>  _service.RoutableUserConnectionManager.GetAllOnlineUsers(true)
            );
        }

        private readonly Service _service;
        private readonly IUtConnectionProvider _utConProvider;
        private readonly CacheChain _usernameCache;
        private readonly SingleMemoryCache<int[]> _allOnlineUsers;

        /// <summary>
        /// 将自定义文法表示的批量用户转换为用户ID
        /// </summary>
        /// <param name="users">用户</param>
        /// <param name="currentUserId">当前用户ID</param>
        /// <returns></returns>
        public int[] Parse(string users)
        {
            if (string.IsNullOrEmpty(users))
                return Array<int>.Empty;

            HashSet<int> hs = new HashSet<int>();
            foreach (string user0 in users.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string user = user0.Trim();
                bool add = true;  // 标识是添加还是删除
                if (user.StartsWith('-'))
                {
                    user = user.Substring(1);
                    add = false;

                    switch (user.Trim().ToLower())
                    {
                        case UserCollectionPrefix.Online:
                            hs.ExceptWith(_allOnlineUsers.Get());
                            continue;

                        case UserCollectionPrefix.Offline:
                            hs.IntersectWith(_allOnlineUsers.Get());
                            continue;
                    }
                }

                IList<int> userIds = _ParseToUserIds(user);
                if (!userIds.IsNullOrEmpty())
                {
                    if (add)
                        hs.AddRange(userIds);
                    else
                        hs.RemoveRange(userIds);
                }
            }

            return hs.ToArray();
        }

        // 获取粉丝
        private IList<int> _GetFollowers(IList<int> userIds)
        {
            List<int> list = new List<int>();
            foreach (int userId in userIds)
            {
                int[] followerIds = _service.UserRelationManager.GetFollowers(userId);
                if (!followerIds.IsNullOrEmpty())
                    list.AddRange(followerIds);
            }

            return list;
        }

        // 获取关注
        private IList<int> _GetFollowings(IList<int> userIds)
        {
            List<int> list = new List<int>();
            foreach (int userId in userIds)
            {
                int[] followingIds = _service.UserRelationManager.GetFollowings(userId);
                if (!followingIds.IsNullOrEmpty())
                    list.AddRange(followingIds);
            }

            return list;
        }

        // 转换为UserId
        private IList<int> _ParseToUserIds(string user)
        {
            int index = user.IndexOf(':');
            if (index < 0)
            {
                switch (user.Trim().ToLower())
                {
                    case UserCollectionPrefix.Online:
                        return _allOnlineUsers.Get();

                    case UserCollectionPrefix.Offline: // 由于数据量问题，不支持该方式
                        return Array<int>.Empty;

                    case UserCollectionPrefix.Me:  // 无用户上下文环境中，不支持该方式
                        return Array<int>.Empty;
                }

                return _ParseUserIdStringToUserIds(user, false);
            }
            else
            {
                string prefix = user.Substring(0, index), value = user.Substring(index + 1);
                switch (prefix.ToLower().Trim())
                {
                    case UserCollectionPrefix.Follower:  // 粉丝
                        return _GetFollowers(_ParseToUserIds(value));

                    case UserCollectionPrefix.Following:  // 关注
                        return _GetFollowings(_ParseToUserIds(value));

                    case UserCollectionPrefix.UserId:   // 用户ID
                        return _ParseUserIdStringToUserIds(value, false);

                    case UserCollectionPrefix.UserName:  // 用户名
                        return _ParseUserNamesToUserIds(value);

                    case UserCollectionPrefix.Group:   // 群组
                        return _ParseGroupIdStringToUserIds(value);

                    case UserCollectionPrefix.GroupName:  // 群组名
                        return _ParseGroupNamesToUserIds(value);

                    case UserCollectionPrefix.UserGroupMember:  // 用户所属组的所有成员
                        return _ParseUserGroupMemersToUserIds(value, false);

                    default:
                        return Array<int>.Empty;
                }
            }
        }

        // 将群组ID转换为用户ID
        private IList<int> _ParseGroupIdStringToUserIds(string groupIds)
        {
            if (string.IsNullOrWhiteSpace(groupIds))
                return Array<int>.Empty;

            List<int> userIds = new List<int>();
            foreach (string sGroupId in groupIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                int groupId;
                if (int.TryParse(sGroupId, out groupId))
                    userIds.AddRange(ParseGroup(groupId));
            }

            return userIds;
        }

        // 将组名转换为用户ID
        private IList<int> _ParseGroupNamesToUserIds(string groupNames)
        {
            if (string.IsNullOrWhiteSpace(groupNames))
                return Array<int>.Empty;

            List<int> userIds = new List<int>();
            foreach (string groupName in groupNames.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string g = groupName.Trim();
                if (g.Length > 0)
                    userIds.AddRange(ParseGroupName(g));
            }

            return userIds;
        }

        /// <summary>
        /// 将组名转换为用户ID
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public int[] ParseGroupName(string groupName)
        {
            return Array<int>.Empty;
        }

        /// <summary>
        /// 将组id转换为用户ID
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public int[] ParseGroup(int groupId)
        {
            GroupInfos item = _service.GroupInfoManager.GetGroupInfo(groupId, GroupInfoMask.Member);
            if (item == null || item.MemberInfos.IsNullOrEmpty())
                return Array<int>.Empty;

            return item.MemberInfos.ToArray(mi => mi.UserId);
        }

        // 将逗号分隔的字符串形式的userid转换为int型
        private IList<int> _ParseUserIdStringToUserIds(string userIds, bool throwError)
        {
            if (string.IsNullOrEmpty(userIds))
                return Array<int>.Empty;

            List<int> list = new List<int>();
            foreach (string user in userIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (string.IsNullOrWhiteSpace(user))
                    continue;

                int userId;
                if (!int.TryParse(user, out userId))
                {
                    if (throwError)
                        throw new ServiceException(ServiceStatusCode.BusinessError, "错误的用户ID：" + user);
                }
                else
                {
                    if (userId != 0)
                        list.Add(userId);
                }
            }

            return list;
        }

        // 将逗号分隔的字符串形式的username转换为userid
        private IList<int> _ParseUserNamesToUserIds(string userNames)
        {
            if (string.IsNullOrWhiteSpace(userNames))
                return Array<int>.Empty;

            string[] usernameArr = userNames.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim()).Where(s => s.Length > 0).ToArray();

            return _usernameCache.GetRange(usernameArr).ToArray(v => (int)v.Value);
        }

        // 获取用户所属组的所有成员
        private IList<int> _ParseUserGroupMemersToUserIds(string userIds, bool throwError)
        {
            if (string.IsNullOrWhiteSpace(userIds))
                return Array<int>.Empty;

            List<int> list = new List<int>();
            foreach (string user in userIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                int userId;
                if (!int.TryParse(user, out userId))
                {
                    if (throwError)
                        throw new ServiceException(ServiceStatusCode.BusinessError, "错误的用户ID：" + user);
                }
                else
                {
                    //int[] groupIds = 
                }
            }

            return list;
        }

        /// <summary>
        /// 将用户组及用户ID合并为用户ID数组
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        public int[] ParseAndCombineToUserIds(IEnumerable<int> userIds, IEnumerable<string> users)
        {
            HashSet<int> hs = new HashSet<int>();

            if (userIds != null)
                hs.AddRange(userIds);

            if (users != null)
            {
                foreach (string user in users)
                {
                    hs.AddRange(Parse(user));
                }
            }

            return hs.ToArray();
        }

        /// <summary>
        /// 将用户组及用户ID合并为用户ID数组
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int[] Parse(Users users)
        {
            Contract.Requires(users != null);

            return ParseAndCombineToUserIds(users.UserIds, new[] { users.Exp });
        }

        private KeyValuePair<string, object>[] _LoadUserIds(string[] usernames, bool refresh)
        {
            if (usernames.Length == 0)
                return Array<KeyValuePair<string, object>>.Empty;

            SqlExpression where = SqlExpression.Equals(DbUserRelation.F_FollowerId, 0);
            KeyValuePair<string, object>[] result = DbUserRelation.SelectKeyValues<string, object>(_utConProvider,
                DbUserRelation.F_UserName, usernames, null, DbUserRelation.F_UserId, (string)where);

            return result;
        }

        /// <summary>
        /// 将用户名转换为用户ID
        /// </summary>
        /// <param name="usernames"></param>
        /// <returns></returns>
        public int[] ConvertUserNames(string[] usernames)
        {
            Contract.Requires(usernames != null);
            if (usernames.Length == 0)
                return Array<int>.Empty;

            if (usernames.Length == 1)
            {
                int userId = ParseUserName(usernames[0]);
                return new[] { userId };
            }

            usernames = usernames.ToArray(un => un.Trim());
            Dictionary<string, object> dict = _usernameCache.GetRange(usernames).ToDictionary(item => item.Key, item => item.Value);
            return usernames.ToArray(name => (int)dict.GetOrDefault(name, 0));
        }

        /// <summary>
        /// 将用户名转换为用户ID
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int ParseUserName(string username)
        {
            Contract.Requires(username != null);

            return (int)(_usernameCache.Get(username) ?? 0);
        }

        /// <summary>
        /// 转换为单一的用户ID，如果不能够转换为单一用户ID，则抛出异常
        /// </summary>
        /// <param name="creator"></param>
        /// <returns></returns>
        public int ParseSingleUser(string user, string errorMsg = null)
        {
            int[] userIds = Parse(user);
            if (userIds.Length != 1)
                throw new ArgumentException(errorMsg);

            return userIds[0];
        }

        /// <summary>
        /// 将用户集合转换为用户名
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public string[] ParseToUserNames(Users users)
        {
            int[] userIds = Parse(users);
            UserBasicInfo[] basicInfos = _service.UserInfoManager.GetUserBasicInfos(userIds);
            return basicInfos.IsNullOrEmpty() ? Array<string>.Empty : basicInfos.ToArray(bi => bi.UserName);
        }

        /// <summary>
        /// 将用户名转换为ID
        /// </summary>
        /// <param name="usernames"></param>
        /// <returns></returns>
        public IDictionary<string, int> ConvertUserNameToIds(string[] usernames)
        {
            Contract.Requires(usernames != null);

            int[] userIds = ConvertUserNames(usernames);
            Dictionary<string, int> dict = new Dictionary<string, int>();
            for (int k = 0; k < userIds.Length; k++)
            {
                string username = usernames[k];
                int userid = userIds[k];
                if (!string.IsNullOrEmpty(username) && userid != 0)
                {
                    dict[username] = userid;
                }
            }

            return dict;
        }

        /// <summary>
        /// 将用户ID转换为用户名
        /// </summary>
        /// <param name="userids"></param>
        /// <returns></returns>
        public IDictionary<int, string> ConvertUserIdToNames(int[] userIds)
        {
            Contract.Requires(userIds != null);

            UserBasicInfo[] basicInfos = _service.UserInfoManager.GetUserBasicInfos(userIds);
            return basicInfos.ToDictionary(bi => bi.UserId, bi => bi.UserName, true);
        }
    }
}

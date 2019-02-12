using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities;
using Common;
using Common.Utility;
using Common.Package;
using Common.Framework.TrayPlatform;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace ServiceFairy.Components
{
    /// <summary>
    /// 用户在线信息
    /// </summary>
    [AppComponent("用户组解析器", "调用用户中心服务，将用户组解析为用户ID，并缓存在本地", AppComponentCategory.System, "Sys_UserParser")]
    public class UserParserAppComponent : AppComponent
    {
        public UserParserAppComponent(SystemAppServiceBase service)
            : base(service)
        {
            _service = service;

            _cache = new CacheChain<string, int[]>(new ICacheChainNode<string, int[]>[] {
                CacheChain<string, int[]>.CreateWeakReferenceCacheNode(),
                //new TrayCacheChainNode<string, int[]>(_service.Context.CacheManager.Get<string, int[]>("USER_PARSE", true, true), TimeSpan.FromMinutes(1)),
                CacheChain<string, int[]>.CreateLoaderNode(_ParseUsers),
            });
        }

        private readonly SystemAppServiceBase _service;
        private readonly CacheChain<string, int[]> _cache;

        private KeyValuePair<string, int[]>[] _ParseUsers(string[] users, bool refresh)
        {
            return users.ToArray(user => new KeyValuePair<string, int[]>(user, Parse(user, refresh)));
        }

        /// <summary>
        /// 将用户组解析为用户ID
        /// </summary>
        /// <param name="users"></param>
        /// <param name="refresh"></param>
        /// <returns></returns>
        public int[] Parse(string users, int curUser, bool refresh = false)
        {
            return Parse(ReviseMe(users, curUser), refresh);
        }

        /// <summary>
        /// 将用户组解析为用户ID
        /// </summary>
        /// <param name="users"></param>
        /// <param name="uss"></param>
        /// <param name="refresh"></param>
        /// <returns></returns>
        public int[] Parse(string users, UserSessionState uss, bool refresh = false)
        {
            return Parse(users, _GetUserId(uss), refresh);
        }

        /// <summary>
        /// 将用户组解析为用户ID
        /// </summary>
        /// <param name="users"></param>
        /// <param name="refresh"></param>
        /// <returns></returns>
        public int[] Parse(string users, bool refresh = false)
        {
            return _service.Invoker.UserCenter.ParseUsers(users);
        }

        /// <summary>
        /// 将用户组解析为用户ID
        /// </summary>
        /// <param name="users"></param>
        /// <param name="curUser"></param>
        /// <param name="refresh"></param>
        /// <returns></returns>
        public int[] Parse(Users users, int curUser, bool refresh = false)
        {
            if (Users.IsEmpty(users))
                return Array<int>.Empty;

            if (users.UserIds.IsNullOrEmpty())
                return Parse(users.Exp, curUser, refresh);

            HashSet<int> hs = new HashSet<int>(Parse(users.Exp, curUser, refresh));
            hs.AddRange(users.UserIds.Where(uid => uid != 0));
            return hs.ToArray();
        }

        /// <summary>
        /// 将用户组解析为用户ID
        /// </summary>
        /// <param name="users"></param>
        /// <param name="uss"></param>
        /// <param name="refresh"></param>
        /// <returns></returns>
        public int[] Parse(Users users, UserSessionState uss, bool refresh = false)
        {
            return Parse(users, _GetUserId(uss), refresh);
        }

        /// <summary>
        /// 将用户组解析为用户ID
        /// </summary>
        /// <param name="users"></param>
        /// <param name="refresh"></param>
        /// <returns></returns>
        public int[] Parse(Users users, bool refresh = false)
        {
            return Parse(users, 0, refresh);
        }

        /// <summary>
        /// 将用户名解析为用户ID
        /// </summary>
        /// <param name="username"></param>
        /// <param name="refresh"></param>
        /// <returns></returns>
        public int ParseUserNameToId(string username, bool refresh = false)
        {
#warning 直接从数据库中查吧！
            int[] userIds = Parse(Users.FromUserName(username), refresh);
            return userIds.FirstOrDefault();
        }

        /// <summary>
        /// 转换为用户名
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public string[] ParseToUserNames(Users users)
        {
            if (users == null)
                return Array<string>.Empty;

            return _service.Invoker.UserCenter.ParseToUserNames(users);
        }

        /// <summary>
        /// 转换为用户名
        /// </summary>
        /// <param name="users"></param>
        /// <param name="curUserId"></param>
        /// <returns></returns>
        public string[] ParseToUserNames(Users users, int curUserId)
        {
            return ParseToUserNames(ReviseMe(users, curUserId));
        }

        /// <summary>
        /// 转换为用户名
        /// </summary>
        /// <param name="users"></param>
        /// <param name="uss"></param>
        /// <returns></returns>
        public string[] ParseToUserNames(Users users, UserSessionState uss)
        {
            return ParseToUserNames(users, _GetUserId(uss));
        }

        private static int _GetUserId(UserSessionState uss)
        {
            return uss == null ? 0 : uss.BasicInfo.UserId;
        }

        /// <summary>
        /// 转换为用户名
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public string[] ParseToUserNames(string users)
        {
            return ParseToUserNames(new Users(users));
        }

        /// <summary>
        /// 通过用户ID获取用户名
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public IDictionary<int, string> ConvertUserIdToNamesDict(int[] userIds)
        {
            Contract.Requires(userIds != null);

            return _service.Invoker.UserCenter.ConvertUserIdToNamesDict(userIds);
        }

        /// <summary>
        /// 通过用户ID获取用户名
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public string[] ConvertUserIdToNames(int[] userIds)
        {
            Contract.Requires(userIds != null);

            return _service.Invoker.UserCenter.ConvertUserIdToNames(userIds);
        }

        /// <summary>
        /// 通过用户ID获取用户名
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string ConvertUserIdToName(int userId)
        {
            return _service.Invoker.UserCenter.ConvertUserIdToName(userId);
        }

        /// <summary>
        /// 通过用户名获取用户ID
        /// </summary>
        /// <param name="usernames"></param>
        /// <returns></returns>
        public IDictionary<string, int> ConvertUserNameToIdsDict(string[] usernames)
        {
            return _service.Invoker.UserCenter.ConvertUserNameToIdsDict(usernames);
        }

        /// <summary>
        /// 通过用户名获取用户ID
        /// </summary>
        /// <param name="usernames"></param>
        /// <returns></returns>
        public int[] ConvertUserNameToIds(string[] usernames)
        {
            return _service.Invoker.UserCenter.ConvertUserNameToIds(usernames);
        }

        /// <summary>
        /// 通过用户名获取用户ID
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int ConvertUserNameToId(string username)
        {
            return _service.Invoker.UserCenter.ConvertUserNameToId(username);
        }

        /// <summary>
        /// 修正用户集合，将其中的me，替换为当前userid
        /// </summary>
        /// <param name="users"></param>
        /// <param name="uss"></param>
        /// <returns></returns>
        public Users ReviseMe(Users users, UserSessionState uss)
        {
            return ReviseMe(users, _GetUserId(uss));
        }

        /// <summary>
        /// 修正用户集合，将其中的me，替换为当前userid
        /// </summary>
        /// <param name="users"></param>
        /// <param name="curUserId"></param>
        /// <returns></returns>
        public Users ReviseMe(Users users, int curUserId)
        {
            return new Users(users.UserIds, ReviseMe(users.Exp, curUserId));
        }

        /// <summary>
        /// 修正用户集合，将其中的me，替换为当前userid
        /// </summary>
        /// <param name="users"></param>
        /// <param name="curUserId"></param>
        /// <returns></returns>
        public string ReviseMe(string users, int curUserId)
        {
            if (users == null || users.IndexOf(UserCollectionPrefix.Me, StringComparison.OrdinalIgnoreCase) < 0)
                return users;

            var list = from user in users.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                       let newUser = _ReviseMe(user, curUserId)
                       where !string.IsNullOrWhiteSpace(newUser)
                       select newUser;

            return list.JoinBy(";");
        }

        /// <summary>
        /// 修正用户集合，将其中的me，替换为当前userid
        /// </summary>
        /// <param name="users"></param>
        /// <param name="uss"></param>
        /// <returns></returns>
        public string ReviseMe(string users, UserSessionState uss)
        {
            return ReviseMe(users, _GetUserId(uss));
        }

        private string _ReviseMe(string user, int curUserId)
        {
            if (string.Equals(user, UserCollectionPrefix.Me, StringComparison.OrdinalIgnoreCase))
                return curUserId == 0 ? null : curUserId.ToString();

            int index = user.IndexOf(':');
            if (index < 0)
                return null;

            string type = user.Substring(0, index), value = user.Substring(index + 1);
            StringBuilder sb = new StringBuilder();
            sb.Append(type).Append(":");

            int k = 0;
            foreach (string item in value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string v = item.Trim();
                if (string.Equals(item, UserCollectionPrefix.Me, StringComparison.OrdinalIgnoreCase))
                {
                    if (curUserId == 0)
                        continue;

                    v = curUserId.ToString();
                }

                if (k++ > 0)
                    sb.Append(",");

                sb.Append(v);
            }

            return k == 0 ? null : sb.ToString();
        }
    }
}

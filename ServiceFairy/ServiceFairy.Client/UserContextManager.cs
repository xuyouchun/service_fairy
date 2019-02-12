using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using ServiceFairy.SystemInvoke;
using System.Collections.Concurrent;
using Common.Utility;

namespace ServiceFairy.Client
{
    /// <summary>
    /// 用户状态管理器
    /// </summary>
    public class UserContextManager : IEnumerable<UserContext>
    {
        public UserContextManager(string navigation)
        {
            Contract.Requires(navigation != null);

            _userConMgr = UserConnectionManager.FromNavigation(navigation);
            _invoker = SystemInvoker.FromNavigation(navigation);
        }

        private readonly UserConnectionManager _userConMgr;
        private readonly SystemInvoker _invoker;
        private readonly ConcurrentDictionary<string, UserContext> _usernameDict = new ConcurrentDictionary<string, UserContext>();
        private readonly ConcurrentDictionary<int, UserContext> _userIdDict = new ConcurrentDictionary<int, UserContext>();

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserContext Add(string username, string password)
        {
            Contract.Requires(username != null);

            UserContext[] arr = Add(new[] { new UserInfo(username, password) });
            return arr.FirstOrDefault();
        }

        /// <summary>
        /// 批量创建
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public UserContext[] Add(UserInfo[] infos)
        {
            Contract.Requires(infos != null);

            List<UserContext> list = new List<UserContext>();

            foreach (UserInfo info in infos)
            {
                string username = info.UserName, password = info.Password;
                if (_usernameDict.ContainsKey(username))
                {
                    list.Add(_usernameDict[username]);
                }

                UserContext userCtx = new UserContext(username, password, _userConMgr);
                _usernameDict[username] = userCtx;
                list.Add(userCtx);
            }

            int[] userIds = _invoker.User.GetUserIds(infos.ToArray(i => i.UserName));
            for (int k = 0; k < list.Count; k++)
            {
                int userId = userIds[k];
                _userIdDict[userId] = list[k];
            }

            return list.ToArray();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserContext Remove(string username)
        {
            Contract.Requires(username != null);

            UserContext userCtx;
            _usernameDict.TryRemove(username, out userCtx);
            return userCtx;
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <returns></returns>
        public UserContext[] GetAll()
        {
            return _usernameDict.Values.ToArray();
        }

        /// <summary>
        /// 获取指定用户的状态
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserContext Get(string username)
        {
            UserContext ctx;
            if (!_usernameDict.TryGetValue(username, out ctx))
            {
                
            }

            return ctx;
        }

        /// <summary>
        /// 获取指定用户的状态
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserContext Get(int userId)
        {
            UserContext ctx;
            if (!_userIdDict.TryGetValue(userId, out ctx))
            {

            }

            return ctx;
        }

        public string GetUserDesc(int userId)
        {
            UserContext ctx = Get(userId);
            if (ctx == null)
                return userId.ToString();

            return userId + "(" + ctx.UserName + ")";
        }

        public string GetUserDesc(int[] userIds)
        {
            return userIds.Select(userId => GetUserDesc(userId)).JoinBy(", ");
        }

        public IEnumerator<UserContext> GetEnumerator()
        {
            return _usernameDict.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

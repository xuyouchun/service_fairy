using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Utility;
using System.Diagnostics.Contracts;
using Pfx = Common.Contracts.Service.UserCollectionPrefix;
using Common;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 用户集合
    /// </summary>
    [Serializable, DataContract]
    public class Users
    {
        public Users()
        {

        }

        public Users(int[] userIds)
        {
            UserIds = userIds;
        }

        public Users(string exp)
        {
            Exp = exp;
        }

        public Users(int[] userIds, string exp)
        {
            UserIds = userIds;
            Exp = exp;
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int[] UserIds { get; private set; }

        /// <summary>
        /// 用户集表达式
        /// </summary>
        [DataMember]
        public string Exp { get; private set; }

        public static Users Contact(params Users[] ucs)
        {
            HashSet<int> userIds = new HashSet<int>();
            StringBuilder users = new StringBuilder();

            foreach (Users uc in ucs)
            {
                if (!uc.UserIds.IsNullOrEmpty())
                    userIds.AddRange(uc.UserIds);

                if (string.IsNullOrWhiteSpace(uc.Exp))
                {
                    if (users.Length > 0)
                        users.Append(";");

                    users.Append(uc.Exp);
                }
            }

            return new Users(userIds.ToArray(), users.ToString());
        }

        public static Users Except(Users uc1, Users uc2)
        {
            if (IsEmpty(uc1))
                return Empty;

            bool uc2Empty = IsEmpty(uc2);
            if (uc2Empty)
                return uc1;

            int[] userIds = uc1.UserIds.IsNullOrEmpty() ? Array<int>.Empty : uc2.UserIds.IsNullOrEmpty() ?
                uc1.UserIds : uc1.UserIds.Except(uc2.UserIds).ToArray();

            string users = string.IsNullOrWhiteSpace(uc1.Exp) ? string.Empty : uc2Empty ? uc1.Exp : (uc1.Exp + ";-" + uc2);
            return new Users(userIds, users);
        }

        public static Users operator +(Users uc1, Users uc2)
        {
            return Contact(uc1, uc2);
        }

        public static Users operator -(Users uc1, Users uc2)
        {
            return Except(uc1, uc2);
        }

        /// <summary>
        /// 用户的所有关注
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Users FromFollowings(int userId)
        {
            return new Users(Pfx.Following + ":" + userId);
        }

        /// <summary>
        /// 用户所有在线关注
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Users FromOnlineFollowings(int userId)
        {
            return FromFollowings(userId) - FromOffline();
        }

        /// <summary>
        /// 当前用户的所有关注
        /// </summary>
        /// <returns></returns>
        public static Users FromMyFollowings()
        {
            return new Users(Pfx.Following + ":" + Pfx.Me);
        }

        /// <summary>
        /// 当前用户所有的在线关注
        /// </summary>
        /// <returns></returns>
        public static Users FromMyOnlineFollowings()
        {
            return FromMyFollowings() - FromOffline();
        }

        /// <summary>
        /// 用户的所有粉丝
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Users FromFollowers(int userId)
        {
            return new Users(Pfx.Follower + ":" + userId);
        }

        /// <summary>
        /// 用户所有在线粉丝
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Users FromOnlineFollowers(int userId)
        {
            return FromFollowers(userId) - FromOffline();
        }

        /// <summary>
        /// 当前用户的所有粉丝
        /// </summary>
        /// <returns></returns>
        public static Users FromMyFollowers()
        {
            return new Users(Pfx.Follower + ":" + Pfx.Me);
        }

        /// <summary>
        /// 当前用户所有的在线粉丝
        /// </summary>
        /// <returns></returns>
        public static Users FromMyOnlineFollowers()
        {
            return FromMyFollowers() - FromOffline();
        }

        /// <summary>
        /// 用户ID集合
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public static Users FromUserIds(params int[] userIds)
        {
            return new Users() { UserIds = userIds };
        }

        /// <summary>
        /// 用户ID集合
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Users FromUserId(int userId)
        {
            return FromUserIds(new[] { userId });
        }

        /// <summary>
        /// 用户集合
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public static Users FromUsers(string users)
        {
            return new Users() { Exp = users };
        }

        /// <summary>
        /// 从用户名转换
        /// </summary>
        /// <param name="usernames"></param>
        /// <returns></returns>
        public static Users FromUserNames(params string[] usernames)
        {
            Contract.Requires(usernames != null);

            return new Users() {
                Exp = _Join(Pfx.UserName, usernames)
            };
        }

        private static string _Join(string pfx, string[] ss)
        {
            StringBuilder sb = new StringBuilder();
            int index = 0;

            foreach (string s in ss)
            {
                if (string.IsNullOrWhiteSpace(s))
                    continue;

                if (index++ == 0)
                    sb.Append(Pfx.UserName).Append(":");
                else
                    sb.Append(",");

                sb.Append(s);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 从用户名转换
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static Users FromUserName(string username)
        {
            return new Users() { Exp = string.IsNullOrWhiteSpace(username) ? "" : (Pfx.UserName + ":" + username) };
        }

        /// <summary>
        /// 从组创建
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static Users FromGroupId(int groupId)
        {
            return new Users() { Exp = Pfx.Group + ":" + groupId };
        }

        /// <summary>
        /// 从组创建
        /// </summary>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public static Users FromGroupIds(params int[] groupIds)
        {
            return new Users() { Exp = groupIds.IsNullOrEmpty() ? "" : (Pfx.Group + ":" + groupIds.JoinBy(",")) };
        }

        /// <summary>
        /// 从组名创建
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static Users FromGroupName(string groupName)
        {
            return new Users() { Exp = string.IsNullOrWhiteSpace(groupName) ? "" : (Pfx.GroupName + ":" + groupName) };
        }

        /// <summary>
        /// 从组名创建
        /// </summary>
        /// <param name="groupNames"></param>
        /// <returns></returns>
        public static Users FromGroupName(params string[] groupNames)
        {
            return new Users() { Exp = _Join(Pfx.GroupName, groupNames) };
        }

        /// <summary>
        /// 用户所属组的所有成员
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Users FromUserGroupMembers(int userId)
        {
            return new Users() { Exp = Pfx.UserGroupMember + ":" + userId };
        }

        /// <summary>
        /// 用户所属组的所有成员
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public static Users FromUsersGroupMembers(params int[] userIds)
        {
            Contract.Requires(userIds != null);

            return new Users() { Exp = Pfx.UserGroupMember + ":" + userIds.JoinBy(",") };
        }

        /// <summary>
        /// 所有在线用户
        /// </summary>
        /// <returns></returns>
        public static Users FromOnline()
        {
            return new Users() { Exp = Pfx.Online };
        }

        /// <summary>
        /// 所有离线用户
        /// </summary>
        /// <returns></returns>
        public static Users FromOffline()
        {
            return new Users() { Exp = Pfx.Offline };
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        /// <returns></returns>
        public static Users FromMe()
        {
            return new Users(Pfx.UserId + ":" + Pfx.Me);
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public static bool IsEmpty(Users users)
        {
            if (users == null)
                return true;

            return users.UserIds.IsNullOrEmpty() && string.IsNullOrEmpty(users.Exp);
        }

        /// <summary>
        /// 空的用户集合
        /// </summary>
        public static readonly Users Empty = new Users();

        public override string ToString()
        {
            if (UserIds.IsNullOrEmpty())
                return Exp;

            if (string.IsNullOrEmpty(Exp))
                return Pfx.UserId + ":" + UserIds.JoinBy(";");

            return Pfx.UserId + ":" + UserIds.JoinBy(";") + ";" + Exp;
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return UserIds.IsNullOrEmpty() && string.IsNullOrWhiteSpace(Exp);
        }

        /// <summary>
        /// 是否为空引用或未包含任何内容
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(Users users)
        {
            return users == null || users.IsEmpty();
        }

        public static explicit operator string(Users users)
        {
            if (users == null)
                return string.Empty;

            return users.ToString();
        }

        public static explicit operator Users(string users)
        {
            if (string.IsNullOrWhiteSpace(users))
                return Empty;

            return new Users(users);
        }
    }

    /// <summary>
    /// 用户集合的前缀
    /// </summary>
    public static class UserCollectionPrefix
    {
        /// <summary>
        /// 关注者前缀
        /// </summary>
        public const string Following = "fg";

        /// <summary>
        /// 粉丝前缀
        /// </summary>
        public const string Follower = "fr";

        /// <summary>
        /// 组前缀
        /// </summary>
        public const string Group = "g";

        /// <summary>
        /// 组名前缀
        /// </summary>
        public const string GroupName = "gn";

        /// <summary>
        /// 用户ID前缀
        /// </summary>
        public const string UserId = "u";

        /// <summary>
        /// 用户名前缀
        /// </summary>
        public const string UserName = "un";

        /// <summary>
        /// 用户所属组的所有成员
        /// </summary>
        public const string UserGroupMember = "ug";

        /// <summary>
        /// 所有在线用户
        /// </summary>
        public const string Online = "online";

        /// <summary>
        /// 所有离线用户
        /// </summary>
        public const string Offline = "offline";

        /// <summary>
        /// 当前用户
        /// </summary>
        public const string Me = "me";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using ServiceFairy.Entities.UserCenter;
using Common.Contracts.Service;
using Common.Utility;
using Common;
using ServiceFairy.Entities;
using System.Diagnostics.Contracts;

namespace ServiceFairy.SystemInvoke
{
    partial class SystemInvoker
    {
        private UserCenterInvoker _userCenter;

        /// <summary>
        /// UserCenter Service
        /// </summary>
        public UserCenterInvoker UserCenter
        {
            get { return _userCenter ?? (_userCenter = new UserCenterInvoker(this)); }
        }

        /// <summary>
        /// 用户中心服务
        /// </summary>
        public class UserCenterInvoker : Invoker
        {
            public UserCenterInvoker(SystemInvoker owner)
                : base(owner)
            {
                
            }

            /// <summary>
            /// 保持用户连接状态
            /// </summary>
            /// <param name="conInfos">用户连接信息</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult KeepUserConnectionSr(UserConnectionInfo[] conInfos, bool enableRoute = true, CallingSettings settings = null)
            {
                return UserCenterService.KeepUserConnection(Sc,
                    new UserCenter_KeepUserConnection_Request() { ConnectionInfos = conInfos, EnableRoute = enableRoute }, settings);
            }

            /// <summary>
            /// 保持用户连接状态
            /// </summary>
            /// <param name="conInfos">用户连接信息</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void KeepUserConnection(UserConnectionInfo[] conInfos, bool enableRoute = true, CallingSettings settings = null)
            {
                InvokeWithCheck(KeepUserConnectionSr(conInfos, enableRoute, settings));
            }

            /// <summary>
            /// 用户连接断开通知
            /// </summary>
            /// <param name="conInfos">用户连接信息</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult UserDisconnectedNotifySr(UserDisconnectedInfo[] conInfos, bool enableRoute = true, CallingSettings settings = null)
            {
                return UserCenterService.UserDisconnectedNotify(Sc,
                    new UserCenter_UserDisconnectedNotify_Request { DisconnectionInfos = conInfos, EnableRoute = enableRoute }, settings);
            }

            /// <summary>
            /// 用户连接断开通知
            /// </summary>
            /// <param name="conInfos">用户连接信息</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            public void UserDisconnectedNotify(UserDisconnectedInfo[] conInfos, bool enableRoute = true, CallingSettings settings = null)
            {
                InvokeWithCheck(UserDisconnectedNotifySr(conInfos, enableRoute, settings));
            }

            /// <summary>
            /// 获取用户的连接信息
            /// </summary>
            /// <param name="userIds"></param>
            /// <param name="enableRoute"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<UserConnectionInfo[]> GetUserConnectionInfosSr(int[] userIds, bool enableRoute = true, CallingSettings settings = null)
            {
                var sr = UserCenterService.GetUserConnectionInfos(Sc,
                    new UserCenter_GetUserConnectionInfos_Request() { UserIds = userIds, EnableRoute = enableRoute }, settings);

                return CreateSr(sr, r => r.Infos);
            }

            /// <summary>
            /// 获取用户的连接信息
            /// </summary>
            /// <param name="userIds"></param>
            /// <param name="enableRoute"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public UserConnectionInfo[] GetUserConnectionInfos(int[] userIds, bool enableRoute = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetUserConnectionInfosSr(userIds, enableRoute, settings));
            }

            /// <summary>
            /// 获取用户的连接信息
            /// </summary>
            /// <param name="userId"></param>
            /// <param name="enableRoute"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<UserConnectionInfo> GetUserConnectionInfoSr(int userId, bool enableRoute = true, CallingSettings settings = null)
            {
                var sr = GetUserConnectionInfosSr(new[] { userId }, enableRoute, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取用户的连接信息
            /// </summary>
            /// <param name="userId"></param>
            /// <param name="enableRoute"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public UserConnectionInfo GetUserConnectionInfo(int userId, bool enableRoute = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetUserConnectionInfoSr(userId, enableRoute, settings));
            }

            /// <summary>
            /// 获取所有的在线用户
            /// </summary>
            /// <param name="enableRoute"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<int[]> GetAllOnlineUsersSr(bool enableRoute = true, CallingSettings settings = null)
            {
                var sr = UserCenterService.GetAllOnlineUsers(Sc, new UserCenter_GetAllOnlineUsers_Request() { EnableRoute = enableRoute }, settings);
                return CreateSr(sr, r => r.UserIds);
            }

            /// <summary>
            /// 获取所有的在线用户
            /// </summary>
            /// <param name="enableRoute"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public int[] GetAllOnlineUsers(bool enableRoute = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetAllOnlineUsersSr(enableRoute, settings));
            }

            /// <summary>
            /// 判断指定的用户在指定的终端上是否存在
            /// </summary>
            /// <param name="userIds"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<int[]> ExistsUserSr(int[] userIds, CallingSettings settings = null)
            {
                var sr = UserCenterService.ExistsUser(Sc, new UserCenter_ExistsUser_Request() { UserIds = userIds }, settings);
                return CreateSr(sr, r => r.ExistsUserIds);
            }

            /// <summary>
            /// 判断指定的用户在指定的终端上是否存在
            /// </summary>
            /// <param name="userIds"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public int[] ExistsUser(int[] userIds, CallingSettings settings = null)
            {
                return InvokeWithCheck(ExistsUserSr(userIds, settings));
            }

            /// <summary>
            /// 判断指定的用户在指定的终端上是否存在
            /// </summary>
            /// <param name="userIds"></param>
            /// <param name="clientId"></param>
            /// <param name="sid"></param>
            /// <returns></returns>
            public ServiceResult<int[]> ExistsUserSr(int[] userIds, Guid clientId, Sid sid = default(Sid))
            {
                return ExistsUserSr(userIds, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 判断指定的用户在指定的终端上是否存在
            /// </summary>
            /// <param name="userIds"></param>
            /// <param name="clientId"></param>
            /// <param name="sid"></param>
            /// <returns></returns>
            public int[] ExistsUser(int[] userIds, Guid clientId, Sid sid = default(Sid))
            {
                return InvokeWithCheck(ExistsUserSr(userIds, clientId, sid));
            }

            /// <summary>
            /// 获取用户所在的终端
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<UserPosition[]> GetUserPositionsSr(int[] userIds, bool enableRoute = true, CallingSettings settings = null)
            {
                var sr = UserCenterService.GetUserPositions(Sc, new UserCenter_GetUserPositions_Request() {
                    UserIds = userIds, EnableRoute = enableRoute }, settings);

                return CreateSr(sr, r => r.Positions);
            }

            /// <summary>
            /// 获取用户所在的终端
            /// </summary>
            /// <param name="userIds"用户ID></param>
            /// <param name="enableRoute">是否允许路由</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public UserPosition[] GetUserPositions(int[] userIds, bool enableRoute = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetUserPositionsSr(userIds, enableRoute, settings));
            }

            /// <summary>
            /// 选取所有在线的用户
            /// </summary>
            /// <param name="userIds"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<int[]> SelectOnlineUsersSr(int[] userIds, CallingSettings settings = null)
            {
                var sr = GetUserPositionsSr(userIds, true, settings);
                return CreateSr(sr, r => r.SelectMany(v => v.UserIds ?? Array<int>.Empty).ToArray());
            }

            /// <summary>
            /// 选取所有在线的用户
            /// </summary>
            /// <param name="userIds"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public int[] SelectOnlineUsers(int[] userIds, CallingSettings settings = null)
            {
                return InvokeWithCheck(SelectOnlineUsersSr(userIds, settings));
            }

            /// <summary>
            /// 获取用户之间的关系
            /// </summary>
            /// <param name="userIds"></param>
            /// <param name="mask"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<UserRelation[]> GetRelationSr(int[] userIds, GetRelationMask mask, CallingSettings settings = null)
            {
                var sr = UserCenterService.GetRelation(Sc, new UserCenter_GetRelation_Request() {
                    Mask = mask, UserIds = userIds
                }, settings);

                return CreateSr(sr, r => r.Relations);
            }

            /// <summary>
            /// 获取用户之间的关系
            /// </summary>
            /// <param name="userIds"></param>
            /// <param name="mask"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public UserRelation[] GetRelation(int[] userIds, GetRelationMask mask = GetRelationMask.All, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetRelationSr(userIds, mask, settings));
            }

            /// <summary>
            /// 获取用户之间的关系
            /// </summary>
            /// <param name="userId"></param>
            /// <param name="mask"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<UserRelation> GetRelationSr(int userId, GetRelationMask mask = GetRelationMask.All, CallingSettings settings = null)
            {
                var sr = GetRelationSr(new[] { userId }, mask, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取用户之间的关系
            /// </summary>
            /// <param name="userId"></param>
            /// <param name="mask"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public UserRelation GetRelation(int userId, GetRelationMask mask = GetRelationMask.All, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetRelationSr(userId, mask, settings));
            }

            /// <summary>
            /// 获取粉丝
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<IDictionary<int, int[]>> GetFollowersSr(int[] userIds, CallingSettings settings = null)
            {
                var sr = GetRelationSr(userIds, GetRelationMask.Follower, settings);
                return CreateSr(sr, delegate(UserRelation[] rs) {
                    return (IDictionary<int, int[]>)rs.ToDictionary(r => r.UserId, r => r.Followers);
                });
            }

            /// <summary>
            /// 获取粉丝
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public IDictionary<int, int[]> GetFollowers(int[] userIds, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetFollowersSr(userIds, settings));
            }

            /// <summary>
            /// 获取粉丝
            /// </summary>
            /// <param name="userId"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<int[]> GetFollowersSr(int userId, CallingSettings settings = null)
            {
                var sr = GetRelationSr(userId, GetRelationMask.Follower, settings);
                return CreateSr(sr, r => r.Followers);
            }

            /// <summary>
            /// 获取粉丝
            /// </summary>
            /// <param name="userId"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public int[] GetFollowers(int userId, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetFollowersSr(userId, settings));
            }

            /// <summary>
            /// 获取关注
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<IDictionary<int, int[]>> GetFollowingsSr(int[] userIds, CallingSettings settings = null)
            {
                var sr = GetRelationSr(userIds, GetRelationMask.Following, settings);
                return CreateSr(sr, delegate(UserRelation[] rs) {
                    return (IDictionary<int, int[]>)rs.ToDictionary(r => r.UserId, r => r.Followings ?? Array<int>.Empty);
                });
            }

            /// <summary>
            /// 获取关注
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public IDictionary<int, int[]> GetFollowings(int[] userIds, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetFollowingsSr(userIds, settings));
            }

            /// <summary>
            /// 获取关注
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<int[]> GetFollowingsSr(int userId, CallingSettings settings = null)
            {
                var sr = GetRelationSr(userId, GetRelationMask.Following, settings);
                return CreateSr(sr, r => r.Followings);
            }

            /// <summary>
            /// 获取关注
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public int[] GetFollowings(int userId, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetFollowingsSr(userId, settings));
            }

            /// <summary>
            /// 将用户组转换为用户ID
            /// </summary>
            /// <param name="users">用户</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<int[]> ParseUsersSr(string users, CallingSettings settings = null)
            {
                var sr = UserCenterService.ParseUsers(Sc, new UserCenter_ParseUsers_Request() {
                    Users = users
                }, settings);

                return CreateSr(sr, r => r.UserIds);
            }

            /// <summary>
            /// 将用户组转换为用户ID
            /// </summary>
            /// <param name="users">用户</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public int[] ParseUsers(string users, CallingSettings settings = null)
            {
                return InvokeWithCheck(ParseUsersSr(users));
            }

            /// <summary>
            /// 将用户名转换为用户ID
            /// </summary>
            /// <param name="usernames">用户名</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<int[]> ConvertUserNameToIdsSr(string[] usernames, CallingSettings settings = null)
            {
                var sr = UserCenterService.ConvertUserNameToIds(Sc,
                    new UserCenter_ConvertUserNameToIds_Request() { UserNames = usernames }, settings);

                return CreateSr(sr, r => r.UserIds);
            }

            /// <summary>
            /// 将用户名转换为用户ID
            /// </summary>
            /// <param name="usernames">用户名</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public int[] ConvertUserNameToIds(string[] usernames, CallingSettings settings = null)
            {
                return InvokeWithCheck(ConvertUserNameToIdsSr(usernames, settings));
            }

            /// <summary>
            /// 将用户名转换为用户ID
            /// </summary>
            /// <param name="usernames">用户名</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<IDictionary<string, int>> ConvertUserNameToIdsDictSr(string[] usernames, CallingSettings settings = null)
            {
                Contract.Requires(usernames != null);

                var sr = ConvertUserNameToIdsSr(usernames, settings);
                int[] userIds;
                if (sr == null || (userIds = sr.Result) == null)
                    return null;

                if (userIds.Length != usernames.Length)
                    throw new ServiceException(ServerErrorCode.ReturnValueError, "返回的用户ID数量与传入的用户名数量不同");

                Dictionary<string, int> dict = new Dictionary<string, int>();
                for (int k = 0; k < usernames.Length; k++)
                {
                    if (usernames[k] != null)
                        dict[usernames[k]] = userIds[k];
                }

                return CreateSr(sr, r => (IDictionary<string, int>)dict);
            }

            /// <summary>
            /// 将用户名转换为用户ID
            /// </summary>
            /// <param name="usernames">用户名</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public IDictionary<string, int> ConvertUserNameToIdsDict(string[] usernames, CallingSettings settings = null)
            {
                return InvokeWithCheck(ConvertUserNameToIdsDictSr(usernames, settings));
            }

            /// <summary>
            /// 将用户名转换为用户ID
            /// </summary>
            /// <param name="username">用户名</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<int> ConvertUserNameToIdSr(string username, CallingSettings settings = null)
            {
                var sr = ConvertUserNameToIdsSr(new[] { username }, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 将用户名转换为用户ID
            /// </summary>
            /// <param name="username">用户名</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public int ConvertUserNameToId(string username, CallingSettings settings = null)
            {
                return InvokeWithCheck(ConvertUserNameToIdSr(username, settings));
            }

            /// <summary>
            /// 将用户ID转换为用户名
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <returns></returns>
            public ServiceResult<string[]> ConvertUserIdToNamesSr(int[] userIds, bool refresh = false, CallingSettings settings = null)
            {
                var sr = UserCenterService.ConvertUserIdToNames(Sc,
                    new UserCenter_ConvertUserIdToNames_Request { UserIds = userIds, Refresh = refresh }, settings);

                return CreateSr(sr, r => r.UserNames);
            }

            /// <summary>
            /// 将用户ID转换为用户名
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <returns></returns>
            public string[] ConvertUserIdToNames(int[] userIds, bool refresh = false, CallingSettings settings = null)
            {
                return InvokeWithCheck(ConvertUserIdToNamesSr(userIds, refresh, settings));
            }

            /// <summary>
            /// 将用户ID转换为用户名
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <returns></returns>
            public ServiceResult<IDictionary<int, string>> ConvertUserIdToNamesDictSr(int[] userIds, bool refresh = false, CallingSettings settings = null)
            {
                Contract.Requires(userIds != null);

                var sr = ConvertUserIdToNamesSr(userIds, refresh, settings);
                string[] usernames;
                if (sr == null || (usernames = sr.Result) == null)
                    return null;

                if (userIds.Length != usernames.Length)
                    throw new ServiceException(ServerErrorCode.ReturnValueError, "返回的用户名数量与传入的用户ID数量不同");

                Dictionary<int, string> dict = new Dictionary<int, string>();
                for (int k = 0; k < userIds.Length; k++)
                {
                    dict[userIds[k]] = usernames[k];
                }

                return CreateSr(sr, r => (IDictionary<int, string>)dict);
            }

            /// <summary>
            /// 将用户ID转换为用户名
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public IDictionary<int, string> ConvertUserIdToNamesDict(int[] userIds, bool refresh = false, CallingSettings settings = null)
            {
                return InvokeWithCheck(ConvertUserIdToNamesDictSr(userIds, refresh, settings));
            }

            /// <summary>
            /// 将用户ID转换为用户名
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<string> ConvertUserIdToNameSr(int userId, bool refresh = false, CallingSettings settings = null)
            {
                var sr = ConvertUserIdToNamesSr(new[] { userId }, refresh, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 将用户ID转换为用户名
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public string ConvertUserIdToName(int userId, bool refresh = false, CallingSettings settings = null)
            {
                return InvokeWithCheck(ConvertUserIdToNameSr(userId, refresh, settings));
            }

            /// <summary>
            /// 获取用户的在线状态
            /// </summary>
            /// <param name="users">用户</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<UserStatusInfo[]> GetUserStatusInfosSr(Users users, CallingSettings settings = null)
            {
                var sr = UserCenterService.GetUserStatusInfos(Sc,
                    new UserCenter_GetUserStatusInfos_Request() { Users = users }, settings);

                return CreateSr(sr, r => r.Infos);
            }

            /// <summary>
            /// 获取用户的在线状态
            /// </summary>
            /// <param name="users">用户</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public UserStatusInfo[] GetUserStatusInfos(Users users, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetUserStatusInfosSr(users, settings));
            }

            /// <summary>
            /// 获取用户的在线状态
            /// </summary>
            /// <param name="users">用户</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<UserStatusInfo[]> GetUserStatusInfosSr(string users, CallingSettings settings = null)
            {
                return GetUserStatusInfosSr(new Users(users), settings);
            }

            /// <summary>
            /// 获取用户的在线状态
            /// </summary>
            /// <param name="users">用户</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public UserStatusInfo[] GetUserStatusInfos(string users, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetUserStatusInfosSr(users, settings));
            }

            /// <summary>
            /// 获取用户的在线状态
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<UserStatusInfo[]> GetUserStatusInfosSr(int[] userIds, CallingSettings settings = null)
            {
                return GetUserStatusInfosSr(new Users(userIds), settings);
            }

            /// <summary>
            /// 获取用户的在线状态
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public UserStatusInfo[] GetUserStatusInfos(int[] userIds, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetUserStatusInfosSr(userIds, settings));
            }

            /// <summary>
            /// 获取用户的在线状态
            /// </summary>
            /// <param name="userId">用户Id</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<UserStatusInfo> GetUserStatusInfoSr(int userId, CallingSettings settings = null)
            {
                var sr = GetUserStatusInfosSr(new[] { userId }, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取用户的在线状态
            /// </summary>
            /// <param name="userId">用户Id</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public UserStatusInfo GetUserStatusInfo(int userId, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetUserStatusInfoSr(userId, settings));
            }

            /// <summary>
            /// 获取用户信息
            /// </summary>
            /// <param name="users">用户</param>
            /// <param name="mask">遮罩</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<UserInfos[]> GetUserInfosSr(Users users, UserInfoMask mask, bool refresh = false, CallingSettings settings = null)
            {
                var sr = UserCenterService.GetUserInfos(Sc,
                    new UserCenter_GetUserInfos_Request() { Mask = mask, Users = users, Refresh = refresh, }, settings);

                return CreateSr(sr, r => r.Infos);
            }

            /// <summary>
            /// 获取用户信息
            /// </summary>
            /// <param name="users">用户</param>
            /// <param name="mask">遮罩</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public UserInfos[] GetUserInfos(Users users, UserInfoMask mask, bool refresh = false, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetUserInfosSr(users, mask, refresh, settings));
            }

            /// <summary>
            /// 获取用户信息
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="mask">遮罩</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<UserInfos> GetUserInfoSr(int userId, UserInfoMask mask, bool refresh = false, CallingSettings settings = null)
            {
                var sr = GetUserInfosSr(Users.FromUserIds(new[] { userId }), mask, refresh, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取用户信息
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="mask">遮罩</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public UserInfos GetUserInfo(int userId, UserInfoMask mask, bool refresh = false, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetUserInfoSr(userId, mask, refresh, settings));
            }

            /// <summary>
            /// 根据用户名获取用户信息
            /// </summary>
            /// <param name="username">用户名</param>
            /// <param name="mask">遮罩</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<UserInfos> GetUserInfoSr(string username, UserInfoMask mask, bool refresh = false, CallingSettings settings = null)
            {
                var sr = GetUserInfosSr(Users.FromUserNames(new[] { username }), mask, refresh, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 根据用户名获取用户信息
            /// </summary>
            /// <param name="username">用户名</param>
            /// <param name="mask">遮罩</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public UserInfos GetUserInfo(string username, UserInfoMask mask, bool refresh = false, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetUserInfoSr(username, mask, refresh, settings));
            }

            /// <summary>
            /// 获取组信息
            /// </summary>
            /// <param name="groupIds">群组ID</param>
            /// <param name="mask">遮罩</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<GroupInfos[]> GetGroupInfosSr(int[] groupIds, GroupInfoMask mask, bool refresh = false, CallingSettings settings = null)
            {
                var sr = UserCenterService.GetGroupInfos(Sc,
                    new UserCenter_GetGroupInfos_Request() { GroupIds = groupIds, Mask = mask, Refresh = refresh }, settings);

                return CreateSr(sr, r => r.Infos);
            }

            /// <summary>
            /// 获取组信息
            /// </summary>
            /// <param name="groupIds">群组ID</param>
            /// <param name="mask">遮罩</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public GroupInfos[] GetGroupInfos(int[] groupIds, GroupInfoMask mask, bool refresh = false, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetGroupInfosSr(groupIds, mask, refresh, settings));
            }

            /// <summary>
            /// 获取组信息
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="mask">遮罩</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<GroupInfos> GetGroupInfoSr(int groupId, GroupInfoMask mask, bool refresh = false, CallingSettings settings = null)
            {
                var sr = GetGroupInfosSr(new[] { groupId }, mask, refresh, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取组信息
            /// </summary>
            /// <param name="groupId">群组ID</param>
            /// <param name="mask">遮罩</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public GroupInfos GetGroupInfo(int groupId, GroupInfoMask mask, bool refresh = false, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetGroupInfoSr(groupId, mask, refresh, settings));
            }

            /// <summary>
            /// 获取用户所属的组
            /// </summary>
            /// <param name="users">用户</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<UserGroupItem[]> GetUserGroupsSr(Users users, bool refresh = false, CallingSettings settings = null)
            {
                var sr = UserCenterService.GetUserGroups(Sc,
                    new UserCenter_GetUserGroups_Request() { Users = users, Refresh = refresh }, settings);

                return CreateSr(sr, r => r.Items);
            }

            /// <summary>
            /// 获取用户所属的组
            /// </summary>
            /// <param name="users">用户</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public UserGroupItem[] GetUserGroups(Users users, bool refresh = false, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetUserGroupsSr(users, refresh, settings));
            }

            /// <summary>
            /// 获取用户所属的组
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<UserGroupItem[]> GetUserGroupsSr(int[] userIds, bool refresh = false, CallingSettings settings = null)
            {
                return GetUserGroupsSr(Users.FromUserIds(userIds), refresh, settings);
            }

            /// <summary>
            /// 获取用户所属的组
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public UserGroupItem[] GetUserGroups(int[] userIds, bool refresh = false, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetUserGroupsSr(userIds, refresh, settings));
            }

            /// <summary>
            /// 获取用户所属的组
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<int[]> GetUserGroupsSr(int userId, bool refresh = false, CallingSettings settings = null)
            {
                var sr = GetUserGroupsSr(new[] { userId }, refresh, settings);
                return CreateSr(sr, r => { var ug = r.FirstOrDefault(); return ug == null ? Array<int>.Empty : ug.GroupIds ?? Array<int>.Empty; });
            }

            /// <summary>
            /// 获取用户所属的组
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="refresh">是否刷新缓存</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public int[] GetUserGroups(int userId, bool refresh = false, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetUserGroupsSr(userId, refresh, settings));
            }

            /// <summary>
            /// 将用户集合转换为用户名
            /// </summary>
            /// <param name="users">用户集合</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<string[]> ParseToUserNamesSr(Users users, CallingSettings settings = null)
            {
                var sr = UserCenterService.ParseToUserNames(Sc,
                    new UserCenter_ParseToUserNames_Request { Users = users }, settings);

                return CreateSr(sr, r => r.UserNames);
            }

            /// <summary>
            /// 将用户集合转换为用户名
            /// </summary>
            /// <param name="users">用户集合</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public string[] ParseToUserNames(Users users, CallingSettings settings = null)
            {
                return InvokeWithCheck(ParseToUserNamesSr(users, settings));
            }

            /// <summary>
            /// 将用户集合转换为用户名
            /// </summary>
            /// <param name="users">用户集合</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<string[]> ParseToUserNamesSr(string users, CallingSettings settings = null)
            {
                return ParseToUserNamesSr(new Users(users), settings);
            }

            /// <summary>
            /// 将用户集合转换为用户名
            /// </summary>
            /// <param name="users">用户集合</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public string[] ParseToUserNames(string users, CallingSettings settings = null)
            {
                return InvokeWithCheck(ParseToUserNamesSr(users, settings));
            }
        }
    }
}

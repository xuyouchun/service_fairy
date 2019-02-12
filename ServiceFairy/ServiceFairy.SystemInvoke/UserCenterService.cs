using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities.UserCenter;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 用户中心服务
    /// </summary>
    public static class UserCenterService
    {
        /// <summary>
        /// 保持用户的订阅状态
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult KeepUserConnection(
            IServiceClient serviceClient, UserCenter_KeepUserConnection_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call(SFNames.ServiceNames.UserCenter + "/KeepUserConnection", request, settings);
        }

        /// <summary>
        /// 用户连接断开通知
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult UserDisconnectedNotify(
            IServiceClient serviceClient, UserCenter_UserDisconnectedNotify_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call(SFNames.ServiceNames.UserCenter + "/UserDisconnectedNotify", request, settings);
        }

        /// <summary>
        /// 获取用户的连接信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<UserCenter_GetUserConnectionInfos_Reply> GetUserConnectionInfos(
            IServiceClient serviceClient, UserCenter_GetUserConnectionInfos_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<UserCenter_GetUserConnectionInfos_Reply>(SFNames.ServiceNames.UserCenter + "/GetUserConnectionInfos", request, settings);
        }

        /// <summary>
        /// 获取所有的在线用户
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<UserCenter_GetAllOnlineUsers_Reply> GetAllOnlineUsers(
            IServiceClient serviceClient, UserCenter_GetAllOnlineUsers_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<UserCenter_GetAllOnlineUsers_Reply>(SFNames.ServiceNames.UserCenter + "/GetAllOnlineUsers", request, settings);
        }

        /// <summary>
        /// 判断指定的用户在该终端上是否存在
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<UserCenter_ExistsUser_Reply> ExistsUser(
            IServiceClient serviceClient, UserCenter_ExistsUser_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<UserCenter_ExistsUser_Reply>(SFNames.ServiceNames.UserCenter + "/ExistsUser", request, settings);
        }

        /// <summary>
        /// 获取用户所在的终端
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<UserCenter_GetUserPositions_Reply> GetUserPositions(
            IServiceClient serviceClient, UserCenter_GetUserPositions_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<UserCenter_GetUserPositions_Reply>(SFNames.ServiceNames.UserCenter + "/GetUserPositions", request, settings);
        }

        /// <summary>
        /// 获取用户关系表
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<UserCenter_GetRelation_Reply> GetRelation(
            IServiceClient serviceClient, UserCenter_GetRelation_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<UserCenter_GetRelation_Reply>(SFNames.ServiceNames.UserCenter + "/GetRelation", request, settings);
        }

        /// <summary>
        /// 将用户组解析为用户ID
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<UserCenter_ParseUsers_Reply> ParseUsers(
            IServiceClient serviceClient, UserCenter_ParseUsers_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<UserCenter_ParseUsers_Reply>(SFNames.ServiceNames.UserCenter + "/ParseUsers", request, settings);
        }

        /// <summary>
        /// 获取用户的当前状态
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<UserCenter_GetUserStatusInfos_Reply> GetUserStatusInfos(
            IServiceClient serviceClient, UserCenter_GetUserStatusInfos_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<UserCenter_GetUserStatusInfos_Reply>(SFNames.ServiceNames.UserCenter + "/GetUserStatusInfos", request, settings);
        }

        /// <summary>
        /// 将用户名转换为用户ID
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<UserCenter_ConvertUserNameToIds_Reply> ConvertUserNameToIds(
            IServiceClient serviceClient, UserCenter_ConvertUserNameToIds_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<UserCenter_ConvertUserNameToIds_Reply>(SFNames.ServiceNames.UserCenter + "/ConvertUserNameToIds", request, settings);
        }

        /// <summary>
        /// 将用户ID转换为用户名
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<UserCenter_ConvertUserIdToNames_Reply> ConvertUserIdToNames(
            IServiceClient serviceClient, UserCenter_ConvertUserIdToNames_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<UserCenter_ConvertUserIdToNames_Reply>(SFNames.ServiceNames.UserCenter + "/ConvertUserIdToNames", request, settings);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<UserCenter_GetUserInfos_Reply> GetUserInfos(
            IServiceClient serviceClient, UserCenter_GetUserInfos_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<UserCenter_GetUserInfos_Reply>(SFNames.ServiceNames.UserCenter + "/GetUserInfos", request, settings);
        }

        /// <summary>
        /// 获取用户组信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<UserCenter_GetGroupInfos_Reply> GetGroupInfos(
            IServiceClient serviceClient, UserCenter_GetGroupInfos_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<UserCenter_GetGroupInfos_Reply>(SFNames.ServiceNames.UserCenter + "/GetGroupInfos", request, settings);
        }

        /// <summary>
        /// 获取用户所属的组
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<UserCenter_GetUserGroups_Reply> GetUserGroups(
            IServiceClient serviceClient, UserCenter_GetUserGroups_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<UserCenter_GetUserGroups_Reply>(SFNames.ServiceNames.UserCenter + "/GetUserGroups", request, settings);
        }

        /// <summary>
        /// 将用户集合转换为用户名
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<UserCenter_ParseToUserNames_Reply> ParseToUserNames(
            IServiceClient serviceClient, UserCenter_ParseToUserNames_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<UserCenter_ParseToUserNames_Reply>(SFNames.ServiceNames.UserCenter + "/ParseToUserNames", request, settings);
        }
    }
}

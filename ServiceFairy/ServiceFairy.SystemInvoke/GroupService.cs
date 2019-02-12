using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Group;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 群组服务
    /// </summary>
    public static class GroupService
    {
        private static string _GetMethod(string method)
        {
            return SFNames.ServiceNames.Group + "/" + method;
        }

        /// <summary>
        /// 创建群组
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Group_CreateGroup_Reply> CreateGroup(IServiceClient serviceClient,
            Group_CreateGroup_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Group_CreateGroup_Reply>(_GetMethod("CreateGroup"), request, settings);
        }

        /// <summary>
        /// 获取群组信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Group_GetGroupInfo_Reply> GetGroupInfo(IServiceClient serviceClient,
             Group_GetGroupInfo_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Group_GetGroupInfo_Reply>(_GetMethod("GetGroupInfo"), request, settings);
        }

        /// <summary>
        /// 批量获取群组信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Group_GetGroupInfos_Reply> GetGroupInfos(IServiceClient serviceClient,
            Group_GetGroupInfos_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Group_GetGroupInfos_Reply>(_GetMethod("GetGroupInfos"), request, settings);
        }

        /// <summary>
        /// 修改群组信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult ModifyGroupInfo(IServiceClient serviceClient,
            Group_ModifyGroupInfo_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("ModifyGroupInfo"), request, settings);
        }

        /// <summary>
        /// 批量获取群组成员
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Group_GetMembersEx_Reply> GetMembersEx(IServiceClient serviceClient,
            Group_GetMembersEx_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Group_GetMembersEx_Reply>(_GetMethod("GetMembersEx"), request, settings);
        }

        /// <summary>
        /// 获取群组成员
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Group_GetMembers_Reply> GetMembers(IServiceClient serviceClient,
            Group_GetMembers_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Group_GetMembers_Reply>(_GetMethod("GetMembers"), request, settings);
        }

        /// <summary>
        /// 批量添加群组成员
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult AddMembers(IServiceClient serviceClient,
            Group_AddMembers_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call(_GetMethod("AddMembers"), request, settings);
        }

        /// <summary>
        /// 添加群组成员
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult AddMember(IServiceClient serviceClient,
            Group_AddMember_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call(_GetMethod("AddMember"), request, settings);
        }

        /// <summary>
        /// 删除群组成员
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult RemoveMembers(IServiceClient serviceClient,
            Group_RemoveMembers_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call(_GetMethod("RemoveMembers"), request, settings);
        }

        /// <summary>
        /// 删除群组成员
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult RemoveMember(IServiceClient serviceClient,
            Group_RemoveMember_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call(_GetMethod("RemoveMember"), request, settings);
        }

        /// <summary>
        /// 退出群组
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult ExitGroup(IServiceClient serviceClient,
            Group_ExitGroup_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call(_GetMethod("ExitGroup"), request, settings);
        }

        /// <summary>
        /// 删除群组
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult RemoveGroup(IServiceClient serviceClient,
            Group_RemoveGroup_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call(_GetMethod("RemoveGroup"), request, settings);
        }

        /// <summary>
        /// 批量删除群组
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult RemoveGroups(IServiceClient serviceClient,
            Group_RemoveGroups_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call(_GetMethod("RemoveGroups"), request, settings);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult SendMessage(IServiceClient serviceClient,
            Group_SendMessage_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call(_GetMethod("SendMessage"), request, settings);
        }

        /// <summary>
        /// 获取我的所有群组
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Group_GetMyGroupInfos_Reply> GetMyGroupInfos(IServiceClient serviceClient,
            CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<Group_GetMyGroupInfos_Reply>(_GetMethod("GetMyGroupInfos"), null, settings);
        }

        /// <summary>
        /// 获取我的所有群组ID
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Group_GetMyGroups_Reply> GetMyGroups(IServiceClient serviceClient,
            CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<Group_GetMyGroups_Reply>(_GetMethod("GetMyGroups"), null, settings);
        }

        /// <summary>
        /// 获取指定群组的版本号
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Group_GetGroupVersions_Reply> GetGroupVersions(IServiceClient serviceClient,
            Group_GetGroupVersions_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<Group_GetGroupVersions_Reply>(_GetMethod("GetGroupVersions"), null, settings);
        }

        /// <summary>
        /// 获取指定群组的版本号
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Group_GetMyGroupInfosEx_Reply> GetMyGroupInfosEx(IServiceClient serviceClient,
            Group_GetMyGroupInfosEx_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<Group_GetMyGroupInfosEx_Reply>(_GetMethod("GetMyGroupInfosEx"), request, settings);
        }

        /// <summary>
        /// 获取我的所有群组的版本号
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Group_GetMyGroupVersions_Reply> GetMyGroupVersions(IServiceClient serviceClient,
            CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);
            return serviceClient.Call<Group_GetMyGroupVersions_Reply>(_GetMethod("GetMyGroupVersions"), null, settings);
        }
    }
}

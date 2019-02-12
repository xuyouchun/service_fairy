using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities.User;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public static class UserService
    {
        private static string _GetMethod(string method)
        {
            return SFNames.ServiceNames.User + "/" + method;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult Register(IServiceClient serviceClient, 
            User_Register_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("Register"), request, settings);
        }

        /// <summary>
        /// 激活手机号
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<User_MobileActive_Reply> MobileActive(IServiceClient serviceClient,
            User_MobileActive_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<User_MobileActive_Reply>(_GetMethod("MobileActive"), request, settings);
        }

        /// <summary>
        /// 停用手机号
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult MobileDeactive(IServiceClient serviceClient, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("MobileDeactive"), null, settings);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<User_Login_Reply> Login(IServiceClient serviceClient, 
            User_Login_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<User_Login_Reply>(_GetMethod("Login"), request, settings);
        }

        /// <summary>
        /// 用户退出登录
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult Logout(IServiceClient serviceClient, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("Logout"), null, settings);
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult ResetPassword(IServiceClient serviceClient, 
            User_ResetPassword_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("ResetPassword"), request, settings);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult ModifyPassword(IServiceClient serviceClient, 
            User_ModifyPassword_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("ModifyPassword"), request, settings);
        }

        /// <summary>
        /// 准备修改手机号码
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<User_PreModifyPhoneNumber_Reply> PreModifyPhoneNumber(IServiceClient serviceClient, 
            User_PreModifyPhoneNumber_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<User_PreModifyPhoneNumber_Reply>(_GetMethod("PreModifyPhoneNumber"), request, settings);
        }

        /// <summary>
        /// 修改手机号码
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult ModifyPhoneNumber(IServiceClient serviceClient, 
            User_ModifyPhoneNumber_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("ModifyPhoneNumber"), request, settings);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult ModifyMyInfo(IServiceClient serviceClient, 
            User_ModifyMyInfo_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("ModifyMyInfo"), request, settings);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<User_SendVerifyCode_Reply> SendVerifyCode(IServiceClient serviceClient, 
            User_SendVerifyCode_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<User_SendVerifyCode_Reply>(_GetMethod("SendVerifyCode"), request, settings);
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<User_GetMyInfo_Reply> GetMyInfo(IServiceClient serviceClient, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<User_GetMyInfo_Reply>(_GetMethod("GetMyInfo"), null, settings);
        }

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult SetStatus(IServiceClient serviceClient,
            User_SetStatus_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("SetStatus"), request, settings);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult SendMessage(IServiceClient serviceClient,
            User_SendMessage_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("SendMessage"), request, settings);
        }

        /// <summary>
        /// 批量添加联系人
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<User_AddContacts_Reply> AddContacts(IServiceClient serviceClient,
            User_AddContacts_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<User_AddContacts_Reply>(_GetMethod("AddContacts"), request, settings);
        }

        /// <summary>
        /// 批量添加联系人
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<User_AddContact_Reply> AddContact(IServiceClient serviceClient,
            User_AddContact_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<User_AddContact_Reply>(_GetMethod("AddContact"), request, settings);
        }

        /*
        /// <summary>
        /// 以组合方式批量添加联系人
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<User_AddContactsEx_Reply> AddContactsEx(IServiceClient serviceClient,
            User_AddContactsEx_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<User_AddContactsEx_Reply>(_GetMethod("AddContactsEx"), request, settings);
        }*/

        /// <summary>
        /// 删除联系人
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult RemoveContacts(IServiceClient serviceClient,
            User_RemoveContacts_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("RemoveContacts"), request, settings);
        }

        /// <summary>
        /// 删除联系人
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult RemoveContact(IServiceClient serviceClient,
            User_RemoveContact_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("RemoveContact"), request, settings);
        }

        /// <summary>
        /// 更新联系人列表
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult UpdateContactList(IServiceClient serviceClient,
            User_UpdateContactList_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("UpdateContactList"), request, settings);
        }

        /// <summary>
        /// 获取联系人列表
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<User_GetContactList_Reply> GetContactList(IServiceClient serviceClient, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<User_GetContactList_Reply>(_GetMethod("GetContactList"), null, settings);
        }

        /// <summary>
        /// 获取关注列表
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<User_GetFollowings_Reply> GetFollowings(IServiceClient serviceClient,
            User_GetFollowings_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<User_GetFollowings_Reply>(_GetMethod("GetFollowings"), request, settings);
        }

        /// <summary>
        /// 获取粉丝列表
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<User_GetFollowers_Reply> GetFollowers(IServiceClient serviceClient, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<User_GetFollowers_Reply>(_GetMethod("GetFollowers"), null, settings);
        }

        /// <summary>
        /// 获取用户的状态
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<User_GetStatus_Reply> GetStatus(IServiceClient serviceClient,
            User_GetStatus_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<User_GetStatus_Reply>(_GetMethod("GetStatus"), request, settings);
        }

        /// <summary>
        /// 获取指定联系人的信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<User_GetUserInfos_Reply> GetUserInfos(IServiceClient serviceClient,
            User_GetUserInfos_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<User_GetUserInfos_Reply>(_GetMethod("GetUserInfos"), request, settings);
        }

        /// <summary>
        /// 获取指定联系人的信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<User_GetUserInfo_Reply> GetUserInfo(IServiceClient serviceClient,
            User_GetUserInfo_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<User_GetUserInfo_Reply>(_GetMethod("GetUserInfo"), request, settings);
        }

        /// <summary>
        /// 将UserName批量转换为UserId
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<User_GetUserIds_Reply> GetUserIds(IServiceClient serviceClient,
            User_GetUserIds_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<User_GetUserIds_Reply>(_GetMethod("GetUserIds"), request, settings);
        }

        /// <summary>
        /// 将UserName转换为UserId
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<User_GetUserId_Reply> GetUserId(IServiceClient serviceClient,
            User_GetUserId_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<User_GetUserId_Reply>(_GetMethod("GetUserId"), request, settings);
        }

        /// <summary>
        /// 根据用户ID获取用户名
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<User_GetUserName_Reply> GetUserName(IServiceClient serviceClient,
            User_GetUserName_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<User_GetUserName_Reply>(_GetMethod("GetUserName"), request, settings);
        }


        /// <summary>
        /// 根据用户ID批量获取用户名
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<User_GetUserNames_Reply> GetUserNames(IServiceClient serviceClient,
            User_GetUserNames_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<User_GetUserNames_Reply>(_GetMethod("GetUserNames"), request, settings);
        }
    }
}

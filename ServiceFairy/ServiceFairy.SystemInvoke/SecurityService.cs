using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Contracts;
using ServiceFairy.Entities.Security;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 安全服务
    /// </summary>
    public static class SecurityService
    {
        private static string _GetMethod(string method)
        {
            return SFNames.ServiceNames.Security + "/" + method;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Security_Login_Reply> Login(IServiceClient serviceClient,
            Security_Login_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Security_Login_Reply>(_GetMethod("Login"), request, settings);
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult ModifyPassword(IServiceClient serviceClient,
            Security_ModifyPassword_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(_GetMethod("ModifyPassword"), request, settings);
        }

        /// <summary>
        /// 申请安全码
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Security_AcquireSid_Reply> AcquireSid(IServiceClient serviceClient,
            Security_AcquireSid_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Security_AcquireSid_Reply>(_GetMethod("AcquireSid"), request, settings);
        }

        /// <summary>
        /// 批量申请安全码
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Security_AcquireSids_Reply> AcquireSids(IServiceClient serviceClient,
            Security_AcquireSids_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Security_AcquireSids_Reply>(_GetMethod("AcquireSids"), request, settings);
        }

        /// <summary>
        /// 获取安全码的信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Security_GetSidInfo_Reply> GetSidInfo(IServiceClient serviceClient,
            Security_GetSidInfo_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Security_GetSidInfo_Reply>(_GetMethod("GetSidInfo"), request, settings);
        }

        /// <summary>
        /// 获取安全码的信息
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Security_GetSidInfos_Reply> GetSidInfos(IServiceClient serviceClient,
            Security_GetSidInfos_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Security_GetSidInfos_Reply>(_GetMethod("GetSidInfos"), request, settings);
        }

        /// <summary>
        /// 批量验证安全码的有效性
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Security_ValidateSids_Reply> ValidateSids(IServiceClient serviceClient,
            Security_ValidateSids_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Security_ValidateSids_Reply>(_GetMethod("ValidateSids"), request, settings);
        }

        /// <summary>
        /// 批量验证安全码的有效性
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <param name="request"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static ServiceResult<Security_ValidateSid_Reply> ValidateSid(IServiceClient serviceClient,
            Security_ValidateSid_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call<Security_ValidateSid_Reply>(_GetMethod("ValidateSid"), request, settings);
        }
    }
}

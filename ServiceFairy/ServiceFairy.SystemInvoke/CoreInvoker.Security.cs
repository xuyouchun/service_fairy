using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Security;

namespace ServiceFairy.SystemInvoke
{
    partial class CoreInvoker
    {
        private SecurityInvoker _security;

        /// <summary>
        /// Security Service
        /// </summary>
        public SecurityInvoker Security
        {
            get { return _security ?? (_security = new SecurityInvoker(this)); }
        }

        /// <summary>
        /// Security Service
        /// </summary>
        public class SecurityInvoker : Invoker
        {
            public SecurityInvoker(CoreInvoker owner)
                : base(owner)
            {

            }

            /// <summary>
            /// 用户登录
            /// </summary>
            /// <param name="username">用户名</param>
            /// <param name="password">密码</param>
            /// <param name="settings">调用设置</param>
            /// <returns>安全码</returns>
            public ServiceResult<SidInfo> LoginSr(string username, string password, CallingSettings settings = null)
            {
                var req = new Security_Login_Request { UserName = username, Password = password };
                var sr = SecurityService.Login(Sc, req, settings);
                return CreateSr(sr, r => r.SidInfo);
            }

            /// <summary>
            /// 用户登录
            /// </summary>
            /// <param name="username">用户名</param>
            /// <param name="password">密码</param>
            /// <param name="settings">调用设置</param>
            /// <returns>安全码</returns>
            public SidInfo Login(string username, string password, CallingSettings settings = null)
            {
                return InvokeWithCheck(LoginSr(username, password, settings));
            }

            /// <summary>
            /// 修改用户密码
            /// </summary>
            /// <param name="newPassword">新密码</param>
            /// <param name="oldPassword">原密码</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult ModifyPasswordSr(string newPassword, string oldPassword, CallingSettings settings = null)
            {
                var req = new Security_ModifyPassword_Request { NewPassword = newPassword, OldPassword = oldPassword };
                return SecurityService.ModifyPassword(Sc, req, settings);
            }

            /// <summary>
            /// 修改用户密码
            /// </summary>
            /// <param name="newPassword">新密码</param>
            /// <param name="oldPassword">原密码</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void ModifyPassword(string newPassword, string oldPassword, CallingSettings settings = null)
            {
                InvokeWithCheck(ModifyPasswordSr(newPassword, oldPassword, settings));
            }

            /// <summary>
            /// 申请安全码
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="securityLevel">安全级别</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<Sid> AcquireSidSr(int userId, SecurityLevel securityLevel, CallingSettings settings = null)
            {
                var req = new Security_AcquireSid_Request { UserId = userId, SecurityLevel = securityLevel };
                var sr = SecurityService.AcquireSid(Sc, req, settings);
                return CreateSr(sr, r=>r.Sid);
            }

            /// <summary>
            /// 申请安全码
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="securityLevel">安全级别</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public Sid AcquireSid(int userId, SecurityLevel securityLevel, CallingSettings settings = null)
            {
                return InvokeWithCheck(AcquireSidSr(userId, securityLevel, settings));
            }

            /// <summary>
            /// 批量申请安全码
            /// </summary>
            /// <param name="items">安全码申请数据</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<AcquiredSidPair[]> AcquireSidsSr(AcquireSidItem[] items, CallingSettings settings = null)
            {
                var req = new Security_AcquireSids_Request { Items = items };
                var sr = SecurityService.AcquireSids(Sc, req, settings);
                return CreateSr(sr, r => r.Sids);
            }

            /// <summary>
            /// 批量申请安全码
            /// </summary>
            /// <param name="items">安全码申请数据</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public AcquiredSidPair[] AcquireSids(AcquireSidItem[] items, CallingSettings settings = null)
            {
                return InvokeWithCheck(AcquireSidsSr(items, settings));
            }

            /// <summary>
            /// 获取安全码的信息
            /// </summary>
            /// <param name="sid">安全码</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<SidInfo> GetSidInfoSr(Sid sid, CallingSettings settings = null)
            {
                var sr = SecurityService.GetSidInfo(Sc, new Security_GetSidInfo_Request { Sid = sid }, settings);
                return CreateSr(sr, r => r.Info);
            }

            /// <summary>
            /// 获取安全码的信息
            /// </summary>
            /// <param name="sid">安全码</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public SidInfo GetSidInfo(Sid sid, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetSidInfoSr(sid, settings));
            }

            /// <summary>
            /// 批量获取安全码的信息
            /// </summary>
            /// <param name="sids">安全码</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<SidInfo[]> GetSidInfosSr(Sid[] sids, CallingSettings settings = null)
            {
                var sr = SecurityService.GetSidInfos(Sc, new Security_GetSidInfos_Request { Sids = sids }, settings);
                return CreateSr(sr, r => r.Infos);
            }

            /// <summary>
            /// 批量获取安全码的信息
            /// </summary>
            /// <param name="sids">安全码</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public SidInfo[] GetSidInfos(Sid[] sids, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetSidInfosSr(sids, settings));
            }


            /// <summary>
            /// 批量验证安全码的有效性
            /// </summary>
            /// <param name="sids">安全码</param>
            /// <param name="settings">调用设置</param>
            /// <returns>用户ID，如果安全码无效，则为0</returns>
            public ServiceResult<int[]> ValidateSidsSr(Sid[] sids, CallingSettings settings = null)
            {
                var sr = SecurityService.ValidateSids(Sc, new Security_ValidateSids_Request { Sids = sids }, settings);
                return CreateSr(sr, r => r.UserIds);
            }

            /// <summary>
            /// 批量验证安全码的有效性
            /// </summary>
            /// <param name="sids">安全码</param>
            /// <param name="settings">调用设置</param>
            /// <returns>用户ID，如果安全码无效，则为0</returns>
            public int[] ValidateSids(Sid[] sids, CallingSettings settings = null)
            {
                return InvokeWithCheck(ValidateSidsSr(sids, settings));
            }

            /// <summary>
            /// 验证安全码的有效性
            /// </summary>
            /// <param name="sid">安全码</param>
            /// <param name="settings">调用设置</param>
            /// <returns>用户ID，如果安全码无效，则为0</returns>
            public ServiceResult<int> ValidateSidSr(Sid sid, CallingSettings settings = null)
            {
                var sr = SecurityService.ValidateSid(Sc, new Security_ValidateSid_Request { Sid = sid }, settings);
                return CreateSr(sr, r => r.UserId);
            }

            /// <summary>
            /// 验证安全码的有效性
            /// </summary>
            /// <param name="sid">安全码</param>
            /// <param name="settings">调用设置</param>
            /// <returns>用户ID，如果安全码无效，则为0</returns>
            public int ValidateSid(Sid sid, CallingSettings settings = null)
            {
                return InvokeWithCheck(ValidateSidSr(sid, settings));
            }
        }
    }
}

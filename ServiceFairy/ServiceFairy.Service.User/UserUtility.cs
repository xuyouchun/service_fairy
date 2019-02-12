using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Data;
using Common.Data.UnionTable;
using Common.Utility;
using ServiceFairy.Entities.User;
using ServiceFairy.SystemInvoke;
using Common.Package;
using Common;
using ServiceFairy.DbEntities;
using Common.Data.SqlExpressions;
using ServiceFairy.DbEntities.User;

namespace ServiceFairy.Service.User
{
    static class UserUtility
    {
        /// <summary>
        /// 将密码加密
        /// </summary>
        /// <param name="password"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static string HashPassword(string password, string username)
        {
            return SecurityUtility.Md5(password + username);
        }

        /// <summary>
        /// 将验证码加密
        /// </summary>
        /// <param name="verifyCode"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static string HashVerifyCode(string verifyCode, string username)
        {
            return SecurityUtility.Md5(verifyCode + username);
        }

        /// <summary>
        /// 生成安全码
        /// </summary>
        /// <param name="service"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Sid GenerateSid(this Service service, int userId)
        {
            return service.UserManager.AcquireSid(userId, SecurityLevel.User);
        }

        /// <summary>
        /// 生成并保存安全码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="service"></param>
        /// <param name="basicInfo"></param>
        /// <returns></returns>
        public static Sid GenerateAndSaveSid(this Service service, string username, out UserBasicInfo basicInfo)
        {
            var usermgr = service.UserManager;
            basicInfo = usermgr.GetUserBasicInfo(username, true, true);
            if (!basicInfo.Enable)
                throw new ServiceException(ServerErrorCode.InvalidUser, "帐号已停用");

            Sid sid = GenerateSid(service, basicInfo.UserId);
            int userId = basicInfo.UserId;

            try
            {
                DbUser user = new DbUser() { Sid = sid.ToString() };
                user.Update(service.DbConnectionManager.Provider, new[] { DbUser.F_Sid }, (string)SqlExpression.Equals(DbUser.F_UserId, userId));
            }
            catch (Exception)
            {
                usermgr.ClearSessionStateCache(sid);
                throw;
            }

            return sid;
        }

        /// <summary>
        /// 删除安全码
        /// </summary>
        /// <param name="service"></param>
        /// <param name="sid"></param>
        public static bool RemoveSid(this Service service, Sid sid)
        {
            var userMgr = service.UserManager;
            UserSessionState uss = userMgr.GetSessionState(sid, false);
            if (uss == null)
                return false;

            userMgr.RemoveSessionState(sid);
            return true;
        }

        private static readonly Random _verifyCodeRandom = new Random();

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="basicInfo"></param>
        /// <returns></returns>
        public static string GenerateVerifyCode()
        {
            return _verifyCodeRandom.Next(10000).ToString().PadLeft(4, '0');
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="service">服务</param>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="verifyCode">验证码</param>
        /// <param name="for">用途</param>
        /// <param name="throwError">是否在发送失败时抛出异常</param>
        public static void SendVerifyCode(this Service service, string phoneNumber, string verifyCode, string @for, bool throwError = false)
        {
            var sr = service.Invoker.Sms.SendSr(phoneNumber, string.Format(VerifyCodeFor.GetTemplate(@for), verifyCode));
            if (sr != null && throwError)
                sr.Validate();
        }

        // 创建验证码的键
        private static string _GetVerifyKey(string username, string @for)
        {
            return @for + "_" + username;
        }

        /// <summary>
        /// 生成并发送验证码
        /// </summary>
        /// <param name="service"></param>
        /// <param name="username"></param>
        /// <param name="for"></param>
        /// <returns></returns>
        public static string GenerateAndSendVerifyCode(this Service service, string username, string @for)
        {
            // 随机生成验证码
            string verifyCode = UserUtility.GenerateVerifyCode();
            service.Invoker.Cache.Set(_GetVerifyKey(username, @for), verifyCode, TimeSpan.FromMinutes(5));
            SendVerifyCode(service, username, verifyCode, @for);
            return verifyCode;
        }

        private static string _R(string sql)
        {
            return DataUtility.ReviseSql(sql);
        }

        // 验证码正确性
        public static void ValidateVerifyCode(this Service service, string username, string verifyCode, string @for)
        {
#warning TODO: 正式上线时去掉该行代码 ....
            if (verifyCode == "9999")
                return;

            string verifyCodeInCache = service.Invoker.Cache.Get<string>(_GetVerifyKey(username, @for), remove: true);
            if (verifyCode != verifyCodeInCache)
                throw new ServiceException(UserStatusCode.InvalidVerifyCode);
        }

        /// <summary>
        /// 验证用户名密码
        /// </summary>
        /// <param name="service">服务</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="throwError">是否在验证失败时抛出异常</param>
        /// <returns></returns>
        public static UserStatusCode CheckPassword(this Service service, string username, string password, bool throwError = false)
        {
            SqlExpression where = SqlExpression.Equals(DbUser.F_UserName, username);
            DbUser dbUser = DbUser.SelectOne(service.DbConnectionManager.Provider, null, new[] { DbUser.F_Password }, where.ToString());
            if (dbUser == null || dbUser.Password != HashPassword(password, username))
            {
                UserStatusCode sc = (dbUser == null) ? UserStatusCode.InvalidUser : UserStatusCode.InvalidPassword;
                if (throwError)
                    throw new ServiceException(sc);

                return sc;
            }

            return (UserStatusCode)ServiceStatusCode.Ok;
        }
    }
}

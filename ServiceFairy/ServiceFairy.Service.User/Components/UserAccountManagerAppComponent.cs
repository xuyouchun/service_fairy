using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using System.Diagnostics.Contracts;
using ServiceFairy.SystemInvoke;
using Common.Contracts.Service;
using Common.Data.UnionTable;
using Common.Data;
using Common.Utility;
using ServiceFairy.Entities.User;
using ServiceFairy.DbEntities;
using ServiceFairy.DbEntities.User;
using ServiceFairy.Entities;
using System.Collections.Concurrent;
using Common;
using Common.Data.SqlExpressions;
using ServiceFairy.Entities.UserCenter;
using CurrentUserInfoCacheChain = Common.Package.CacheChain<int, ServiceFairy.Entities.User.UserIdName>;
using ICurrentUserInfoCacheChainNode = Common.Package.ICacheChainNode<int, ServiceFairy.Entities.User.UserIdName>;
using CurrentUserInfoDistribuedCacheChainNode = ServiceFairy.SystemInvoke.DistribuedCacheChainNode<int, ServiceFairy.Entities.User.UserIdName>;
using ServiceFairy.Components;
using Common.Package;

namespace ServiceFairy.Service.User.Components
{
    /// <summary>
    /// 用户管理器
    /// </summary>
    [AppComponent("用户帐号管理器", "负责用户的注册、登录等功能")]
    class UserAccountManagerAppComponent : TimerAppComponentBase
    {
        public UserAccountManagerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(1))
        {
            _service = service;
            _userManager = service.UserManager;
        }

        private readonly Service _service;
        private readonly UserManagerAppComponent _userManager;
        private HashSet<int> _loginUserIds = new HashSet<int>(), _registerUserIds = new HashSet<int>();

        /*
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="password">密码</param>
        /// <param name="autoLogin">是否自动登录</param>
        /// <param name="verifyCode">验证码</param>
        /// <param name="userId">用户ID</param>
        /// <returns>安全码</returns>
        public Sid SafeRegister(string phoneNumber, string password, bool autoLogin, string verifyCode, out int userId)
        {
            // 验证码正确性
            _service.ValidateVerifyCode(phoneNumber, verifyCode, VerifyCodeFor.Register);

            // 注册
            return Register(phoneNumber, password, autoLogin, out userId);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="password">密码</param>
        /// <param name="autoLogin">是否自动登录</param>
        /// <param name="verifyCode">验证码</param>
        /// <returns>安全码</returns>
        public Sid SafeRegister(string phoneNumber, string password, bool autoLogin, string verifyCode)
        {
            int userId;
            return SafeRegister(phoneNumber, password, autoLogin, verifyCode, out userId);
        }*/

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="autoLogin">是否自动登录</param>
        /// <param name="userId">用户ID</param>
        /// <returns>安全码</returns>
        public Sid Register(string username, string password, bool autoLogin, out int userId)
        {
            if (_userManager.ExistsUser(username))
                throw new ServiceException(UserStatusCode.UserAlreadyExists);

            DateTime utcNow = DateTime.UtcNow;
            DbUser user = new DbUser() {
                UserName = username, CreateTime = utcNow, StatusChangedTime = utcNow, DetailChangedTime = utcNow,
                Password = UserUtility.HashPassword(password ?? string.Empty, username), Enable = true
            };

            user.Insert(_service.DbConnectionManager.Provider);
            InvokeNoThrow(delegate { _service.UserRelationManager.CreateRelations(username, user.UserId, utcNow); });
            userId = user.UserId;
            _registerUserIds.SafeAdd(userId);

            _service.MessageSender.SendToFollowers<User_NewUser_Message>(
                new User_NewUser_Message { UserIds = new[] { userId } }, userId);

            if (autoLogin)
                return Login(username, password);

            return Sid.Empty;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="autoLogin">是否自动登录</param>
        /// <param name="userId">用户ID</param>
        /// <returns>安全码</returns>
        public Sid Register(string username, string password, bool autoLogin)
        {
            int userId;
            return Register(username, password, autoLogin, out userId);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="userId">用户ID</param>
        /// <returns>安全码</returns>
        public Sid Login(string username, string password, out int userId)
        {
            _service.CheckPassword(username, password, true);
            return Login(username, out userId);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>安全码</returns>
        public Sid Login(string username, string password)
        {
            int userId;
            return Login(username, password, out userId);
        }

        /// <summary>
        /// 登录（无密码验证）
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="userId">用户ID</param>
        /// <returns>安全码</returns>
        public Sid Login(string username, out int userId)
        {
            UserBasicInfo basicInfo;
            Sid sid = _service.GenerateAndSaveSid(username, out basicInfo);
            userId = basicInfo.UserId;

            _loginUserIds.SafeAdd(basicInfo.UserId);
            return sid;
        }

        /// <summary>
        /// 登录（无密码验证）
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>安全码</returns>
        public Sid Login(string username)
        {
            int userId;
            return Login(username, out userId);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="uss"></param>
        public bool Logout(UserSessionState uss)
        {
            if (uss == null)
                return false;

            return _service.RemoveSid(uss.Sid);
        }

        /*
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="uss">会话状态</param>
        /// <param name="oldPassword">原密码</param>
        /// <param name="newPassword">新密码</param>
        public Sid ModifyPassword(UserSessionState uss, string oldPassword, string newPassword)
        {
            // 验证旧密码
            _service.CheckPassword(uss.BasicInfo.UserName, oldPassword, true);

            // 修改为新密码
            DbUser user = new DbUser() {
                UserId = uss.BasicInfo.UserId, Password = UserUtility.HashPassword(newPassword, uss.BasicInfo.UserName),
            };

            user.Update(_service.DbConnectionManager.Provider, new[] { DbUser.F_Password });

            // 重新登录
            return Login(uss.BasicInfo.UserName, newPassword);
        }

        /// <summary>
        /// 准备修改手机号码
        /// </summary>
        /// <param name="uss">会话状态</param>
        /// <param name="newPhoneNumber">新手机号</param>
        /// <param name="oldPassword">原手机号</param>
        public string PreModifyPhoneNumber(UserSessionState uss, string newPhoneNumber, string oldPassword)
        {
            Contract.Requires(newPhoneNumber != null && oldPassword != null);

            // 验证用户密码是否正确
            _service.CheckPassword(uss.BasicInfo.UserName, oldPassword, true);

            // 检查新用户是否已经存在
            if (_userManager.ExistsUser(newPhoneNumber))
                throw new ServiceException(UserStatusCode.UserAlreadyExists);

            // 发送验证码
            return UserUtility.GenerateAndSendVerifyCode(_service, newPhoneNumber, VerifyCodeFor.ModifyPhoneNumber);
        }

        /// <summary>
        /// 修改手机号码
        /// </summary>
        /// <param name="uss"></param>
        /// <param name="newPassword">新手机号</param>
        /// <param name="newPhoneNumber">原手机号</param>
        /// <param name="verifyCode">验证码</param>
        public Sid ModifyPhoneNumber(UserSessionState uss, string newPhoneNumber, string newPassword, string verifyCode)
        {
            // 验证码正确性
            _service.ValidateVerifyCode(newPhoneNumber, verifyCode, VerifyCodeFor.ModifyPhoneNumber);

            // 修改手机号与密码
            DbUser entity = new DbUser() {
                UserId = uss.BasicInfo.UserId, UserName = newPhoneNumber,
                Password = UserUtility.HashPassword(newPassword, newPhoneNumber)
            };

            entity.Update(_service.DbConnectionManager.Provider, new[] { DbUser.F_UserName, DbUser.F_Password });
            _userManager.ClearSessionStateCache(uss.Sid);
            return Login(newPhoneNumber, newPassword);
        }

        /// <summary>
        /// 密码重置
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="verifyCode">验证码</param>
        /// <returns></returns>
        public Sid ResetPassword(string username, string password, string verifyCode, bool autoLogin)
        {
            _service.ValidateVerifyCode(username, verifyCode, VerifyCodeFor.ResetPassword);

            UserBasicInfo uInfo = _userManager.GetUserBasicInfo(username, true);
            DbUser user = new DbUser() {
                UserId = uInfo.UserId, Password = UserUtility.HashPassword(password, username)
            };

            user.Update(_service.DbConnectionManager.Provider, new[] { DbUser.F_Password });

            if (autoLogin)
                return Login(username, password);

            return Sid.Empty;
        }*/

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="for">用途</param>
        public string SendVerifyCode(string phoneNumber, string @for)
        {
            return _service.GenerateAndSendVerifyCode(phoneNumber, @for);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="name">姓名</param>
        /// <param name="vCard">名片</param>
        public void ModifyUserInfo(int userId, string name, string vCard)
        {
            UserDetailInfo detailInfo = _service.UserManager.GetUserDetailInfo(userId, refresh: true);
            
            DbUser dbUser = new DbUser {
                Detail = vCard, DetailChangedTime = DateTime.UtcNow,
                UserId = userId, Name = name
            };

            dbUser.Update(_service.DbConnectionManager.Provider, new[] { DbUser.F_Detail, DbUser.F_DetailChangedTime, DbUser.F_Name });

            _service.UserManager.ClearCache(userId);
            var entity = new User_InfoChanged_Message { UserIds = new[] { userId } };
            _service.MessageSender.SendToFollowers<User_InfoChanged_Message>(entity, userId, property: MsgProperty.Override);
        }

        /*
        /// <summary>
        /// 获取当前用户的信息
        /// </summary>
        /// <param name="uss"></param>
        public UserInfo SafeGetUserInfo(UserSessionState uss)
        {
            Contract.Requires(uss != null);

            UserBasicInfo bi = uss.BasicInfo;
            DbUser user = DbUser.SelectOne(_service.DbConnectionManager.Provider, bi.UserId,
                new[] { DbUser.F_Detail, DbUser.F_DetailChangedTime });

            if (user == null)
                return null;

            return new UserInfo { UserId = bi.UserId, UserName = bi.UserName, VCard = DbUser.DetailToDict(user.Detail), ChangedTime = user.DetailChangedTime };
        }*/
         
        /// <summary>
        /// 获取指定用户的信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserInfo GetUserInfo(int userId)
        {
            DbUser user = DbUser.SelectOne(_service.DbConnectionManager.Provider, userId,
                new[] { DbUser.F_UserName, DbUser.F_Detail, DbUser.F_DetailChangedTime });

            if (user == null)
                return null;

            return new UserInfo { UserId = userId, UserName = user.UserName, VCard = user.Detail, ChangedTime = user.DetailChangedTime };
        }

        /// <summary>
        /// 批量获取指定用户的信息
        /// </summary>
        /// <param name="userIds">用户ID</param>
        /// <param name="since">起始变化时间</param>
        /// <returns></returns>
        public UserInfo[] GetUserInfos(int[] userIds, DateTime since)
        {
            if (userIds.IsNullOrEmpty())
                return Array<UserInfo>.Empty;

            SqlExpression where = (since == default(DateTime)) ? SqlExpression.Empty
                : SqlExpression.Large(DbUser.F_DetailChangedTime, since);

            DbUser[] users = DbUser.Select(_service.DbConnectionManager.Provider, userIds,
                new[] { DbUser.F_UserId, DbUser.F_UserName, DbUser.F_Detail }, where.ToString());

            return users.ToArray(user => new UserInfo {
                UserId = user.UserId, UserName = user.UserName, VCard = user.Detail, ChangedTime = user.DetailChangedTime
            });
        }

        protected override void OnExecuteTask(string taskName)
        {
            int[] loginUserIds = _loginUserIds.SafeToArray(clear: true, trimExcess: true);
            if (loginUserIds.Length > 0)
                _service.ServiceEvent.Raise(new User_Login_Event() { UserIds = loginUserIds });

            int[] registerUserIds = _loginUserIds.SafeToArray(clear: true, trimExcess: true);
            if (registerUserIds.Length > 0)
                _service.ServiceEvent.Raise(new User_Register_Event() { UserIds = registerUserIds });
        }
    }
}

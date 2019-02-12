using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using Common.Collection;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities.Security;
using Common.Utility;

namespace ServiceFairy.Service.Security.Components
{
    /// <summary>
    /// 用户账号管理器
    /// </summary>
    [AppComponent("用户账号管理器", "管理用户的账号密码")]
    class SystemAccountManagerAppComponent : AppComponent
    {
        public SystemAccountManagerAppComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;
        private readonly IgnoreCaseDictionary<SystemUser> _userinfos = new IgnoreCaseDictionary<SystemUser>();

        protected override void OnStart()
        {
            base.OnStart();

            
        }

        private int _sysUserIds = -1;

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public SystemUser Login(string username, string password, bool throwError = true)
        {
            return new SystemUser {
                UserId = Interlocked.Decrement(ref _sysUserIds),
                SecurityLevel = _GetSecurityLevel(username),
                Password = password,
                UserName = username
            };

            /*
            Contract.Requires(username != null && password != null);

            SystemUser user;
            if (!_userinfos.TryGetValue(username, out user))
            {
                if (throwError)
                    throw Utility.CreateException(SecurityStatusCode.InvalidUser);

                return null;
            }

            if (password != user.Password)
            {
                if (throwError)
                    throw Utility.CreateException(SecurityStatusCode.InvalidPassword);

                return null;
            }

            return user;*/
        }

        private SecurityLevel _GetSecurityLevel(string username)
        {
            if (username == "su")
                return SecurityLevel.SuperAdmin;

            if (username.StartsWith("Core."))
                return SecurityLevel.CoreRunningLevel;

            if (username.StartsWith("Sys."))
                return SecurityLevel.SysRunningLevel;

            if (username.StartsWith("App."))
                return SecurityLevel.AppRunningLevel;

            throw Utility.CreateException(SecurityStatusCode.InvalidUser);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="uss"></param>
        public void Logout(UserSessionState uss)
        {
            Contract.Requires(uss != null);
        }
    }
}

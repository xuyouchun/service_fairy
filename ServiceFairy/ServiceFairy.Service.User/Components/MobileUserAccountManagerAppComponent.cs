using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities.User;
using System.Diagnostics.Contracts;
using ServiceFairy.DbEntities.User;

namespace ServiceFairy.Service.User.Components
{
    /// <summary>
    /// 手机用户帐号管理器
    /// </summary>
    [AppComponent("手机用户帐号管理器", "负责用户的激活、停用等功能")]
    class MobileUserAccountManagerAppComponent : AppComponent
    {
        public MobileUserAccountManagerAppComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;

        /// <summary>
        /// 以安全方式激活
        /// </summary>
        /// <param name="phoneNuber">手机号</param>
        /// <param name="verifyCode">验证码</param>
        /// <param name="userId">用户ID</param>
        /// <returns>安全码</returns>
        public Sid SafeActive(string phoneNuber, string verifyCode, out int userId)
        {
            Contract.Requires(phoneNuber != null && verifyCode != null);

            _service.ValidateVerifyCode(phoneNuber, verifyCode, VerifyCodeFor.ActiviteMobile);
            return Active(phoneNuber, out userId);
        }

        /// <summary>
        /// 以安全方式激活
        /// </summary>
        /// <param name="phoneNuber">手机号</param>
        /// <param name="verifyCode">验证码</param>
        /// <returns>安全码</returns>
        public Sid SafeActive(string phoneNuber, string verifyCode)
        {
            int userId;
            return SafeActive(phoneNuber, verifyCode, out userId);
        }

        /// <summary>
        /// 以安全方式激活手机号
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="userId">用户ID</param>
        /// <returns>安全码</returns>
        public Sid Active(string phoneNumber, out int userId)
        {
            Contract.Requires(phoneNumber != null);

            Sid sid;
            var ua = _service.UserAccountManager;
            UserBasicInfo basicInfo = _service.UserManager.GetUserBasicInfo(phoneNumber, refresh: true);
            if (basicInfo == null)
            {
                sid = ua.Register(phoneNumber, Settings.DefaultPassword, true, out userId);
            }
            else
            {
                userId = basicInfo.UserId;
                if (!basicInfo.Enable)
                {
                    _SetActive(basicInfo.UserId, true);
                    _service.UserRelationManager.CreateRelations(phoneNumber, basicInfo.UserId);
                    _service.UserManager.ClearCache(basicInfo.UserId);
                }

                sid = ua.Login(phoneNumber);
            }

            return sid;
        }

        /// <summary>
        /// 以安全方式激活手机号
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <returns>安全码</returns>
        public Sid Active(string phoneNumber)
        {
            int userId;
            return Active(phoneNumber, out userId);
        }

        /// <summary>
        /// 停用手机号
        /// </summary>
        /// <param name="uss">用户会话状态</param>
        public void Deactive(UserSessionState uss)
        {
            Contract.Requires(uss != null);

            int userId = uss.BasicInfo.UserId;
            if (uss.BasicInfo.Enable)
            {
                _SetActive(userId, false);
                _service.UserManager.ClearCache(userId);
            }

            var ua = _service.UserAccountManager;
            InvokeNoThrow(() => ua.Logout(uss));

            // 清空消息池
            _service.Invoker.MessageCenter.ClearSr(userId);

            // 退出所有群组
#warning 退出所有群组
        }

        private void _SetActive(int userId, bool enable)
        {
            DbUser user = new DbUser { UserId = userId, Enable = enable };
            user.Update(_service.DbConnectionManager.Provider, DbUser.F_Enable);
        }
    }
}

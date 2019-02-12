using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.User;
using Common.Contracts.Service;
using ServiceFairy.Service.User.Components;
using Common.Contracts;

namespace ServiceFairy.Service.User
{
    /// <summary>
    /// 用户登录　
    /// </summary>
    [AppCommand("Login", title: "用户登录", SecurityLevel = SecurityLevel.Public), Remarks(Remarks), DisabledCommand]
    class LoginAppCommand : ACS<Service>.Func<User_Login_Request, User_Login_Reply>
    {
        protected override User_Login_Reply OnExecute(AppCommandExecuteContext<Service> context, User_Login_Request req, ref ServiceResult sr)
        {
            // 确保不允许使用默认密码登录
            if (req.Password == Settings.DefaultPassword)
                throw new ServiceException(UserStatusCode.InvalidPassword);

            int userId;
            Sid sid = context.Service.UserAccountManager.Login(req.UserName, req.Password, out userId);
            sr = new ServiceResult(sid);

            return new User_Login_Reply { UserId = userId };
        }

        const string Remarks = @"用户登录，如果用户不存在则返回错误码：InvalidUser，同时向该手机号发送验证码，以便进入注册的流程。";
    }
}

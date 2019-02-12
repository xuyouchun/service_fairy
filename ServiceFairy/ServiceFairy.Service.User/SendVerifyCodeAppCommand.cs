using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.User;
using ServiceFairy.Service.User.Components;
using Common.Contracts;

namespace ServiceFairy.Service.User
{
    /// <summary>
    /// 获取验证码
    /// </summary>
    [AppCommand("SendVerifyCode", title: "获取验证码", SecurityLevel = SecurityLevel.Public), Remarks(Remarks)]
    class SendVerifyCodeAppCommand : ACS<Service>.Func<User_SendVerifyCode_Request, User_SendVerifyCode_Reply>
    {
        protected override User_SendVerifyCode_Reply OnExecute(AppCommandExecuteContext<Service> context, User_SendVerifyCode_Request req, ref ServiceResult sr)
        {
            string verifyCode = context.Service.UserAccountManager.SendVerifyCode(req.PhoneNumber, req.For);
            return new User_SendVerifyCode_Reply() { HashVerifyCode = UserUtility.HashVerifyCode(verifyCode, req.PhoneNumber) };
        }

        const string Remarks = @"获取验证码，用于注册新用户、密码重置、修改手机号等场合，确定手机号所有者的身份。";
    }
}

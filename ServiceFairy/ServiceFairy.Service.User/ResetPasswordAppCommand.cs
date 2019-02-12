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
    /*
    /// <summary>
    /// 找回密码
    /// </summary>
    [AppCommand("ResetPassword", title: "密码重置", SecurityLevel = SecurityLevel.Public), Remarks(Remarks), DisabledCommand]
    class ResetPasswordAppCommand : ACS<Service>.Action<User_ResetPassword_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, User_ResetPassword_Request req, ref ServiceResult sr)
        {
            Sid sid = context.Service.UserAccountManager.ResetPassword(req.UserName, req.NewPassword, req.VerifyCode, req.AutoLogin);
            sr = new ServiceResult(sid);
        }

        const string Remarks = @"用户当前无需处于登录状态，需要先调用SendVerifyCode获取验证码，再调用该接口实现密码重置。";
    }*/
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.User;
using ServiceFairy.Service.User.Components;
using Common.Contracts;

namespace ServiceFairy.Service.User
{/*
    /// <summary>
    /// 准备修改手机号码
    /// </summary>
    [AppCommand("PreModifyPhoneNumber", title: "准备修改手机号码", SecurityLevel = SecurityLevel.User), Remarks(Remarks), DisabledCommand]
    class PreModifyPhoneNumberAppCommand : ACS<Service>.Func<User_PreModifyPhoneNumber_Request, User_PreModifyPhoneNumber_Reply>
    {
        protected override User_PreModifyPhoneNumber_Reply OnExecute(AppCommandExecuteContext<Service> context, User_PreModifyPhoneNumber_Request req, ref ServiceResult sr)
        {
            string verifyCode = context.Service.UserAccountManager.PreModifyPhoneNumber(context.GetSessionState(), req.NewPhoneNumber, req.Password);

            return new User_PreModifyPhoneNumber_Reply() { HashVerifyCode = UserUtility.HashVerifyCode(verifyCode, req.NewPhoneNumber) };
        }

        const string Remarks = @"准备修改手机号码，该接口是修改手机号码的第一步，需要用户当前处于登录状态。
填入新手机号码，调用该接口将会向新手机号码发送验证码，再调用ModifyPhoneNumber修改手机号码。";
    }*/
}

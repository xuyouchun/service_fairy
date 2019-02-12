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
    /// 修改手机号码
    /// </summary>
    [AppCommand("ModifyPhoneNumber", title: "修改手机号码", SecurityLevel = SecurityLevel.User), Remarks(Remarks), DisabledCommand]
    class ModifyPhoneNumberAppCommand : ACS<Service>.Action<User_ModifyPhoneNumber_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, User_ModifyPhoneNumber_Request req, ref ServiceResult sr)
        {
            Sid sid = context.Service.UserAccountManager.ModifyPhoneNumber(context.GetSessionState(), req.NewPhoneNumber, req.NewPassword, req.VerifyCode);
            sr = new ServiceResult(sid);
        }

        const string Remarks = @"该接口是修改手机号码的第二步。
要求用户先用原密码登录，再调用PreModifyPhoneNumber获取验证码，再调用该接口修改手机号码。
密码需要同时修改。";
    }*/
}

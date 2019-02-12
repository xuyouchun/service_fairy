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
    /// 用户注册
    /// </summary>
    [AppCommand("Register", title: "用户注册", SecurityLevel = SecurityLevel.Public), Remarks(Remarks), DisabledCommand]
    class RegisterAppCommand : ACS<Service>.Action<User_Register_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, User_Register_Request req, ref ServiceResult sr)
        {
            Sid sid = context.Service.UserAccountManager.SafeRegister(req.UserName, req.Password, req.AutoLogin, req.VerifyCode);
            sr = new ServiceResult(sid);
        }

        const string Remarks = @"新用户注册，如果该用户已经存在，将返回错误码：UserAlreadyExists";
    }*/
}

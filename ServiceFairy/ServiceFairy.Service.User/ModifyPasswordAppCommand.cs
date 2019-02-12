using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.User;
using ServiceFairy.Service.User.Components;
using ServiceFairy.SystemInvoke;
using Common.Contracts;

namespace ServiceFairy.Service.User
{
    /*
    /// <summary>
    /// 修改密码
    /// </summary>
    [AppCommand("ModifyPassword", title: "修改密码", SecurityLevel = SecurityLevel.User), Remarks(Remarks), DisabledCommand]
    class ModifyPasswordAppCommand : ACS<Service>.Action<User_ModifyPassword_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, User_ModifyPassword_Request req, ref ServiceResult sr)
        {
            Sid sid = context.Service.UserAccountManager.ModifyPassword(context.GetSessionState(), req.OldPassword, req.NewPassword);
            sr = new ServiceResult(sid);
        }

        const string Remarks = @"修改用户密码，要求用户当前处于登录状态。";
    }*/
}

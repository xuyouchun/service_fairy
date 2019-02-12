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
    /// 用户退出登录
    /// </summary>
    [AppCommand("Logout", title: "用户退出登录", SecurityLevel = SecurityLevel.User), Remarks(Remarks), DisabledCommand]
    class LogoutAppCommand : ACS<Service>.Action
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, ref ServiceResult sr)
        {
            context.Service.UserAccountManager.Logout(context.SessionState);
        }

        const string Remarks = @"用户退出登录";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Security;
using ServiceFairy.Service.Security.Components;

namespace ServiceFairy.Service.Security
{
    /// <summary>
    /// 登录
    /// </summary>
    [AppCommand("Login", "登录", SecurityLevel = SecurityLevel.Public)]
    class LoginAppCommand : ACS<Service>.Func<Security_Login_Request, Security_Login_Reply>
    {
        protected override Security_Login_Reply OnExecute(AppCommandExecuteContext<Service> context, Security_Login_Request request, ref ServiceResult sr)
        {
            var srv = context.Service;
            SystemUser user = srv.SystemAccountManager.Login(request.UserName, request.Password);
            sr = new ServiceResult(srv.SidGenerator.CreateSid(user.UserId, user.SecurityLevel, SidProperty.SystemUser));
            return new Security_Login_Reply { SidInfo = new SidInfo { SecurityLevel = user.SecurityLevel, UserId = user.UserId } };
        }
    }
}

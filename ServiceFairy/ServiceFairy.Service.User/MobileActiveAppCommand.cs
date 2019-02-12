using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.User;
using Common.Contracts.Service;
using Common.Contracts;

namespace ServiceFairy.Service.User
{
    /// <summary>
    /// 激活手机号
    /// </summary>
    [AppCommand("MobileActive", "激活手机号", SecurityLevel = SecurityLevel.Public), Remarks(Remarks)]
    class MobileActiveAppCommand : ACS<Service>.Func<User_MobileActive_Request, User_MobileActive_Reply>
    {
        protected override User_MobileActive_Reply OnExecute(AppCommandExecuteContext<Service> context, User_MobileActive_Request request, ref ServiceResult sr)
        {
            int userId;
            Sid sid = context.Service.MobileUserAccountManager.SafeActive(request.PhoneNumber, request.VerifyCode, out userId);

            sr = new ServiceResult(sid);
#warning 这里为什么需要userid来着？
            return new User_MobileActive_Reply { UserId = userId };
        }

        const string Remarks = @"使用手机号可直接进行不需要密码的注册，需要先调用SendVerifyCode接口获取验证码。";
    }
}

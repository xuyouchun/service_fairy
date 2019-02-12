using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Contracts;
using ServiceFairy.Entities.User;

namespace ServiceFairy.Service.User
{
    /// <summary>
    /// 停用手机号
    /// </summary>
    [AppCommand("MobileDeactive", "停用手机号", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class MobileDeactiveAppCommand : ACS<Service>.Action
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, ref ServiceResult sr)
        {
            context.Service.MobileUserAccountManager.Deactive(context.GetSessionState());
        }

        const string Remarks = @"停用手机号，可以使用MobileActive接口再次激活";
    }
}

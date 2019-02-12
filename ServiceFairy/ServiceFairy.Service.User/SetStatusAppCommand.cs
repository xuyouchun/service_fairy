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
    /// 设置用户状态
    /// </summary>
    [AppCommand("SetStatus", "设置用户状态", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class SetStatusAppCommand : ACS<Service>.Action<User_SetStatus_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, User_SetStatus_Request req, ref ServiceResult sr)
        {
            context.Service.UserStateManager.SetStatus(context.GetSessionState(), req.Status, req.StatusUrl);
        }

        private const string Remarks = @"设置当前登录用户的状态";
    }
}

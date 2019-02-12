using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.UserCenter;

namespace ServiceFairy.Service.UserCenter
{
    /// <summary>
    /// 获取用户所在的终端
    /// </summary>
    [AppCommand("GetUserPositions", "获取用户所在的终端", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class GetUserPositionsAppCommand : ACS<Service>.Func<UserCenter_GetUserPositions_Request, UserCenter_GetUserPositions_Reply>
    {
        protected override UserCenter_GetUserPositions_Reply OnExecute(AppCommandExecuteContext<Service> context, UserCenter_GetUserPositions_Request req, ref ServiceResult sr)
        {
            UserPosition[] positions = context.Service.RoutableUserConnectionManager.GetUserPositions(req.UserIds, req.EnableRoute);

            return new UserCenter_GetUserPositions_Reply() { Positions = positions };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.User;
using Common.Utility;
using Common.Contracts;

namespace ServiceFairy.Service.User
{
    /// <summary>
    /// 获取所关注用户的状态
    /// </summary>
    [AppCommand("GetStatus", "获取用户的状态", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class GetStatusAppCommand : ACS<Service>.Func<User_GetStatus_Request, User_GetStatus_Reply>
    {
        protected override User_GetStatus_Reply OnExecute(AppCommandExecuteContext<Service> context, User_GetStatus_Request req, ref ServiceResult sr)
        {
            UserStatusInfo[] status = context.Service.UserStateManager.GetStatus(context.GetSessionState(), req.UserIds);

            return new User_GetStatus_Reply() {
                Status = status.ToArray(st => new UserStatus {
                    UserId = st.UserId, ChangedTime = st.ChangedTime,
                    Online = st.Online, Status = st.Status
                })
            };
        }

        const string Remarks = @"获取在指定时间内发生变化的用户的状态";
    }
}

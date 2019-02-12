using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Group;
using Common.Contracts.Service;
using Common.Contracts;

namespace ServiceFairy.Service.Group
{
    /// <summary>
    /// 发送消息
    /// </summary>
    [AppCommand("SendMessage", "发送消息", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class SendMessageAppCommand : ACS<Service>.Action<Group_SendMessage_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Group_SendMessage_Request req, ref ServiceResult sr)
        {
            int userId = context.GetSessionState().BasicInfo.UserId;
            context.Service.GroupMessageDispatcher.SendMessage(userId, req.GroupId, req.Content);
        }

        const string Remarks = @"发送群组消息，要求用户必须为该群组的成员。";
    }
}

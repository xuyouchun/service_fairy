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
    /// 发送消息
    /// </summary>
    [AppCommand("SendMessage", "发送消息", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class SendMessageAppCommand : ACS<Service>.Action<User_SendMessage_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, User_SendMessage_Request req, ref ServiceResult sr)
        {
            context.Service.UserMessageDispatcher.SendUserMessage(context.GetSessionState(), req.To, req.Content, req.Property);
        }

        const string Remarks = @"向指定的用户发送用户自定义的消息，对方接收到消息时也需要按预定格式来解析。";
    }
}

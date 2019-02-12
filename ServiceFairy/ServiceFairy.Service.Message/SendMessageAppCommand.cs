using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Message;
using ServiceFairy.Service.Message.Components;

namespace ServiceFairy.Service.Message
{
    /// <summary>
    /// 发送消息
    /// </summary>
    [AppCommand("SendMessage", "发送消息", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class SendMessageAppCommand : ACS<Service>.Action<Message_SendMessage_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Message_SendMessage_Request request, ref ServiceResult sr)
        {
            Service service = context.Service;

            UserMsg[] msgItems = service.CombineMsgs(request.Message, request.ToUsers);
            service.MessageDispatcher.AddMessages(msgItems);
        }
    }
}

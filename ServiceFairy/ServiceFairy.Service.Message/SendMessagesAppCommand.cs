using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Message;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using ServiceFairy.Service.Message.Components;

namespace ServiceFairy.Service.Message
{
    /// <summary>
    /// 批量发送消息
    /// </summary>
    [AppCommand("SendMessages", "批量发送消息", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class SendMessagesAppCommand : ACS<Service>.Action<Message_SendMessages_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Message_SendMessages_Request req, ref ServiceResult sr)
        {
            Service service = context.Service;

            List<UserMsg> msgList = new List<UserMsg>();
            foreach (UserMsgArray arr in req.MsgArrs)
            {
                Users toUsers = arr.ToUsers;
                msgList.AddRange(service.CombineMsgs(arr.Msgs, toUsers));
            }

            service.MessageDispatcher.AddMessages(msgList.ToArray());
        }
    }
}

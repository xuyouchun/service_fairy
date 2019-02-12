using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Message;

namespace ServiceFairy.Service.Message
{
    /// <summary>
    /// 删除消息
    /// </summary>
    [AppCommand("RemoveMessage", "删除消息", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class RemoveMessageAppCommand : ACS<Service>.Action<Message_RemoveMessage_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Message_RemoveMessage_Request request, ref ServiceResult sr)
        {
            Service service = context.Service;
            service.MessageDispatcher.RemoveMessage(request.MsgIndex);
        }
    }
}

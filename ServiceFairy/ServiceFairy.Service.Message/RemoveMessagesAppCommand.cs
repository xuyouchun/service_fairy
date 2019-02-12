using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Message;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Message
{
    /// <summary>
    /// 删除消息
    /// </summary>
    [AppCommand("RemoveMessages", title: "批量删除消息", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class RemoveMessagesAppCommand : ACS<Service>.Action<Message_RemoveMessages_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Message_RemoveMessages_Request req, ref ServiceResult sr)
        {
            Service service = context.Service;
            service.MessageDispatcher.RemoveMessages(req.MsgIndexes);
        }
    }
}

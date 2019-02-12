using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Message;
using ServiceFairy.Entities;

namespace ServiceFairy.Service.Message
{
    /// <summary>
    /// 获取指定用户的Message
    /// </summary>
    [AppCommand("GetMessages", title: "获取指定用户的消息", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class GetMessagesAppCommand : ACS<Service>.Func<Message_GetMessages_Request, Message_GetMessages_Reply>
    {
        protected override Message_GetMessages_Reply OnExecute(AppCommandExecuteContext<Service> context, Message_GetMessages_Request req, ref ServiceResult sr)
        {
            Msg[] msgs = context.Service.MessageDispatcher.GetMessages(req.UserIds);

            return new Message_GetMessages_Reply() { Msgs = msgs };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.MessageCenter;
using Common.Contracts.Service;
using Common.Contracts;

namespace ServiceFairy.Service.MessageCenter
{
    /// <summary>
    /// 清空消息
    /// </summary>
    [AppCommand("Clear", "清空消息", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class ClearAppCommand : ACS<Service>.Action<MessageCenter_Clear_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, MessageCenter_Clear_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            context.Service.MessageStorage.Clear(request.UserIds);
        }

        const string Remarks = @"清空指定用户的消息";
    }
}

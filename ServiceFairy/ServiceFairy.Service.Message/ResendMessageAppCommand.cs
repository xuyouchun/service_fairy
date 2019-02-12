using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Message;

namespace ServiceFairy.Service.Message
{
    /// <summary>
    /// 将指定用户的已持久化的消息从消息中心提取出来重新发送
    /// </summary>
    [AppCommand("ResendMessage", "将指定用户的已持久化的消息从消息中心提取出来重新发送", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class ResendMessageAppCommand : ACS<Service>.Action<Message_ResendMessage_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Message_ResendMessage_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            int[] userIds = request.UserIds;
            
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Contracts.Service;
using ServiceFairy.Entities.MessageCenter;

namespace ServiceFairy.Service.MessageCenter
{
    /// <summary>
    /// 将消息批量持久化存储
    /// </summary>
    [AppCommand("Save", "将消息批量持久化存储", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class SaveAppCommand : ACS<Service>.Action<MessageCenter_Save_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, MessageCenter_Save_Request request, ref ServiceResult sr)
        {
            context.Service.MessageStorage.Save(request.MsgArrs);
        }
    }
}

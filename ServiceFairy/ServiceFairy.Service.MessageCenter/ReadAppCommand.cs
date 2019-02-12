using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Contracts.Service;
using ServiceFairy.Entities.MessageCenter;
using ServiceFairy.Entities;

namespace ServiceFairy.Service.MessageCenter
{
    /// <summary>
    /// 读取指定用户持久化存储的消息
    /// </summary>
    [AppCommand("Read", "读取指定用户持久化存储的消息", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class ReadAppCommand : ACS<Service>.Func<MessageCenter_Read_Request, MessageCenter_Read_Reply>
    {
        protected override MessageCenter_Read_Reply OnExecute(AppCommandExecuteContext<Service> context, MessageCenter_Read_Request request, ref ServiceResult sr)
        {
            Msg[] arrs = context.Service.MessageStorage.Read(request.UserId);
            return new MessageCenter_Read_Reply { Msgs = arrs };
        }
    }
}

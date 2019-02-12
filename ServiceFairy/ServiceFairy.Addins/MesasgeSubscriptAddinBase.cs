using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.Entities.Addins.MesssageSubscript;

namespace ServiceFairy.Addins
{
    /// <summary>
    /// 插件
    /// </summary>
    public abstract class MessageSubscriptAddinBase : TrayAppServiceAddinBase
    {
        public MessageSubscriptAddinBase(AppServiceBase service)
            : base(service)
        {
            
        }

        [AppCommand("ApplyMessage", "接收消息")]
        protected abstract MesasgeSubscript_ApplyMessage_Reply OnApplyMessage(AppCommandExecuteContext context, MesasgeSubscript_ApplyMessage_Request request, ref ServiceResult sr);
    }
}

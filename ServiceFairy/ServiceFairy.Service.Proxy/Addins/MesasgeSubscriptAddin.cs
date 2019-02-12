using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Components;
using ServiceFairy.Entities.Addins.MesssageSubscript;
using Common.Package;
using Common.Utility;
using ServiceFairy.Entities.Message;
using ServiceFairy.Entities;
using ServiceFairy.Addins;

namespace ServiceFairy.Service.Proxy.Addins
{
    /// <summary>
    /// 消息通知插件
    /// </summary>
    [AppServiceAddin(ServiceAddinNames.ADDIN_MESSAGE_SUBSCRIPT, "1.0", "消息订阅通知插件")]
    class MesasgeSubscriptAddin : MessageSubscriptAddinBase
    {
        public MesasgeSubscriptAddin(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;

        protected override MesasgeSubscript_ApplyMessage_Reply OnApplyMessage(AppCommandExecuteContext context, MesasgeSubscript_ApplyMessage_Request request, ref ServiceResult sr)
        {
            if (!request.UserMsgs.IsNullOrEmpty())
                _service.MessageDispatcher.DispatchMessage(request.UserMsgs, context.CommunicateContext.Caller);

            return new MesasgeSubscript_ApplyMessage_Reply();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy;
using ServiceFairy.Entities.Sms;
using Common.Contracts.Service;
using Common.Package;
using Common.Utility;

namespace ServiceFairy.Service.Sms
{
    [AppCommand("Send", "发送短信", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class SendAppCommand : ACS<Service>.Action<Sms_Send_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Sms_Send_Request req, ref ServiceResult sr)
        {
            return;

            Service srv = (Service)context.Service;
            Dictionary<string, string> result = srv.SmsManager.Send(req.PhoneNumbers, req.Content);
            if ((result == null) || (result.Count == 0))
                throw new ServiceException(ServiceStatusCode.BusinessError, "发送失败");
        }
    }
}

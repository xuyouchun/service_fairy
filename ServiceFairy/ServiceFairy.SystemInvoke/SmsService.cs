using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Sms;
using Common.Package.Service;
using System.Diagnostics.Contracts;
using ServiceFairy.Entities.Cache;
using Common.Contracts.Service;
using Common.Contracts;

namespace ServiceFairy.SystemInvoke
{
    public static class SmsService
    {
        public static ServiceResult Send(IServiceClient serviceClient, Sms_Send_Request request, CallingSettings settings = null)
        {
            Contract.Requires(serviceClient != null);

            return serviceClient.Call(SFNames.ServiceNames.Sms + "/Send", request, settings);
        }
    }
}

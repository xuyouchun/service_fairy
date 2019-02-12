using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Security;

namespace ServiceFairy.Service.Security
{
    /// <summary>
    /// 申请安全码
    /// </summary>
    [AppCommand("AcquireSid", "申请安全码", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class AcquireSidAppCommand : ACS<Service>.Func<Security_AcquireSid_Request, Security_AcquireSid_Reply>
    {
        protected override Security_AcquireSid_Reply OnExecute(AppCommandExecuteContext<Service> context, Security_AcquireSid_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            /*if (request.SecurityLevel >= context.GetSessionState().SecurityLevel)
                throw Utility.CreateException(SecurityStatusCode.NoSecurityWhenAcquireSid);*/

            Sid sid = context.Service.SidGenerator.CreateSid(request.UserId, request.SecurityLevel);
            return new Security_AcquireSid_Reply { Sid = sid };
        }
    }
}

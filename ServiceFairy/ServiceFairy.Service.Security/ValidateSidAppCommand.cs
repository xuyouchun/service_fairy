using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Security;

namespace ServiceFairy.Service.Security
{
    /// <summary>
    /// 验证安全码是否有效
    /// </summary>
    [AppCommand("ValidateSid", "验证安全码是否有效", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class ValidateSidAppCommand : ACS<Service>.Func<Security_ValidateSid_Request, Security_ValidateSid_Reply>
    {
        protected override Security_ValidateSid_Reply OnExecute(AppCommandExecuteContext<Service> context, Security_ValidateSid_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            int userId = context.Service.GetUserId(request.Sid);

            return new Security_ValidateSid_Reply { UserId = userId };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Security;
using Common.Utility;

namespace ServiceFairy.Service.Security
{
    /// <summary>
    /// 批量验证安全码的有效性
    /// </summary>
    [AppCommand("ValidateSids", "批量验证安全码的有效性", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class ValidateSidsAppCommand : ACS<Service>.Func<Security_ValidateSids_Request, Security_ValidateSids_Reply>
    {
        protected override Security_ValidateSids_Reply OnExecute(AppCommandExecuteContext<Service> context, Security_ValidateSids_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            int[] userIds = request.Sids.ToArray(sid => context.Service.GetUserId(sid));
            return new Security_ValidateSids_Reply { UserIds = userIds };
        }
    }
}

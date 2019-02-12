using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Security;
using ServiceFairy.Service.Security.Components;

namespace ServiceFairy.Service.Security
{
    /// <summary>
    /// 获取安全码信息
    /// </summary>
    [AppCommand("GetSidInfo", "获取安全码信息", SecurityLevel = SecurityLevel.Public)]
    class GetSidInfoAppCommand : ACS<Service>.Func<Security_GetSidInfo_Request, Security_GetSidInfo_Reply>
    {
        protected override Security_GetSidInfo_Reply OnExecute(AppCommandExecuteContext<Service> context, Security_GetSidInfo_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            var sidGenerator = context.Service.SidGenerator;

            SidX sid = sidGenerator.Decrypt(request.Sid, true);
            return new Security_GetSidInfo_Reply { Info = new SidInfo { UserId = sid.UserId, SecurityLevel = sid.SecurityLevel } };
        }
    }
}

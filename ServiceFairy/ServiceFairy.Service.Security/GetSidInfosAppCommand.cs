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
    /// 批量获取安全码的信息
    /// </summary>
    [AppCommand("GetSidInfos", "批量获取安全码的信息", SecurityLevel = SecurityLevel.Public)]
    class GetSidInfosAppCommand : ACS<Service>.Func<Security_GetSidInfos_Request, Security_GetSidInfos_Reply>
    {
        protected override Security_GetSidInfos_Reply OnExecute(AppCommandExecuteContext<Service> context, Security_GetSidInfos_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            var sidGenerator = context.Service.SidGenerator;

            Sid[] sids = request.Sids;
            SidInfo[] sidInfos = new SidInfo[sids.Length];
            for (int k = 0, length = sidInfos.Length; k < length; k++)
            {
                SidX sid = sidGenerator.Decrypt(sids[k], false);
                if (!sid.IsEmpty())
                    sidInfos[k] = new SidInfo { UserId = sid.UserId, SecurityLevel = sid.SecurityLevel };
            }

            return new Security_GetSidInfos_Reply { Infos = sidInfos };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Security;

namespace ServiceFairy.Service.Security
{
    /// <summary>
    /// 批量申请安全码
    /// </summary>
    [AppCommand("AcquireSids", "批量申请安全码", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class AcquireSidsAppCommand : ACS<Service>.Func<Security_AcquireSids_Request, Security_AcquireSids_Reply>
    {
        protected override Security_AcquireSids_Reply OnExecute(AppCommandExecuteContext<Service> context, Security_AcquireSids_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            UserSessionState uss = context.GetSessionState();
            //_ValidateSecurityLevel(uss, request.Items);

            var sidGenerator = context.Service.SidGenerator;
            List<AcquiredSidPair> pairs = new List<AcquiredSidPair>();

            foreach (AcquireSidItem item in request.Items)
            {
                if (item.Ids == null)
                    continue;

                foreach (int id in item.Ids)
                {
                    Sid sid = sidGenerator.CreateSid(id, item.SecurityLevel);
                    pairs.Add(new AcquiredSidPair { Id = id, Sid = sid });
                }
            }

            return new Security_AcquireSids_Reply { Sids = pairs.ToArray() };
        }

        private void _ValidateSecurityLevel(UserSessionState uss, AcquireSidItem[] items)
        {
            if (items.Any(item => item.SecurityLevel >= uss.SecurityLevel))
                throw Utility.CreateException(SecurityStatusCode.NoSecurityWhenAcquireSid);
        }
    }
}

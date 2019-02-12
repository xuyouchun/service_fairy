using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Watch;

namespace ServiceFairy.Service.Watch
{
    /// <summary>
    /// 获取监控信息
    /// </summary>
    [AppCommand("GetWatchInfo", "获取监控信息", SecurityLevel = SecurityLevel.CoreRunningLevel)]
    class GetWatchInfoAppCommand : ACS<Service>.Func<Watch_GetWatchInfo_Reply>
    {
        protected override Watch_GetWatchInfo_Reply OnExecute(AppCommandExecuteContext<Service> context, ref ServiceResult sr)
        {
            throw new NotImplementedException();
        }
    }
}

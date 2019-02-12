using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Tray;

namespace ServiceFairy.Service.Tray
{
    /// <summary>
    /// 获取线程信息
    /// </summary>
    [AppCommand("GetThreads", "获取线程信息", SecurityLevel = SecurityLevel.Admin)]
    class GetThreadsAppCommand : ACS<Service>.Func<Tray_GetThreads_Request, Tray_GetThreads_Reply>
    {
        protected override Tray_GetThreads_Reply OnExecute(AppCommandExecuteContext<Service> context, Tray_GetThreads_Request req, ref ServiceResult sr)
        {
            Service srv = context.Service;
            ThreadInfoCollection[] cols = req.ProcessIds.Distinct().Select(
                pId => new ThreadInfoCollection() { ProcessId = pId, ThreadInfos = srv.ProcessManager.GetThreadInfos(pId) }
            ).ToArray();

            return new Tray_GetThreads_Reply() { ThreadInfoCollections = cols };
        }
    }
}

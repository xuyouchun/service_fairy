using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Package.Service;
using Common.Framework.TrayPlatform;
using ServiceFairy.Entities.Station;

namespace ServiceFairy.Service.Station
{
    /// <summary>
    /// 引发事件
    /// </summary>
    [AppCommand("RaiseEvent", "引发事件", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class RaiseEventAppCommand : ACS<Service>.Action<Station_RaiseEvent_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Station_RaiseEvent_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            srv.ServiceEventManager.Raise(new ServiceEndPoint(req.ClientID, req.Caller), req.EventName, req.EventArgs, req.EnableRoute);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Utility;
using Common.Package.Service;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Service.Master.StationAppCommands
{
    /// <summary>
    /// 上传终端信息
    /// </summary>
    [AppCommand("StationHeartBeat", "响应基站服务的心跳", SecurityLevel = SecurityLevel.CoreRunningLevel)]
    class StationHeartBeatAppCommand : ACS<Service>.Action<Master_StationHeartBeat_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Master_StationHeartBeat_Request req, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            if (!req.AppClientInfos.IsNullOrEmpty())
            {
                srv.AppClientManager.UpdateClientInfos(req.AppClientInfos);
            }

            if (!req.DisconnectedClientIds.IsNullOrEmpty())
            {
                srv.AppClientManager.DisconnectedNotify(req.DisconnectedClientIds);
            }
        }
    }
}

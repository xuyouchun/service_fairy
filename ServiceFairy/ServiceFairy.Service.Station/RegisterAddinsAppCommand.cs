using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Station;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Station
{
    /// <summary>
    /// 注册插件
    /// </summary>
    [AppCommand("RegisterAddins", "注册插件", SecurityLevel = SecurityLevel.AppRunningLevel)]
    class RegisterAddinsAppCommand : ACS<Service>.Action<Station_RegisterAddins_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Station_RegisterAddins_Request request, ref ServiceResult sr)
        {
            context.Service.ServiceAddinManager.Register(request.Relations);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Proxy;

namespace ServiceFairy.Service.Proxy
{
    /// <summary>
    /// 设置终端设备的信息
    /// </summary>
    [AppCommand("Init", "设置终端设备的信息", SecurityLevel = SecurityLevel.Public)]
    class InitAppCommand : ACS<Service>.Action<Proxy_Init_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, Proxy_Init_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            
        }
    }
}

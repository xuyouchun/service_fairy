using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Package.Service;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Service.Master
{
    /// <summary>
    /// 获取所有的服务终端描述信息
    /// </summary>
    [AppCommand("GetAllClientDescs", "获取所有的服务终端描述信息", SecurityLevel = SecurityLevel.CoreRunningLevel)]
    class GetAllClientDescsAppCommand : ACS<Service>.Func<Master_GetAllClientDesc_Reply>
    {
        protected override Master_GetAllClientDesc_Reply OnExecute(AppCommandExecuteContext<Service> context, ref ServiceResult sr)
        {
            Service srv = (Service)context.Service;
            ClientDesc[] clientDescs = srv.AppClientManager.GetAllClientDescs();
            return new Master_GetAllClientDesc_Reply() { ClientDescs = clientDescs };
        }
    }
}

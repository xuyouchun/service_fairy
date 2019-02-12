using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Deploy;

namespace ServiceFairy.Service.Deploy
{
    /// <summary>
    /// 获取所有服务终端的唯一标识
    /// </summary>
    [AppCommand("GetAllClientIds", "获取所有服务终端的唯一标识")]
    class GetAllClientIdsAppCommand : ACS<Service>.Func<Deploy_GetAllClientIds_Reply>
    {
        protected override Deploy_GetAllClientIds_Reply OnExecute(AppCommandExecuteContext<Service> context, ref ServiceResult sr)
        {
            Guid[] clientIds = context.Service.DeployMapManager.GetAllClientIds();
            return new Deploy_GetAllClientIds_Reply { ClientIds = clientIds };
        }
    }
}

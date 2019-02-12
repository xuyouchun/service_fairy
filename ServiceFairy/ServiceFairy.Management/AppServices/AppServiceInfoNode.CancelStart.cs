using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.UIObject;
using Common.Contracts.UIObject;
using Common.Contracts.Service;

namespace ServiceFairy.Management.AppServices
{
    partial class AppServiceInfoNode
    {
        /// <summary>
        /// 取消启动
        /// </summary>
        class CancelStartAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                CurrentStatus = ServiceStatus.Default;
                StopAllRefresh(ServiceObjectRefreshResult.Dispose);
                Invoker.Master.StopService(ClientCtx.ClientDesc.ClientID, ServieDesc);
            }
        }
    }
}

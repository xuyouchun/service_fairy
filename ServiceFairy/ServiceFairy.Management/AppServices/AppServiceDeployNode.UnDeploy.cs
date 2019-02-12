using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;
using Common.Utility;
using Common.Contracts.Service;
using Common.WinForm;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Management.AppServices
{
    partial class AppServiceDeployNode
    {
        /// <summary>
        /// 停止服务
        /// </summary>
        class UnDeployAction : ActionBase
        {
            public override void Execute(Common.Contracts.UIObject.IUIObjectExecuteContext context, Common.Contracts.Service.IServiceObject serviceObject)
            {
                IUIOperation uiOp = context.ServiceProvider.GetService<IUIOperation>(true);
                ClientDesc cd = Kernel.ClientDesc;
                ServiceDesc sd = Kernel.ServiceDesc;

                if (!sd.CanStop())
                {
                    uiOp.ShowError("该服务不允许手动卸载");
                    return;
                }

                if (uiOp.ShowQuery(string.Format("确认要在终端“{0}”上卸载服务“{1}”吗？", cd, sd)) != true)
                    return;

                Kernel.CurrentStatus = ServiceStatus.Stopping;
                Kernel._mgrCtx.Invoker.Master.StopService(cd.ClientID, sd);
                Kernel.StartRefresh(delegate {
                    ServiceDeployInfo sdInfo = Kernel._mgrCtx.ServiceDeployInfos.Get(sd);
                    if (sdInfo != null && !sdInfo.ClientIDs.IsNullOrEmpty() && sdInfo.ClientIDs.Contains(cd.ClientID))
                        return ServiceObjectRefreshResult.ContinueAndRefresh;

                    return ServiceObjectRefreshResult.CompletedAndDispose;
                });
            }
        }
    }
}

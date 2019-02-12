using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.UIObject;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using Common.WinForm;
using Common.Utility;
using Common.Contracts;
using Common.Package.Service;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Management.AppClients;

namespace ServiceFairy.Management.AppServices
{
    partial class AppServiceInfoNode
    {
        class StopServiceAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation uiOp = context.ServiceProvider.GetService<IUIOperation>(true);
                ServiceDesc sd = Kernel._serviceInfo.ServiceDesc;
                if (!sd.CanStop())
                {
                    uiOp.ShowError("不允许停止该服务");
                    return;
                }

                string prompt = SFNames.ServiceNames.IsCoreService(sd) ? "该服务为核心服务，确认要停止吗？" : "确认要停止该服务吗？";
                if (uiOp.ShowQuery(prompt) == false)
                    return;

                AppClientContext clientCtx = Kernel._clientCtx;
                SystemInvoker invoker = clientCtx.MgrCtx.Invoker;
                invoker.Master.StopService(clientCtx.ClientDesc.ClientID, sd);

                Kernel.StartRefresh(delegate {
                    ServiceInfo serviceInfo = clientCtx.GetServiceInfo(sd);
                    if (serviceInfo == null)
                    {
                        Kernel.CurrentStatus = ServiceStatus.Default;
                        return ServiceObjectRefreshResult.CompletedAndDispose;
                    }
                    else
                    {
                        Kernel.CurrentStatus = ServiceStatus.Stopping;
                        return ServiceObjectRefreshResult.ContinueAndRefresh;
                    }
                });
            }
        }
    }
}

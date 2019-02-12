using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.UIObject;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.SystemInvoke;
using Common.WinForm;
using Common.Utility;
using Common;
using ServiceFairy.Client;
using ServiceFairy.Management.AppClients;

namespace ServiceFairy.Management.AppServices
{
	partial class AppServiceInfoNode
	{
        /// <summary>
        /// 重新启动服务
        /// </summary>
        class RestartServiceAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation uiOp = context.ServiceProvider.GetService<IUIOperation>(true);
                AppClientContext clientCtx = Kernel._clientCtx;
                SystemInvoker invoker = clientCtx.MgrCtx.Invoker;
                ServiceDesc sd = Kernel._serviceInfo.ServiceDesc;

                if (!sd.CanRestart())
                {
                    uiOp.ShowError("不允许重启该服务");
                    return;
                }

                string prompt = SFNames.ServiceNames.IsCoreService(sd) ? "该服务为核心服务，确认要重启吗？" : "确认要重启该服务吗？";
                if (uiOp.ShowQuery(prompt) == false)
                    return;

                Kernel.CurrentStatus = ServiceStatus.Restarting;
                bool stopping = true;
                invoker.Master.StopService(clientCtx.ClientDesc.ClientID, sd);
                Kernel.StartRefresh(delegate() {
                    ServiceInfo si = clientCtx.GetServiceInfo(sd);
                    if (stopping)
                    {
                        if (si == null)
                        {
                            invoker.Master.StartService(clientCtx.ClientDesc.ClientID, sd);
                            stopping = false;
                        }
                    }
                    else
                    {
                        if (si != null && si.Status == AppServiceStatus.Running)
                        {
                            Kernel.CurrentStatus = ServiceStatus.Default;
                            Kernel._serviceInfo = si;
                            return ServiceObjectRefreshResult.CompletedAndRefresh;
                        }
                    }

                    return ServiceObjectRefreshResult.ContinueAndRefresh;
                });
            }
        }
	}
}

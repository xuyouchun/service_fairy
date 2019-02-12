using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using Common.WinForm;
using ServiceFairy.SystemInvoke;
using Common.Utility;

namespace ServiceFairy.Management.AppClients
{
    partial class AppClientCommunicationNode
    {
        /// <summary>
        /// 关闭
        /// </summary>
        class CloseAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation operation = context.ServiceProvider.GetService<IUIOperation>(true);
                if (operation.ShowQuery("确认要停止该信道吗？", defaultButton: false) == false)
                    return;

                AppClientContext clientCtx = Kernel._clientCtx;
                SystemInvoker invoker = clientCtx.MgrCtx.Invoker;
                invoker.Master.CloseCommunication(clientCtx.AppClientInfo.ClientId, Kernel._communicationOption.Address);

                Kernel.CurrentStatus = ServiceStatus.Stopping;
                Kernel.StartRefresh(delegate {

                    if (clientCtx.GetCommunicationOption(Kernel._communicationOption.Address) == null)
                    {
                        Kernel.CurrentStatus = ServiceStatus.Stopped;
                        return ServiceObjectRefreshResult.CompletedAndDispose;
                    }

                    return ServiceObjectRefreshResult.ContinueAndRefresh;
                });
            }
        }
    }
}

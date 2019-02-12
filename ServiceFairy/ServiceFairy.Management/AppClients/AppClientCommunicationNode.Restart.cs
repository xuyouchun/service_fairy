using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.WinForm;
using Common.Utility;
using ServiceFairy.SystemInvoke;
using Common.Contracts.Service;

namespace ServiceFairy.Management.AppClients
{
	partial class AppClientCommunicationNode
	{
        /// <summary>
        /// 关闭
        /// </summary>
        class RestartAction : ActionBase
        {
            public override void Execute(Common.Contracts.UIObject.IUIObjectExecuteContext context, Common.Contracts.Service.IServiceObject serviceObject)
            {
                IUIOperation operation = context.ServiceProvider.GetService<IUIOperation>(true);
                if (operation.ShowQuery("确认要重新启动该信道吗？", defaultButton: false) == false)
                    return;

                AppClientContext clientCtx = Kernel._clientCtx;
                SystemInvoker invoker = clientCtx.MgrCtx.Invoker;
                ServiceAddress sa = Kernel._communicationOption.Address;
                invoker.Master.CloseCommunication(clientCtx.AppClientInfo.ClientId, sa);
                Kernel.CurrentStatus = ServiceStatus.Restarting;
                bool stopping = true;
                Kernel.StartRefresh(delegate {
                    bool exist = clientCtx.ExistCommunication(sa);
                    if (stopping)
                    {
                        if (!exist)
                        {
                            stopping = false;
                            invoker.Master.OpenCommunication(clientCtx.AppClientInfo.ClientId, Kernel._communicationOption);
                        }
                    }
                    else
                    {
                        if (exist)
                        {
                            Kernel.CurrentStatus = ServiceStatus.Default;
                            return ServiceObjectRefreshResult.CompletedAndRefresh;
                        }
                    }

                    return ServiceObjectRefreshResult.ContinueAndRefresh;
                });
            }
        }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using Common.WinForm;
using Common.Utility;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Management.DeployPackage
{
    partial class PlatformDeployPackageNode
    {
        /// <summary>
        /// 部署到指定的终端
        /// </summary>
        class DeployAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {

            }
        }

        private string _deployingRate;

        /// <summary>
        /// 部署到所有终端
        /// </summary>
        class DeployAllAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation uiOp = context.ServiceProvider.GetService<IUIOperation>(true);
                if (uiOp.ShowQuery("该项操作风险比较大，可能造成服务中断，是否继续？") == false)
                    return;

                Kernel._mgrCtx.Invoker.Master.DeployPlatformPackageToAllClients(Kernel._packageInfo.Id);

                Kernel._deployingRate = "";
                Kernel.CurrentStatus = ServiceStatus.Deploying;
                Kernel.StartRefresh(_RefreshStatus);
            }

            private ServiceObjectRefreshResult _RefreshStatus()
            {
                try
                {
                    PlatformDeployProgress[] progresses = Kernel._mgrCtx.Invoker.Master.GetPlatformDeployProgress();
                    if (progresses.IsNullOrEmpty())
                    {
                        Kernel.CurrentStatus = ServiceStatus.Deployed;
                        return ServiceObjectRefreshResult.CompletedAndRefresh;
                    }

                    int completedCount =progresses.Count(p => _IsCompleted(p)); 
                    float rate = (float) completedCount/ progresses.Length;
                    Kernel._deployingRate = string.Format("{0}% ({1}/{2})", (int)(rate * 100), completedCount, progresses.Length);
                    if (rate >= 1)
                    {
                        Kernel.CurrentStatus = ServiceStatus.Deployed;
                        return ServiceObjectRefreshResult.CompletedAndRefresh;
                    }

                    Kernel.CurrentStatus = ServiceStatus.Deploying;
                    return ServiceObjectRefreshResult.ContinueAndRefresh;
                }
                catch (Exception)
                {
                    Kernel.CurrentStatus = ServiceStatus.SignalBreak;
                    return ServiceObjectRefreshResult.Continue;
                }
            }

            private bool _IsCompleted(PlatformDeployProgress p)
            {
                return p.Status == PlatformDeployStatus.Completed || p.Status == PlatformDeployStatus.Error || p.Status == PlatformDeployStatus.Timeout;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using Common.WinForm;
using Common.Utility;
using ServiceFairy.Entities.Master;
using ServiceFairy.Entities;
using ServiceFairy;

namespace ServiceFairy.Management.DeployPackage
{
    partial class ServiceDeployPackageNode
    {
        /// <summary>
        /// 部署到指定的终端
        /// </summary>
        class DeployAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                throw new NotImplementedException();
            }
        }

        private float _deployingRate = 0.0f;

        /// <summary>
        /// 部署到所有终端
        /// </summary>
        class DeployAllAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation uiOp = context.ServiceProvider.GetService<IUIOperation>(true);
                if (!_ShowPrompt(uiOp))
                    return;

                Kernel._mgrCtx.Invoker.Master.DeployServicePackageToAllClients(Kernel._packageInfo.Id);
                Kernel._deployingRate = 0.0f;
                Kernel.CurrentStatus = ServiceStatus.Deploying;
                Kernel.StartRefresh(_RefreshStatus);
            }

            private ServiceObjectRefreshResult _RefreshStatus()
            {
                try
                {
                    ServiceDeployProgress[] progresses = Kernel._mgrCtx.Invoker.Master.GetServiceDeployProgress(Kernel._packageInfo.ServiceDesc);
                    if (progresses.IsNullOrEmpty())
                    {
                        Kernel.CurrentStatus = ServiceStatus.Deployed;
                        return ServiceObjectRefreshResult.CompletedAndRefresh;
                    }

                    float rate = (float)progresses.Count(p => _IsCompleted(p)) / progresses.Length;
                    Kernel._deployingRate = rate;
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

            private bool _IsCompleted(ServiceDeployProgress p)
            {
                return p.Status == ServiceDeployStatus.Completed || p.Status == ServiceDeployStatus.Error || p.Status == ServiceDeployStatus.Timeout;
            }

            private bool _ShowPrompt(IUIOperation uiOp)
            {
                ServiceDesc sd = Kernel._packageInfo.ServiceDesc;
                ServiceDeployInfo deployInfo = Kernel._mgrCtx.ServiceDeployInfos.Get(sd);
                if (deployInfo == null || deployInfo.ClientIDs.IsNullOrEmpty())
                {
                    return uiOp.ShowQuery(string.Format("服务“{0}”尚未在任何终端上运行，该版本将被置为当前版本，是否继续？", sd)) == true;
                }

                if (sd.IsTrayService())
                {
                    return uiOp.ShowQuery(string.Format("服务“{0}”的部署将导致所有服务重新启动，可能导致服务中断，是否继续？", sd)) == true;
                }

                if (sd.IsMasterService())
                {
                    return uiOp.ShowQuery(string.Format("服务“{0}”的部署将导致中心服务重新启动，是否继续？", sd)) == true;
                }

                if (deployInfo.ClientIDs.Length == 1)  // 该服务只有一份部署
                {
                    return uiOp.ShowQuery(string.Format("服务“{0}”只在一个终端上运行，该部署将导致服务中断，是否继续？", sd)) == true;
                }

                return uiOp.ShowQuery(string.Format("服务“{0}”在{1}个终端上运行，确认要升级该服务吗？", sd, deployInfo.ClientIDs.Length)) == true;
            }
        }

        /// <summary>
        /// 置为当前版本
        /// </summary>
        class SetCurrentAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation uiOp = context.ServiceProvider.GetService<IUIOperation>(true);
                if (uiOp.ShowQuery("确认要置为当前版本吗？") == false)
                    return;

                Kernel._mgrCtx.Invoker.Master.DeployServicePackage(new Guid[0], Kernel._packageInfo.Id);
                MgrCtx.ServiceDeployPackageInfos.ClearCache();
                Refresh(context, serviceObject);
            }
        }
    }
}

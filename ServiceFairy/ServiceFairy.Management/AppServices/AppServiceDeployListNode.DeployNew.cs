using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.WinForm;
using Common.Utility;
using ServiceFairy.Management.AppClients;
using Common.Package.Service;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Management.AppServices
{
	partial class AppServiceDeployListNode
	{
        /// <summary>
        /// 部署新终端
        /// </summary>
        class DeployNewAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation uiOp = context.ServiceProvider.GetService<IUIOperation>(true);
                ServiceDesc sd = Kernel._serviceDesc;
                if (!sd.CanStart())
                {
                    uiOp.ShowError("该服务不允许手动部署");
                    return;
                }

                ClientDesc[] clientDescs = Kernel._mgrCtx.ClientDescs.GetAll();
                ClientDesc[] selectedClientDescs = uiOp.SelectWithImage(
                    clientDescs.Except(Kernel._GetAllClientDescs()),
                    "选择要部署该服务的终端", ResourceNames.AppClient
                );

                if (selectedClientDescs.IsNullOrEmpty()
                    || (selectedClientDescs = selectedClientDescs.Except(Kernel._GetAllClientDescs()).ToArray()).IsNullOrEmpty())
                    return;

                Kernel._mgrCtx.Invoker.Master.StartService(selectedClientDescs.ToArray(cd => cd.ClientID), new[] { sd});
                Kernel.AddRange(selectedClientDescs.Select(cd => new AppClientNodeProcess(Kernel, cd)));
                ServiceUtility.RefreshCurrentListView(context, serviceObject);
            }

            class AppClientNodeProcess : AppServiceDeployNode
            {
                public AppClientNodeProcess(AppServiceDeployListNode kernel, ClientDesc clientDesc)
                    : base(kernel._mgrCtx, clientDesc, kernel._serviceDesc)
                {
                    _clientDesc = clientDesc;
                    _kernel = kernel;
                    CurrentStatus = ServiceStatus.Starting;
                }

                private readonly ClientDesc _clientDesc;
                private readonly AppServiceDeployListNode _kernel;

                protected override ServiceObjectRefreshResult OnInitRefresh(InitContext initCtx)
                {
                    if (_kernel._GetAllClientDescs().Contains(_clientDesc))
                    {
                        CurrentStatus = ServiceStatus.Default;
                        return ServiceObjectRefreshResult.CompletedAndRefresh;
                    }

                    return ServiceObjectRefreshResult.ContinueAndRefresh;
                }
            }
        }
	}
}

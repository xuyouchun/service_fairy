using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.SystemInvoke;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using Common.WinForm;
using System.Windows.Forms;
using Common.Utility;
using Common.Package.UIObject;
using Common.Package.Service;
using Common.Contracts;
using ServiceFairy.Client;
using ServiceFairy.Management.AppServices;
using ServiceFairy.Management.AppClients;

namespace ServiceFairy.Management.AppServices
{
    partial class AppClientServiceInfoListNode
	{
        /// <summary>
        /// 启动新服务
        /// </summary>
        class StartNewServiceAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                AppClientContext clientCtx = Kernel._clientCtx;
                SystemInvoker invoker = clientCtx.MgrCtx.Invoker;
                ServiceDesc[] sds = invoker.Master.GetAllServices();

                IUIOperation uiOperation = context.ServiceProvider.GetService<IUIOperation>(true);
                var selectedSds = uiOperation.SelectWithImage(
                    sds.Except(clientCtx.AppClientInfo.ServiceInfos.Select(si => si.ServiceDesc)),
                    "选择要启动的服务", ResourceNames.AppServiceInfo, SelectionMode.MultiExtended
                );
                if (selectedSds == null)
                    return;

                ServiceDesc[] cannotStartSds = selectedSds.Where(sd => !sd.CanStart()).ToArray();
                if (cannotStartSds.Length > 0)
                {
                    string msg = "服务" + string.Join(", ", (object[])cannotStartSds) + "不允许手动启动";
                    if (selectedSds.Length == cannotStartSds.Length)
                    {
                        uiOperation.ShowError(msg);
                        return;
                    }
                    else
                    {
                        if (uiOperation.ShowQuery(msg + "，是否继续启动其它服务？") != true)
                            return;
                    }
                }

                selectedSds = selectedSds.Except(clientCtx.AppClientInfo.ServiceInfos.Select(si => si.ServiceDesc))
                    .Where(sd => sd.CanStart()).ToArray();

                if (selectedSds.Length > 0)
                {
                    foreach (ServiceDesc sd in selectedSds)
                    {
                        Kernel.Add(AppServiceInfoNode.StartNew(Kernel._clientCtx, sd));
                    }

                    ServiceUtility.RefreshCurrentListView(context, serviceObject);
                }
            }
        }
    }
}

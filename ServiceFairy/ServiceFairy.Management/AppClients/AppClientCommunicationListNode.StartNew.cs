using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Client;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Service;
using Common.WinForm;
using Common.Contracts.UIObject;
using System.Windows.Forms;
using Common.Utility;
using Common.Contracts;
using ServiceFairy.WinForm;
using Common.Communication.Wcf;

namespace ServiceFairy.Management.AppClients
{
    partial class AppClientCommunicationListNode
    {
        /// <summary>
        /// 开启新信道
        /// </summary>
        class StartNewCommunicationAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                AppClientInfo info = Kernel._clientCtx.AppClientInfo;
                SystemInvoker invoker = Kernel._clientCtx.MgrCtx.Invoker;
                IUIOperation uiOp = context.ServiceProvider.GetService<IUIOperation>(true);
                CommunicationDialog dlg = new CommunicationDialog(ipEditable: false, isDirectEnable: true);
                dlg.SetDefault(info.IPs, _CreateDefaultCommunicationOption());

                if (uiOp.ShowDialog(dlg) != DialogResult.OK)
                    return;

                Kernel.Add(AppClientCommunicationNode.StartNew(Kernel._clientCtx, dlg.CommunicationOption));
                Refresh(context, serviceObject);
            }

            private CommunicationOption _CreateDefaultCommunicationOption()
            {
                AppClientInfo info = Kernel._clientCtx.AppClientInfo;
                return new CommunicationOption(
                    new ServiceAddress("127.0.0.1", _FindNotUsedPort()), CommunicationType.Http);
            }

            private int _FindNotUsedPort()
            {
                AppClientInfo info = Kernel._clientCtx.AppClientInfo;
                for (int port = 9000; port <= 9999; port++)
                {
                    if (info.Communications.All(c => c.Address.Port != port))
                        return port;
                }

                return 0;
            }
        }
    }
}

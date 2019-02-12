using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Service;
using Common.Package.UIObject.Actions;
using ServiceFairy.Entities.Tray;

namespace ServiceFairy.Management.AppClients
{
    [SoInfo("进程列表"), UIObjectImage(ResourceNames.AppClientProcessList)]
    partial class AppClientProcessListNode : ServiceObjectKernelTreeNodeBase
    {
        public AppClientProcessListNode(AppClientContext clientCtx)
        {
            _clientCtx = clientCtx;
            _clientId = clientCtx.ClientDesc.ClientID;
        }

        private readonly AppClientContext _clientCtx;
        private readonly Guid _clientId;

        protected override object GetKey()
        {
            return GetType();
        }

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            ProcessInfo[] infos = _clientCtx.MgrCtx.Invoker.Tray.GetProcesses(_clientId);
            return infos.Select(info => new AppClientProcessNode(_clientCtx, info));
        }

        [SoInfo("打开")]
        [ServiceObjectAction(ServiceObjectActionType.Default | ServiceObjectActionType.Open)]
        [UIObjectAction(typeof(OpenAction)), UIObjectImage("open"), ServiceObjectGroup("open")]
        public void Open()
        {

        }

        [SoInfo("在新窗口中打开")]
        [ServiceObjectAction(ServiceObjectActionType.AttachDefault | ServiceObjectActionType.OpenInNewWindow)]
        [UIObjectAction(typeof(OpenInNewWindowAction)), ServiceObjectGroup("open")]
        public void OpenInNewWindow()
        {

        }

        [SoInfo("数量", Name = "Count"), ServiceObjectProperty]
        public string Count
        {
            get { return ""; }
        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return "查看运行在该终端的所有进程"; }
        }
    }
}

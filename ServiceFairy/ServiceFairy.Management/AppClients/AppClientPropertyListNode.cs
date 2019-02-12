using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Package.UIObject.Actions;
using ServiceFairy.Entities.Tray;

namespace ServiceFairy.Management.AppClients
{
    /// <summary>
    /// 系统信息
    /// </summary>
    [SoInfo("系统信息"), UIObjectImage(ResourceNames.AppClientPropertyList)]
    class AppClientPropertyListNode : ServiceObjectKernelTreeNodeBase
    {
        public AppClientPropertyListNode(AppClientContext clientCtx)
        {
            _clientCtx = clientCtx;
        }

        private readonly AppClientContext _clientCtx;

        protected override object GetKey()
        {
            return new Tuple<Type, Guid>(GetType(), _clientCtx.ClientDesc.ClientID);
        }

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            SystemProperty[] properties = _clientCtx.MgrCtx.Invoker.Tray.GetAllSystemProperties(_clientCtx.ClientDesc.ClientID);
            return properties.Select(p => new AppClientPropertyNode(_clientCtx, p));
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
            get { return "查看该终端的硬件信息、资源利用情况等"; }
        }
    }
}

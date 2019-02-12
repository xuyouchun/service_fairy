using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities;
using Common.Package.UIObject;
using Common.Package.Service;
using Common.Utility;
using Common.Package.UIObject.Actions;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Management.AppClients
{
    /// <summary>
    /// 终端列表节点
    /// </summary>
    [SoInfo("终端列表", Name = "AppClientList"), UIObjectImage(ResourceNames.AppClientList)]
    class AppClientListNode : ListServiceObjectKernelTreeNodeBase
    {
        public AppClientListNode(SfManagementContext mgrCtx)
        {
            _mgrCtx = mgrCtx;
        }

        private readonly SfManagementContext _mgrCtx;

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            ClientDesc[] cds = _mgrCtx.ClientDescs.GetAll();
            return cds.Select(sd => new AppClientNode(_mgrCtx, sd));
        }

        protected override object GetKey()
        {
            return GetType();
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

        [SoInfo("刷新")]
        [ServiceObjectAction(ServiceObjectActionType.Refresh), UIObjectAction(typeof(RefreshAction))]
        public void Refresh()
        {

        }
    }
}

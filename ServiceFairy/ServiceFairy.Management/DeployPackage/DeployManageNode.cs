using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Service;
using Common.Package.UIObject.Actions;

namespace ServiceFairy.Management.DeployPackage
{
    /// <summary>
    /// 部署管理
    /// </summary>
    [SoInfo("部署管理"), UIObjectImage(ResourceNames.DeployManager)]
    class DeployManageNode : ServiceObjectKernelTreeNodeBase
    {
        public DeployManageNode(SfManagementContext mgrCtx)
        {
            _mgrCtx = mgrCtx;
        }

        private readonly SfManagementContext _mgrCtx;

        protected override object GetKey()
        {
            return GetType();
        }

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            return new IServiceObjectTreeNode[] {
                new PlatformDeployPackageListNode(_mgrCtx),
                new ServiceDeployPackageListNode(_mgrCtx),
                new DeplyMapManagerNode(_mgrCtx),
            };
        }

        [SoInfo("打开")]
        [ServiceObjectAction(ServiceObjectActionType.Default | ServiceObjectActionType.Open)]
        [UIObjectAction(typeof(OpenAction)), UIObjectImage("open")]
        public void Open()
        {

        }

        [SoInfo("在新窗口中打开")]
        [ServiceObjectAction(ServiceObjectActionType.AttachDefault | ServiceObjectActionType.OpenInNewWindow)]
        [UIObjectAction(typeof(OpenInNewWindowAction))]
        public void OpenInNewWindow()
        {

        }
    }
}

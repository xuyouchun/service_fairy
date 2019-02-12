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
    [SoInfo("部署地图"), UIObjectImage(ResourceNames.DeployMap)]
    class DeplyMapManagerNode : ServiceObjectKernelTreeNodeBase
    {
        public DeplyMapManagerNode(SfManagementContext mgrCtx)
        {
            _mgrCtx = mgrCtx;
        }

        private readonly SfManagementContext _mgrCtx;

        protected override object GetKey()
        {
            return GetType();
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

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return "查看并修改部署地图"; }
        }
    }
}

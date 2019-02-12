using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.UIObject;
using Common.Package.Service;
using Common.Contracts.UIObject;
using Common.Package.UIObject.Actions;

namespace ServiceFairy.Management
{
    [SoInfo("服务", Name = "Root"), UIObjectImage(ResourceNames.Root)]
    class RootServiceObjectNode : ServiceObjectKernelTreeNodeBase
    {
        public RootServiceObjectNode(IServiceObjectTreeNode[] subNodes)
        {
            _subNodes = subNodes;
        }

        private readonly IServiceObjectTreeNode[] _subNodes;

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            return _subNodes;
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

        protected override object GetKey()
        {
            return GetType();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using ServiceFairy.SystemInvoke;
using Common.Contracts.Service;

namespace ServiceFairy.Management
{
    [SoInfo("其它"), UIObjectImage(ResourceNames.Others)]
    class OthersTreeNode : ServiceObjectKernelTreeNodeBase
    {
        public OthersTreeNode(SfManagementContext mgrCtx)
        {
            _mgrCtx = mgrCtx;
        }

        private readonly SfManagementContext _mgrCtx;

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            return new IServiceObjectTreeNode[] {
                new SceneTestNode(_mgrCtx),
            };
        }

        public override void OnRefreshSubNodes(bool force)
        {
            
        }

        protected override object GetKey()
        {
            return GetType();
        }
    }
}

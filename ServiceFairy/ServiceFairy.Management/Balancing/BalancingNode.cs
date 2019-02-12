using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;

namespace ServiceFairy.Management.Balancing
{
    [SoInfo("负载均衡"), UIObjectImage(ResourceNames.Balancing)]
    class BalancingNode : ServiceObjectKernelTreeNodeBase
    {
        public BalancingNode(SfManagementContext mgrCtx)
        {
            _mgrCtx = mgrCtx;
        }

        private readonly SfManagementContext _mgrCtx;

        protected override object GetKey()
        {
            return GetType();
        }
    }
}

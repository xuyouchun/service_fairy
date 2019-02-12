using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Service;

namespace ServiceFairy.Management.AppServices
{
    [SoInfo("参数配置"), UIObjectImage(ResourceNames.AppServiceConfiguration)]
    class AppServiceConfigurationNode : ServiceObjectKernelTreeNodeBase
    {
        public AppServiceConfigurationNode(SfManagementContext mgrCtx, ServiceDesc serviceDesc)
        {
            _mgrCtx = mgrCtx;
            _serviceDesc = serviceDesc;
        }

        private readonly SfManagementContext _mgrCtx;
        private readonly ServiceDesc _serviceDesc;

        protected override object GetKey()
        {
            return new Tuple<Type, ServiceDesc>(GetType(), _serviceDesc);
        }
    }
}

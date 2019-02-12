using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Service;

namespace ServiceFairy.Management.AppClients
{
    /// <summary>
    /// 终端与终端之间的调用方式
    /// </summary>
    [SoInfo("调用方式(按服务查看)"), UIObjectImage("AppServiceInfo")]
    class AppClientInvokeInfoNode_ByAppService : ServiceObjectKernelTreeNodeBase
    {
        public AppClientInvokeInfoNode_ByAppService(AppClientContext clientContext, AppInvokeInfo invokeInfo, ServiceDesc serviceDesc)
        {
            _clientContext = clientContext;
            _invokeInfo = invokeInfo;
            _serviceDesc = serviceDesc;
        }

        private readonly AppClientContext _clientContext;
        private readonly AppInvokeInfo _invokeInfo;
        private readonly ServiceDesc _serviceDesc;

        protected override object GetKey()
        {
            return new Tuple<Type, ServiceDesc>(GetType(), _serviceDesc);
        }

        [SoInfo("服务描述"), ServiceObjectProperty]
        public string ServiceDesc
        {
            get { return _serviceDesc.ToString(); }
        }

        [SoInfo("终端"), ServiceObjectProperty]
        public string ClientDesc
        {
            get { return _clientContext.MgrCtx.ClientDescs.GetClientTitle(_invokeInfo.ClientID); }
        }

        [SoInfo("信道"), ServiceObjectProperty]
        public string Communications
        {
            get { return string.Join(", ", (object[])_invokeInfo.CommunicateOptions); }
        }

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_serviceDesc.ToString());
        }

        protected override bool? HasChildren()
        {
            return false;
        }
    }
}

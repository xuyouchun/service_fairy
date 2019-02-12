using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Service;
using Common.Utility;

namespace ServiceFairy.Management.AppClients
{
    /// <summary>
    /// 终端与终端之间的调用方式
    /// </summary>
    [SoInfo("调用方式(按终端查看)"), UIObjectImage(ResourceNames.AppClient)]
    class AppClientInvokeInfoNode_ByAppClient : ServiceObjectKernelTreeNodeBase
    {
        public AppClientInvokeInfoNode_ByAppClient(AppClientContext clientContext, AppInvokeInfo invokeInfo)
        {
            _clientContext = clientContext;
            _invokeInfo = invokeInfo;
        }

        private readonly AppClientContext _clientContext;
        private readonly AppInvokeInfo _invokeInfo;

        protected override object GetKey()
        {
            return new Tuple<Type, Guid>(GetType(), _invokeInfo.ClientID);
        }

        [SoInfo("服务"), ServiceObjectProperty]
        public string ServiceDescs
        {
            get { return _invokeInfo.ServiceDescs.JoinBy(", ", true); }
        }

        [SoInfo("信道"), ServiceObjectProperty]
        public string Communications
        {
            get { return string.Join(", ", (object[])_invokeInfo.CommunicateOptions); }
        }

        protected override bool? HasChildren()
        {
            return false;
        }

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_clientContext.MgrCtx.ClientDescs.GetClientTitle(_invokeInfo.ClientID));
        }
    }
}

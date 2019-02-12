using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Service;
using Common.Utility;
using Common.Communication.Wcf;

namespace ServiceFairy.Management.AppClients
{
    /// <summary>
    /// 终端与终端之间的调用方式
    /// </summary>
    [SoInfo("调用方式(按信道查看)"), UIObjectImage(ResourceNames.AppClientCommunication)]
    class AppClientInvokeInfoNode_ByCommunication : ServiceObjectKernelTreeNodeBase
    {
        public AppClientInvokeInfoNode_ByCommunication(AppClientContext clientContext, AppInvokeInfo invokeInfo, CommunicationOption communicationOption)
        {
            _clientContext = clientContext;
            _invokeInfo = invokeInfo;
            _communicationOption = communicationOption;
        }

        private readonly AppClientContext _clientContext;
        private readonly AppInvokeInfo _invokeInfo;
        private readonly CommunicationOption _communicationOption;

        protected override object GetKey()
        {
            return new Tuple<Type, CommunicationOption>(GetType(), _communicationOption);
        }

        [SoInfo("协议"), ServiceObjectProperty]
        public string CommunicationType
        {
            get { return _communicationOption.Type.ToString(); }
        }

        [SoInfo("通信方式"), ServiceObjectProperty]
        public string SupportDuplex
        {
            get { return _communicationOption.Duplex ? "双向" : "单向"; }
        }

        [SoInfo("终端"), ServiceObjectProperty]
        public string ClientDesc
        {
            get { return _clientContext.MgrCtx.ClientDescs.GetClientTitle(_invokeInfo.ClientID); }
        }

        [SoInfo("服务"), ServiceObjectProperty]
        public string ServiceInfos
        {
            get { return _invokeInfo.ServiceDescs.JoinBy(", ", true); }
        }

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_communicationOption.Address.ToString());
        }

        protected override bool? HasChildren()
        {
            return false;
        }
    }
}

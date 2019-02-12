using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.SystemInvoke;
using Common.Contracts.Service;
using Common.Package.UIObject;
using Common.Package.Service;
using Common.Utility;
using Common.WinForm;
using Common.Contracts.UIObject;
using ServiceFairy.Client;
using Common.Contracts;
using Common.Package;
using Common.Communication.Wcf;

namespace ServiceFairy.Management.AppClients
{
    /// <summary>
    /// 信道服务节点
    /// </summary>
    [SoInfo("信道"), UIObjectImage(ResourceNames.AppClientCommunication)]
    partial class AppClientCommunicationNode : ServiceObjectKernelTreeNodeBase
    {
        public AppClientCommunicationNode(AppClientContext clientCtx, CommunicationOption communicationOption)
        {
            _clientCtx = clientCtx;
            _communicationOption = communicationOption;
        }

        private readonly AppClientContext _clientCtx;
        private readonly CommunicationOption _communicationOption;

        public static AppClientCommunicationNode StartNew(AppClientContext clientCtx, CommunicationOption communicationOption)
        {
            AppClientCommunicationNode node = new AppClientCommunicationNode(clientCtx, communicationOption);
            clientCtx.MgrCtx.Invoker.Master.OpenCommunication(clientCtx.ClientDesc.ClientID, communicationOption);
            node.CurrentStatus = ServiceStatus.Starting;
            return node;
        }

        protected override ServiceObjectRefreshResult OnInitRefresh(InitContext initCtx)
        {
            if (_clientCtx.ExistCommunication(_communicationOption.Address))
            {
                CurrentStatus = ServiceStatus.Default;
                return ServiceObjectRefreshResult.CompletedAndRefresh;
            }

            return ServiceObjectRefreshResult.ContinueAndRefresh;
        }

        public CommunicationOption CommunicationOption
        {
            get { return _communicationOption; }
        }

        protected override bool? HasChildren()
        {
            return false;
        }

        [SoInfo("停止")]
        [ServiceObjectAction(ServiceObjectActionType.Close)]
        [UIObjectAction(typeof(CloseAction))]
        [Enable(ServiceStatus.Default)]
        public void Close()
        {

        }

        [SoInfo("重新启动")]
        [ServiceObjectAction(ServiceObjectActionType.Restart),]
        [UIObjectAction(typeof(RestartAction))]
        [Enable(ServiceStatus.Default)]
        public void Restart()
        {

        }

        [SoInfo("IP"), ServiceObjectProperty]
        public string IP
        {
            get { return _communicationOption.Address.Address; }
        }

        [SoInfo("端口"), ServiceObjectProperty]
        public int Port
        {
            get { return _communicationOption.Address.Port; }
        }

        [SoInfo("协议"), ServiceObjectProperty]
        public string Desc
        {
            get { return _communicationOption.Type.GetDesc(); }
        }

        [SoInfo("通信方式"), ServiceObjectProperty]
        public string SupportDuplex
        {
            get { return _communicationOption.Duplex ? "双向" : "单向"; }
        }

        [SoInfo("状态"), ServiceObjectProperty]
        public string Status
        {
            get { return GetCurrentServiceStatusDesc(); }
        }

        protected override string GetServiceStatusDesc(ServiceStatus status)
        {
            switch (status)
            {
                case ServiceStatus.Default:
                    return "正在侦听";

                case ServiceStatus.Stopping:
                    return "正在停止侦听";

                default:
                    return base.GetServiceStatusDesc(status);
            }
        }

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_communicationOption.Address.ToString());
        }

        protected override object GetKey()
        {
            return new Tuple<Type, CommunicationOption>(GetType(), _communicationOption);
        }

        abstract class ActionBase : ActionBase<AppClientCommunicationNode>
        {

        }
    }
}

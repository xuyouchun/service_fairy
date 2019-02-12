using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Client;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Service;
using Common.WinForm;
using Common.Contracts.UIObject;
using System.Windows.Forms;
using Common.Utility;
using Common.Contracts;
using Common.Package.UIObject.Actions;
using Common.Package;

namespace ServiceFairy.Management.AppClients
{
    /// <summary>
    /// 信道列表节点
    /// </summary>
    [SoInfo("信道列表"), UIObjectImage(ResourceNames.AppClientCommunicationList)]
    partial class AppClientCommunicationListNode : ListServiceObjectKernelTreeNodeBase
    {
        public AppClientCommunicationListNode(AppClientContext clientCtx)
        {
            _clientCtx = clientCtx;
        }

        private readonly AppClientContext _clientCtx;

        protected override object GetKey()
        {
            return new Tuple<Type, Guid>(GetType(), _clientCtx.ClientDesc.ClientID);
        }

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            return _clientCtx.AppClientInfo.Communications.Select(
                communication => new AppClientCommunicationNode(_clientCtx, communication)
            );
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

        [SoInfo("刷新")]
        [ServiceObjectAction(ServiceObjectActionType.Refresh)]
        [UIObjectAction(typeof(RefreshAction))]
        public void Refresh()
        {

        }

        [SoInfo("开启新信道 ...")]
        [ServiceObjectAction(ServiceObjectActionType.New)]
        [UIObjectAction(typeof(StartNewCommunicationAction))]
        [ServiceObjectGroup("service")]
        public void StartNewCommunication()
        {

        }

        [SoInfo("数量", Name = "Count"), ServiceObjectProperty]
        public int CommunicationCount
        {
            get { return _clientCtx.CommunicationOptions.Length; }
        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return "该终端上开启的所有端口"; }
        }

        [SoInfo("详细"), ServiceObjectProperty]
        public string Detail
        {
            get { return _clientCtx.AppClientInfo.Communications.JoinBy("; "); }
        }

        abstract class ActionBase : ActionBase<AppClientCommunicationListNode>
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Client;
using ServiceFairy.SystemInvoke;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Package;
using Common.Package.UIObject.Actions;
using Common.Utility;

namespace ServiceFairy.Management.AppClients
{
    /// <summary>
    /// 交互方式列表
    /// </summary>
    [SoInfo("交互方式"), UIObjectImage(ResourceNames.AppClientInvokeInfoList)]
    class AppClientInvokeInfoListNode : ListServiceObjectKernelTreeNodeBase
    {
        public AppClientInvokeInfoListNode(AppClientContext clientContext)
        {
            _clientCtx = clientContext;
        }

        private readonly AppClientContext _clientCtx;

        protected override object GetKey()
        {
            return new Tuple<Type, Guid>(GetType(), _clientCtx.ClientDesc.ClientID);
        }

        protected override IEnumerable<IServiceObjectTreeNode> LoadServiceObjectSubTreeNodes()
        {
            return new AppClientInvokeInfoWatchTypeNode[] {
                new AppClientInvokeInfoWatchTypeNode("按终端查看", _clientCtx.InvokeInfos.ToArray(
                    iv => new AppClientInvokeInfoNode_ByAppClient(_clientCtx, iv)), "查看所调用的终端上所使用的信道"),
                new AppClientInvokeInfoWatchTypeNode("按服务查看", _clientCtx.InvokeInfos.SelectMany(
                    iv=>iv.ServiceDescs.Select(si=> new AppClientInvokeInfoNode_ByAppService(_clientCtx, iv, si))).ToArray(), "查看所调用的服务位于哪个终端上"),
                new AppClientInvokeInfoWatchTypeNode("按信道查看", _clientCtx.InvokeInfos.SelectMany(
                    iv=>iv.CommunicateOptions.Select(co=> new AppClientInvokeInfoNode_ByCommunication(_clientCtx, iv, co))).ToArray(), "查看所使用的信道位于哪些终端上"),
            };
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

        /*
        [SoInfo("按服务查看"), ServiceObjectAction, UIObjectAction(typeof(OpenByServiceAction))]
        public void OpenByService
        {

        }*/

        [SoInfo("刷新")]
        [ServiceObjectAction(ServiceObjectActionType.Refresh), UIObjectAction(typeof(RefreshAction))]
        public void Refresh()
        {

        }

        [SoInfo("数量", Name = "Count"), ServiceObjectProperty]
        public int InvokeInfoCount
        {
            get { return _clientCtx.InvokeInfos.Length; }
        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return "该终端与其它终端的交互方式"; }
        }

        [SoInfo("详细"), ServiceObjectProperty]
        public string Detail
        {
            get { return "提供按终端查看、按服务查看、按信道查看三种视图"; }
        }
    }
}

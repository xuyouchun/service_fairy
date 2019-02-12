using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Package.UIObject.Actions;
using ServiceFairy.Client;
using ServiceFairy.Entities.Proxy;

namespace ServiceFairy.Management.AppClients
{
    /// <summary>
    /// 在线用户
    /// </summary>
    [SoInfo("在线用户"), UIObjectImage(ResourceNames.AppClientOnlineUserList)]
    class AppClientOnlineUserListNode : ListServiceObjectKernelTreeNodeBase
    {
        public AppClientOnlineUserListNode(AppClientContext clientContext)
        {
            _clientCtx = clientContext;
        }

        private readonly AppClientContext _clientCtx;

        protected override object GetKey()
        {
            return new Tuple<Type, Guid>(GetType(), _clientCtx.AppClientInfo.ClientId);
        }

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            ProxyOnlineUserInfo[] infos = _clientCtx.MgrCtx.Invoker.Proxy.GetOnlineUsers(GetOnlineUsersSortField.ConnectionTime,
                SortType.Desc, 0, 200, CallingSettings.FromTarget(_clientCtx.ClientDesc.ClientID));

            return infos.Select(info => new AppClientOnlineUserNode(_clientCtx, info));
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

        [SoInfo("数量", Name = "Count"), ServiceObjectProperty]
        public string Count
        {
            get
            {
                AppClientRuntimeInfo rInfo;
                if ((rInfo = _clientCtx.AppClientInfo.RuntimeInfo) != null && rInfo.OnlineUserStatInfo != null)
                    return rInfo.OnlineUserStatInfo.CurrentOnlineUserCount.ToString();

                return "?";
            }
        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return "该终端上保持长连接的用户"; }
        }

        [SoInfo("详细"), ServiceObjectProperty]
        public string Detail
        {
            get { return ""; }
        }
    }
}

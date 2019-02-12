using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Package.UIObject;
using ServiceFairy.Entities.Proxy;

namespace ServiceFairy.Management.AppClients
{
    /// <summary>
    /// 在线用户
    /// </summary>
    [SoInfo("在线用户"), UIObjectImage(ResourceNames.AppClientOnlineUser)]
    class AppClientOnlineUserNode : ServiceObjectKernelTreeNodeBase
    {
        public AppClientOnlineUserNode(AppClientContext clientContext, ProxyOnlineUserInfo conInfo)
        {
            _clientCtx = clientContext;
            _conInfo = conInfo;
        }

        private readonly AppClientContext _clientCtx;
        private readonly ProxyOnlineUserInfo _conInfo;

        protected override object GetKey()
        {
            return new Tuple<Type, Guid, int>(GetType(), _clientCtx.AppClientInfo.ClientId, _conInfo.UserId);
        }

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_conInfo.UserName ?? "");
        }

        [SoInfo("用户ID"), ServiceObjectProperty]
        public int UserId
        {
            get { return _conInfo.UserId; }
        }

        [SoInfo("连接时间"), ServiceObjectProperty]
        public string ConnectionTime
        {
            get { return _conInfo.ConnectionTime.GetLocalTimeString(); }
        }

        [SoInfo("在线时长"), ServiceObjectProperty]
        public string OnlineTime
        {
            get { return _conInfo.ConnectionTime.GetUtcUntilNowString(); }
        }
    }
}

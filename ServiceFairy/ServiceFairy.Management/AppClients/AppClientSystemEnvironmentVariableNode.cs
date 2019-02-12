using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Package.UIObject;
using ServiceFairy.Entities.Tray;

namespace ServiceFairy.Management.AppClients
{
    /// <summary>
    /// 环境变量
    /// </summary>
    [SoInfo("环境变量"), UIObjectImage(ResourceNames.AppClientSystemEnvironmentVariable)]
    class AppClientSystemEnvironmentVariableNode : ServiceObjectKernelTreeNodeBase
    {
        public AppClientSystemEnvironmentVariableNode(AppClientContext clientCtx, SystemEnvironmentVariable valiable)
        {
            _clientCtx = clientCtx;
            _valiable = valiable;
        }

        private readonly AppClientContext _clientCtx;
        private readonly SystemEnvironmentVariable _valiable;

        protected override object GetKey()
        {
            return new Tuple<Type, Guid, string>(GetType(), _clientCtx.ClientDesc.ClientID, _valiable.Name);
        }

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_valiable.Name);
        }

        [SoInfo("值"), ServiceObjectProperty]
        public string User
        {
            get { return _valiable.Value; }
        }
    }
}

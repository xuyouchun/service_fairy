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
    /// 系统信息
    /// </summary>
    [SoInfo("系统信息"), UIObjectImage(ResourceNames.AppClientProperty)]
    class AppClientPropertyNode : ServiceObjectKernelTreeNodeBase
    {
        public AppClientPropertyNode(AppClientContext clientCtx, SystemProperty property)
        {
            _clientCtx = clientCtx;
            _property = property;
        }

        private readonly AppClientContext _clientCtx;
        private readonly SystemProperty _property;

        protected override object GetKey()
        {
            return new Tuple<Type, Guid, string>(GetType(), _clientCtx.ClientDesc.ClientID, _property.Name);
        }

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_property.Desc);
        }

        [SoInfo("值"), ServiceObjectProperty]
        public string User
        {
            get { return _property.Value; }
        }
    }
}

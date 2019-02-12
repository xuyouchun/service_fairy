using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using ServiceFairy.Management.AppClients;
using Common.Contracts.Service;
using Common.Package.UIObject.Actions;

namespace ServiceFairy.Management.AppServiceAddins
{
    /// <summary>
    /// 插件列表
    /// </summary>
    [SoInfo("插件列表"), UIObjectImage(ResourceNames.AppServiceAddinList)]
    class AppServiceAddinListNode : ListServiceObjectKernelTreeNodeBase
    {
        public AppServiceAddinListNode(AppClientContext clientCtx, ServiceDesc serviceDesc)
        {
            _clientCtx = clientCtx;
            _clientId = clientCtx.ClientDesc.ClientID;
            _serviceDesc = serviceDesc;
        }

        private readonly AppClientContext _clientCtx;
        private readonly Guid _clientId;
        private readonly ServiceDesc _serviceDesc;

        protected override object GetKey()
        {
            return new Tuple<Type, ServiceDesc>(GetType(), _serviceDesc);
        }

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            return _clientCtx.MgrCtx.GetAppServiceAddinManager(_clientId, _serviceDesc).GetAll()
                .Select(info => new AppServiceAddinNode(_clientCtx, _serviceDesc, info));
        }

        [SoInfo("打开")]
        [ServiceObjectAction(ServiceObjectActionType.Default | ServiceObjectActionType.Open)]
        [UIObjectAction(typeof(OpenAction)), UIObjectImage("open"), ServiceObjectGroup("open")]
        public void Open() { }

        [SoInfo("在新窗口中打开")]
        [ServiceObjectAction(ServiceObjectActionType.AttachDefault | ServiceObjectActionType.OpenInNewWindow)]
        [UIObjectAction(typeof(OpenInNewWindowAction)), ServiceObjectGroup("open")]
        public void OpenInNewWindow() { }

        [SoInfo("数量"), ServiceObjectProperty]
        public string Count
        {
            get { return _clientCtx.MgrCtx.GetAppServiceAddinManager(_clientId, _serviceDesc).GetAll().Length.ToString(); }
        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return "查看插件信息，包括接入插件及接出插件"; }
        }
    }
}

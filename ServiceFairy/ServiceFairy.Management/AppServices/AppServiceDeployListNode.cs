using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Utility;
using Common.Package.UIObject.Actions;
using Common;
using ServiceFairy.Management.AppClients;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Management.AppServices
{
    [SoInfo("部署列表"), UIObjectImage(ResourceNames.AppServiceDeployList)]
    partial class AppServiceDeployListNode : ListServiceObjectKernelTreeNodeBase
    {
        public AppServiceDeployListNode(SfManagementContext mgrCtx, ServiceDesc serviceDesc)
        {
            _mgrCtx = mgrCtx;
            _serviceDesc = serviceDesc;
        }

        private readonly SfManagementContext _mgrCtx;
        private readonly ServiceDesc _serviceDesc;

        protected override object GetKey()
        {
            return new Tuple<Type, ServiceDesc>(GetType(), _serviceDesc);
        }

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            return _GetAllClientDescs().ToArray(clientDesc => new DeployedAppClientNode(this, clientDesc));
        }

        private ClientDesc[] _GetAllClientDescs()
        {
            ServiceDeployInfo sdInfo = _mgrCtx.ServiceDeployInfos.Get(_serviceDesc);
            if (sdInfo == null || sdInfo.ClientIDs.IsNullOrEmpty())
                return Array<ClientDesc>.Empty;

            return sdInfo.ClientIDs.Select(clientId => _mgrCtx.ClientDescs.Get(clientId)).WhereNotNull().ToArray();
        }

        [SoInfo("打开"), ServiceObjectAction(ServiceObjectActionType.Default | ServiceObjectActionType.Open), UIObjectAction(typeof(OpenAction)), UIObjectImage("open"), ServiceObjectGroup("open")]
        public void Open()
        {

        }

        [SoInfo("在新窗口中打开"), ServiceObjectAction(ServiceObjectActionType.AttachDefault | ServiceObjectActionType.OpenInNewWindow), UIObjectAction(typeof(OpenInNewWindowAction)), ServiceObjectGroup("open")]
        public void OpenInNewWindow()
        {

        }

        [SoInfo("部署到新终端 ..."), ServiceObjectAction(ServiceObjectActionType.New), UIObjectAction(typeof(DeployNewAction)), ServiceObjectGroup("deploy")]
        public void DeployNew() { }

        [SoInfo("刷新"), ServiceObjectAction(ServiceObjectActionType.Refresh), UIObjectAction(typeof(RefreshAction))]
        public void Refresh()
        {

        }

        [SoInfo("数量"), ServiceObjectProperty]
        public int Count
        {
            get
            {
                ServiceDeployInfo sdInfo = _mgrCtx.ServiceDeployInfos.Get(_serviceDesc);
                if (sdInfo == null)
                    return 0;

                return sdInfo.ClientIDs.CountOrDefault();
            }
        }

        [SoInfo("终端"), ServiceObjectProperty]
        public string AppClients
        {
            get
            {
                return _GetAllClientDescs().JoinBy(", ", true);
            }
        }

        class DeployedAppClientNode : AppServiceDeployNode
        {
            public DeployedAppClientNode(AppServiceDeployListNode kernel, ClientDesc clientDesc)
                : base(kernel._mgrCtx, clientDesc, kernel._serviceDesc)
            {
                
            }
        }

        abstract class ActionBase : ActionBase<AppServiceDeployListNode>
        {

        }
    }
}

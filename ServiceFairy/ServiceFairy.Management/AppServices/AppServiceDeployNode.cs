using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using ServiceFairy.Entities;
using ServiceFairy.Management.AppClients;
using Common.Contracts.Service;
using Common.Package.UIObject;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Management.AppServices
{
    partial class AppServiceDeployNode : AppClientNode
    {
        public AppServiceDeployNode(SfManagementContext mgrCtx, ClientDesc clientDesc, ServiceDesc serviceDesc)
            : base(mgrCtx, clientDesc)
        {
            _mgrCtx = mgrCtx;
            ClientDesc = clientDesc;
            ServiceDesc = serviceDesc;
        }

        public ClientDesc ClientDesc { get; private set; }

        public ServiceDesc ServiceDesc { get; private set; }

        private readonly SfManagementContext _mgrCtx;

        protected override object GetKey()
        {
            return new Tuple<Type, ClientDesc, ServiceDesc>(GetType(), ClientDesc, ServiceDesc);
        }

        [SoInfo(Weight = 10), ServiceObjectGroupDesc]
        private int deploy { get; set; }

        [SoInfo("卸载 ..."), ServiceObjectAction(ServiceObjectActionType.Close), UIObjectAction(typeof(UnDeployAction)), ServiceObjectGroup("deploy"), Visiable(ServiceStatus.Default)]
        public void UnDeploy() { }

        protected override string GetServiceStatusDesc(ServiceStatus status)
        {
            switch (status)
            {
                case ServiceStatus.Starting:
                    return "正在部署";

                case ServiceStatus.Default:
                    return "正在运行";

                case ServiceStatus.Stopping:
                    return "正在卸载";
            }

            return base.GetServiceStatusDesc(status);
        }

        [SoInfo("状态", Weight = 15), ServiceObjectProperty]
        public string Status
        {
            get { return GetCurrentServiceStatusDesc(); }
        }

        abstract class ActionBase : ActionBase<AppServiceDeployNode>
        {

        }
    }
}

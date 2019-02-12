using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Service;
using Common.Package.UIObject.Actions;
using ServiceFairy.SystemInvoke;

namespace ServiceFairy.Management.DeployPackage
{
    [SoInfo("服务安装包"), UIObjectImage(ResourceNames.ServiceDeployPackageList)]
    partial class ServiceDeployPackageListNode : ListServiceObjectKernelTreeNodeBase
    {
        public ServiceDeployPackageListNode(SfManagementContext mgrCtx)
        {
            _mgrCtx = mgrCtx;
        }

        private readonly SfManagementContext _mgrCtx;

        protected override object GetKey()
        {
            return this.GetType();
        }

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            return _mgrCtx.ServiceDeployPackageInfos.GetAll().Select(info => new ServiceDeployPackageNode(_mgrCtx, info));
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

        [SoInfo("上传安装包 ...")]
        [ServiceObjectAction()]
        [UIObjectAction(typeof(UploadAction)), ServiceObjectGroup("upload")]
        public void Upload()
        {

        }

        [SoInfo("同步 ...")]
        [ServiceObjectAction()]
        [UIObjectAction(typeof(SyncAction)), ServiceObjectGroup("upload")]
        public void Sync()
        {

        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return "上传并管理服务安装包"; }
        }

        abstract class ActionBase : ActionBase<ServiceDeployPackageListNode>
        {
            public SfManagementContext MgrCtx { get { return Kernel._mgrCtx; } }
            public SystemInvoker Invoker { get { return Kernel._mgrCtx.Invoker; } }
        }
    }
}

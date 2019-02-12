using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Service;
using Common.Package.UIObject.Actions;
using ServiceFairy.Entities.Master;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities;

namespace ServiceFairy.Management.DeployPackage
{
    [SoInfo("平台安装包"), UIObjectImage(ResourceNames.PlatformDeployPackageList)]
    partial class PlatformDeployPackageListNode : ListServiceObjectKernelTreeNodeBase
    {
        public PlatformDeployPackageListNode(SfManagementContext mgrCtx)
        {
            _mgrCtx = mgrCtx;
        }

        private readonly SfManagementContext _mgrCtx;

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            PlatformDeployPackageInfo[] infos = _mgrCtx.PlatformDeployPackageInfos.GetAll();
            return infos.Select(info => new PlatformDeployPackageNode(_mgrCtx, info));
        }

        protected override object GetKey()
        {
            return GetType();
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
        [UIObjectAction(typeof(UploadAction)), ServiceObjectGroup("deploy")]
        public void Upload()
        {

        }

        [SoInfo("同步 ...")]
        [ServiceObjectAction()]
        [UIObjectAction(typeof(SyncDeployAction)), ServiceObjectGroup("deploy")]
        public void SyncDeploy()
        {

        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return "上传并管理平台安装包"; }
        }

        abstract class ActionBase : ActionBase<PlatformDeployPackageListNode>
        {
            public SfManagementContext MgrCtx { get { return Kernel._mgrCtx; } }

            public SystemInvoker Invoker { get { return MgrCtx.Invoker; } }
        }
    }
}

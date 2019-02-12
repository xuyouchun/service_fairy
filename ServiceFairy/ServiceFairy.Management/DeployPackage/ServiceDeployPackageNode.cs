using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Service;
using ServiceFairy.Entities.Master;
using Common.Utility;
using ServiceFairy.SystemInvoke;
using Common.Package.UIObject.Actions;
using Common;
using System.IO;
using ServiceFairy.Entities;

namespace ServiceFairy.Management.DeployPackage
{
    /// <summary>
    /// 安装包
    /// </summary>
    [SoInfo("服务安装包"), UIObjectImage(ResourceNames.ServiceDeployPackage)]
    partial class ServiceDeployPackageNode : ServiceObjectKernelTreeNodeBase
    {
        public ServiceDeployPackageNode(SfManagementContext mgrCtx, ServiceDeployPackageInfo packageInfo)
        {
            _mgrCtx = mgrCtx;
            _packageInfo = packageInfo;
        }

        public static ServiceDeployPackageNode StartNew(SfManagementContext mgrCtx, ServiceDeployPackageInfo packageInfo)
        {
            ServiceDeployPackageNode node = new ServiceDeployPackageNode(mgrCtx, packageInfo);
            node.CurrentStatus = ServiceStatus.Uploading;
            return node;
        }

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            var package = _mgrCtx.Invoker.Master.DownloadServiceDeployPackage(_packageInfo.Id);
            if (package != null && package.Content != null)
            {
                string tempDirectory = Path.Combine(Path.GetTempPath(), "packages", _packageInfo.Id.ToString());
                PathUtility.ClearPath(tempDirectory);
                StreamUtility.DecompressToDirectory(package.Content, tempDirectory);
                AppFileInfo[] fileInfos = AppFileInfo.LoadFromDirectory(tempDirectory);
                return fileInfos.ToArray(fi => new DeployPackageFileNode(_mgrCtx, _packageInfo, fi, tempDirectory));
            }

            return Array<IServiceObjectTreeNode>.Empty;
        }

        private readonly ServiceDeployPackageInfo _packageInfo;
        private readonly SfManagementContext _mgrCtx;

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

        protected override ServiceObjectRefreshResult OnInitRefresh(InitContext initCtx)
        {
            switch (CurrentStatus)
            {
                case ServiceStatus.Uploading:
                    return ServiceObjectRefreshResult.ContinueAndRefresh;

                case ServiceStatus.Default:
                    return ServiceObjectRefreshResult.CompletedAndRefresh;

                case ServiceStatus.Error:
                    return ServiceObjectRefreshResult.Dispose;
            }

            return base.OnInitRefresh(initCtx);
        }

        public void UploadCompletedNotify(Exception ex)
        {
            CurrentStatus = (ex == null) ? ServiceStatus.Default : ServiceStatus.Error;
        }

        protected override object GetKey()
        {
            return new Tuple<Type, Guid>(GetType(), _packageInfo.Id);
        }

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_packageInfo.Title);
        }

        [SoInfo("删除")]
        [ServiceObjectAction()]
        [UIObjectAction(typeof(DeleteAction))]
        public void Delete()
        {

        }

        [SoInfo("部署到 ...")]
        [ServiceObjectAction()]
        [UIObjectAction(typeof(DeployAction)), ServiceObjectGroup("deploy")]
        public void Deploy()
        {

        }

        [SoInfo("部署到所有运行该服务的终端 ...")]
        [ServiceObjectAction()]
        [UIObjectAction(typeof(DeployAllAction)), ServiceObjectGroup("deploy")]
        public void DeployAll()
        {

        }

        [SoInfo("置为当前版本")]
        [ServiceObjectAction()]
        [UIObjectAction(typeof(SetCurrentAction)), ServiceObjectGroup("deploy")]
        public void SetCurrent()
        {

        }

        [SoInfo("下载 ..."), ServiceObjectAction]
        [UIObjectAction(typeof(DownloadAction)), ServiceObjectGroup("download")]
        public void Download()
        {

        }

        [SoInfo("下载并解压 ..."), ServiceObjectAction]
        [UIObjectAction(typeof(ExtratAction)), ServiceObjectGroup("download")]
        public void Extrat()
        {

        }

        [SoInfo("服务"), ServiceObjectProperty]
        public string Service
        {
            get { return _packageInfo.ServiceDesc.ToString(); }
        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return _mgrCtx.ServiceUIInfos.GetTitle(_packageInfo.ServiceDesc); }
        }

        [SoInfo("当前版本？"), ServiceObjectProperty]
        public string IsCurrentVersion
        {
            get { return _mgrCtx.ServiceDeployPackageInfos.IsCurrentVersion(_packageInfo.Id) ? "是" : "否"; }
        }

        [SoInfo("最后修改时间"), ServiceObjectProperty]
        public string ModifyTime
        {
            get
            {
                return _packageInfo.LastUpdate.GetLocalTimeString();
            }
        }

        [SoInfo("大小"), ServiceObjectProperty]
        public string Size
        {
            get
            {
                return StringUtility.GetSizeString(_packageInfo.Size);
            }
        }

        [SoInfo("状态"), ServiceObjectProperty]
        public string Status
        {
            get
            {
                return GetCurrentServiceStatusDesc();
            }
        }

        [SoInfo("上传时间"), ServiceObjectProperty]
        public string UploadTime
        {
            get
            {
                return _packageInfo.UploadTime.GetLocalTimeString();
            }
        }

        [SoInfo("最后更新"), ServiceObjectProperty]
        public string LastUpdate
        {
            get { return _packageInfo.LastUpdate.GetLocalTimeString(); }
        }

        [SoInfo("格式"), ServiceObjectProperty]
        public string Format
        {
            get { return _packageInfo.Format.GetDesc(); }
        }

        protected override string GetServiceStatusDesc(ServiceStatus status)
        {
            switch (status)
            {
                case ServiceStatus.Default:
                    return "已上传";

                case ServiceStatus.Deploying:
                    return "正在部署：" + (int)(_deployingRate * 100) + "%";
            }

            return base.GetServiceStatusDesc(status);
        }

        abstract class ActionBase : ActionBase<ServiceDeployPackageNode>
        {
            public SfManagementContext MgrCtx { get { return Kernel._mgrCtx; } }
            public SystemInvoker Invoker { get { return MgrCtx.Invoker; } }
            public ServiceDeployPackageInfo PackageInfo { get { return Kernel._packageInfo; } }
        }
    }
}

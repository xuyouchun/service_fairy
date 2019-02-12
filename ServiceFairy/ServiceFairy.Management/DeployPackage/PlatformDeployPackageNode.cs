using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Service;
using Common.Utility;
using ServiceFairy.Entities.Master;
using Common.WinForm;
using System.IO;
using Common;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities;

namespace ServiceFairy.Management.DeployPackage
{
    /// <summary>
    /// Framework安装包
    /// </summary>
    [SoInfo("平台安装包"), UIObjectImage(ResourceNames.PlatformDeployPackage)]
    partial class PlatformDeployPackageNode : ServiceObjectKernelTreeNodeBase
    {
        public PlatformDeployPackageNode(SfManagementContext mgrCtx, PlatformDeployPackageInfo packageInfo)
        {
            _mgrCtx = mgrCtx;
            _packageInfo = packageInfo;
        }

        private readonly SfManagementContext _mgrCtx;
        private readonly PlatformDeployPackageInfo _packageInfo;

        public static PlatformDeployPackageNode StartNew(SfManagementContext mgrCtx, PlatformDeployPackageInfo packageInfo)
        {
            PlatformDeployPackageNode node = new PlatformDeployPackageNode(mgrCtx, packageInfo);
            node.CurrentStatus = ServiceStatus.Uploading;
            return node;
        }

        protected override ServiceObjectRefreshResult OnInitRefresh(InitContext initCtx)
        {
            switch(CurrentStatus)
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

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_packageInfo.Title);
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

        [SoInfo("部署到所有终端 ...")]
        [ServiceObjectAction()]
        [UIObjectAction(typeof(DeployAllAction)), ServiceObjectGroup("deploy")]
        public void DeployAll()
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

        [SoInfo("最后修改时间"), ServiceObjectProperty]
        public string ModifyTime
        {
            get
            {
                return _packageInfo.LastUpdate.ToLocalTime().GetString();
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

        [SoInfo("上传时间"), ServiceObjectProperty]
        public string UploadTime
        {
            get
            {
                return _packageInfo.UploadTime.ToLocalTime().GetString();
            }
        }

        [SoInfo("最后更新"), ServiceObjectProperty]
        public string LastUpdate
        {
            get { return _packageInfo.LastUpdate.ToLocalTime().GetString(); }
        }

        [SoInfo("格式"), ServiceObjectProperty]
        public string Format
        {
            get { return _packageInfo.Format.GetDesc(); }
        }

        [SoInfo("状态"), ServiceObjectProperty]
        public string Status
        {
            get
            {
                return GetCurrentServiceStatusDesc();
            }
        }

        protected override string GetServiceStatusDesc(ServiceStatus status)
        {
            switch (status)
            {
                case ServiceStatus.Default:
                    return "已上传";

                case ServiceStatus.Deploying:
                    return "正在部署：" + _deployingRate;
            }

            return base.GetServiceStatusDesc(status);
        }

        protected override object GetKey()
        {
            return new Tuple<Type, PlatformDeployPackageInfo>(GetType(), _packageInfo);
        }

        abstract class ActionBase : ActionBase<PlatformDeployPackageNode>
        {
            public SfManagementContext Ctx { get { return Kernel._mgrCtx; } }
            public SystemInvoker Invoker { get { return Ctx.Invoker; } }
            public PlatformDeployPackageInfo PackageInfo { get { return Kernel._packageInfo; } }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.Entities.Master;
using ServiceFairy.SystemInvoke;
using Common.Utility;
using Common.Package.UIObject;
using ServiceFairy.Entities;

namespace ServiceFairy.Management.DeployPackage
{
    [SoInfo("安装包中的文件"), UIObjectImage(ResourceNames.DeployPackageFile)]
    partial class DeployPackageFileNode : ServiceObjectKernelTreeNodeBase
    {
        public DeployPackageFileNode(SfManagementContext mgrCtx, DeployPackageInfo packageInfo, AppFileInfo fileInfo, string baseDirectory)
        {
            _mgrCtx = mgrCtx;
            _packageInfo = packageInfo;
            _fileInfo = fileInfo;
            _baseDirectory = baseDirectory;
        }

        private readonly SfManagementContext _mgrCtx;
        private readonly DeployPackageInfo _packageInfo;
        private readonly AppFileInfo _fileInfo;
        private readonly string _baseDirectory;

        protected override object GetKey()
        {
            return new Tuple<Type, Guid, string>(GetType(), _packageInfo.Id, _fileInfo.FileName.ToLower());
        }

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_fileInfo.FileName);
        }

        [SoInfo("打开")]
        [ServiceObjectAction(ServiceObjectActionType.Default)]
        [UIObjectAction(typeof(OpenAction)), UIObjectImage("open"), ServiceObjectGroup("open")]
        public void Open()
        {

        }

        [SoInfo("打开所在目录")]
        [ServiceObjectAction(ServiceObjectActionType.Default)]
        [UIObjectAction(typeof(OpenFolderAction)), UIObjectImage("open_folder"), ServiceObjectGroup("open")]
        public void OpenFolder()
        {

        }

        [SoInfo("大小"), ServiceObjectProperty]
        public string Size
        {
            get { return StringUtility.GetSizeString(_fileInfo.Size); }
        }

        [SoInfo("修改时间"), ServiceObjectProperty]
        public string LastUpdate
        {
            get { return _fileInfo.LastModifyTime.GetLocalTimeString(); }
        }

        [SoInfo("类别"), ServiceObjectProperty]
        public string Category
        {
            get { return SmUtility.GetFileCategory(_fileInfo.FileName); }
        }

        abstract class ActionBase : ActionBase<DeployPackageFileNode>
        {
            public SfManagementContext MgrCtx { get { return Kernel._mgrCtx; } }
            public SystemInvoker Invoker { get { return MgrCtx.Invoker; } }
            public DeployPackageInfo PackageInfo { get { return Kernel._packageInfo; } }
            public AppFileInfo FileInfo { get { return Kernel._fileInfo; } }
            public string BaseDirectory { get { return Kernel._baseDirectory; } }
        }
    }
}

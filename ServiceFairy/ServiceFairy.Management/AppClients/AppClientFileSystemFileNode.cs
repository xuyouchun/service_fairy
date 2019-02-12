using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using ServiceFairy.Entities.Tray;
using Common.Contracts.Service;
using Common.Utility;
using ServiceFairy.SystemInvoke;
using Common.Contracts.UIObject;
using Common.Package.Drawing;
using System.Drawing;
using Common.Package;

namespace ServiceFairy.Management.AppClients
{
    /// <summary>
    /// 文件
    /// </summary>
    [SoInfo("文件"), UIObjectImage(ResourceNames.AppClientFileSystemFile)]
    partial class AppClientFileSystemFileNode : ServiceObjectKernelTreeNodeBase
    {
        public AppClientFileSystemFileNode(AppClientContext clientCtx, FsDirectoryInfoItem dInfoItem, FsFileInfo fileInfo)
        {
            _clientCtx = clientCtx;
            _fileInfo = fileInfo;
            _dInfoItem = dInfoItem;
        }

        private readonly AppClientContext _clientCtx;
        private readonly FsFileInfo _fileInfo;
        private readonly FsDirectoryInfoItem _dInfoItem;
        private readonly ResourceUIObjectImageLoader _defaultImageLoader = ResourceUIObjectImageLoader.Load(typeof(AppClientFileSystemFileNode), ResourceNames.AppClientFileSystemFile);

        protected override object GetKey()
        {
            return new Tuple<Type, FsFileInfo>(GetType(), _fileInfo);
        }

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_fileInfo.Name);
        }

        //protected override IUIObject OnCreateUIObject()
        //{
        //    return new ServiceUIObject(OnGetServiceObjectInfo(),
        //        UIObjectImageLoader.FromFileSystemIcon(_fileInfo.Name) ?? _defaultImageLoader);
        //}

        [SoInfo("打开")]
        [ServiceObjectAction(ServiceObjectActionType.Default)]
        [UIObjectAction(typeof(OpenAction)), UIObjectImage("open"), ServiceObjectGroup("open")]
        public void Open()
        {

        }

        [SoInfo("下载 ...")]
        [ServiceObjectAction(ServiceObjectActionType.AttachDefault)]
        [UIObjectAction(typeof(DownloadAction)), ServiceObjectGroup("edit")]
        public void Download()
        {

        }

        [SoInfo("删除 ...")]
        [ServiceObjectAction]
        [UIObjectAction(typeof(DeleteAction)), ServiceObjectGroup("edit")]
        public void Delete()
        {

        }

        [SoInfo("标题", Weight = 10), ServiceObjectProperty]
        public string Title
        {
            get { return _fileInfo.Title; }
        }

        [SoInfo("描述", Weight = 20), ServiceObjectProperty]
        public string Desc
        {
            get { return _fileInfo.Desc; }
        }

        [SoInfo("创建时间", Weight = 30), ServiceObjectProperty]
        public string CreationTime
        {
            get { return _fileInfo.CreationTime.GetLocalTimeString(); }
        }

        [SoInfo("修改时间", Weight = 40), ServiceObjectProperty]
        public string LastModifyTime
        {
            get { return _fileInfo.LastModifyTime.GetLocalTimeString(); }
        }

        [SoInfo("大小", Weight = 50), ServiceObjectProperty]
        public string Size
        {
            get { return StringUtility.GetSizeString(_fileInfo.Size); }
        }

        [SoInfo("完整路径", Weight = 60), ServiceObjectProperty]
        public string Path
        {
            get { return System.IO.Path.Combine(_dInfoItem.Directory, _fileInfo.Path); }
        }

        abstract class ActionBase : ActionBase<AppClientFileSystemFileNode>
        {
            public AppClientContext ClientCtx { get { return Kernel._clientCtx; } }
            public SystemInvoker Invoker { get { return ClientCtx.MgrCtx.Invoker; } }
            public SfManagementContext MgrCtx { get { return ClientCtx.MgrCtx; } }
            public FsFileInfo Info { get { return Kernel._fileInfo; } }
            public FsDirectoryInfoItem DirectoryInfoItem { get { return Kernel._dInfoItem; } }
        }
    }
}

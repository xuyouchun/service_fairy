using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Service;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities.Tray;
using Common.Utility;
using Common.Package.UIObject.Actions;
using Common;
using Common.Contracts.UIObject;

namespace ServiceFairy.Management.AppClients
{
    /// <summary>
    /// 文件系统
    /// </summary>
    [SoInfo("目录"), UIObjectImage(ResourceNames.AppClientFileSystemDirectory)]
    class AppClientFileSystemDirectoryNode : ServiceObjectKernelTreeNodeBase
    {
        public AppClientFileSystemDirectoryNode(AppClientContext clientCtx,
            FsDirectoryInfoItem dInfoItem, FsDirectoryInfo directoryInfo)
        {
            _clientCtx = clientCtx;
            _invoker = clientCtx.MgrCtx.Invoker;
            _dInfoItem = dInfoItem;
            _directoryInfo = directoryInfo;
        }

        private readonly AppClientContext _clientCtx;
        private readonly SystemInvoker _invoker;
        private readonly FsDirectoryInfoItem _dInfoItem;
        private readonly FsDirectoryInfo _directoryInfo;

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(
                StringUtility.GetFirstNotNullOrWhiteSpaceString(_directoryInfo.Name));
        }

        protected override IUIObject OnCreateUIObject()
        {
            string imgRes = null;

            if (FsDirectoryInfo.IsLogicDriverType(_directoryInfo.SpecialFolder))
            {
                if (_directoryInfo.SpecialFolder == FsDirectoryInfo.LogicDriver_CDRom)
                    imgRes = "AppClientFileSystemDirectory_CDRom";
                else if (_directoryInfo.SpecialFolder == FsDirectoryInfo.LogicDriver_Removable)
                    imgRes = "AppClientFileSystemDirectory_Movable";
                else
                    imgRes = "AppClientFileSystemDirectory_Driver";
            }
            else if (FsDirectoryInfo.IsAppServiceSystemDirectory(_directoryInfo.SpecialFolder))
            {
                imgRes = "AppClientFileSystemDirectory_System";
            }
            else if (_directoryInfo.SpecialFolder != FsDirectoryInfo.Normal)
            {
                imgRes = "AppClientFileSystemDirectory_Special";
            }

            if (imgRes == null)
                return null;

            return new ServiceUIObject(OnGetServiceObjectInfo(),
                SmUtility.CreateResourceImageLoader(imgRes)
            );
        }

        protected override object GetKey()
        {
            return new Tuple<Type, FsDirectoryInfo>(GetType(), _directoryInfo);
        }

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            FsDirectoryInfoItem item = _invoker.Tray.FsGetDirectoryInfo(
                Path, FsGetDirectoryInfosOption.All, false, _clientCtx.ClientDesc.ClientID);

            if (item == null)
                return Array<IServiceObjectTreeNode>.Empty;

            List<IServiceObjectTreeNode> nodes = new List<IServiceObjectTreeNode>();
            if (item.SubDirectoriesInfos != null)
                nodes.AddRange(item.SubDirectoriesInfos.Select(d => new AppClientFileSystemDirectoryNode(_clientCtx, item, d)));

            if (item.FileInfos != null)
                nodes.AddRange(item.FileInfos.Select(f => new AppClientFileSystemFileNode(_clientCtx, item, f)));

            return nodes;
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

        [SoInfo("标题", Weight = 10), ServiceObjectProperty]
        public string Title
        {
            get { return _directoryInfo.Title; }
        }

        [SoInfo("描述", Weight = 20), ServiceObjectProperty]
        public string Desc
        {
            get { return _directoryInfo.Desc; }
        }

        [SoInfo("创建时间", Weight = 30), ServiceObjectProperty]
        public string CreationTime
        {
            get { return _directoryInfo.CreationTime.GetLocalTimeString(); }
        }

        [SoInfo("修改时间", Weight = 40), ServiceObjectProperty]
        public string LastModifyTime
        {
            get { return _directoryInfo.LastModifyTime.GetLocalTimeString(); }
        }

        [SoInfo("完整路径", Weight = 60), ServiceObjectProperty]
        public string Path
        {
            get
            {
                if (_dInfoItem == null)
                    return _directoryInfo.Path;

                return System.IO.Path.Combine(_dInfoItem.Directory, _directoryInfo.Path);
            }
        }
    }
}

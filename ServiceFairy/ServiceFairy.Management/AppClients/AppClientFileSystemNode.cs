using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Service;
using ServiceFairy.Entities.Tray;
using ServiceFairy.SystemInvoke;
using Common.Package.UIObject.Actions;

namespace ServiceFairy.Management.AppClients
{
    /// <summary>
    /// 文件系统
    /// </summary>
    [SoInfo("文件系统"), UIObjectImage(ResourceNames.AppClientFileSystem)]
    class AppClientFileSystemNode : ListServiceObjectKernelTreeNodeBase
    {
        public AppClientFileSystemNode(AppClientContext clientCtx)
        {
            _clientCtx = clientCtx;
            _invoker = clientCtx.MgrCtx.Invoker;
        }

        private readonly AppClientContext _clientCtx;
        private readonly SystemInvoker _invoker;

        protected override object GetKey()
        {
            return GetType();
        }

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            FsDirectoryInfo[] infos = _invoker.Tray.FsGetRootDirectoryInfos(_clientCtx.ClientDesc.ClientID);
            return infos.Select(info => new AppClientFileSystemDirectoryNode(_clientCtx, null, info));
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

        [SoInfo("数量", Name = "Count"), ServiceObjectProperty]
        public string Count
        {
            get { return ""; }
        }


        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return "查看本地文件"; }
        }

        [SoInfo("详细"), ServiceObjectProperty]
        public string Detail
        {
            get { return ""; }
        }
    }
}

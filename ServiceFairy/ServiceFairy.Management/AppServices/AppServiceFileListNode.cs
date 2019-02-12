using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using ServiceFairy.Management.AppClients;
using Common.Contracts.Service;
using Common.Package.UIObject.Actions;

namespace ServiceFairy.Management.AppServices
{
    /// <summary>
    /// 文件列表
    /// </summary>
    [SoInfo("文件列表"), UIObjectImage(ResourceNames.AppServiceFileList)]
    class AppServiceFileListNode : ListServiceObjectKernelTreeNodeBase
    {
        public AppServiceFileListNode(AppClientContext clientCtx, ServiceDesc serviceDesc)
        {
            _clientCtx = clientCtx;
            _clientId = clientCtx.ClientDesc.ClientID;
            _serviceDesc = serviceDesc;
        }

        private readonly Guid _clientId;
        private readonly AppClientContext _clientCtx;
        private readonly ServiceDesc _serviceDesc;

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            AppFileInfo[] fileInfos = _clientCtx.MgrCtx.GetAppFileManager(_clientId, _serviceDesc).GetAll();
            return fileInfos.Where(fi => !string.IsNullOrWhiteSpace(fi.FileName)).Select(fi => new AppServiceFileNode(_clientCtx, _serviceDesc, fi));
        }

        protected override object GetKey()
        {
            return GetType();
        }

        [SoInfo("打开"), ServiceObjectAction(ServiceObjectActionType.Default)]
        [UIObjectAction(typeof(OpenAction)), UIObjectImage("open"), ServiceObjectGroup("open")]
        public void Open()
        {

        }

        [SoInfo("在新窗口中打开"), ServiceObjectAction(ServiceObjectActionType.AttachDefault | ServiceObjectActionType.OpenInNewWindow)]
        [UIObjectAction(typeof(OpenInNewWindowAction)), ServiceObjectGroup("open")]
        public void OpenInNewWindow()
        {

        }

        [SoInfo("数量"), ServiceObjectProperty]
        public int Count
        {
            get { return _clientCtx.MgrCtx.GetAppFileManager(_clientId, _serviceDesc).GetAll().Length; }
        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return "查看该服务的所有文件"; }
        }
    }
}

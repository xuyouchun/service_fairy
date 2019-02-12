using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Package.UIObject.Actions;
using ServiceFairy.Management.AppClients;
using Common.Utility;

namespace ServiceFairy.Management.AppCommands
{
    [SoInfo("接口列表"), UIObjectImage(ResourceNames.AppCommandList)]
    partial class AppCommandListNode : ListServiceObjectKernelTreeNodeBase
    {
        public AppCommandListNode(AppClientContext clientCtx, ServiceDesc serviceDesc)
        {
            _clientId = clientCtx.AppClientInfo.ClientId;
            _clientCtx = clientCtx;
            _serviceDesc = serviceDesc;
        }

        private readonly Guid _clientId;
        private readonly AppClientContext _clientCtx;
        private readonly ServiceDesc _serviceDesc;

        protected override IEnumerable<Common.Contracts.Service.IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            return _clientCtx.MgrCtx.GetAppCommandManager(_clientId, _serviceDesc).GetAll()
                .Where(cmdInfo => _showAll || cmdInfo.Category == AppCommandCategory.Application)
                .Select(cmdInfo => new AppCommandNode(_clientCtx, _serviceDesc, cmdInfo));
        }

        protected override object GetKey()
        {
            return GetType();
        }

        [SoInfo("打开")]
        [ServiceObjectAction(ServiceObjectActionType.Default | ServiceObjectActionType.Open)]
        [UIObjectAction(typeof(OpenAction)), UIObjectImage("open"), ServiceObjectGroup("open")]
        public void Open() { }

        [SoInfo("在新窗口中打开")]
        [ServiceObjectAction(ServiceObjectActionType.AttachDefault | ServiceObjectActionType.OpenInNewWindow)]
        [UIObjectAction(typeof(OpenInNewWindowAction)), ServiceObjectGroup("open")]
        public void OpenInNewWindow() { }

        [SoInfo("显示所有接口")]
        [ServiceObjectAction, UIObjectAction(typeof(ShowAllAction)), ServiceObjectGroup("show")]
        public void ShowAll() { }

        [SoInfo("数量"), ServiceObjectProperty]
        public string Count
        {
            get
            {
                AppCommandInfo[] infos = _clientCtx.MgrCtx.GetAppCommandManager(_clientId, _serviceDesc).GetAll();
                return new[] {
                    "全部:" + infos.Length,
                    "应用:" + infos.Count(info=>info.Category == AppCommandCategory.Application ),
                    "系统:" + infos.Count(info=>info.Category== AppCommandCategory.System)
                }.JoinBy("; ", true);
            }
        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return "查看接口信息，参数类型等"; }
        }

        abstract class ActionBase : ActionBase<AppCommandListNode>
        {

        }
    }
}

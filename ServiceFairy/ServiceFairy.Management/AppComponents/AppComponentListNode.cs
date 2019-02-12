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

namespace ServiceFairy.Management.AppComponents
{
    [SoInfo("组件列表"), UIObjectImage(ResourceNames.AppComponentList)]
    partial class AppComponentListNode : ListServiceObjectKernelTreeNodeBase
    {
        public AppComponentListNode(AppClientContext clientCtx, ServiceDesc serviceDesc)
        {
            _clientCtx = clientCtx;
            _serviceDesc = serviceDesc;
            _clientId = _clientCtx.ClientDesc.ClientID;
        }

        private readonly Guid _clientId;
        private readonly AppClientContext _clientCtx;
        private readonly ServiceDesc _serviceDesc;

        protected override object GetKey()
        {
            return GetType();
        }

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            return _clientCtx.MgrCtx.GetAppComponentManager(_clientId, _serviceDesc).GetAll()
                .Where(info => _showAll || info.Category == AppComponentCategory.Application)
                .Select(cmdInfo => new AppComponentNode(_clientCtx, _serviceDesc, cmdInfo));
        }

        [SoInfo("打开")]
        [ServiceObjectAction(ServiceObjectActionType.Default | ServiceObjectActionType.Open)]
        [UIObjectAction(typeof(OpenAction)), UIObjectImage("open"), ServiceObjectGroup("open")]
        public void Open() { }

        [SoInfo("在新窗口中打开")]
        [ServiceObjectAction(ServiceObjectActionType.AttachDefault | ServiceObjectActionType.OpenInNewWindow)]
        [UIObjectAction(typeof(OpenInNewWindowAction)), ServiceObjectGroup("open")]
        public void OpenInNewWindow() { }

        [SoInfo("显示所有组件")]
        [ServiceObjectAction, UIObjectAction(typeof(ShowAllAction)), ServiceObjectGroup("show")]
        public void ShowAll() { }

        [SoInfo("数量"), ServiceObjectProperty]
        public string Count
        {
            get 
            {
                AppComponentInfo[] infos = _clientCtx.MgrCtx.GetAppComponentManager(_clientId, _serviceDesc).GetAll();
                return new[] {
                    "全部:" + infos.Length,
                    "应用:" + infos.Count(info=>info.Category== AppComponentCategory.Application),
                    "系统:" + infos.Count(info=>info.Category== AppComponentCategory.System)
                }.JoinBy("; ", true);
            }
        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return "查看组件信息，数据及对组件进行控制"; }
        }

        abstract class ActionBase : ActionBase<AppComponentListNode> { }
    }
}

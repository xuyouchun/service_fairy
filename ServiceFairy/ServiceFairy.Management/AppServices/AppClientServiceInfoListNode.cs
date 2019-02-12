using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Client;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.UIObject;
using Common.WinForm;
using System.Windows.Forms;
using Common.Utility;
using Common.Contracts;
using Common.Package.UIObject.Actions;
using Common.Package;
using ServiceFairy.Management.AppServices;
using ServiceFairy.Management.AppClients;

namespace ServiceFairy.Management.AppServices
{
    /// <summary>
    /// 服务列表节点
    /// </summary>
    [SoInfo("服务列表"), UIObjectImage(ResourceNames.AppClientServiceList)]
    partial class AppClientServiceInfoListNode : ListServiceObjectKernelTreeNodeBase
    {
        public AppClientServiceInfoListNode(AppClientContext clientCtx)
        {
            _clientCtx = clientCtx;
        }

        private AppClientContext _clientCtx;

        protected override object GetKey()
        {
            return new Tuple<Type, Guid>(GetType(), _clientCtx.ClientDesc.ClientID);
        }

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            return _clientCtx.ServiceInfos.Select(
                serviceInfo => new AppServiceInfoNode(_clientCtx, serviceInfo)
            );
        }

        [SoInfo("打开")]
        [ServiceObjectAction(ServiceObjectActionType.Default | ServiceObjectActionType.Open)]
        [UIObjectAction(typeof(OpenAction)), UIObjectImage("open"), ServiceObjectGroup("open")]
        public void Open() { }

        [SoInfo("在新窗口中打开")]
        [ServiceObjectAction(ServiceObjectActionType.AttachDefault | ServiceObjectActionType.OpenInNewWindow)]
        [UIObjectAction(typeof(OpenInNewWindowAction)), ServiceObjectGroup("open")]
        public void OpenInNewWindow() { }

        [SoInfo("刷新")]
        [ServiceObjectAction(ServiceObjectActionType.Refresh), UIObjectAction(typeof(RefreshAction))]
        public void Refresh() { }

        [SoInfo("启动新服务 ...")]
        [ServiceObjectAction(ServiceObjectActionType.New)]
        [UIObjectAction(typeof(StartNewServiceAction)), ServiceObjectGroup("service")]
        public void StartNewService() { }

        [SoInfo("数量", Name = "Count"), ServiceObjectProperty]
        public int ServiceInfoCount
        {
            get { return _clientCtx.ServiceInfos.Length; }
        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return "该终端上运行的所有服务"; }
        }

        [SoInfo("详细"), ServiceObjectProperty]
        public string Detail
        {
            get { return _clientCtx.ServiceInfos.Select(si => _ToStr(si)).JoinBy("; "); }
        }

        private string _ToStr(ServiceInfo si)
        {
            ServiceDesc sd = si.ServiceDesc;
            return sd.Version.IsEmpty || sd.Version == "1.0" ? sd.Name : sd.ToString();
        }

        abstract class ActionBase : ActionBase<AppClientServiceInfoListNode>
        {

        }
    }
}

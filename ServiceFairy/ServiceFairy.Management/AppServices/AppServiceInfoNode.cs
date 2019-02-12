using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.SystemInvoke;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Utility;
using Common.Contracts.UIObject;
using Common.WinForm;
using ServiceFairy.Client;
using Common.Contracts;
using Common.Package;
using System.Threading;
using ServiceFairy.Management.AppClients;
using ServiceFairy.Management.AppCommands;
using ServiceFairy.Management.AppComponents;
using Common.Package.UIObject.Actions;
using ServiceFairy.Management.AppServiceAddins;

namespace ServiceFairy.Management.AppServices
{
    /// <summary>
    /// 服务信息节点
    /// </summary>
    [UIObjectImage(ResourceNames.AppServiceInfo)]
    partial class AppServiceInfoNode : ServiceObjectKernelTreeNodeBase
    {
        public AppServiceInfoNode(AppClientContext clientCtx, ServiceInfo serviceInfo)
        {
            _clientCtx = clientCtx;
            _serviceInfo = serviceInfo;
            _sd = serviceInfo.ServiceDesc;
        }

        private AppServiceInfoNode(AppClientContext clientCtx, ServiceDesc sd)
        {
            _clientCtx = clientCtx;
            _sd = sd;
        }

        public static AppServiceInfoNode StartNew(AppClientContext clientCtx, ServiceDesc sd)
        {
            AppServiceInfoNode node = new AppServiceInfoNode(clientCtx, sd);
            clientCtx.MgrCtx.Invoker.Master.StartService(clientCtx.ClientDesc.ClientID, sd);
            node.CurrentStatus = ServiceStatus.Starting;

            return node;
        }

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            return new IServiceObjectTreeNode[] {
                new AppCommandListNode(_clientCtx, _sd),
                new AppComponentListNode(_clientCtx, _sd),
                new AppServiceAddinListNode(_clientCtx, _sd),
                new AppServiceFileListNode(_clientCtx, _sd),
            };
        }

        protected override object GetKey()
        {
            return new Tuple<Type, ServiceDesc>(GetType(), _sd);
        }

        protected override ServiceObjectRefreshResult OnInitRefresh(InitContext initCtx)
        {
            if ((_serviceInfo = _clientCtx.GetServiceInfo(_sd)) == null || _serviceInfo.Status != AppServiceStatus.Running)
            {
                return ServiceObjectRefreshResult.ContinueAndRefresh;
            }

            CurrentStatus = ServiceStatus.Default;
            return ServiceObjectRefreshResult.CompletedAndRefresh;
        }

        private ServiceInfo _serviceInfo;
        private readonly AppClientContext _clientCtx;
        private readonly ServiceDesc _sd;
        private readonly AppServiceCategory _category;

        public ServiceDesc ServiceDesc
        {
            get { return _sd; }
        }

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_sd.Name.ToString());
        }

        [SoInfo("打开"), ServiceObjectAction(ServiceObjectActionType.Default | ServiceObjectActionType.Open)]
        [UIObjectAction(typeof(OpenAction)), UIObjectImage("open"), ServiceObjectGroup("open"), Enable(ServiceStatus.Default)]
        public void Open()
        {

        }

        [SoInfo("在新窗口中打开"), ServiceObjectAction(ServiceObjectActionType.AttachDefault | ServiceObjectActionType.OpenInNewWindow)]
        [UIObjectAction(typeof(OpenInNewWindowAction)), ServiceObjectGroup("open"), Enable(ServiceStatus.Default)]
        public void OpenInNewWindow()
        {

        }

        [SoInfo("停止")]
        [ServiceObjectAction(ServiceObjectActionType.Close)]
        [UIObjectAction(typeof(StopServiceAction)), Enable(ServiceStatus.Default)]
        public void Stop() { }

        [SoInfo("重新启动")]
        [ServiceObjectAction(ServiceObjectActionType.Restart)]
        [UIObjectAction(typeof(RestartServiceAction)), Enable(ServiceStatus.Default)]
        public void Restart() { }

        [SoInfo("取消")]
        [ServiceObjectAction(ServiceObjectActionType.Cancel)]
        [UIObjectAction(typeof(CancelStartAction))]
        [Visiable(ServiceStatus.Starting), ServiceObjectGroup("cancel")]
        public void CancelStart() { }

        [SoInfo("标题"), ServiceObjectProperty]
        public string Title
        {
            get { return _serviceInfo == null ? _clientCtx.MgrCtx.ServiceUIInfos.GetTitle(_sd) ?? "?" : _serviceInfo.Title; }
        }

        protected override string GetServiceStatusDesc(ServiceStatus status)
        {
            if (status == ServiceStatus.Default)
                return _serviceInfo == null ? "?" : _serviceInfo.Status.GetDesc();

            return base.GetServiceStatusDesc(status);
        }

        [SoInfo("版本"), ServiceObjectProperty]
        public string Version
        {
            get { return _sd.Version; }
        }

        [SoInfo("启动时间"), ServiceObjectProperty]
        public string StartTime
        {
            get { return _serviceInfo == null ? "?" : _serviceInfo.StartTime.GetLocalTimeString(); }
        }

        [SoInfo("运行时长"), ServiceObjectProperty]
        public string RunningTime
        {
            get { return _serviceInfo == null ? "?" : _serviceInfo.StartTime.GetUtcUntilNowString(); }
        }

        [SoInfo("内存占用"), ServiceObjectProperty]
        public string Memory
        {
            get
            {
                if (_serviceInfo == null || _serviceInfo.RuntimeInfo == null)
                    return "?";

                return StringUtility.GetSizeString(_serviceInfo.RuntimeInfo.Memory);
            }
        }

        [SoInfo("状态"), ServiceObjectProperty]
        public string Status
        {
            get { return GetCurrentServiceStatusDesc(); }
        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return _serviceInfo == null ? "" : _serviceInfo.Desc; }
        }

        abstract class ActionBase : ActionBase<AppServiceInfoNode>
        {
            public AppClientContext ClientCtx { get { return Kernel._clientCtx; } }
            public SfManagementContext MgrCtx { get { return ClientCtx.MgrCtx; } }
            public SystemInvoker Invoker { get { return MgrCtx.Invoker; } }
            public ServiceInfo ServiceInfo { get { return Kernel._serviceInfo; } }
            public ServiceDesc ServieDesc { get { return Kernel._sd; } }
        }
    }
}

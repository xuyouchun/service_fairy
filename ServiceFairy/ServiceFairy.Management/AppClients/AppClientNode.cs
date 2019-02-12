using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities;
using Common.Package.Service;
using ServiceFairy.Client;
using Common.Package.UIObject;
using Common.Utility;
using Common.Package.UIObject.Actions;
using Common.Package;
using ServiceFairy.Entities.Master;
using ServiceFairy.Management.AppServices;
using Common;

namespace ServiceFairy.Management.AppClients
{
    /// <summary>
    /// 服务终端节点
    /// </summary>
    [UIObjectImage(ResourceNames.AppClient)]
    partial class AppClientNode : ServiceObjectKernelTreeNodeBase
    {
        public AppClientNode(SfManagementContext mgrCtx, ClientDesc clientDesc)
        {
            _clientDesc = clientDesc;
            _mgrCtx = mgrCtx;
            _clientCtx = new AppClientContext(mgrCtx, clientDesc);
        }

        private readonly ClientDesc _clientDesc;
        private readonly SfManagementContext _mgrCtx;
        private readonly AppClientContext _clientCtx;

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            string title = _clientDesc.Title;
            if (!_clientDesc.IPs.IsNullOrEmpty())
            {
                if (string.IsNullOrWhiteSpace(title))
                    title = _clientDesc.IPs[0];
                else
                    title += " (" + _clientDesc.IPs[0] + ")";
            }

            if (string.IsNullOrWhiteSpace(title))
                title = _clientDesc.ToString();

            return new ServiceObjectInfo(null, _clientDesc.ClientID.ToString(), title);
        }

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            return new IServiceObjectTreeNode[] {
                new AppClientServiceInfoListNode(_clientCtx),   // 服务列表
                new AppClientCommunicationListNode(_clientCtx), // 信道列表
                new AppClientInvokeInfoListNode(_clientCtx),    // 调用方式
                new AppClientLocalLogListNode(_clientCtx),      // 本地日志
                new AppClientSystemLogListNode(_clientCtx),     // 系统日志
                new AppClientPropertyListNode(_clientCtx),      // 系统属性
                new AppClientSystemEnvironmentVariableListNode(_clientCtx), // 环境变量
                new AppClientFileSystemNode(_clientCtx),        // 文件系统
                new AppClientProcessListNode(_clientCtx),       // 进程列表
                new AppClientOnlineUserListNode(_clientCtx),    // 在线用户
            };
        }

        protected override object GetKey()
        {
            return new Tuple<Type, Guid>(GetType(), _clientDesc.ClientID);
        }

        [SoInfo(Weight = 10), ServiceObjectGroupDesc]
        private int open { get; set; }

        [SoInfo("打开")]
        [ServiceObjectAction(ServiceObjectActionType.Default | ServiceObjectActionType.Open)]
        [UIObjectAction(typeof(OpenAction)), ServiceObjectGroup("open")]
        public void Open()
        {

        }

        [SoInfo("在新窗口中打开")]
        [ServiceObjectAction(ServiceObjectActionType.AttachDefault | ServiceObjectActionType.OpenInNewWindow)]
        [UIObjectAction(typeof(OpenInNewWindowAction)), ServiceObjectGroup("open")]
        public void OpenInNewWindow()
        {

        }

        [SoInfo("控制台")]
        [ServiceObjectAction, UIObjectAction(typeof(ConsoleAction)), ServiceObjectGroup("local")]
        public void Console()
        {

        }

        [SoInfo("刷新")]
        [ServiceObjectAction(ServiceObjectActionType.Refresh), UIObjectAction(typeof(RefreshAction))]
        public void Refresh()
        {

        }

        [SoInfo("主机", Weight = 10), ServiceObjectProperty]
        public string HostName
        {
            get { return _clientDesc.HostName; }
        }

        [SoInfo("描述", Weight = 20), ServiceObjectProperty]
        public string Desc
        {
            get { return _clientDesc.Desc; }
        }

        [SoInfo("服务数量", Weight = 30), ServiceObjectProperty]
        public int ServiceCount
        {
            get { return _clientDesc.ServiceCount; }
        }

        [SoInfo("信道数量", Weight = 40), ServiceObjectProperty]
        public int CommunicationCount
        {
            get { return _clientDesc.CommunicationCount; }
        }

        [SoInfo("连接时间", Weight = 50), ServiceObjectProperty]
        public string StartTime
        {
            get { return _clientDesc.ConnectedTime.GetLocalTimeString(); }
        }

        [SoInfo("连接时长", Weight = 60), ServiceObjectProperty]
        public string RunningTime
        {
            get { return _clientDesc.ConnectedTime.GetUtcUntilNowString(); }
        }

        [SoInfo("内存(工作设置/专用)", Weight = 70), ServiceObjectProperty]
        public string Memory
        {
            get
            {
                AppClientRuntimeInfo r;
                if (_clientCtx.AppClientInfo == null || (r = _clientCtx.AppClientInfo.RuntimeInfo) == null)
                    return "?";

                return StringUtility.GetSizeString(r.WorkingSetMemorySize) + "/" + StringUtility.GetSizeString(r.PrivateMemorySize);
            }
        }

        [SoInfo("IP", Weight = 80), ServiceObjectProperty]
        public string IP
        {
            get { return (_clientDesc.IPs ?? Array<string>.Empty).JoinBy(", "); }
        }

        private OnlineUserStatInfo _GetOnlineUserStatInfo()
        {
            AppClientRuntimeInfo rInfo = _clientCtx.AppClientInfo.RuntimeInfo;
            return rInfo == null ? null : rInfo.OnlineUserStatInfo;
        }

        [SoInfo("在线用户(当前/最大/时间)", Weight = 90), ServiceObjectProperty]
        public string ConnectedUserCount
        {
            get
            {
                OnlineUserStatInfo statInfo = _GetOnlineUserStatInfo();
                return statInfo == null ? "?" : string.Format("{0}/{1} [{2}]",
                    statInfo.CurrentOnlineUserCount, statInfo.MaxOnlineUserCount, statInfo.MaxOnlineUserCountTime.GetLocalTimeString());
            }
        }

        [SoInfo("ID", Weight = 109), ServiceObjectProperty]
        public string ID
        {
            get { return _clientDesc.ClientID.ToString(); }
        }

        [SoInfo("平台版本", Weight = 110), ServiceObjectProperty]
        public string PlatformDeployId
        {
            get { return _clientCtx.MgrCtx.PlatformDeployPackageInfos.GetTitle(_clientCtx.AppClientInfo.PlatformDeployId); }
        }

        abstract class ActionBase : ActionBase<AppClientNode>
        {

        }
    }
}

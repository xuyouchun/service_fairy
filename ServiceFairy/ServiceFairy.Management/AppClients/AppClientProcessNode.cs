using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using ServiceFairy.Entities.Tray;
using Common.Contracts.Service;
using Common.Utility;
using Common.Package.UIObject.Actions;

namespace ServiceFairy.Management.AppClients
{
    [SoInfo("进程"), UIObjectImage(ResourceNames.AppClientProcess)]
    partial class AppClientProcessNode : ServiceObjectKernelTreeNodeBase
    {
        public AppClientProcessNode(AppClientContext clientCtx, ProcessInfo processInfo)
        {
            _clientCtx = clientCtx;
            _processInfo = processInfo;
        }

        private readonly AppClientContext _clientCtx;
        private readonly ProcessInfo _processInfo;

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            ThreadInfoCollection list = _clientCtx.MgrCtx.Invoker.Tray.GetThreads(_processInfo.ID, _clientCtx.AppClientInfo.ClientId);
            return list.ThreadInfos.Select(threadInfo => new AppClientThreadNode(_clientCtx, _processInfo, threadInfo));
        }

        protected override object GetKey()
        {
            return new Tuple<Type, int>(GetType(), _processInfo.ID);
        }

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_processInfo.Name);
        }

        [SoInfo("查看线程")]
        [ServiceObjectAction(ServiceObjectActionType.Default | ServiceObjectActionType.Open)]
        [UIObjectAction(typeof(OpenAction)), UIObjectImage("open"), ServiceObjectGroup("open")]
        public void Open()
        {

        }

        [SoInfo("在新窗口中查看线程")]
        [ServiceObjectAction(ServiceObjectActionType.AttachDefault | ServiceObjectActionType.OpenInNewWindow)]
        [UIObjectAction(typeof(OpenInNewWindowAction)), ServiceObjectGroup("open")]
        public void OpenInNewWindow()
        {

        }

        [SoInfo("结束进程")]
        [ServiceObjectAction]
        [UIObjectAction(typeof(KillProcessAction)), ServiceObjectGroup("process")]
        public void KillProcess()
        {

        }

        [SoInfo("启动时间"), ServiceObjectProperty]
        public string StartTime
        {
            get { return _processInfo.StartTime.GetLocalTimeString(); }
        }

        [SoInfo("运行时长"), ServiceObjectProperty]
        public string RunningTime
        {
            get { return _processInfo.StartTime.GetUtcUntilNowString(); }
        }

        [SoInfo("线程数"), ServiceObjectProperty]
        public string ThreadCount
        {
            get { return _processInfo.ThreadCount < 0 ? "" : _processInfo.ThreadCount.ToString(); }
        }

        [SoInfo("用户"), ServiceObjectProperty]
        public string User
        {
            get { return _processInfo.User; }
        }

        [SoInfo("内存(工作设置/专用)"), ServiceObjectProperty]
        public string PagedMemorySize
        {
            get
            {
                return _GetSizeString(_processInfo.WorkingSet) + " / " + _GetSizeString(_processInfo.PrivateMemorySize);
            }
        }

        [SoInfo("窗口标题"), ServiceObjectProperty]
        public string Title
        {
            get { return _processInfo.MainWindowTitle; }
        }

        [SoInfo("文件"), ServiceObjectProperty]
        public string CommandLine
        {
            get { return _processInfo.FileName; }
        }

        private static string _GetSizeString(long size)
        {
            if (size < 0)
                return "?";

            return StringUtility.GetSizeString(size);
        }

        abstract class ActionBase : ActionBase<AppClientProcessNode>
        {
            public AppClientContext Ctx { get { return Kernel._clientCtx; } }

            public ProcessInfo PInfo { get { return Kernel._processInfo; } }
        }
    }
}

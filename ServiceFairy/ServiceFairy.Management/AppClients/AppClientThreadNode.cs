using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using ServiceFairy.Entities.Tray;
using Common.Contracts.Service;
using Common.Utility;

namespace ServiceFairy.Management.AppClients
{
    [SoInfo("线程"), UIObjectImage(ResourceNames.AppClientThread)]
    class AppClientThreadNode : ServiceObjectKernelTreeNodeBase
    {
        public AppClientThreadNode(AppClientContext clientCtx, ProcessInfo processInfo, ThreadInfo threadInfo)
        {
            _clientCtx = clientCtx;
            _processInfo = processInfo;
            _threadInfo = threadInfo;
        }

        private readonly AppClientContext _clientCtx;
        private readonly ProcessInfo _processInfo;
        private readonly ThreadInfo _threadInfo;

        protected override object GetKey()
        {
            return new Tuple<Type, int, int>(GetType(), _processInfo.ID, _threadInfo.ID);
        }

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(StringUtility.GetFirstNotNullOrWhiteSpaceString(
                _threadInfo.Name, "线程－" + _threadInfo.ID));
        }

        [SoInfo("启动时间"), ServiceObjectProperty]
        public string StartTime
        {
            get { return _threadInfo.StartTime.GetLocalTimeString(); }
        }

        [SoInfo("运行时长"), ServiceObjectProperty]
        public string RunningTime
        {
            get { return _threadInfo.StartTime.GetUtcUntilNowString(); }
        }

        [SoInfo("优先级"), ServiceObjectProperty]
        public string PriorityLevel
        {
            get { return _threadInfo.ThreadPriorityLevel.ToString(); }
        }

        [SoInfo("状态"), ServiceObjectProperty]
        public string ThreadState
        {
            get { return _threadInfo.ThreadState.ToString(); }
        }

        [SoInfo("等待原因"), ServiceObjectProperty]
        public string ThreadWaitReason
        {
            get
            {
                if (_threadInfo.ThreadState != System.Diagnostics.ThreadState.Wait)
                    return "";

                return _threadInfo.ThreadWaitReason.ToString();
            }
        }
    }
}

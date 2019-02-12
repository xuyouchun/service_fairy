using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Common.Contracts.Service;
using Common.Contracts.UIObject;
using Common.Package.Service;
using Common.Package.UIObject;
using ServiceFairy.Entities.Tray;

namespace ServiceFairy.Management.AppClients
{
    /// <summary>
    /// 系统日志
    /// </summary>
    [SoInfo("系统日志"), UIObjectImage(ResourceNames.AppClientSystemLogItem)]
    partial class AppClientSystemLogItemNode : ServiceObjectKernelTreeNodeBase
    {
        public AppClientSystemLogItemNode(AppClientContext clientCtx, SystemLogGroup logGroup, SystemLogItem logItem)
        {
            _clientCtx = clientCtx;
            _logGroup = logGroup;
            _logItem = logItem;
        }

        private readonly AppClientContext _clientCtx;
        private readonly SystemLogGroup _logGroup;
        private readonly SystemLogItem _logItem;
        private readonly Guid _key = Guid.NewGuid();

        protected override object GetKey()
        {
            return new Tuple<Type, Guid, string, Guid>(GetType(), _clientCtx.ClientDesc.ClientID, _logGroup.Name, _key);
        }

        private string _GetResourceImageName()
        {
            switch (_logItem.Type)
            {
                case EventLogEntryType.Error:
                case EventLogEntryType.FailureAudit:
                    return ResourceNames.AppClientSystemLogItem_Error;

                case EventLogEntryType.Information:
                case EventLogEntryType.SuccessAudit:
                    return ResourceNames.AppClientSystemLogItem_Message;

                case EventLogEntryType.Warning:
                    return ResourceNames.AppClientSystemLogItem_Warning;
            }

            return ResourceNames.AppClientSystemLogItem;
        }

        [SoInfo("查看 ...")]
        [ServiceObjectAction(ServiceObjectActionType.Default)]
        [UIObjectAction(typeof(OpenAction)), UIObjectImage("open"), ServiceObjectGroup("open")]
        public void Open()
        {

        }

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            string msg = _logItem.Message ?? string.Empty;
            return ServiceObjectInfo.OfTitle(msg.Replace("\n", " ").Replace("\r", ""));
        }

        protected override IUIObject OnCreateUIObject()
        {
            return new ServiceUIObject(OnGetServiceObjectInfo(),
                SmUtility.CreateResourceImageLoader(_GetResourceImageName()));
        }

        /// <summary>
        /// 类别
        /// </summary>
        [SoInfo("类别"), ServiceObjectProperty]
        public string Category
        {
            get { return _GetCategory(_logItem.Type); }
        }

        private static string _GetCategory(EventLogEntryType type)
        {
            switch (type)
            {
                case EventLogEntryType.Error: return "错误";
                case EventLogEntryType.Warning: return "警告";
                case EventLogEntryType.Information: return "信息";
                case EventLogEntryType.SuccessAudit: return "成功审核";
                case EventLogEntryType.FailureAudit: return "失败审核";
            }

            return type.ToString();
        }

        /// <summary>
        /// 时间
        /// </summary>
        [SoInfo("时间"), ServiceObjectProperty]
        public string Time
        {
            get { return _logItem.Time.GetLocalTimeString(); }
        }

        /// <summary>
        /// 源
        /// </summary>
        [SoInfo("源"), ServiceObjectProperty]
        public string Source
        {
            get { return _logItem.Source; }
        }

        abstract class ActionBase : ActionBase<AppClientSystemLogItemNode>
        {
            public SystemLogItem LogItem { get { return Kernel._logItem; } }
        }
    }
}

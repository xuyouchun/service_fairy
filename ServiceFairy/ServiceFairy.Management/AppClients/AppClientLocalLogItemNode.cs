using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Log;
using Common.Contracts.Service;
using Common.Utility;
using System.Text.RegularExpressions;
using ServiceFairy.SystemInvoke;
using Common.Contracts.UIObject;
using Common.Contracts;

namespace ServiceFairy.Management.AppClients
{
    [SoInfo("日志"), UIObjectImage(ResourceNames.AppClientLocalLogItem)]
    partial class AppClientLocalLogItemNode : ServiceObjectKernelTreeNodeBase
    {
        public AppClientLocalLogItemNode(AppClientContext clientContext, LogItemGroup group, LogItem logItem)
        {
            _clientContext = clientContext;
            _group = group;
            _logItem = logItem;
            _clientId = _clientContext.ClientDesc.ClientID;
        }

        private readonly AppClientContext _clientContext;
        private readonly LogItemGroup _group;
        private readonly LogItem _logItem;
        private readonly Guid _clientId;

        private readonly Guid _key = Guid.NewGuid();
        protected override object GetKey()
        {
            return new Tuple<Type, string, Guid>(GetType(), _group.Name.ToLower(), _key);
        }

        private static readonly Regex _replaceLines = new Regex(@"[\r\n]+", RegexOptions.Singleline);

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_replaceLines.Replace(_logItem.Title, " "));
        }

        protected override IUIObject OnCreateUIObject()
        {
            return new ServiceUIObject(OnGetServiceObjectInfo(),
                SmUtility.CreateResourceImageLoader(_GetResourceImageName()));
        }

        private string _GetResourceImageName()
        {
            switch (_logItem.Type)
            {
                case MessageType.Error:
                    return ResourceNames.AppClientLocalLogItem_Error;

                case MessageType.Message:
                    return ResourceNames.AppClientLocalLogItem_Message;

                case MessageType.Warning:
                    return ResourceNames.AppClientLocalLogItem_Warning;
            }

            return ResourceNames.AppClientLocalLogItem;
        }

        [SoInfo("查看 ...")]
        [ServiceObjectAction(ServiceObjectActionType.Default)]
        [UIObjectAction(typeof(OpenAction)), UIObjectImage("open"), ServiceObjectGroup("open")]
        public void Open()
        {

        }

        /// <summary>
        /// 类别
        /// </summary>
        [SoInfo("类别"), ServiceObjectProperty]
        public string Category
        {
            get { return _logItem.Type.GetDesc(); }
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

        /// <summary>
        /// 详细信息
        /// </summary>
        [SoInfo("详细信息"), ServiceObjectProperty]
        public string Detail
        {
            get { return _replaceLines.Replace(_logItem.Detail ?? string.Empty, " "); }
        }

        abstract class ActionBase : ActionBase<AppClientLocalLogItemNode>
        {
            public LogItemGroup Group { get { return Kernel._group; } }
            public LogItem LogItem { get { return Kernel._logItem; } }
            public AppClientContext ClientCtx { get { return Kernel._clientContext; } }
            public SfManagementContext MgrCtx { get { return ClientCtx.MgrCtx; } }
            public SystemInvoker SystemInvoker { get { return MgrCtx.Invoker; } }
        }
    }
}

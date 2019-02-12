using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Log;
using Common.Contracts.Service;
using ServiceFairy.SystemInvoke;
using Common.Package.UIObject.Actions;
using Common.Utility;

namespace ServiceFairy.Management.AppClients
{
    /// <summary>
    /// 日志组
    /// </summary>
    [SoInfo("日志组"), UIObjectImage(ResourceNames.AppClientLocalLogGroup)]
    class AppClientLocalLogGroupNode : ServiceObjectKernelTreeNodeBase
    {
        public AppClientLocalLogGroupNode(AppClientContext clientContext, LogItemGroup group)
        {
            _clientContext = clientContext;
            _group = group;
            _clientId = clientContext.ClientDesc.ClientID;
        }

        private readonly AppClientContext _clientContext;
        private readonly LogItemGroup _group;
        private readonly Guid _clientId;

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            SystemInvoker invoker = _clientContext.MgrCtx.Invoker;
            LogItemGroup[] groups = invoker.Tray.GetLocalLogGroups(_clientId, parentGroup: _group.Name);
            LogItem[] logItems = invoker.Tray.GetLocalLogItems(_clientId, group: _group.Name);

            return groups.Select(g => (IServiceObjectTreeNode)new AppClientLocalLogGroupNode(_clientContext, g)).Union(
                logItems.Select(li => (IServiceObjectTreeNode)new AppClientLocalLogItemNode(_clientContext, _group, li))
            );
        }

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_group.Name);
        }

        protected override object GetKey()
        {
            return new Tuple<Guid, string>(_clientContext.ClientDesc.ClientID, _group.Name.ToLower());
        }

        [SoInfo("打开")]
        [ServiceObjectAction(ServiceObjectActionType.Default | ServiceObjectActionType.Open)]
        [UIObjectAction(typeof(OpenAction)), UIObjectImage("open"), ServiceObjectGroup("open")]
        public void Open()
        {

        }

        [SoInfo("在新窗口中打开")]
        [ServiceObjectAction(ServiceObjectActionType.AttachDefault | ServiceObjectActionType.OpenInNewWindow)]
        [UIObjectAction(typeof(OpenInNewWindowAction)), ServiceObjectGroup("open")]
        public void OpenInNewWindow()
        {

        }

        /// <summary>
        /// 时间
        /// </summary>
        [SoInfo("创建时间"), ServiceObjectProperty]
        public string Time
        {
            get { return _group.CreationTime.GetLocalTimeString(); }
        }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [SoInfo("最后修改时间"), ServiceObjectProperty]
        public string LastModifyTime
        {
            get { return _group.LastModifyTime.GetLocalTimeString(); }
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        [SoInfo("文件大小"), ServiceObjectProperty]
        public string Size
        {
            get
            {
                long size = _group.Size;
                if (size < 0)
                    return "未知";

                return StringUtility.GetSizeString(size);
            }
        }

        abstract class ActionBase : ActionBase<AppClientLocalLogGroupNode>
        {

        }
    }
}

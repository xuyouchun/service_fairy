using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Package.UIObject.Actions;
using ServiceFairy.Entities.Tray;

namespace ServiceFairy.Management.AppClients
{
    /// <summary>
    /// 系统日志组
    /// </summary>
    [SoInfo("系统日志组"), UIObjectImage(ResourceNames.AppClientSystemLogGroup)]
    class AppClientSystemLogGroupNode : ServiceObjectKernelTreeNodeBase
    {
        public AppClientSystemLogGroupNode(AppClientContext clientCtx, SystemLogGroup logGroup)
        {
            _clientCtx = clientCtx;
            _logGroup = logGroup;
        }

        private AppClientContext _clientCtx;
        private readonly SystemLogGroup _logGroup;
        private int _maxCount = 5000;

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            var reply = _clientCtx.MgrCtx.Invoker.Tray.GetSystemLogItems(_logGroup.Name, 0, _maxCount, _clientCtx.ClientDesc.ClientID);
            StringTable st = new StringTable(reply.StringTable);
            return reply.LogItems.Select(li => new AppClientSystemLogItemNode(_clientCtx, _logGroup, li.ToSystemLogItem(st)));
        }

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_logGroup.Name);
        }

        protected override object GetKey()
        {
            return new Tuple<Type, Guid, string>(GetType(), _clientCtx.ClientDesc.ClientID, _logGroup.Name);
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

        [SoInfo("标题"), ServiceObjectProperty]
        public string Title
        {
            get { return _logGroup.Title; }
        }

        [SoInfo("数量"), ServiceObjectProperty]
        public int Count
        {
            get { return _logGroup.Count; }
        }
    }
}

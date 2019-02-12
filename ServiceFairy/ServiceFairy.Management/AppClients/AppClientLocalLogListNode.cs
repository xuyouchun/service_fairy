using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Service;
using Common.Package.UIObject.Actions;
using Common.Contracts.Log;
using Common.Utility;

namespace ServiceFairy.Management.AppClients
{
    /// <summary>
    /// 本地日志
    /// </summary>
    [SoInfo("本地日志"), UIObjectImage(ResourceNames.AppClientLocalLogList)]
    class AppClientLocalLogListNode : ListServiceObjectKernelTreeNodeBase
    {
        public AppClientLocalLogListNode(AppClientContext clientContext)
        {
            _clientContext = clientContext;
            _clientId = clientContext.ClientDesc.ClientID;
        }

        private readonly AppClientContext _clientContext;
        private readonly Guid _clientId;

        protected override object GetKey()
        {
            return GetType();
        }

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            LogItemGroup[] groups = _clientContext.MgrCtx.Invoker.Tray.GetLocalLogGroups(_clientId);
            return groups.ToArray(g => new AppClientLocalLogGroupNode(_clientContext, g));
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

        [SoInfo("数量", Name = "Count"), ServiceObjectProperty]
        public string Count
        {
            get { return ""; }
        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return "该终端上记录的日志"; }
        }

        [SoInfo("详细"), ServiceObjectProperty]
        public string Detail
        {
            get { return ""; }
        }
    }
}

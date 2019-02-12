using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Package.UIObject.Actions;
using ServiceFairy.Entities.Tray;

namespace ServiceFairy.Management.AppClients
{
    /// <summary>
    /// 系统日志
    /// </summary>
    [SoInfo("系统日志"), UIObjectImage(ResourceNames.AppClientSystemLogList)]
    partial class AppClientSystemLogListNode : ListServiceObjectKernelTreeNodeBase
    {
        public AppClientSystemLogListNode(AppClientContext clientCtx)
        {
            _clientCtx = clientCtx;
        }

        private readonly AppClientContext _clientCtx;

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            SystemLogGroup[] groups = _clientCtx.MgrCtx.Invoker.Tray.GetSystemLogGroups(_clientCtx.ClientDesc.ClientID);
            return groups.Select(g => new AppClientSystemLogGroupNode(_clientCtx, g));
        }

        protected override object GetKey()
        {
            return GetType();
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
            get { return "该终端上记录的系统日志"; }
        }

        [SoInfo("详细"), ServiceObjectProperty]
        public string Detail
        {
            get { return ""; }
        }
    }
}

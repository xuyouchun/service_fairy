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
    /// 环境变量
    /// </summary>
    [SoInfo("环境变量"), UIObjectImage(ResourceNames.AppClientSystemEnvironmentVariableList)]
    class AppClientSystemEnvironmentVariableListNode : ServiceObjectKernelTreeNodeBase
    {
        public AppClientSystemEnvironmentVariableListNode(AppClientContext clientCtx)
        {
            _clientCtx = clientCtx;
        }

        private readonly AppClientContext _clientCtx;

        protected override object GetKey()
        {
            return new Tuple<Type, Guid>(GetType(), _clientCtx.ClientDesc.ClientID);
        }

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            SystemEnvironmentVariable[] variables = _clientCtx.MgrCtx.Invoker.Tray
                .GetAllSystemEnvironmentVariables(_clientCtx.ClientDesc.ClientID);

            return variables.Select(v => new AppClientSystemEnvironmentVariableNode(_clientCtx, v));
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
            get { return "该终端上的环境变量"; }
        }

        [SoInfo("详细"), ServiceObjectProperty]
        public string Detail
        {
            get { return ""; }
        }
    }
}

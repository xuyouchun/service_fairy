using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.Service;
using Common.Package.UIObject.Actions;

namespace ServiceFairy.Management.AppClients
{
    /// <summary>
    /// 调用列表的查看方式
    /// </summary>
    [SoInfo, UIObjectImage(ResourceNames.AppClientInvokeInfoWatchType)]
    class AppClientInvokeInfoWatchTypeNode : ServiceObjectKernelTreeNodeBase
    {
        public AppClientInvokeInfoWatchTypeNode(string title, IServiceObjectTreeNode[] nodes, string desc)
        {
            _title = title;
            _nodes = nodes;
            _desc = desc;
        }

        private readonly string _title, _desc;
        private readonly IServiceObjectTreeNode[] _nodes;

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

        [SoInfo("数量"), ServiceObjectProperty]
        public int Count
        {
            get { return _nodes.Length; }
        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string desc
        {
            get { return _desc; }
        }


        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            return _nodes;
        }

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_title);
        }

        private readonly Guid _key = Guid.NewGuid();
        protected override object GetKey()
        {
            return _key;
        }
    }
}

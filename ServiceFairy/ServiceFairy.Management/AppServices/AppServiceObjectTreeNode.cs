using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Contracts.UIObject;
using System.IO;
using Common.Package;
using Common.Package.Service;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities;
using Common.Package.UIObject;
using Common.Package.UIObject.Actions;
using Common.Utility;
using System.Drawing;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Management.AppServices
{
    /// <summary>
    /// AppService的节点
    /// </summary>
    class AppServiceObjectTreeNode : ServiceObjectKernelTreeNodeBase
    {
        public AppServiceObjectTreeNode(SfManagementContext mgrCtx, ServiceUIInfo uiInfo, ServiceObjectInfo info)
        {
            _mgrCtx = mgrCtx;
            _serviceDesc = uiInfo.ServiceDesc;
            _uiInfo = uiInfo;
            _serviceObjectInfo = info;
        }

        private IUIObjectImageLoader _CreateImageLoader()
        {
            return (IUIObjectImageLoader)UIObjectImageLoader.FromBytes(_uiInfo.Icon) ??
                AppServiceCategoryUtility.CreateUIObjectImageLoader(_uiInfo.Category);
        }

        protected override IUIObject OnCreateUIObject()
        {
            return new ServiceUIObject(_serviceObjectInfo, _CreateImageLoader());
        }

        protected override object GetKey()
        {
            return new Tuple<Type, ServiceDesc>(GetType(), _serviceDesc);
        }

        class Wrapper
        {
            public AppDomain AppDomain;
            public IServiceUI ServiceUI;
        }

        private readonly SfManagementContext _mgrCtx;
        private readonly ServiceDesc _serviceDesc;
        private readonly ServiceObjectInfo _serviceObjectInfo;
        private ServiceUIInfo _uiInfo;
        private Wrapper _w;

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            IServiceObjectTreeNode[] nodes = new IServiceObjectTreeNode[] {
                new AppServiceDeployListNode(_mgrCtx, _serviceDesc),
                new AppServiceConfigurationNode(_mgrCtx, _serviceDesc),
            };

            if (_w != null)
                _w.AppDomain.SafeUnload();

            _w = _LoadServiceUI();
            if (_w == null)
                return nodes;

            return nodes.Union(new Proxy(_w.ServiceUI.GetTree().Root, this));
        }

        private Wrapper _LoadServiceUI()
        {
            SystemInvoker invoker = _mgrCtx.Invoker;
            _uiInfo = invoker.Master.DownloadServiceUIInfo(_serviceDesc, true);  // 每次刷新都重新加载程序集
            if (_uiInfo.MainAssembly.IsNullOrEmpty())
                return null;

            AppDomain domain;
            IServiceUI serviceUI = AppDomainServiceLoader.Load<IServiceUI>(_uiInfo.MainAssembly, _uiInfo.AppConfig, out domain);
            return new Wrapper { AppDomain = domain, ServiceUI = serviceUI };
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
        
        [SoInfo("系统名称"), ServiceObjectProperty]
        public string SystemName
        {
            get { return _serviceDesc.Name; }
        }

        [SoInfo("版本号"), ServiceObjectProperty]
        public string Version
        {
            get { return _serviceDesc.Version.ToString(); }
        }

        [SoInfo("部署数量"), ServiceObjectProperty]
        public int Count
        {
            get
            {
                ServiceDeployInfo sdInfo = _mgrCtx.ServiceDeployInfos.Get(_serviceDesc);
                return (sdInfo == null || sdInfo.ClientIDs == null) ? 0 : sdInfo.ClientIDs.Length;
            }
        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return _serviceObjectInfo.Desc; }
        }

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return _serviceObjectInfo;
        }

        #region Class Proxy ...

        class Proxy : MarshalByRefObjectEx, IServiceObjectTreeNode
        {
            public Proxy(IServiceObjectTreeNode treeNode, AppServiceObjectTreeNode owner)
            {
                _treeNode = treeNode;
                _owner = owner;
                ServiceObject so = Common.Package.Service.ServiceObject.FromObject(owner);
                _so = Common.Package.Service.ServiceObject.Combine(new IServiceObject[] {
                    treeNode.ServiceObject, so
                }, so.Info);
            }

            private readonly IServiceObjectTreeNode _treeNode;
            private readonly AppServiceObjectTreeNode _owner;
            private readonly IServiceObject _so;

            public IServiceObject ServiceObject
            {
                get { return _so; }
            }

            public int Count
            {
                get { return _treeNode.Count; }
            }

            public IServiceObjectTreeNode this[int index]
            {
                get { return _treeNode[index]; }
            }

            public bool? HasChildren
            {
                get { return _treeNode.HasChildren; }
            }

            public IServiceObjectTreeNode Parent
            {
                get { return _owner.Parent; }
            }

            public IEnumerator<IServiceObjectTreeNode> GetEnumerator()
            {
                return _treeNode.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return ((System.Collections.IEnumerable)_treeNode).GetEnumerator();
            }
        }

        #endregion
    }
}

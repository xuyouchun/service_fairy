using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Package.UIObject;
using ServiceFairy.Management.AppClients;
using Common.Utility;
using ServiceFairy.Entities.Sys;
using Common.Package.UIObject.Actions;

namespace ServiceFairy.Management.AppComponents
{
    [SoInfo("组件"), UIObjectImage(ResourceNames.AppComponent)]
    class AppComponentNode : ServiceObjectKernelTreeNodeBase
    {
        public AppComponentNode(AppClientContext clientCtx, ServiceDesc serviceDesc, AppComponentInfo componentInfo)
        {
            _clientCtx = clientCtx;
            _serviceDesc = serviceDesc;
            _componentInfo = componentInfo;

            _properties = new Lazy<PropertyWrapper>(_LoadPropertyWrapper);
        }

        private AppClientContext _clientCtx;
        private ServiceDesc _serviceDesc;
        private AppComponentInfo _componentInfo;

        class PropertyWrapper
        {
            public ObjectProperty[] Properties;
            public Dictionary<string, ObjectProperty> Dict;
        }

        private readonly Lazy<PropertyWrapper> _properties;

        private PropertyWrapper _LoadPropertyWrapper()
        {
            ObjectProperty[] properties = _clientCtx.MgrCtx.Invoker.Sys
                .GetAppComponentProperties(_clientCtx.ClientDesc.ClientID, _serviceDesc, _componentInfo.Name);

            return new PropertyWrapper { Properties = properties, Dict = properties.ToDictionary(p => p.Name, true) };
        }

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            PropertyWrapper w = _properties.Value;
            AppComponentPropertyValue[] values = _clientCtx.MgrCtx.Invoker.Sys
                .GetAppComponentPropertyValues(_clientCtx.ClientDesc.ClientID, _serviceDesc, _componentInfo.Name);

            var dict = values.ToDictionary(v => v.Name, true);
            return w.Properties.Select(p => new AppComponentPropertyNode(_clientCtx, _serviceDesc, _componentInfo, p, dict.GetOrDefault(p.Name)));
        }

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_componentInfo.Name);
        }

        protected override object GetKey()
        {
            return new Tuple<Type, ServiceDesc, string>(GetType(), _serviceDesc, _componentInfo.Name);
        }

        [SoInfo("打开")]
        [ServiceObjectAction(ServiceObjectActionType.Default | ServiceObjectActionType.Open)]
        [UIObjectAction(typeof(OpenAction)), UIObjectImage("open"), ServiceObjectGroup("open")]
        public void Open() { }

        [SoInfo("在新窗口中打开")]
        [ServiceObjectAction(ServiceObjectActionType.AttachDefault | ServiceObjectActionType.OpenInNewWindow)]
        [UIObjectAction(typeof(OpenInNewWindowAction)), ServiceObjectGroup("open")]
        public void OpenInNewWindow() { }

        [SoInfo("标题"), ServiceObjectProperty]
        public string Title
        {
            get { return _componentInfo.Title; }
        }

        [SoInfo("类型"), ServiceObjectProperty]
        public string Type
        {
            get { return _componentInfo.Category.GetDesc(); }
        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return _componentInfo.Desc; }
        }
    }
}

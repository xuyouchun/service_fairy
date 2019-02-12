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
using Common.Contracts.UIObject;
using Common.WinForm;
using Common.Package.UIObject.Actions;

namespace ServiceFairy.Management.AppComponents
{
    /// <summary>
    /// 组件属性
    /// </summary>
    [SoInfo("组件属性"), UIObjectImage(ResourceNames.AppComponentProperty)]
    class AppComponentPropertyNode : ServiceObjectKernelTreeNodeBase
    {
        public AppComponentPropertyNode(AppClientContext clientCtx, ServiceDesc serviceDesc,
            AppComponentInfo componentInfo, ObjectProperty property, AppComponentPropertyValue value)
        {
            _clientCtx = clientCtx;
            _serviceDesc = serviceDesc;
            _componentInfo = componentInfo;
            _property = property;
            _value = value;
        }

        private readonly AppClientContext _clientCtx;
        private readonly ServiceDesc _serviceDesc;
        private readonly ObjectProperty _property;
        private readonly AppComponentInfo _componentInfo;
        private readonly AppComponentPropertyValue _value;

        protected override ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_property.Name);
        }

        protected override object GetKey()
        {
            return new Tuple<Type, string>(GetType(), _property.Name);
        }

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            
        }

        /// <summary>
        /// 打开
        /// </summary>
        [SoInfo("打开")]
        [ServiceObjectAction(ServiceObjectActionType.Default)]
        [UIObjectAction(typeof(OpenAction)), UIObjectImage("open"), ServiceObjectGroup("open")]
        public void Open() { }

        /// <summary>
        /// 查看
        /// </summary>
        [SoInfo("查看 ..")]
        [ServiceObjectAction(ServiceObjectActionType.AttachDefault)]
        [UIObjectAction(typeof(ViewAction))]
        public void View() { }

        class ViewAction : ActionBase
        {
            public override void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                IUIOperation uiOp = context.ServiceProvider.GetService<IUIOperation>(true);
                uiOp.ShowText(Kernel.Value, Kernel.Title);
            }
        }

        [SoInfo("标题"), ServiceObjectProperty]
        public string Title
        {
            get { return _property.Title; }
        }

        [SoInfo("值"), ServiceObjectProperty]
        public string Value
        {
            get { return _value == null ? "<null>" : _value.Value.ToStringIgnoreNull("<null>"); }
        }

        [SoInfo("类型"), ServiceObjectProperty]
        public string Type
        {
            get { return _property.Type.GetDesc(); }
        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return _property.Desc; }
        }

        abstract class ActionBase : ActionBase<AppComponentPropertyNode>
        {

        }
    }
}

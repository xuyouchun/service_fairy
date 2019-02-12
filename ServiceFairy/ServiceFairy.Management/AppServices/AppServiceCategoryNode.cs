using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Package.UIObject;
using Common.Contracts.UIObject;
using Common.Package.UIObject.Actions;
using Common.Utility;

namespace ServiceFairy.Management.AppServices
{
    /// <summary>
    /// AppService的类型核心类
    /// </summary>
    class AppServiceCategoryNode : ServiceObjectKernelTreeNodeBase, IServiceObjectInfoProvider
    {
        public AppServiceCategoryNode(SfManagementContext mgrCtx, AppServiceCategory category, IServiceObjectTreeNode[] subNodes)
        {
            _mgrCtx = mgrCtx;
            _category = category;
            _subNodes = subNodes;
        }

        private readonly SfManagementContext _mgrCtx;
        private readonly AppServiceCategory _category;
        private readonly IServiceObjectTreeNode[] _subNodes;

        protected override IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            return _subNodes;
        }

        [SoInfo("打开")]
        [ServiceObjectAction(ServiceObjectActionType.Default | ServiceObjectActionType.Open)]
        [UIObjectAction(typeof(OpenAction)), UIObjectImage("open")]
        public void Open()
        {
            
        }

        [SoInfo("在新窗口中打开")]
        [ServiceObjectAction(ServiceObjectActionType.AttachDefault | ServiceObjectActionType.OpenInNewWindow)]
        [UIObjectAction(typeof(OpenInNewWindowAction))]
        public void OpenInNewWindow()
        {

        }

        [SoInfo("描述"), ServiceObjectProperty]
        public string Desc
        {
            get { return AppServiceCategoryUtility.GetDesc(_category); }
        }

        public ServiceObjectInfo GetServiceObjectInfo()
        {
            return ServiceObjectInfo.OfTitle(_category.GetDesc());
        }

        protected override object GetKey()
        {
            return new Tuple<Type, AppServiceCategory>(GetType(), _category);
        }
    }
}

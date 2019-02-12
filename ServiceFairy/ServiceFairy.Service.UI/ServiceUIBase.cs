using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Contracts.UIObject;
using System.Diagnostics.Contracts;
using Common.Package;
using Common.Package.Service;
using Common.Utility;

namespace ServiceFairy.Service.UI
{
    /// <summary>
    /// ServiceUI的基类
    /// </summary>
    public abstract class ServiceUIBase : ServiceObjectKernelTreeNodeBase, IServiceUI
    {
        public ServiceUIBase()
        {
            _serviceDesc = GetServiceDesc();
            Contract.Assert(_serviceDesc != null);
        }

        protected virtual ServiceDesc GetServiceDesc()
        {
            return ServiceUIAttribute.GetFromType(GetType());
        }

        private readonly ServiceDesc _serviceDesc;

        protected virtual IServiceObjectTreeNode[] OnGetNodes()
        {
            return new IServiceObjectTreeNode[0];
        }

        /// <summary>
        /// 获取ServiceObjectProvider
        /// </summary>
        /// <returns></returns>
        public virtual IServiceObjectTree GetTree()
        {
            List<IServiceObjectTreeNode> nodes = new List<IServiceObjectTreeNode>();
            nodes.AddRange(new IServiceObjectTreeNode[] {
                new AppCommandListKernelNode(_serviceDesc),
            });

            nodes.AddRange(OnGetNodes().WhereNotNull());
            IServiceObjectTreeNode root = new ServiceObjectTreeNode(ServiceObject.FromObject(new RootKernel()), nodes.ToArray());
            return new ServiceObjectTree(root);
        }

        protected override object GetKey()
        {
            return new Tuple<Type, ServiceDesc>(GetType(), _serviceDesc);
        }
    }
}

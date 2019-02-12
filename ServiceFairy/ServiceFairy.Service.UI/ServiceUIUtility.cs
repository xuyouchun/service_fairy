using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Package.Service;
using Common.Utility;
using ServiceFairy.Entities;
using System.IO;
using ServiceFairy.Entities.Master;

namespace ServiceFairy.Service.UI
{
    public static class ServiceUIUtility
    {
        public static ServiceObjectTreeNode CreateTreeNode(object kernel, IEnumerable<IServiceObjectTreeNode> subNodes = null)
        {
            Contract.Requires(kernel != null);

            return new ServiceObjectTreeNode(ServiceObject.FromObject(kernel), subNodes);
        }

        public static IServiceObjectTreeNode[] CreateTreeNodes(IEnumerable<object> kernels)
        {
            if (kernels == null)
                return new ServiceObjectTreeNode[0];

            return kernels.ToArray(kernel => (kernel is IServiceObjectTreeNode) ? (IServiceObjectTreeNode)kernel : CreateTreeNode(kernel));
        }

        public static IServiceObjectTreeNode CreateTreeNode(object kernel, IEnumerable<object> subNodes)
        {
            Contract.Requires(kernel != null);

            return CreateTreeNode(kernel, CreateTreeNodes(subNodes));
        }
    }
}

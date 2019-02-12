using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Package.Service;
using Common.Utility;
using Common.Contracts;
using System.Collections;
using System.Threading;
using Common.Collection;

namespace Common.Package.Service
{
    /// <summary>
    /// 支持列表的ObjectService树节点基类
    /// </summary>
    public abstract class ListServiceObjectKernelTreeNodeBase : ServiceObjectKernelTreeNodeBase
    {
        private static readonly Dictionary<object, Dictionary<object, IServiceObjectTreeNode>> _refreshableNodes
            = new Dictionary<object, Dictionary<object, IServiceObjectTreeNode>>();

        static ListServiceObjectKernelTreeNodeBase()
        {
            GlobalTimer<ITask>.Default.Add(TimeSpan.FromSeconds(60), new TaskFuncAdapter(delegate {
                lock (_refreshableNodes)
                {
                    _refreshableNodes.RemoveWhere(v => v.Value.Count == 0);
                }
            }));
        }

        private static object _GetKey(IServiceObjectTreeNode node)
        {
            IObjectKeyProvider op = node as IObjectKeyProvider
                ?? node.ServiceObject as IObjectKeyProvider
                ?? node.ServiceObject.Get<IObjectKeyProvider>();

            if (op != null)
                return op.GetKey();

            throw new InvalidOperationException("无法获取节点" + node + "的键");
        }

        /// <summary>
        /// 添加一个节点
        /// </summary>
        /// <param name="node"></param>
        public void Add(ServiceObjectKernelTreeNodeBase node, object state = null)
        {
            Contract.Requires(node != null);

            lock (_refreshableNodes)
            {
                object key = _GetKey(node);
                _refreshableNodes.GetOrSet(_GetKey(this))[key] = node;
                node.BeginInit(_InitServiceObjectTreeNodeCallback, state);
            }
        }

        /// <summary>
        /// 批量添加节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="state"></param>
        public void AddRange(IEnumerable<ServiceObjectKernelTreeNodeBase> nodes, object state = null)
        {
            Contract.Requires(nodes != null);

            foreach (ServiceObjectKernelTreeNodeBase node in nodes)
            {
                Add(node, state);
            }
        }

        public void _InitServiceObjectTreeNodeCallback(IAsyncResult ar)
        {
            ServiceObjectKernelTreeNodeBase node = (ServiceObjectKernelTreeNodeBase)ar.AsyncState;

            try
            {
                node.EndInit(ar);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
            }
            finally
            {
                Remove(_GetKey(node));
            }
        }

        #region Class ServiceObjectRefresherDecorator ...

        class ServiceObjectRefresherDecorator : IServiceObjectRefresher
        {
            public ServiceObjectRefresherDecorator(ServiceObjectKernelTreeNodeBase node, IServiceObjectRefresher refresher, ListServiceObjectKernelTreeNodeBase owner)
            {
                _node = node;
                _refresher = refresher;
                _owner = owner;
            }

            private readonly ServiceObjectKernelTreeNodeBase _node;
            private readonly IServiceObjectRefresher _refresher;
            private readonly ListServiceObjectKernelTreeNodeBase _owner;

            public ServiceObjectRefreshResult Refresh()
            {
                ServiceObjectRefreshResult result = _refresher.Refresh();
                if ((result & ServiceObjectRefreshResult.Completed) != 0)
                    _refreshableNodes.GetOrSet(_GetKey(_owner)).Remove(_GetKey(_node));

                return result;
            }
        }

        #endregion

        /// <summary>
        /// 删除一个节点
        /// </summary>
        /// <param name="key"></param>
        public void Remove(object key)
        {
            Contract.Requires(key != null);

            lock (_refreshableNodes)
            {
                _refreshableNodes.GetOrSet(_GetKey(this)).Remove(key);
            }
        }

        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <returns></returns>
        internal protected override IEnumerable<IServiceObjectTreeNode> LoadServiceObjectSubTreeNodes()
        {
            return _CreateServiceObjectSubTreeNodes().Select(SetParent);
        }

        private IEnumerable<IServiceObjectTreeNode> _CreateServiceObjectSubTreeNodes()
        {
            IEnumerable<IServiceObjectTreeNode> nodes = OnCreateServiceObjectSubTreeNodes() ?? Array<IServiceObjectTreeNode>.Empty;
            Dictionary<object, IServiceObjectTreeNode> dict = nodes.ToDictionary(node => _GetKey(node));

            lock (_refreshableNodes)
            {
                return _refreshableNodes.GetOrSet(_GetKey(this)).Values.Union(nodes).ToArray();
            }
        }
    }
}

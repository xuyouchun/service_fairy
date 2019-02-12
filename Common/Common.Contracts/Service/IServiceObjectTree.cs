using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Collection;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 服务对象的树型结构
    /// </summary>
    public interface IServiceObjectTree
    {
        /// <summary>
        /// 根节点
        /// </summary>
        IServiceObjectTreeNode Root { get; }
    }

    /// <summary>
    /// 服务对象的树型结构
    /// </summary>
    public class ServiceObjectTree : MarshalByRefObjectEx, IServiceObjectTree
    {
        public ServiceObjectTree(IServiceObjectTreeNode root)
        {
            Contract.Requires(root != null);

            Root = root;
        }

        /// <summary>
        /// 根节点
        /// </summary>
        public IServiceObjectTreeNode Root { get; private set; }
    }

    /// <summary>
    /// 服务对象的树节点
    /// </summary>
    public interface IServiceObjectTreeNode : IReadOnlyList<IServiceObjectTreeNode>, IEnumerable<IServiceObjectTreeNode>
    {
        /// <summary>
        /// 服务对象
        /// </summary>
        IServiceObject ServiceObject { get; }

        /// <summary>
        /// 是否具有子节点
        /// </summary>
        bool? HasChildren { get; }

        /// <summary>
        /// 父节点
        /// </summary>
        IServiceObjectTreeNode Parent { get; }
    }

    /// <summary>
    /// 支持树节点的刷新操作
    /// </summary>
    public interface IRefreshableServiceObjectTreeNode : IServiceObjectTreeNode
    {
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="force"></param>
        void Refresh(bool force);
    }

    /// <summary>
    /// 树节点
    /// </summary>
    public class ServiceObjectTreeNode : MarshalByRefObjectEx, IServiceObjectTreeNode
    {
        public ServiceObjectTreeNode(IServiceObject serviceObject, IEnumerable<IServiceObjectTreeNode> subNodes = null, bool? hasChildren = null, IServiceObjectTreeNode parent = null)
        {
            Contract.Requires(serviceObject != null);

            _serviceObject = serviceObject;
            _subNodes = new Lazy<IList<IServiceObjectTreeNode>>(delegate {
                if (subNodes == null)
                    return new IServiceObjectTreeNode[0];
                IList<IServiceObjectTreeNode> nodes = subNodes as IList<IServiceObjectTreeNode>;
                return nodes != null ? nodes : subNodes.ToArray();
            });

            _hasChildren = hasChildren;
            serviceObject.SetTreeNode(this);
            _parent = parent;
        }

        private readonly IServiceObject _serviceObject;
        private readonly IServiceObjectTreeNode _parent;
        private readonly Lazy<IList<IServiceObjectTreeNode>> _subNodes;
        private bool? _hasChildren;

        public IServiceObject ServiceObject
        {
            get { return _serviceObject; }
        }

        public int Count
        {
            get { return _subNodes.Value.Count; }
        }

        public IServiceObjectTreeNode this[int index]
        {
            get { return _subNodes.Value[index]; }
        }

        public bool? HasChildren
        {
            get
            {
                return _hasChildren;
            }
        }

        public IServiceObjectTreeNode Parent
        {
            get { return _parent; }
        }

        public IEnumerator<IServiceObjectTreeNode> GetEnumerator()
        {
            return _subNodes.Value.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ServiceObjectTreeCollection : MarshalByRefObjectEx, IServiceObjectTree
    {
        public ServiceObjectTreeCollection(IServiceObject rootSo, IServiceObjectTree[] trees, IServiceObjectTreeNode parent)
        {
            Contract.Requires(rootSo != null && trees != null);

            _root = new ServiceObjectTreeNode(rootSo, trees, parent);
        }

        private readonly IServiceObjectTreeNode _root;

        public IServiceObjectTreeNode Root
        {
            get { return _root; }
        }

        #region Class ServiceObjectTreeNode ...

        class ServiceObjectTreeNode : MarshalByRefObjectEx, IServiceObjectTreeNode
        {
            public ServiceObjectTreeNode(IServiceObject serviceObject, IServiceObjectTree[] trees, IServiceObjectTreeNode parent)
            {
                _serviceObject = serviceObject;
                _trees = trees;
                _parent = parent;
            }

            private readonly IServiceObject _serviceObject;
            private readonly IServiceObjectTree[] _trees;
            private readonly IServiceObjectTreeNode _parent;

            public IServiceObject ServiceObject
            {
                get { return _serviceObject; }
            }

            public int Count
            {
                get { return _trees.Length; }
            }

            public bool? HasChildren
            {
                get { return Count > 0; }
            }

            public IServiceObjectTreeNode this[int index]
            {
                get { return _trees[index].Root; }
            }

            public IServiceObjectTreeNode Parent
            {
                get { return _parent; }
            }

            public IEnumerator<IServiceObjectTreeNode> GetEnumerator()
            {
                foreach (IServiceObjectTree tree in _trees)
                {
                    yield return tree.Root;
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;

namespace Common.Collection
{
    /// <summary>
    /// 树
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TNodeCollection"></typeparam>
    public class Tree<TKey, TValue, TNodeCollection>
        where TNodeCollection : class, ITreeNodeCollection<TKey, TValue, TNodeCollection>
    {
        public Tree(Func<Tree<TKey, TValue, TNodeCollection>, TreeNode<TKey, TValue, TNodeCollection>, TNodeCollection> collectionCreator)
        {
            Contract.Requires(collectionCreator != null);

            _nodeCollectionCreator = collectionCreator;
            Root = new TreeNode<TKey, TValue, TNodeCollection>(this, null);
        }

        /// <summary>
        /// 节点集合的创建器
        /// </summary>
        private Func<Tree<TKey, TValue, TNodeCollection>, TreeNode<TKey, TValue, TNodeCollection>, TNodeCollection> _nodeCollectionCreator;

        internal TNodeCollection CreateNodeCollection(TreeNode<TKey, TValue, TNodeCollection> parent)
        {
            return _nodeCollectionCreator(this, parent);
        }

        /// <summary>
        /// 根节点
        /// </summary>
        public TreeNode<TKey, TValue, TNodeCollection> Root { get; private set; }
    }

    /// <summary>
    /// 树节点
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TNodeCollection"></typeparam>
    public class TreeNode<TKey, TValue, TNodeCollection>
        where TNodeCollection : class, ITreeNodeCollection<TKey, TValue, TNodeCollection>
    {
        public TreeNode(Tree<TKey, TValue, TNodeCollection> tree, TreeNode<TKey, TValue, TNodeCollection> parent)
        {
            Contract.Requires(tree != null);

            Parent = parent;
            Tree = tree;
        }

        /// <summary>
        /// 节点的值
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// 树
        /// </summary>
        public Tree<TKey, TValue, TNodeCollection> Tree { get; private set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public TreeNode<TKey, TValue, TNodeCollection> Parent { get; private set; }

        private volatile TNodeCollection _nodeCollection;

        /// <summary>
        /// 子节点
        /// </summary>
        public TNodeCollection Nodes
        {
            get
            {
                if (_nodeCollection != null)
                    return _nodeCollection;

                lock (this)
                {
                    return _nodeCollection ?? (_nodeCollection = Tree.CreateNodeCollection(this));
                }
            }
        }

        /// <summary>
        /// 是否包含子节点
        /// </summary>
        /// <returns></returns>
        public bool HasChildren()
        {
            return _nodeCollection != null && _nodeCollection.Count > 0;
        }
    }

    /// <summary>
    /// 树节点的集合
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface ITreeNodeCollection<TKey, TValue, TNodeCollection>
        : IEnumerable<KeyValuePair<TKey, TreeNode<TKey, TValue, TNodeCollection>>>
        where TNodeCollection : class, ITreeNodeCollection<TKey, TValue, TNodeCollection>
    {
        /// <summary>
        /// 数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TreeNode<TKey, TValue, TNodeCollection> this[TKey key] { get; }
    }

    /// <summary>
    /// 基于哈希表的节点集合实现
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public sealed class DictionaryTreeNodeCollection<TKey, TValue> : ITreeNodeCollection<TKey, TValue, DictionaryTreeNodeCollection<TKey, TValue>>
    {
        public DictionaryTreeNodeCollection(Tree<TKey, TValue, DictionaryTreeNodeCollection<TKey, TValue>> tree,
            TreeNode<TKey, TValue, DictionaryTreeNodeCollection<TKey, TValue>> parent,
            IDictionary<TKey, TreeNode<TKey, TValue, DictionaryTreeNodeCollection<TKey, TValue>>> dict = null)
        {
            Contract.Requires(tree != null && parent != null);

            Tree = tree;
            Parent = parent;
            _dict = dict ?? _CreateDictionary();
        }

        private IDictionary<TKey, TreeNode<TKey, TValue, DictionaryTreeNodeCollection<TKey, TValue>>> _CreateDictionary()
        {
            return new Dictionary<TKey, TreeNode<TKey, TValue, DictionaryTreeNodeCollection<TKey, TValue>>>();
        }

        private readonly IDictionary<TKey, TreeNode<TKey, TValue, DictionaryTreeNodeCollection<TKey, TValue>>> _dict;

        /// <summary>
        /// 数量
        /// </summary>
        public int Count
        {
            get { return _dict.Count; }
        }

        /// <summary>
        /// 树
        /// </summary>
        public Tree<TKey, TValue, DictionaryTreeNodeCollection<TKey, TValue>> Tree { get; private set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public TreeNode<TKey, TValue, DictionaryTreeNodeCollection<TKey, TValue>> Parent { get; private set; }

        /// <summary>
        /// 按索引读取或设置节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TreeNode<TKey, TValue, DictionaryTreeNodeCollection<TKey, TValue>> this[TKey key]
        {
            get
            {
                Contract.Requires(key != null);

                TreeNode<TKey, TValue, DictionaryTreeNodeCollection<TKey, TValue>> node;
                _dict.TryGetValue(key, out node);
                return node;
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            _dict.Clear();
        }

        /// <summary>
        /// 添加指定的键值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public TreeNode<TKey, TValue, DictionaryTreeNodeCollection<TKey, TValue>> Add(TKey key, TValue value = default(TValue))
        {
            Contract.Requires(key != null);

            return _dict[key] = new TreeNode<TKey, TValue, DictionaryTreeNodeCollection<TKey, TValue>>(Tree, Parent) { Value = value };
        }

        /// <summary>
        /// 删除指定的Key
        /// </summary>
        /// <param name="key"></param>
        public bool Remove(TKey key)
        {
            Contract.Requires(key != null);

            return _dict.Remove(key);
        }

        /// <summary>
        /// 获取哈希表
        /// </summary>
        /// <returns></returns>
        public IDictionary<TKey, TreeNode<TKey, TValue, DictionaryTreeNodeCollection<TKey, TValue>>> GetDictionary()
        {
            return _dict;
        }

        public IEnumerator<KeyValuePair<TKey, TreeNode<TKey, TValue, DictionaryTreeNodeCollection<TKey, TValue>>>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// 基于动态数组的节点集合的实现
    /// </summary>
    /// <typeparam name="?"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public sealed class ListTreeNodeCollection<TValue> : ITreeNodeCollection<int, TValue, ListTreeNodeCollection<TValue>>
    {
        public ListTreeNodeCollection(Tree<int, TValue, ListTreeNodeCollection<TValue>> tree,
            TreeNode<int, TValue, ListTreeNodeCollection<TValue>> parent)
        {
            Contract.Requires(tree != null && parent != null);

            Tree = tree;
            Parent = parent;
        }

        private readonly List<TreeNode<int, TValue, ListTreeNodeCollection<TValue>>> _list = new List<TreeNode<int, TValue, ListTreeNodeCollection<TValue>>>();

        /// <summary>
        /// 数量
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// 树
        /// </summary>
        public Tree<int, TValue, ListTreeNodeCollection<TValue>> Tree { get; private set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public TreeNode<int, TValue, ListTreeNodeCollection<TValue>> Parent { get; private set; }

        /// <summary>
        /// 根据索引读取节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TreeNode<int, TValue, ListTreeNodeCollection<TValue>> this[int key]
        {
            get { return _list[key]; }
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetValue(int key, TValue value)
        {
            _list[key].Value = value;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue GetValue(int key)
        {
            return _list[key].Value;
        }

        /// <summary>
        /// 添加值
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>新值索引</returns>
        public int Add(TValue value)
        {
            _list.Add(new TreeNode<int, TValue, ListTreeNodeCollection<TValue>>(Tree, Parent) { Value = value });
            return _list.Count - 1;
        }

        /// <summary>
        /// 插入值
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Insert(int index, TValue value)
        {
            _list.Insert(index, new TreeNode<int, TValue, ListTreeNodeCollection<TValue>>(Tree, Parent));
        }

        /// <summary>
        /// 删除指定索引处的值
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        /// <summary>
        /// 清空所有的值
        /// </summary>
        public void Clear()
        {
            _list.Clear();
        }

        public IEnumerator<KeyValuePair<int, TreeNode<int, TValue, ListTreeNodeCollection<TValue>>>> GetEnumerator()
        {
            for (int k = 0, length = _list.Count; k < length; k++)
            {
                yield return new KeyValuePair<int, TreeNode<int, TValue, ListTreeNodeCollection<TValue>>>(k, _list[k]);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

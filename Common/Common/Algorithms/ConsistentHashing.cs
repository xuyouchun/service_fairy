using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Collection;

namespace Common.Algorithms
{
    /// <summary>
    /// 一致性哈希算法
    /// </summary>
    public class ConsistentHashing<TNode> where TNode : IConsistentNode
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ConsistentHashing()
        {
            
        }

        class Wrapper
        {
            public KeyValuePair<int, SortedSet<TNode>>[] NodeDict { get; set; }

            public TNode[] Nodes { get; set; }
        }

        private volatile Wrapper _w = new Wrapper() {
            NodeDict = new KeyValuePair<int, SortedSet<TNode>>[0], Nodes = new TNode[0]
        };

        /// <summary>
        /// 获取所有节点
        /// </summary>
        /// <returns></returns>
        public TNode[] GetAllNodes()
        {
            return _w.Nodes;
        }

        /// <summary>
        /// 获取节点的总数
        /// </summary>
        public int NodeCount
        {
            get { return _w.Nodes.Length; }
        }

        /// <summary>
        /// 添加一个节点
        /// </summary>
        /// <param name="node"></param>
        public void AddNode(TNode node)
        {
            Contract.Requires(node != null);
            AddNodes(new[] { node });
        }

        /// <summary>
        /// 批量添加节点
        /// </summary>
        /// <param name="nodes"></param>
        public void AddNodes(TNode[] nodes)
        {
            Contract.Requires(nodes != null);
            if (nodes.Length == 0)
                return;

            _AddNodes(nodes, _w.NodeDict.ToDictionary(v => v.Key, v => v.Value));
        }

        private void _AddNodes(TNode[] nodes, IDictionary<int, SortedSet<TNode>> baseOf)
        {
            Dictionary<int, SortedSet<TNode>> dict = new Dictionary<int, SortedSet<TNode>>(baseOf);
            foreach (TNode node in nodes)
            {
                foreach (int vNodeHc in node.GetVirtualNodeHashCodes())
                {
                    dict.GetOrSet(vNodeHc).Add(node);
                }
            }

            _w = _ToWrapper(dict);
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="node"></param>
        public void RemoveNode(TNode node)
        {
            Contract.Requires(node != null);
            RemoveNodes(new[] { node });
        }

        /// <summary>
        /// 批量删除节点
        /// </summary>
        /// <param name="nodes"></param>
        public void RemoveNodes(TNode[] nodes)
        {
            Contract.Requires(nodes != null);
            if (nodes.Length == 0)
                return;

            Dictionary<int, SortedSet<TNode>> dict = new Dictionary<int, SortedSet<TNode>>();
            foreach (TNode node in nodes)
            {
                foreach (KeyValuePair<int, SortedSet<TNode>> item in _w.NodeDict)
                {
                    foreach (TNode node0 in item.Value)
                    {
                        if (!object.Equals(node0, node))
                        {
                            dict.GetOrSet(item.Key).Add(node0);
                        }
                    }
                }
            }

            _w = _ToWrapper(dict);
        }

        /// <summary>
        /// 用指定的节点替换
        /// </summary>
        /// <param name="nodes"></param>
        public void InsteadOf(TNode[] nodes)
        {
            Contract.Requires(nodes != null);

            _AddNodes(nodes, new Dictionary<int, SortedSet<TNode>>());
        }

        private static Wrapper _ToWrapper(Dictionary<int, SortedSet<TNode>> dict)
        {
            return new Wrapper() {
                NodeDict = dict.OrderBy(v => v.Key).ToArray(),
                Nodes = dict.SelectMany(v => v.Value).Distinct().ToArray()
            };
        }

        /// <summary>
        /// 根据哈希码获取节点
        /// </summary>
        /// <param name="hashcode"></param>
        /// <returns></returns>
        public TNode GetNode(int hashcode)
        {
            KeyValuePair<int, SortedSet<TNode>>[] nodes = _w.NodeDict;
            if (nodes.Length == 0)
                throw new InvalidOperationException("尚未添加节点");

            if (nodes.Length == 1)
                return nodes[0].Value.Min;

            return _BinarySearch(nodes, hashcode);
        }

        private static TNode _BinarySearch(KeyValuePair<int, SortedSet<TNode>>[] nodes, int hashcode)
        {
            int begin = 0, end = nodes.Length - 1;
            while (begin <= end)
            {
                int mid = (begin + end) / 2;
                KeyValuePair<int, SortedSet<TNode>> node = nodes[mid];

                if (mid >= nodes.Length - 1)
                    break;

                if (hashcode > node.Key && hashcode <= nodes[mid + 1].Key)
                {
                    return node.Value.Min;
                }
                else if (hashcode < node.Key)
                {
                    end = mid - 1;
                }
                else
                {
                    begin = mid + 1;
                }
            }

            return nodes[0].Value.Min;
        }
    }

    /// <summary>
    /// 一致性哈希算法的节点
    /// </summary>
    public interface IConsistentNode : IComparable<IConsistentNode>
    {
        /// <summary>
        /// 获取虚拟节点的啥希值
        /// </summary>
        /// <returns></returns>
        int[] GetVirtualNodeHashCodes();
    }
}

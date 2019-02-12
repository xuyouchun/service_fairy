using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Collection;
using System.Diagnostics.Contracts;
using Common.Package.Cache.CacheExpireDependencies;
using Common.Utility;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Cache;
using System.Threading;
using Common.Package.Serializer;
using Common.Contracts.Service;
using System.Collections.Concurrent;

using CacheTree = Common.Collection.Tree<
    System.String,
    ServiceFairy.Service.Cache.Components.CacheEntity,
    Common.Collection.DictionaryTreeNodeCollection<System.String, ServiceFairy.Service.Cache.Components.CacheEntity>
>;

using CacheTreeNode = Common.Collection.TreeNode<
    System.String,
    ServiceFairy.Service.Cache.Components.CacheEntity,
    Common.Collection.DictionaryTreeNodeCollection<System.String, ServiceFairy.Service.Cache.Components.CacheEntity>
>;

using CacheTreeNodeCollection = Common.Collection.DictionaryTreeNodeCollection<
    System.String,
    ServiceFairy.Service.Cache.Components.CacheEntity
>;

using CacheTreeNodeDictionary = Common.Collection.LargeDictionary<
    System.String,
    Common.Collection.TreeNode<
        System.String,
        ServiceFairy.Service.Cache.Components.CacheEntity,
        Common.Collection.DictionaryTreeNodeCollection<System.String, ServiceFairy.Service.Cache.Components.CacheEntity>
    >
>;

using CacheTreeNodeDictionaryThreadSafeWrapper = Common.Collection.ThreadSafeDictionaryWrapper<
    System.String,
    Common.Collection.TreeNode<
        System.String,
        ServiceFairy.Service.Cache.Components.CacheEntity,
        Common.Collection.DictionaryTreeNodeCollection<System.String, ServiceFairy.Service.Cache.Components.CacheEntity>
    >
>;

namespace ServiceFairy.Service.Cache.Components
{
    /// <summary>
    /// 缓存管理器
    /// </summary>
    [AppComponent("缓存管理器", "存储缓存键值对，检查并删除过期缓存")]
    class CacheManagerAppComponent : TimerAppComponentBase
    {
        public CacheManagerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            
        }

        protected override void OnExecuteTask(string taskName)
        {
            _CheckTimeExpired(_cacheTree.Root, DateTime.UtcNow);
            GC.Collect();
        }

        /// <summary>
        /// 批量获取指定路径的缓存值
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="remove"></param>
        /// <returns></returns>
        public CacheEntity[] Get(string[] keys, bool remove = false)
        {
            Contract.Requires(keys != null);
            if (keys.Length == 0)
                return new CacheEntity[0];

            return keys.SelectMany(key => Get(key, remove)).ToArray();
        }

        /// <summary>
        /// 获取指定路径的缓存值
        /// </summary>
        /// <param name="key">路径</param>
        /// <param name="remove">是否同时删除该缓存</param>
        /// <returns></returns>
        public CacheEntity[] Get(string key, bool remove = false)
        {
            Contract.Requires(key != null);

            CacheTreeNode[] nodes = _FindTreeNodes(key, false);

            CacheEntity[] ces = new CacheEntity[nodes.Length];
            DateTime now = DateTime.UtcNow;

            for (int k = 0; k < nodes.Length; k++)
            {
                CacheTreeNode node = nodes[k];
                lock (node)
                {
                    if (remove)
                        _RemoveNode(node);
                    else
                        node.Value.LastAccessTime = now;

                    ces[k] = node.Value;
                }
            }

            return ces;
        }

        private CacheTreeNode[] _FindTreeNodes(string key, bool autoAppend = false)
        {
            CacheKeyParser p = new CacheKeyParser(key);
            return _FindTreeNodes(p.RoutePathParts, p.NamePathParts, autoAppend);
        }

        // 寻找树节点
        private CacheTreeNode[] _FindTreeNodes(string[] routePathParts, string[] namePathParts, bool autoAppend = false)
        {
            CacheTreeNode[] nodes = new[] { _cacheTree.Root };
            nodes = _FindTreeNodes(nodes, routePathParts, autoAppend);
            nodes = _FindTreeNodes(nodes, namePathParts, autoAppend);

            return nodes;
        }

        private CacheTreeNode[] _FindTreeNodes(CacheTreeNode[] roots, string[] parts, bool autoAppend = false)
        {
            if (roots.IsNullOrEmpty())
                return _emptyCacheTreeNodeArray;

            for (int k = 0; k < parts.Length; k++)
            {
                if ((roots = _FindTreeNodes(roots, parts[k], autoAppend)).IsNullOrEmpty())
                    return _emptyCacheTreeNodeArray;
            }

            return roots;
        }

        private static readonly CacheTreeNode[] _emptyCacheTreeNodeArray = new CacheTreeNode[0];

        private CacheTreeNode[] _FindTreeNodes(CacheTreeNode[] roots, string name, bool autoAppend = false)
        {
            if (roots == null || string.IsNullOrEmpty(name))
                return _emptyCacheTreeNodeArray;
            
            HashSet<CacheTreeNode> nodes = new HashSet<CacheTreeNode>();
            for (int k = 0; k < roots.Length; k++)
            {
                CacheTreeNode node = roots[k];
                if (name == "*")  // 查找全部
                {
                    nodes.AddRange(node.Nodes.Select(v => v.Value));
                }
                else
                {
                    CacheTreeNode node0 = node.Nodes[name];
                    if (node0 == null)
                    {
                        if (autoAppend)
                            nodes.Add(node.Nodes.Add(name, new CacheEntity(name, null, DateTime.MaxValue)));
                    }
                    else
                    {
                        nodes.Add(node0);
                    }
                }
            }

            return nodes.ToArray();
        }

        /// <summary>
        /// 设置指定路径的缓存值
        /// </summary>
        /// <param name="key">路径</param>
        /// <param name="buffer">缓存</param>
        /// <param name="expired">过期时间</param>
        public void Set(string key, byte[] buffer, TimeSpan expired)
        {
            Contract.Requires(key != null);

            if (buffer == null || expired <= TimeSpan.Zero)
            {
                Remove(key);
            }
            else
            {
                CacheTreeNode[] nodes = _FindTreeNodes(key, true);
                for (int k = 0; k < nodes.Length; k++)
                {
                    CacheTreeNode node = nodes[k];
                    lock (node)
                    {
                        if (node.Value.ValueLoader != null)
                            node.Value.ValueLoader.Dispose();

                        node.Value.ValueLoader = new ValueLoader(buffer);
                        node.Value.ExpiredTime = DateTime.UtcNow + expired;
                    }
                }
            }
        }

        /// <summary>
        /// 批量设置缓存
        /// </summary>
        /// <param name="items"></param>
        public void Set(CacheItem[] items)
        {
            Contract.Requires(items != null);
            if (items.Length == 0)
                return;

            for (int k = 0; k < items.Length; k++)
            {
                CacheItem item = items[k];
                Set(item.Key, item.Data, item.Expired);
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="item"></param>
        public void Set(CacheItem item)
        {
            Contract.Requires(item != null);

            Set(new[] { item });
        }


        /// <summary>
        /// 删除指定键的缓存
        /// </summary>
        /// <param name="keys"></param>
        public void Remove(string[] keys)
        {
            Contract.Requires(keys != null);

            foreach (string key in keys)
            {
                Remove(key);
            }
        }

        /// <summary>
        /// 删除指定路径的缓存值
        /// </summary>
        /// <param name="key"></param>
        private void Remove(string key)
        {
            Contract.Requires(key != null);

            CacheTreeNode[] nodes = _FindTreeNodes(key, false);
            if (nodes.IsNullOrEmpty())
                return;

            for (int k = 0; k < nodes.Length; k++)
            {
                _RemoveNode(nodes[k]);
            }
        }

        /// <summary>
        /// 返回存在的缓存键
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public string[] GetKeys(string[] keys)
        {
            Contract.Requires(keys != null);

            return keys.SelectMany(key => GetKey(key)).ToArray();
        }

        /// <summary>
        /// 返回存在的缓存键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string[] GetKey(string key)
        {
            Contract.Requires(key != null);

            CacheTreeNode[] nodes = _FindTreeNodes(key, false);
            if (nodes.IsNullOrEmpty())
                return new string[0];

            return nodes.ToArray(node => node.Value.Key);
        }

        private void _RemoveNode(CacheTreeNode node)
        {
            if (node == null || node.Parent == null)
                return;

            IDisposable dis = node.Value.ValueLoader;
            if (dis != null)
                dis.Dispose();

            node.Parent.Nodes.Remove(node.Value.Key);
        }

        /// <summary>
        /// 将缓存值赋予一个增量
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public CacheIncreaseResult[] Increase(CacheIncreaseItem[] items)
        {
            Contract.Requires(items != null);

            return items.SelectMany(item => Increase(item)).ToArray();
        }

        /// <summary>
        /// 将缓存值赋予一个增量
        /// </summary>
        /// <param name="items"></param>
        public CacheIncreaseResult[] Increase(CacheIncreaseItem item)
        {
            Contract.Requires(item != null);

            List<CacheIncreaseResult> results = new List<CacheIncreaseResult>();
            foreach (CacheTreeNode node in _FindTreeNodes(item.Key, true))
            {
                lock (node)
                {
                    IValueLoader valueLoader = node.Value.ValueLoader;
                    byte[] oldValueBytes = (valueLoader == null) ? null : valueLoader.Load();

                    decimal oldValue, newValue;
                    bool exists = _TryParseToDecimal(oldValueBytes, out oldValue);

                    if (item.Checked)
                    {
                        try
                        {
                            checked
                            {
                                newValue = oldValue + item.Increament;
                            }
                        }
                        catch (OverflowException)
                        {
                            throw new ServiceException(ServerErrorCode.DataError, "数据溢出");
                        }
                    }
                    else
                    {
                        unchecked
                        {
                            newValue = oldValue + item.Increament;
                        }
                    }

                    if (valueLoader != null)
                        valueLoader.Dispose();

                    node.Value.ValueLoader = new ValueLoader(SerializerUtility.OptimalBinarySerialize(newValue));

                    results.Add(new CacheIncreaseResult() {
                        Exists = exists, OldValue = oldValue, NewValue = newValue
                    });
                }
            }

            return results.ToArray();
        }

        private static bool _TryParseToDecimal(byte[] bytes, out decimal value)
        {
            if (bytes == null)
            {
                value = default(decimal);
                return false;
            }

            try
            {
                object obj = SerializerUtility.OptimalBinaryDeserialize(bytes);
                if (obj == null)
                    goto _end;

                if (obj.GetType() == typeof(decimal))
                {
                    value = (decimal)obj;
                    return true;
                }

                value = (decimal)Convert.ChangeType(obj, typeof(decimal));
                return true;
            }
            catch
            {
                goto _end;
            }

        _end:
            value = default(decimal);
            return false;
        }

        // 缓存树
        private readonly CacheTree _cacheTree = new CacheTree(
            (tree, parentNode) => new CacheTreeNodeCollection(tree, parentNode, new CacheTreeNodeDictionaryThreadSafeWrapper(new CacheTreeNodeDictionary())));

        private void _CheckTimeExpired(CacheTreeNode node, DateTime now)
        {
            bool monitorExited = false;
            Monitor.Enter(node);

            try
            {
                CacheEntity e = node.Value;
                IValueLoader vl = (e == null) ? null : e.ValueLoader;
                if (e != null && e.ExpiredTime < now || vl == null && !node.HasChildren())
                {
                    _RemoveNode(node);
                }
                else
                {
                    if (vl != null && now - e.LastAccessTime > TimeSpan.FromMinutes(5))
                        vl.Disuse();

                    if (node.HasChildren())
                    {
                        Monitor.Exit(node);
                        monitorExited = true;

                        foreach (KeyValuePair<string, CacheTreeNode> item in node.Nodes.ToArray())
                        {
                            CacheTreeNode n = item.Value;
                            _CheckTimeExpired(n, now);
                        }

                        CacheTreeNodeDictionaryThreadSafeWrapper dict = (CacheTreeNodeDictionaryThreadSafeWrapper)node.Nodes.GetDictionary();

                        using (dict.SyncLocker.Write())
                        {
                            ((CacheTreeNodeDictionary)dict.GetInnerDict()).TrimExcess();
                        }
                    }
                }
            }
            finally
            {
                if (!monitorExited)
                    Monitor.Exit(node);
            }
        }
    }
}

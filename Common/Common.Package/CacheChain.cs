using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Collection;
using System.Threading;

namespace Common.Package
{
    /// <summary>
    /// 缓存链
    /// </summary>
    public partial class CacheChain<TKey, TValue> where TValue : class
    {
        public CacheChain()
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes"></param>
        public CacheChain(IEnumerable<ICacheChainNode<TKey, TValue>> nodes)
            : this(nodes == null ? Array<ICacheChainNode<TKey, TValue>>.Empty : nodes.ToArray())
        {

        }

        public CacheChain(params ICacheChainNode<TKey, TValue>[] nodes)
        {
            AddNodes(nodes);
        }

        private ICacheChainNode<TKey, TValue> _head, _last;
        private readonly object _syncLocker = new object();

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="node"></param>
        public void AddNode(ICacheChainNode<TKey, TValue> node)
        {
            Contract.Requires(node != null);

            AddNodes(new[] { node });
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="nodes"></param>
        public void AddNodes(params ICacheChainNode<TKey, TValue>[] nodes)
        {
            Contract.Requires(nodes != null);

            lock (_syncLocker)
            {
                foreach (ICacheChainNode<TKey, TValue> node in nodes)
                {
                    if (_head == null)
                        _head = node;
                    else
                        _last.SetNextNode(node);

                    _last = node;
                }
            }
        }

        /// <summary>
        /// 添加一项内存缓存节点
        /// </summary>
        /// <param name="expire"></param>
        public void AddMemoryCache(TimeSpan expire)
        {
            AddNode(CreateMemoryCacheNode(expire));
        }

        private static readonly KeyValuePair<TKey, TValue>[] _emptyKeyValueArr = new KeyValuePair<TKey, TValue>[0];

        #region Class MemoryCacheChainNodeBase ...

        abstract class MemoryCacheChainNodeBase : CacheChainNodeBase<TKey, TValue>
        {
            private readonly Cache<TKey, TValue> _cache = new Cache<TKey, TValue>();

            public Cache<TKey, TValue> Cache
            {
                get { return _cache; }
            }

            protected override KeyValuePair<TKey, TValue>[] OnGet(TKey[] keys)
            {
                if (keys.Length == 0)
                    return null;

                if (keys.Length == 1)
                {
                    TValue value = _cache.Get(keys[0]);
                    return (value == null) ? null : new[] { new KeyValuePair<TKey, TValue>(keys[0], value) };
                }

                return _cache.GetRange(keys).ToArray();
            }

            protected override void OnRemove(TKey[] keys)
            {
                _cache.RemoveRange(keys);
            }
        }

        #endregion

        #region Class RelativeMemoryCacheChainNode ...

        class RelativeMemoryCacheChainNode : MemoryCacheChainNodeBase
        {
            public RelativeMemoryCacheChainNode(TimeSpan expire)
            {
                _expire = expire;
            }

            private readonly TimeSpan _expire;

            protected override void OnAdd(KeyValuePair<TKey, TValue>[] items)
            {
                for (int k = 0; k < items.Length; k++)
                {
                    KeyValuePair<TKey, TValue> item = items[k];
                    Cache.AddOfRelative(item.Key, item.Value, _expire);
                }
            }
        }

        #endregion

        /// <summary>
        /// 添加一项动态时间的内存缓存节点
        /// </summary>
        /// <param name="init"></param>
        /// <param name="step"></param>
        /// <param name="max"></param>
        public void AddMemoryCache(TimeSpan init, TimeSpan step, TimeSpan max)
        {
            AddNode(CreateMemoryCacheNode(init, step, max));
        }

        #region Class DynamicMemoryCacheChainNode ...

        class DynamicMemoryCacheChainNode : MemoryCacheChainNodeBase
        {
            public DynamicMemoryCacheChainNode(TimeSpan init, TimeSpan step, TimeSpan max)
            {
                _init = init;
                _step = step;
                _max = max;
            }

            private readonly TimeSpan _init, _step, _max;

            protected override void OnAdd(KeyValuePair<TKey, TValue>[] items)
            {
                for (int k = 0; k < items.Length; k++)
                {
                    KeyValuePair<TKey, TValue> item = items[k];
                    Cache.AddOfDynamic(item.Key, item.Value, _init, _step, _max);
                }
            }
        }

        #endregion

        /// <summary>
        /// 添加一项弱引用缓存节点
        /// </summary>
        public void AddWeakReferenceCache()
        {
            AddNode(CreateWeakReferenceCacheNode());
        }

        #region Class WeakReferenceCacheChainNode ...

        class WeakReferenceCacheChainNode : CacheChainNodeBase<TKey, TValue>
        {
            public WeakReferenceCacheChainNode()
            {
                var dict = new AutoReleaseWeakReferenceDictionary<TKey, TValue>();
                var wrapper = new ThreadSafeDictionaryWrapper<TKey, TValue>(dict);
                dict.SyncLocker = wrapper.SyncLocker;

                _dict = wrapper;
            }

            private readonly IDictionary<TKey, TValue> _dict;

            protected override KeyValuePair<TKey, TValue>[] OnGet(TKey[] keys)
            {
                return _dict.GetRange(keys);
            }

            protected override void OnAdd(KeyValuePair<TKey, TValue>[] items)
            {
                _dict.AddRange(items, true);
            }

            protected override void OnRemove(TKey[] keys)
            {
                _dict.RemoveRange(keys);
            }
        }

        #endregion

        /// <summary>
        /// 创建内存缓存节点
        /// </summary>
        /// <param name="expire"></param>
        /// <returns></returns>
        public static ICacheChainNode<TKey, TValue> CreateMemoryCacheNode(TimeSpan expire)
        {
            return new RelativeMemoryCacheChainNode(expire);
        }

        /// <summary>
        /// 创建内存缓存节点
        /// </summary>
        /// <param name="init"></param>
        /// <param name="step"></param>
        /// <param name="max"></param>
        public static ICacheChainNode<TKey, TValue> CreateMemoryCacheNode(TimeSpan init, TimeSpan step, TimeSpan max)
        {
            return new DynamicMemoryCacheChainNode(init, step, max);
        }

        /// <summary>
        /// 根据加载器创建缓存节点
        /// </summary>
        /// <param name="loader">加载器</param>
        /// <returns></returns>
        public static ICacheChainNode<TKey, TValue> CreateLoaderNode(CacheChainValueLoader<TKey, TValue> loader)
        {
            Contract.Requires(loader != null);
            return new LoaderCacheChainNode(loader);
        }

        /// <summary>
        /// 创建弱引用缓存节点
        /// </summary>
        /// <returns></returns>
        public static ICacheChainNode<TKey, TValue> CreateWeakReferenceCacheNode()
        {
            return new WeakReferenceCacheChainNode();
        }

        #region Class LoaderCacheChainNode ...

        class LoaderCacheChainNode : CacheChainNodeBase<TKey, TValue>, ICacheChainNode<TKey, TValue>
        {
            public LoaderCacheChainNode(CacheChainValueLoader<TKey, TValue> loader)
            {
                _loader = loader ?? new CacheChainValueLoader<TKey, TValue>((keys, refresh) => _emptyKeyValueArr);
            }

            private readonly CacheChainValueLoader<TKey, TValue> _loader;

            KeyValuePair<TKey, TValue>[] ICacheChainNode<TKey, TValue>.Get(TKey[] keys, bool refresh)
            {
                var result = _loader(keys, refresh);

                if (NextNode != Empty)
                {
                    TKey[] notExistsKeys = keys.Except(result.Select(item => item.Key)).ToArray();
                    if (notExistsKeys.Length > 0)
                    {
                        var newItems = NextNode.Get(notExistsKeys, refresh);
                        result = CollectionUtility.Concat(result, newItems);
                    }
                }

                return result;
            }

            protected override KeyValuePair<TKey, TValue>[] OnGet(TKey[] keys)
            {
                return null;
            }

            protected override void OnAdd(KeyValuePair<TKey, TValue>[] items)
            {
                
            }

            protected override void OnRemove(TKey[] keys)
            {
                
            }
        }

        #endregion

        /// <summary>
        /// 添加加载器
        /// </summary>
        /// <param name="loader"></param>
        public void AddLoader(CacheChainValueLoader<TKey, TValue> loader)
        {
            AddNode(CreateLoaderNode(loader));
        }

        /// <summary>
        /// 创建聚合器
        /// </summary>
        /// <param name="priority">线程优先级</param>
        /// <returns></returns>
        public ICacheChainNode<TKey, TValue> CreatePolyNode(ThreadPriority priority = ThreadPriority.Normal)
        {
            return new PolyCacheChainNode(priority);
        }

        /// <summary>
        /// 添加聚合器
        /// </summary>
        public void AddPoly(ThreadPriority priority)
        {
            AddNode(CreatePolyNode(priority));
        }

        /// <summary>
        /// 添加聚合器
        /// </summary>
        public void AddPoly()
        {
            AddPoly(ThreadPriority.Normal);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="keys">键</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns></returns>
        public KeyValuePair<TKey, TValue>[] GetRange(TKey[] keys, bool refresh)
        {
            if (keys.IsNullOrEmpty() || _head == null)
                return _emptyKeyValueArr;

            return _head.Get(keys, refresh);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="keys">键</param>
        /// <returns>缓存值</returns>
        public KeyValuePair<TKey, TValue>[] GetRange(TKey[] keys)
        {
            return GetRange(keys, false);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns>缓存值</returns>
        public TValue Get(TKey key, bool refresh)
        {
            if (key == null)
                return null;

            KeyValuePair<TKey, TValue>[] items = GetRange(new[] { key }, refresh);
            if (items.IsNullOrEmpty() || !object.Equals(items[0].Key, key))
                return null;

            return items[0].Value;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="refresh">是否刷新缓存</param>
        /// <returns>缓存值</returns>
        public TValue Get(TKey key)
        {
            return Get(key, false);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="items">缓存项</param>
        public void AddRange(KeyValuePair<TKey, TValue>[] items)
        {
            if (_head != null)
            {
                _head.Add(items);
            }
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Add(TKey key, TValue value)
        {
            AddRange(new[] { new KeyValuePair<TKey, TValue>(key, value) });
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="keys">键</param>
        public void RemoveRange(TKey[] keys)
        {
            if (!keys.IsNullOrEmpty() && _head != null)
            {
                _head.Remove(keys);
            }
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">键</param>
        public void Remove(TKey key)
        {
            if (key == null)
                return;

            RemoveRange(new[] { key });
        }

        public static readonly ICacheChainNode<TKey, TValue> Empty = CacheChainNodeBase<TKey, TValue>.Empty;
    }

    /// <summary>
    /// 缓存链节点
    /// </summary>
    public interface ICacheChainNode<TKey, TValue> where TValue : class
    {
        /// <summary>
        /// 从该节点上获取一项缓存，如果不存在则返回空
        /// </summary>
        /// <param name="keys">键</param>
        /// <param name="refresh">是否在获取之前首先刷新缓存</param>
        /// <returns></returns>
        KeyValuePair<TKey, TValue>[] Get(TKey[] keys, bool refresh);

        /// <summary>
        /// 在该节点上设置一项缓存
        /// </summary>
        /// <param name="items">缓存项</param>
        void Add(KeyValuePair<TKey, TValue>[] items);

        /// <summary>
        /// 从该节点上删除缓存
        /// </summary>
        /// <param name="keys">键</param>
        void Remove(TKey[] keys);

        /// <summary>
        /// 设置下一个节点
        /// </summary>
        /// <param name="nextNode">下一个节点</param>
        void SetNextNode(ICacheChainNode<TKey, TValue> nextNode);
    }

    /// <summary>
    /// 缓存链节点基类
    /// </summary>
    /// <typeparam name="TKey">键</typeparam>
    /// <typeparam name="TValue">值</typeparam>
    public abstract class CacheChainNodeBase<TKey, TValue> : ICacheChainNode<TKey, TValue> where TValue : class
    {
        public CacheChainNodeBase()
        {
            NextNode = EmptyCacheChainNode.Instance;
        }

        private static readonly KeyValuePair<TKey, TValue>[] _emptyArray = Array<KeyValuePair<TKey, TValue>>.Empty;

        KeyValuePair<TKey, TValue>[] ICacheChainNode<TKey, TValue>.Get(TKey[] keys, bool refresh)
        {
            KeyValuePair<TKey, TValue>[] result, newItems = null;
            if (refresh)
            {
                result = newItems = NextNode.Get(keys, refresh);
            }
            else
            {
                result = OnGet(keys ?? Array<TKey>.Empty) ?? _emptyArray;

                if (NextNode != Empty)
                {
                    TKey[] notExistsKeys = keys.Except(result.Select(item => item.Key)).ToArray();
                    if (notExistsKeys.Length > 0)
                    {
                        newItems = NextNode.Get(notExistsKeys, refresh);
                        result = CollectionUtility.Concat(result, newItems);
                    }
                }
            }

            if (newItems != null)
            {
                OnAdd(newItems);
            }

            return result;
        }

        /// <summary>
        /// 加载缓存项
        /// </summary>
        /// <param name="keys">键</param>
        /// <returns></returns>
        protected abstract KeyValuePair<TKey, TValue>[] OnGet(TKey[] keys);

        void ICacheChainNode<TKey, TValue>.Add(KeyValuePair<TKey, TValue>[] items)
        {
            OnAdd(items ?? _emptyArray);
            NextNode.Add(items);
        }

        /// <summary>
        /// 添加缓存项
        /// </summary>
        /// <param name="items">缓存项</param>
        protected abstract void OnAdd(KeyValuePair<TKey, TValue>[] items);

        void ICacheChainNode<TKey, TValue>.Remove(TKey[] keys)
        {
            OnRemove(keys);
            NextNode.Remove(keys);
        }

        /// <summary>
        /// 删除缓存项
        /// </summary>
        /// <param name="keys">键</param>
        protected abstract void OnRemove(TKey[] keys);

        void ICacheChainNode<TKey, TValue>.SetNextNode(ICacheChainNode<TKey, TValue> nextNode)
        {
            NextNode = nextNode ?? Empty;
        }

        public ICacheChainNode<TKey, TValue> NextNode { get; private set; }

        #region Class EmptyCacheChainNode ...

        class EmptyCacheChainNode : ICacheChainNode<TKey, TValue>
        {
            public KeyValuePair<TKey, TValue>[] Get(TKey[] keys, bool refresh)
            {
                return Array<KeyValuePair<TKey, TValue>>.Empty;
            }

            public void Add(KeyValuePair<TKey, TValue>[] items)
            {
                
            }

            public void Remove(TKey[] keys)
            {
                
            }

            public void SetNextNode(ICacheChainNode<TKey, TValue> nextNode)
            {
                
            }

            public static readonly EmptyCacheChainNode Instance = new EmptyCacheChainNode();
        }

        #endregion

        /// <summary>
        /// 空的节点
        /// </summary>
        public static readonly ICacheChainNode<TKey, TValue> Empty = EmptyCacheChainNode.Instance;
    }

    /// <summary>
    /// 缓存链节点缓存值加载器
    /// </summary>
    /// <typeparam name="TKey">键类型</typeparam>
    /// <typeparam name="TValue">值类型</typeparam>
    /// <param name="keys">键</param>
    /// <param name="refresh">是否首先刷新缓存</param>
    /// <returns></returns>
    public delegate KeyValuePair<TKey, TValue>[] CacheChainValueLoader<TKey, TValue>(TKey[] keys, bool refresh)
        where TValue : class;
}

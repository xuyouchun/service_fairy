using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common;
using ServiceFairy.Entities.Cache;
using Common.Contracts.Service;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 分布式缓存的缓存链节点
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class DistribuedCacheChainNode<TKey, TValue> : CacheChainNodeBase<TKey, TValue> where TValue : class
    {
        public DistribuedCacheChainNode(SystemInvoker invoker, TimeSpan expire, Func<TKey, string> keyBuilder)
        {
            Contract.Requires(invoker != null && keyBuilder != null);

            _invoker = invoker.Cache;
            _expire = expire;
            _keyBuilder = keyBuilder;
        }

        public DistribuedCacheChainNode(SystemInvoker invoker, TimeSpan expire, string prefix = null)
            : this(invoker, expire, (key) => _GetKey(key, prefix))
        {

        }

        private static string _GetKey(TKey key, string prefix)
        {
            string s = prefix + key;
            if (s.IndexOf('*') >= 0)
            {
                s = s.Replace('*', '?');  // 分布式缓存的键不允许包含星号
            }

            return s;
        }

        private readonly SystemInvoker.CacheInvoker _invoker;
        private TimeSpan _expire;
        private readonly Func<TKey, string> _keyBuilder;

        private string _BuildKey(TKey key)
        {
            return _keyBuilder(key);
        }

        private static readonly KeyValuePair<TKey, TValue>[] _emptyKeyValueArr = new KeyValuePair<TKey, TValue>[0];

        protected override KeyValuePair<TKey, TValue>[] OnGet(TKey[] keys)
        {
            if (keys.IsNullOrEmpty())
                return _emptyKeyValueArr;

            if (keys.Length == 1)
            {
                TValue value = _invoker.Get<TValue>(_BuildKey(keys[0]));
                if (value == null)
                    return _emptyKeyValueArr;

                return new[] { new KeyValuePair<TKey, TValue>(keys[0], value) };
            }

            Dictionary<string, TValue> dict = _invoker.GetRange<TValue>(keys.ToArray(_BuildKey)).ToDictionary(item => item.Key, item => item.Value);
            var list = from key in keys
                       let value = dict.GetOrDefault(_BuildKey(key))
                       where value != null
                       select new KeyValuePair<TKey, TValue>(key, value);

            return list.ToArray();
        }

        protected override void OnAdd(KeyValuePair<TKey, TValue>[] items)
        {
            if (items.IsNullOrEmpty())
                return;

            _invoker.SetRange<TValue>(items.ToArray(item => new CacheItem<TValue>(_BuildKey(item.Key), item.Value, _expire)),
                settings: CallingSettings.OneWay);
        }

        protected override void OnRemove(TKey[] keys)
        {
            if (keys.IsNullOrEmpty())
                return;

            _invoker.Remove(keys.ToArray(_BuildKey), settings: CallingSettings.OneWay);
        }
    }
}

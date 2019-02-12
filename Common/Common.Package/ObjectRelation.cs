using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Cache;
using System.Diagnostics.Contracts;

namespace Common.Package
{
    /// <summary>
    /// 设置对象之间的关联，当对象被释放时，其关联对象也随之释放
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ObjectRelation<TKey, TValue>
        where TKey : class
        where TValue : class
    {
        public ObjectRelation()
        {

        }

        private readonly Cache<KeyWrapper, TValue> _cache = new Cache<KeyWrapper, TValue>();

        #region Class CacheExpireDependency ...

        class CacheExpireDependency : ICacheExpireDependency
        {
            public CacheExpireDependency(WeakReference wr)
            {
                _wr = wr;
            }

            private readonly WeakReference _wr;

            public void Reset()
            {
                
            }

            public void AccessNotify()
            {
                
            }

            public bool HasExpired()
            {
                return !_wr.IsAlive;
            }
        }

        #endregion

        #region Class KeyWrapper ...

        class KeyWrapper
        {
            public KeyWrapper(TKey key)
            {
                _wr = new WeakReference<TKey>(key);
                _hashCode = key.GetHashCode();
            }

            private WeakReference<TKey> _wr;
            private int _hashCode;

            public WeakReference<TKey> Wr
            {
                get { return _wr; }
            }

            public override int GetHashCode()
            {
                return _hashCode;
            }

            public override bool Equals(object obj)
            {
                KeyWrapper w = (KeyWrapper)obj;
                return object.ReferenceEquals(w._wr.Target, _wr.Target);
            }
        }

        #endregion

        /// <summary>
        /// 设置对象的关联
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(TKey key, TValue value)
        {
            Contract.Requires(key != null);

            KeyWrapper w = new KeyWrapper(key);
            if (value == null)
                _cache.Remove(w);
            else
                _cache.Add(w, value, new CacheExpireDependency(w.Wr));
        }

        /// <summary>
        /// 获取对象的关联
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue Get(TKey key)
        {
            Contract.Requires(key != null);

            return _cache.Get(new KeyWrapper(key));
        }

        /// <summary>
        /// 获取关联，如果不存在则创建
        /// </summary>
        /// <param name="key"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public TValue GetOrSet(TKey key, Func<TKey, TValue> creator = null)
        {
            Contract.Requires(key != null);

            TValue value = Get(key);
            if (value != null)
                return value;

            lock (key)
            {
                if ((value = Get(key)) == null)
                    Set(key, value = (creator == null ? Activator.CreateInstance<TValue>() : creator(key)));

                return value;
            }
        }
    }
}

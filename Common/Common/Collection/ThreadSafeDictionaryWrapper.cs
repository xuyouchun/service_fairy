using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics.Contracts;

namespace Common.Collection
{
    /// <summary>
    /// 线程安全的Dictionary
    /// </summary>
    /// <typeparam name="TKey">键类型</typeparam>
    /// <typeparam name="TValue">值类型</typeparam>
    [Serializable, System.Diagnostics.DebuggerDisplay("Count = {Count}")]
    public class ThreadSafeDictionaryWrapper<TKey, TValue> : MarshalByRefObject, IDictionary<TKey, TValue>, IDictionaryRange<TKey, TValue>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="innerDict">内含的Dict</param>
        public ThreadSafeDictionaryWrapper(IDictionary<TKey, TValue> innerDict = null)
        {
            _Dict = innerDict ?? new Dictionary<TKey, TValue>();
        }

        private readonly IDictionary<TKey, TValue> _Dict;
        private readonly RwLocker _rwLocker = new RwLocker();

        /// <summary>
        /// 读写锁
        /// </summary>
        public RwLocker SyncLocker
        {
            get { return _rwLocker; }
        }

        #region IDictionary<TKey,TValue> Members

        /// <summary>
        /// 添加一组键值对
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            using (_rwLocker.Write())
            {
                _Dict.Add(key, value);
            }
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="items"></param>
        /// <param name="ignoreDupKey"></param>
        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items, bool ignoreDupKey = false)
        {
            Contract.Requires(items != null);

            using (_rwLocker.Write())
            {
                if (ignoreDupKey)
                {
                    foreach (var item in items)
                    {
                        _Dict[item.Key] = item.Value;
                    }
                }
                else
                {
                    foreach (var item in items)
                    {
                        _Dict.Add(item.Key, item.Value);
                    }
                }
            }
        }

        /// <summary>
        /// 是否包含指定的键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            return _Dict.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return _Dict.Keys; }
        }

        /// <summary>
        /// 删除指定键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(TKey key)
        {
            using (_rwLocker.Write())
            {
                return _Dict.Remove(key);
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keys"></param>
        public int RemoveRange(IEnumerable<TKey> keys)
        {
            Contract.Requires(keys != null);

            using (_rwLocker.Write())
            {
                int count = 0;
                foreach (TKey key in keys)
                {
                    if (_Dict.Remove(key))
                        count++;
                }

                return count;
            }
        }

        /// <summary>
        /// 批量获取
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public KeyValuePair<TKey, TValue>[] GetRange(IEnumerable<TKey> keys)
        {
            Contract.Requires(keys != null);

            using (_rwLocker.Read())
            {
                List<KeyValuePair<TKey, TValue>> list = new List<KeyValuePair<TKey, TValue>>();
                foreach (TKey key in keys)
                {
                    TValue value;
                    if (_Dict.TryGetValue(key, out value))
                        list.Add(new KeyValuePair<TKey, TValue>(key, value));
                }

                return list.ToArray();
            }
        }

        /// <summary>
        /// 获取一项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _Dict.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get { return _Dict.Values; }
        }

        public TValue this[TKey key]
        {
            get
            {
                return _Dict[key];
            }
            set
            {
                using (_rwLocker.Write())
                {
                    _Dict[key] = value;
                }
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            using (_rwLocker.Write())
            {
                ((IDictionary<TKey, TValue>)_Dict).Add(item);
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            using (_rwLocker.Write())
            {
                _Dict.Clear();
            }
        }

        /// <summary>
        /// 是否包含指定的键值对
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            using (_rwLocker.Read())
            {
                return ((ICollection<KeyValuePair<TKey,TValue>>)_Dict).Contains(item);
            }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            using (_rwLocker.Read())
            {
                ((ICollection<KeyValuePair<TKey, TValue>>)_Dict).CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count
        {
            get { return _Dict.Count; }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<TKey, TValue>>)_Dict).IsReadOnly; }
        }

        bool ICollection<KeyValuePair<TKey,TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)_Dict).Remove(item);
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            using (_rwLocker.Read())
            {
                foreach (var item in _Dict)
                {
                    yield return item;
                }
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public IDictionary<TKey, TValue> GetInnerDict()
        {
            return _Dict;
        }
    }
}

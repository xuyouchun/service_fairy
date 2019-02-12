using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace Common.Collection
{
    /// <summary>
    /// 基于弱引用值的哈希表
    /// </summary>
    /// <typeparam name="TKey">键类型</typeparam>
    /// <typeparam name="TValue">值类型</typeparam>
    [Serializable, System.Diagnostics.DebuggerDisplay("Count = {Count}")]
    public class WeakReferenceDictionary<TKey, TValue> : IDictionary<TKey, TValue> where TValue : class
    {
        public WeakReferenceDictionary()
        {

        }

        private readonly Dictionary<TKey, WeakReference<TValue>> _dict = new Dictionary<TKey, WeakReference<TValue>>();

        public TKey[] RemoveCollected()
        {
            List<TKey> keys = new List<TKey>();
            foreach (KeyValuePair<TKey, WeakReference<TValue>> item in _dict)
            {
                if (!item.Value.IsAlive)
                    keys.Add(item.Key);
            }

            _dict.RemoveRange(keys);
            return keys.ToArray();
        }

        public void Add(TKey key, TValue value)
        {
            Contract.Requires(key != null);

            if (value != null)
                _dict.Add(key, new WeakReference<TValue>(value));
            else
                _dict.Remove(key);
        }

        public bool ContainsKey(TKey key)
        {
            return _dict.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return _dict.Keys; }
        }

        public bool Remove(TKey key)
        {
            return _dict.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            WeakReference<TValue> v;
            if (!_dict.TryGetValue(key, out v))
            {
                value = null;
                return false;
            }

            return (value = v.Target) != null;
        }

        public ICollection<TValue> Values
        {
            get
            {
                List<TValue> values = new List<TValue>();
                foreach (WeakReference<TValue> w in _dict.Values)
                {
                    TValue value = w.Target;
                    if (value != null)
                        values.Add(value);
                }

                return values;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                TryGetValue(key, out value);
                return value;
            }
            set
            {
                if (value != null)
                    _dict[key] = new WeakReference<TValue>(value);
                else
                    _dict.Remove(key);
            }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            if (item.Value == null)
                return;

            ((ICollection<KeyValuePair<TKey, WeakReference<TValue>>>)_dict)
                .Add(new KeyValuePair<TKey, WeakReference<TValue>>(item.Key, new WeakReference<TValue>(item.Value)));
        }

        public void Clear()
        {
            _dict.Clear();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dict.ContainsKey(item.Key);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            foreach (KeyValuePair<TKey, TValue> item in this)
            {
                array[arrayIndex++] = item;
            }
        }

        public int Count
        {
            get { return _dict.Count; }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return _dict.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var item in _dict)
            {
                TValue value = item.Value.Target;
                if (value != null)
                    yield return new KeyValuePair<TKey, TValue>(item.Key, value);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

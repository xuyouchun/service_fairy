using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;

namespace Common.Collection
{
    /// <summary>
    /// 大数据量的哈希表，支持TrimExcess操作
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Serializable, System.Diagnostics.DebuggerDisplay("Count = {Count}")]
    public class LargeDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, ICollection, IEnumerable, ISerializable, IDeserializationCallback
    {
        public LargeDictionary()
        {
            _dict = new Dictionary<TKey, TValue>();
        }

        public LargeDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _dict = new Dictionary<TKey, TValue>(dictionary);
        }

        public LargeDictionary(IEqualityComparer<TKey> comparer)
        {
            _dict = new Dictionary<TKey, TValue>(comparer);
        }

        public LargeDictionary(int capacity)
        {
            _dict = new Dictionary<TKey, TValue>(capacity);
        }

        public LargeDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            _dict = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        public LargeDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            _dict = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        private volatile Dictionary<TKey, TValue> _dict;

        public IEqualityComparer<TKey> Comparer
        {
            get { return _dict.Comparer; }
        }

        public void Add(TKey key, TValue value)
        {
            _dict.Add(key, value);
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
            if (_dict.Remove(key))
            {
                _removedCount++;
                return true;
            }

            return false;
        }

        public bool Remove(TKey key, bool trimExcess)
        {
            if (Remove(key))
            {
                if (trimExcess)
                    TrimExcess();

                return true;
            }

            return false;
        }

        private int _removedCount;

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dict.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get { return _dict.Values; }
        }

        public TValue this[TKey key]
        {
            get
            {
                return _dict[key];
            }
            set
            {
                _dict[key] = value;
            }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)_dict).Add(item);
        }

        public void Clear()
        {
            _dict.Clear();
        }

        public void Clear(bool trimExcess)
        {
            Clear();
            if (trimExcess)
                TrimExcess();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)_dict).Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)_dict).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _dict.Count; }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<TKey, TValue>>)_dict).IsReadOnly; }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)_dict).Remove(item);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)_dict).GetEnumerator();
        }

        public void TrimExcess()
        {
            int total;
            if (_removedCount < 100 || (total = _removedCount + Count) == 0)
                return;

            if ((float)_removedCount / total > 0.1)
            {
                _dict = new Dictionary<TKey, TValue>(_dict, Comparer);
                _removedCount = 0;
            }
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            ((ISerializable)_dict).GetObjectData(info, context);
        }

        void IDeserializationCallback.OnDeserialization(object sender)
        {
            ((IDeserializationCallback)_dict).OnDeserialization(sender);
        }

        void IDictionary.Add(object key, object value)
        {
            ((IDictionary)_dict).Add(key, value);
        }

        void IDictionary.Clear()
        {
            ((IDictionary)_dict).Clear();
        }

        bool IDictionary.Contains(object key)
        {
            return ((IDictionary)_dict).Contains(key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary)_dict).GetEnumerator();
        }

        bool IDictionary.IsFixedSize
        {
            get { return ((IDictionary)_dict).IsFixedSize; }
        }

        bool IDictionary.IsReadOnly
        {
            get { return ((IDictionary)_dict).IsReadOnly; }
        }

        ICollection IDictionary.Keys
        {
            get { return ((IDictionary)_dict).Keys; }
        }

        void IDictionary.Remove(object key)
        {
            ((IDictionary)_dict).Remove(key);
        }

        ICollection IDictionary.Values
        {
            get { return ((IDictionary)_dict).Values; }
        }

        object IDictionary.this[object key]
        {
            get
            {
                return ((IDictionary)_dict)[key];
            }
            set
            {
                ((IDictionary)_dict)[key] = value;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((IDictionary)_dict).CopyTo(array, index);
        }

        int ICollection.Count
        {
            get { return ((IDictionary)_dict).Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return ((IDictionary)_dict).IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { return ((IDictionary)_dict).SyncRoot; }
        }
    }
}

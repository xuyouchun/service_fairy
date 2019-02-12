using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Common.Collection
{
    /// <summary>
    /// 支持快速检索到对象位置的哈希列表
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [System.Diagnostics.DebuggerDisplay("Count = {Count}")]
    public class DictList<TKey, TValue> : IList<TValue>, IEnumerable<TValue>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="keySelector">键选择器</param>
        public DictList(Func<TValue, TKey> keySelector)
        {
            Contract.Requires(keySelector != null);

            _list = new List<TValue>();
            _dict = new Dictionary<TKey, int>();

            _keySelector = keySelector;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="comparer"></param>
        /// <param name="keySelector">键选择器</param>
        public DictList(Func<TValue, TKey> keySelector, int capacity, IEqualityComparer<TKey> comparer)
        {
            Contract.Requires(keySelector != null);

            _list = new List<TValue>(capacity);
            _dict = new Dictionary<TKey, int>(capacity, comparer);

            _keySelector = keySelector;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="keySelector">键选择器</param>
        /// <param name="comparer"></param>
        public DictList(Func<TValue, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            Contract.Requires(keySelector != null);

            _list = new List<TValue>();
            _dict = new Dictionary<TKey, int>(comparer);

            _keySelector = keySelector;
        }

        private readonly Func<TValue, TKey> _keySelector;
        private readonly List<TValue> _list;
        private readonly Dictionary<TKey, int> _dict;

        private TKey _GetKey(TValue value, bool throwError = true)
        {
            if (!throwError)
            {
                return _keySelector(value);
            }
            else
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                TKey key = _keySelector(value);
                if (key == null)
                    throw new InvalidOperationException("键为空");

                return key;
            }
        }

        /// <summary>
        /// 获取对象的索引位置
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(TValue item)
        {
            if (item == null)
                return -1;

            return IndexOfKey(_GetKey(item));
        }

        /// <summary>
        /// 获取对象键的索引位置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int IndexOfKey(TKey key)
        {
            int index;
            if (key == null || !(_dict.TryGetValue(key, out index)))
                return -1;

            return index;
        }

        /// <summary>
        /// 插入元素
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, TValue item)
        {
            if (_dict.ContainsKey(_GetKey(item)))
                throw new InvalidOperationException("不允许插入重复的值");

            _list.Insert(index, item);
            for (int k = index; k < _list.Count; k++)
            {
                _dict[_GetKey(_list[k])] = k;
            }
        }

        /// <summary>
        /// 删除指定位置的元素
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            TValue item = _list[index];
            _list.RemoveAt(index);
            _dict.Remove(_GetKey(item));

            for (int k = index; k < _list.Count; k++)
            {
                _dict[_GetKey(_list[k])] = k;
            }
        }

        /// <summary>
        /// 按索引读取或写入
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TValue this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                Contract.Requires(value != null);
                if (_dict.ContainsKey(_GetKey(value)) && !object.Equals(value, _list[index]))
                    throw new InvalidOperationException("不允许写入重复的值");

                _dict.Remove(_GetKey(_list[index]));
                _list[index] = value;

                _dict[_GetKey(value)] = index;
            }
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="item"></param>
        public void Add(TValue item)
        {
            Add(item, false);
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="item">元素</param>
        /// <param name="overwrite">是否在元素已经存在的情况下将其覆盖</param>
        /// <returns>该元素之前是否已经存在</returns>
        public bool Add(TValue item, bool overwrite)
        {
            Contract.Requires(item != null);

            int index;
            if (_dict.TryGetValue(_GetKey(item), out index))
            {
                if (overwrite)
                    _list[index] = item;

                return true;
            }

            _list.Add(item);
            _dict.Add(_GetKey(item), _list.Count - 1);
            return false;
        }

        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            _list.Clear();
            _dict.Clear();
        }

        /// <summary>
        /// 是否包含指定的元素
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(TValue item)
        {
            return _dict.ContainsKey(_GetKey(item));
        }

        /// <summary>
        /// 是否包含指定的键
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            return key != null && _dict.ContainsKey(key);
        }

        /// <summary>
        /// 复制到指定的数组中
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(TValue[] array, int arrayIndex)
        {
            Contract.Requires(array != null);

            for (int k = 0; k < _list.Count; k++)
            {
                array[arrayIndex++] = _list[k];
            }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// 是否为只读
        /// </summary>
        bool ICollection<TValue>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(TValue item)
        {
            int index;
            if (_dict.TryGetValue(_GetKey(item), out index))
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 转换为数组
        /// </summary>
        /// <returns></returns>
        public TValue[] ToArray()
        {
            return _list.ToArray();
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// 忽略大小写的支持快速检索到对象位置的哈希列表
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class IgnoreCaseDictList<TValue> : DictList<string, TValue>
    {
        public IgnoreCaseDictList(Func<TValue, string> keySelector)
            : base(keySelector, IgnoreCaseEqualityComparer.Instance)
        {

        }

        public IgnoreCaseDictList(Func<TValue, string> keySelector, int capacity)
            : base(keySelector, capacity, IgnoreCaseEqualityComparer.Instance)
        {

        }
    }
}

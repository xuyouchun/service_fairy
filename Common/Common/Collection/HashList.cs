using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Common.Collection
{
    /// <summary>
    /// 支持快速检索到对象位置的列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Diagnostics.DebuggerDisplay("Count = {Count}")]
    public class HashList<T> : IList<T>, IEnumerable<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HashList()
        {
            _list = new List<T>();
            _dict = new Dictionary<T, int>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="comparer"></param>
        public HashList(int capacity, IEqualityComparer<T> comparer)
        {
            _list = new List<T>(capacity);
            _dict = new Dictionary<T, int>(capacity, comparer);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="comparer"></param>
        public HashList(IEqualityComparer<T> comparer)
        {
            _list = new List<T>();
            _dict = new Dictionary<T, int>(comparer);
        }

        private readonly List<T> _list;
        private readonly Dictionary<T, int> _dict;

        /// <summary>
        /// 获取对象的索引位置
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            int index;
            if (item == null || !(_dict.TryGetValue(item, out index)))
                return -1;

            return index;
        }

        /// <summary>
        /// 插入元素
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, T item)
        {
            if (_dict.ContainsKey(item))
                throw new InvalidOperationException("不允许插入重复的值");

            _list.Insert(index, item);
            for (int k = index; k < _list.Count; k++)
            {
                _dict[_list[k]] = k;
            }
        }

        /// <summary>
        /// 删除指定位置的元素
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            T item = _list[index];
            _list.RemoveAt(index);
            _dict.Remove(item);

            for (int k = index; k < _list.Count; k++)
            {
                _dict[_list[k]] = k;
            }
        }

        /// <summary>
        /// 按索引读取或写入
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                Contract.Requires(value != null);
                if (_dict.ContainsKey(value) && !object.Equals(value, _list[index]))
                    throw new InvalidOperationException("不允许写入重复的值");

                _dict.Remove(_list[index]);
                _list[index] = value;

                _dict[value] = index;
            }
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            Add(item, false);
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="item">元素</param>
        /// <param name="overwrite">是否在元素已经存在的情况下将其覆盖</param>
        /// <returns>该元素之前是否已经存在</returns>
        public bool Add(T item, bool overwrite)
        {
            Contract.Requires(item != null);

            int index;
            if (_dict.TryGetValue(item, out index))
            {
                if (overwrite)
                    _list[index] = item;

                return true;
            }

            _list.Add(item);
            _dict.Add(item, _list.Count - 1);
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
        public bool Contains(T item)
        {
            return _dict.ContainsKey(item);
        }

        /// <summary>
        /// 复制到指定的数组中
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
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
        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            int index;
            if (_dict.TryGetValue(item, out index))
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
        public T[] ToArray()
        {
            return _list.ToArray();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

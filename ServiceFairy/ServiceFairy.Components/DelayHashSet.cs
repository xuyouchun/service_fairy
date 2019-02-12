using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package;
using Common;
using System.Collections;

namespace ServiceFairy.Components
{
    /// <summary>
    /// 支持延时删除元素的哈希表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class DelayHashSet<T> : ICollection<T>, IEnumerable<T>, IDisposable, ICollection
    {
        public DelayHashSet(TimeSpan delay)
            : this(delay, 0)
        {
            _hs = new HashSet<T>();
        }

        public DelayHashSet(IEqualityComparer<T> equalityComparer, TimeSpan delay)
            : this(delay, 0)
        {
            _hs = new HashSet<T>(equalityComparer);
        }

        private DelayHashSet(TimeSpan delay, int nouse)
        {
            _delay = delay;
            _deletedDict = new Dictionary<T, DateTime>();
            _objMgr.Register(this);
        }

        private readonly HashSet<T> _hs;
        private readonly Dictionary<T, DateTime> _deletedDict;
        private readonly object _syncLocker = new object();

        private readonly TimeSpan _delay;
        private static readonly ObjectManager<DelayHashSet<T>> _objMgr = new ObjectManager<DelayHashSet<T>>(TimeSpan.FromSeconds(1), _CheckExpired);

        private static void _CheckExpired(DelayHashSet<T> obj)
        {
            if (obj._deletedDict.Count == 0)
                return;

            DateTime now = DateTime.UtcNow;
            lock (obj._syncLocker)
            {
                foreach (KeyValuePair<T, DateTime> item in obj._deletedDict)
                {
                    if (item.Value <= now)
                        obj._hs.Remove(item.Key);
                }
            }
        }

        /// <summary>
        /// 添加一个元素
        /// </summary>
        /// <param name="item">添加的元素</param>
        /// <returns></returns>
        public bool Add(T item)
        {
            lock (_syncLocker)
            {
                return _Add(item);
            }
        }

        /// <summary>
        /// 批量添加元素
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public void AddRange(IEnumerable<T> items)
        {
            if (items == null)
                return;

            lock (_syncLocker)
            {
                foreach (T item in items)
                {
                    _Add(item);
                }
            }
        }

        private bool _Add(T item)
        {
            _deletedDict.Remove(item);
            return _hs.Add(item);
        }

        /// <summary>
        /// 删除一个元素
        /// </summary>
        /// <param name="item">添加的元素</param>
        /// <param name="delay">是否延迟删除</param>
        /// <returns></returns>
        public bool Remove(T item, bool delay)
        {
            lock (_syncLocker)
            {
                return _Remove(item, delay, DateTime.UtcNow + _delay);
            }
        }

        /// <summary>
        /// 批量删除元素
        /// </summary>
        /// <param name="items">元素</param>
        /// <param name="delay">是否延迟删除</param>
        public void RemoveRange(IEnumerable<T> items, bool delay = true)
        {
            if (items == null)
                return;

            lock (_syncLocker)
            {
                DateTime delayTime = DateTime.UtcNow + _delay;
                foreach (T item in items)
                {
                    _Remove(item, delay, delayTime);
                }
            }
        }

        private bool _Remove(T item, bool delay, DateTime delayTime)
        {
            bool removed;
            if (!delay)
            {
                removed = _hs.Remove(item);
                if (removed)
                    _deletedDict.Remove(item);
            }
            else
            {
                removed = _hs.Contains(item);
                if (removed && !_deletedDict.ContainsKey(item))
                {
                    _deletedDict.Add(item, delayTime);
                }
            }

            return removed;
        }

        /// <summary>
        /// 清空集合
        /// </summary>
        public void Clear(bool delay)
        {
            if (_hs.Count == 0)
                return;

            if (!delay)
            {
                _hs.Clear();
                _deletedDict.Clear();
            }
            else
            {
                DateTime delayTime = DateTime.UtcNow + _delay;
                foreach (T item in _hs)
                {
                    if (!_deletedDict.ContainsKey(item))
                        _deletedDict.Add(item, delayTime);
                }

                _hs.Clear();
            }
        }

        /// <summary>
        /// 清空集合
        /// </summary>
        public void Clear()
        {
            Clear(true);
        }

        /// <summary>
        /// 是否包含指定的元素
        /// </summary>
        /// <param name="item"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public bool Contains(T item, bool includeDeleted)
        {
            lock (_syncLocker)
            {
                return _hs.Contains(item) && (includeDeleted || !_deletedDict.ContainsKey(item));
            }
        }

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="includeDeleted">是否包含已经延迟删除的项</param>
        /// <returns></returns>
        public int GetCount(bool includeDeleted)
        {
            lock (_syncLocker)
            {
                if (includeDeleted)
                    return _hs.Count;

                return _hs.Count - _deletedDict.Count;
            }
        }

        /// <summary>
        /// 转换为数组
        /// </summary>
        /// <param name="includeDeleted">是否包含延迟删除的元素</param>
        /// <param name="clear">是否清空</param>
        public T[] ToArray(bool includeDeleted, bool clear = false)
        {
            if (_hs.Count == 0)
                return Array<T>.Empty;

            lock (_syncLocker)
            {
                List<T> list = new List<T>();
                foreach (T item in _hs)
                {
                    if (includeDeleted || !_deletedDict.ContainsKey(item))
                        list.Add(item);
                }

                if (clear)
                    Clear();

                return list.ToArray();
            }
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return _hs.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _objMgr.Unregister(this);
        }

        #endregion

        #region ICollection<T> Members

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (_syncLocker)
            {
                _hs.CopyTo(array, arrayIndex);
            }
        }

        public int Count
        {
            get { return _hs.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return Remove(item, true);
        }

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        bool ICollection<T>.Contains(T item)
        {
            return Contains(item, true);
        }

        #endregion

        #region ICollection Members

        void ICollection.CopyTo(Array array, int index)
        {
            lock (_syncLocker)
            {
                if (array is T[])
                {
                    CopyTo((T[])array, index);
                }
                else
                {
                    foreach (T item in _hs)
                    {
                        array.SetValue(item, index++);
                    }
                }
            }
        }

        int ICollection.Count
        {
            get { return _hs.Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return true; }
        }

        object ICollection.SyncRoot
        {
            get { return _syncLocker; }
        }

        #endregion
    }
}

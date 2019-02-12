using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Collection;
using Common.Utility;
using System.Diagnostics.Contracts;
using System.Collections.Concurrent;
using System.Collections;
using System.Runtime.Serialization;

namespace Common.Data.UnionTable.Metadata
{
    /// <summary>
    /// 元数据的对象集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable, DataContract]
    public class MtCollection<T, TOwner> : IEnumerable<T>
        where T : MtBase<TOwner>
        where TOwner : MtBase
    {
        internal MtCollection(TOwner owner)
        {
            Owner = owner;
        }

        private readonly IgnoreCaseConcurrentDictionary<T> _dictOfName = new IgnoreCaseConcurrentDictionary<T>();

        /// <summary>
        /// 拥有者
        /// </summary>
        public TOwner Owner { get; private set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count
        {
            get { return _dictOfName.Count; }
        }

        /// <summary>
        /// 所有名称
        /// </summary>
        /// <returns></returns>
        public ICollection<string> AllNames
        {
            get
            {
                return _dictOfName.Keys;
            }
        }

        /// <summary>
        /// 所有元素
        /// </summary>
        public ICollection<T> Items
        {
            get
            {
                return _dictOfName.Values;
            }
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            Contract.Requires(item != null);

            if (!_dictOfName.TryAdd(item.Name, item))
                throw new InvalidOperationException("已经存在名称为“" + item.Name + "”的元数据");

            item.Owner = Owner;
        }

        /// <summary>
        /// 批量添加元素
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(IEnumerable<T> items)
        {
            Contract.Requires(items != null);

            foreach (T item in items)
            {
                Add(item);
            }
        }

        /// <summary>
        /// 根据名称获取或设置元素
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public T this[string name]
        {
            get
            {
                return Get(name);
            }
            set
            {
                if (value == null)
                    Remove(name);
                else
                    _dictOfName[name] = value;
            }
        }

        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="name"></param>
        public T Remove(string name)
        {
            return _dictOfName.Remove(name);
        }

        /// <summary>
        /// 根据名称获取元素
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public T Get(string name)
        {
            return _dictOfName.GetOrDefault(name);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Collection
{
    /// <summary>
    /// 集合的变化方式
    /// </summary>
    public enum CollectionChangedType
    {
        /// <summary>
        /// 未改变
        /// </summary>
        NoChange,

        /// <summary>
        /// 添加
        /// </summary>
        Add,

        /// <summary>
        /// 删除
        /// </summary>
        Remove,

        /// <summary>
        /// 更新
        /// </summary>
        Update,
    }

    /// <summary>
    /// 哈希表变化事件参数
    /// </summary>
    public class DictionaryChangedEventArgs<TKey, TValue> : EventArgs
    {
        public DictionaryChangedEventArgs(CollectionChangedType changeType, TKey key, TValue oldValue, TValue newValue)
        {
            ChangeType = changeType;
            Key = key;
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// 变化方式
        /// </summary>
        public CollectionChangedType ChangeType { get; private set; }

        /// <summary>
        /// 键
        /// </summary>
        public TKey Key { get; set; }

        /// <summary>
        /// 源值
        /// </summary>
        public TValue OldValue { get; set; }

        /// <summary>
        /// 新值
        /// </summary>
        public TValue NewValue { get; set; }
    }

    /// <summary>
    /// 哈希表变化事件委托
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DictionaryChangedChanged<TKey, TValue>(object sender, DictionaryChangedEventArgs<TKey, TValue> e);

}

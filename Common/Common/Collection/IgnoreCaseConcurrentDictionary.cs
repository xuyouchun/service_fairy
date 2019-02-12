using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace Common.Collection
{
    /// <summary>
    /// 以忽略大小写的字符串作为键的哈希表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IgnoreCaseConcurrentDictionary<T> : ConcurrentDictionary<string, T>
    {
        public IgnoreCaseConcurrentDictionary()
            : base(IgnoreCaseEqualityComparer.Instance)
        {

        }
    }
}

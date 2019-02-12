using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Collection
{
    /// <summary>
    /// 键为忽略大小写的字符串的哈希表
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class IgnoreCaseDictionary<TValue> : Dictionary<string, TValue>
    {
        public IgnoreCaseDictionary()
            : base(IgnoreCaseEqualityComparer.Instance)
        {

        }
    }

    #region Class IgnoreCaseEqualityComparer ...

    public class IgnoreCaseEqualityComparer : IEqualityComparer<string>
    {
        #region IEqualityComparer<string> Members

        public bool Equals(string x, string y)
        {
            return string.Compare(x, y, true) == 0;
        }

        public int GetHashCode(string obj)
        {
            if (obj == null)
                return 0;

            return obj.ToLower().GetHashCode();
        }

        #endregion

        public static readonly IgnoreCaseEqualityComparer Instance = new IgnoreCaseEqualityComparer();
    }

    #endregion
}

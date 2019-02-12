using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Collection
{
    /// <summary>
    /// 忽略字符串大小写的HashSet
    /// </summary>
    public class IgnoreCaseHashSet : HashSet<string>
    {
        public IgnoreCaseHashSet()
            : base(IgnoreCaseEqualityComparer.Instance)
        {

        }
    }
}

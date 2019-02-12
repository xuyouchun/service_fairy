using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Collection
{
    /// <summary>
    /// 忽略字符串大小写的HashSet
    /// </summary>
    [Serializable, System.Diagnostics.DebuggerDisplay("Count = {Count}")]
    public class IgnoreCaseHashSet : HashSet<string>
    {
        public IgnoreCaseHashSet()
            : base(IgnoreCaseEqualityComparer.Instance)
        {

        }

        public IgnoreCaseHashSet(IEnumerable<string> items)
            : base(items ?? Array<string>.Empty, IgnoreCaseEqualityComparer.Instance)
        {

        }
    }
}

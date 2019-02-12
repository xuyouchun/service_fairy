using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    /// <summary>
    /// 列的索引类型
    /// </summary>
    public enum DbColumnIndexType
    {
        /// <summary>
        /// 无索引
        /// </summary>
        None = 0,

        /// <summary>
        /// 辅助查询索引
        /// </summary>
        [Desc("辅助索引")]
        Slave = 1,

        /// <summary>
        /// 主查询索引
        /// </summary>
        [Desc("主索引")]
        Master = 2,
    }
}

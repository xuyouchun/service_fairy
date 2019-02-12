using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    /// <summary>
    /// 数据库操作的类型
    /// </summary>
    public enum DbAction
    {
        /// <summary>
        /// 选择数据
        /// </summary>
        Select,

        /// <summary>
        /// 更新数据
        /// </summary>
        Update,

        /// <summary>
        /// 插入数据
        /// </summary>
        Insert,

        /// <summary>
        /// 删除数据
        /// </summary>
        Delete,
    }
}

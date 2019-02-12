using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    /// <summary>
    /// 表连接的用途
    /// </summary>
    public enum DbConnectionUsageType
    {
        /// <summary>
        /// 元数据
        /// </summary>
        Metadata,

        /// <summary>
        /// 数据
        /// </summary>
        Data,

        /// <summary>
        /// 备份
        /// </summary>
        Back,

        /// <summary>
        /// 缓存
        /// </summary>
        Cache,
    }
}

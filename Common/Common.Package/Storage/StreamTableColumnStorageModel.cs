using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Package.Storage
{
    /// <summary>
    /// 列的类型
    /// </summary>
    public enum StreamTableColumnStorageModel
    {
        /// <summary>
        /// 自动识别
        /// </summary>
        Auto,

        /// <summary>
        /// 单个元素
        /// </summary>
        Element,

        /// <summary>
        /// 定长数组
        /// </summary>
        Array,

        /// <summary>
        /// 变长数组
        /// </summary>
        DynamicArray,
    }
}

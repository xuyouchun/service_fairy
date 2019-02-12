using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts
{
    /// <summary>
    /// 数据缓冲区的格式
    /// </summary>
    public enum BufferType
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 字节数组
        /// </summary>
        Bytes = 1,

        /// <summary>
        /// 字节流
        /// </summary>
        Stream = 2,
    }
}

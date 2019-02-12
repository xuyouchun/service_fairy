using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts
{
    /// <summary>
    /// 数据格式
    /// </summary>
    public enum DataFormat : byte
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 二进制
        /// </summary>
        Binary = 1,

        /// <summary>
        /// Json
        /// </summary>
        Json = 2,

        /// <summary>
        /// Xml
        /// </summary>
        Xml = 3,


    }
}

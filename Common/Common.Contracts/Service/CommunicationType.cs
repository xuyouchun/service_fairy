using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 通信策略
    /// </summary>
    public enum CommunicationType
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// HTTP
        /// </summary>
        [Desc("HTTP")]
        Http = 1,

        /// <summary>
        /// WCF TCP方式
        /// </summary>
        [Desc("WCF TCP")]
        WTcp = 2,

        /// <summary>
        /// 基本的Socket方式，自定义编码
        /// </summary>
        [Desc("TCP")]
        Tcp = 3,
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts;
using Common.Contracts.Service;

namespace ServiceFairy.Client
{
    /// <summary>
    /// AppClient的运行状态
    /// </summary>
    [Serializable, DataContract]
    public class AppClientRuntimeInfo
    {
        public AppClientRuntimeInfo()
        {

        }

        /// <summary>
        /// CPU使用率
        /// </summary>
        [DataMember]
        public float CpuRate { get; set; }

        /// <summary>
        /// 内存（工作设置）
        /// </summary>
        [DataMember]
        public long WorkingSetMemorySize { get; set; }

        /// <summary>
        /// 内存（专用）
        /// </summary>
        [DataMember]
        public long PrivateMemorySize { get; set; }

        /// <summary>
        /// 在线用户统计数据
        /// </summary>
        [DataMember]
        public OnlineUserStatInfo OnlineUserStatInfo { get; set; }
    }
}

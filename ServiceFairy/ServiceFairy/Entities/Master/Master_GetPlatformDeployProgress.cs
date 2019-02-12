using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common;
using Common.Contracts;

namespace ServiceFairy.Entities.Master
{
    /// <summary>
    /// 获取平台部署进度－应答
    /// </summary>
    [Serializable, DataContract]
    public class Master_GetPlatformDeployProgress_Reply : ReplyEntity
    {
        /// <summary>
        /// 各终端的部署进度
        /// </summary>
        [DataMember]
        public PlatformDeployProgress[] Progresses { get; set; }
    }

    /// <summary>
    /// 终端部署进度
    /// </summary>
    [Serializable, DataContract]
    public class PlatformDeployProgress
    {
        /// <summary>
        /// 终端标识
        /// </summary>
        [DataMember]
        public Guid ClientID { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember]
        public PlatformDeployStatus Status { get; set; }

        /// <summary>
        /// 开始部署时间
        /// </summary>
        [DataMember]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 是否为连接状态
        /// </summary>
        [DataMember]
        public bool Connected { get; set; }
    }

    /// <summary>
    /// 终端部署状态
    /// </summary>
    public enum PlatformDeployStatus
    {
        /// <summary>
        /// 正在等待
        /// </summary>
        [Desc("正在等待")]
        Waiting,

        /// <summary>
        /// 正在部署
        /// </summary>
        [Desc("正在部署")]
        Deploying,

        /// <summary>
        /// 部署完成
        /// </summary>
        [Desc("部署完成")]
        Completed,

        /// <summary>
        /// 超时
        /// </summary>
        [Desc("超时")]
        Timeout,

        /// <summary>
        /// 出现错误
        /// </summary>
        [Desc("出现错误")]
        Error,
    }
}

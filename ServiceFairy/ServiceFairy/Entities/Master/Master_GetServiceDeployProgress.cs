using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;
using Common;
using Common.Contracts;

namespace ServiceFairy.Entities.Master
{
    /// <summary>
    /// 获取服务的部署状态－请求
    /// </summary>
    [Serializable, DataContract]
    public class Master_GetServiceDeployProgress_Request : RequestEntity
    {
        /// <summary>
        /// 服务
        /// </summary>
        [DataMember]
        public ServiceDesc[] ServiceDescs { get; set; }
    }

    /// <summary>
    /// 获取服务的部署状态－应答
    /// </summary>
    [Serializable, DataContract]
    public class Master_GetServiceDeployProgress_Reply : ReplyEntity
    {
        /// <summary>
        /// 部署进度
        /// </summary>
        [DataMember]
        public ServiceDeployProgress[] Progresses { get; set; }
    }

    /// <summary>
    /// 服务的部署状态
    /// </summary>
    [Serializable, DataContract]
    public class ServiceDeployProgress
    {
        /// <summary>
        /// 服务
        /// </summary>
        [DataMember]
        public ServiceDesc ServiceDesc { get; set; }

        /// <summary>
        /// 所在的终端
        /// </summary>
        [DataMember]
        public Guid ClientID { get; set; }

        /// <summary>
        /// 服务的部署状态
        /// </summary>
        [DataMember]
        public ServiceDeployStatus Status { get; set; }

        /// <summary>
        /// 开始时间
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
    /// 服务的部署状态
    /// </summary>
    public enum ServiceDeployStatus
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

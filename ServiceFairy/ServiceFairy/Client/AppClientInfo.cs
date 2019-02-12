using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;
using System.Runtime.Serialization;
using Common.Communication.Wcf;
using Common.Contracts;

namespace ServiceFairy.Client
{
    /// <summary>
    /// 终端
    /// </summary>
    [Serializable, DataContract]
    public class AppClientInfo
    {
        public AppClientInfo()
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clientId">终端ID</param>
        public AppClientInfo(Guid clientId)
        {
            ClientId = clientId;
        }

        /// <summary>
        /// 终端的ID
        /// </summary>
        [DataMember]
        public Guid ClientId { get; set; }

        /// <summary>
        /// 可用IP地址
        /// </summary>
        [DataMember]
        public string[] IPs { get; set; }

        /// <summary>
        /// 连接时间（UTC时间）
        /// </summary>
        [DataMember]
        public DateTime ConnectedTime { get; set; }

        /// <summary>
        /// 服务信息
        /// </summary>
        [DataMember]
        public ServiceInfo[] ServiceInfos { get; set; }

        /// <summary>
        /// 通信方式
        /// </summary>
        [DataMember]
        public CommunicationOption[] Communications { get; set; }

        /// <summary>
        /// 信道
        /// </summary>
        [DataMember]
        public AppInvokeInfo[] InvokeInfos { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Desc { get; set; }

        /// <summary>
        /// 主机名
        /// </summary>
        [DataMember]
        public string HostName { get; set; }

        /// <summary>
        /// 是否为有效状态
        /// </summary>
        [DataMember]
        public bool Avaliable { get; set; }

        /// <summary>
        /// 是否正在部署
        /// </summary>
        [DataMember]
        public bool Deploying { get; set; }

        /// <summary>
        /// 平台版本
        /// </summary>
        [DataMember]
        public Guid PlatformDeployId { get; set; }

        /// <summary>
        /// 向终端发送的命令
        /// </summary>
        [DataMember]
        public string Command { get; set; }

        /// <summary>
        /// 运行状态
        /// </summary>
        [DataMember]
        public AppClientRuntimeInfo RuntimeInfo { get; set; }

        public override string ToString()
        {
            return string.Format("ClientID={0}", ClientId);
        }
    }
}

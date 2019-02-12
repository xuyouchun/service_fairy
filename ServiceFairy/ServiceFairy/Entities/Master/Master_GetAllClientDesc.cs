using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Utility;
using Common.Contracts;

namespace ServiceFairy.Entities.Master
{
    /// <summary>
    /// 获取服务终端列表－应答
    /// </summary>
    [Serializable, DataContract]
    public class Master_GetAllClientDesc_Reply : ReplyEntity
    {
        /// <summary>
        /// 服务终端列表
        /// </summary>
        [DataMember]
        public ClientDesc[] ClientDescs;
    }

    /// <summary>
    /// 服务终端描述信息
    /// </summary>
    [Serializable, DataContract]
    public class ClientDesc
    {
        /// <summary>
        /// 服务终端ID
        /// </summary>
        [DataMember]
        public Guid ClientID { get; set; }

        /// <summary>
        /// 名称
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
        /// 启动时间
        /// </summary>
        [DataMember]
        public DateTime ConnectedTime { get; set; }

        /// <summary>
        /// 服务数量
        /// </summary>
        [DataMember]
        public int ServiceCount { get; set; }

        /// <summary>
        /// 信道数量
        /// </summary>
        [DataMember]
        public int CommunicationCount { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        [DataMember]
        public string[] IPs { get; set; }

        public override int GetHashCode()
        {
            return ClientID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(ClientDesc))
                return false;

            return ClientID == ((ClientDesc)obj).ClientID;
        }

        public static bool operator ==(ClientDesc obj1, ClientDesc obj2)
        {
            return object.Equals(obj1, obj2);
        }

        public static bool operator !=(ClientDesc obj1, ClientDesc obj2)
        {
            return !object.Equals(obj1, obj2);
        }

        public override string ToString()
        {
            return StringUtility.GetFirstNotNullOrWhiteSpaceString(Title, HostName, ClientID.ToString());
        }
    }
}

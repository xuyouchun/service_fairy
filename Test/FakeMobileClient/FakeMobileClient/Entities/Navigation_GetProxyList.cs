using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FakeMobileClient.Entities
{
    /// <summary>
    /// 获取代理列表－请求
    /// </summary>
    [Serializable, DataContract]
    public class Navigation_GetProxyList_Request
    {
        /// <summary>
        /// 最大数量
        /// </summary>
        [DataMember]
        public int MaxCount { get; set; }

        /// <summary>
        /// 所支持的通信方式
        /// </summary>
        [DataMember]
        public CommunicationType CommunicationType { get; set; }

        /// <summary>
        /// 代理的类型
        /// </summary>
        [DataMember]
        public TrayAppServiceProxyType ProxyType { get; set; }
    }

    /// <summary>
    /// 获取代理列表－应答
    /// </summary>
    [Serializable, DataContract]
    public class Navigation_GetProxyList_Reply
    {
        /// <summary>
        /// 通讯列表
        /// </summary>
        [DataMember]
        public CommunicationOption[] CommunicationOptions { get; set; }
    }

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
        Http = 1,

        /// <summary>
        /// TCP
        /// </summary>
        Tcp = 2,
    }

    [Serializable, DataContract]
    public class CommunicationOption
    {
        public CommunicationOption()
        {

        }

        public CommunicationOption(ServiceAddress address, CommunicationType communicationType = CommunicationType.Tcp, bool supportDuplex = false)
        {
            Address = address;
            CommunicationType = communicationType;
            SupportDuplex = supportDuplex;
        }

        /// <summary>
        /// 终端
        /// </summary>
        [DataMember]
        public ServiceAddress Address { get; set; }

        /// <summary>
        /// 通信方式
        /// </summary>
        [DataMember]
        public CommunicationType CommunicationType { get; set; }

        /// <summary>
        /// 是否支持双向通信
        /// </summary>
        [DataMember]
        public bool SupportDuplex { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Address, CommunicationType);
        }
    }

    [Serializable, DataContract]
    public class ServiceAddress
    {
        public ServiceAddress(string address, int port)
        {
            Address = address;
            Port = port;
        }

        /// <summary>
        /// 地址
        /// </summary>
        [DataMember]
        public string Address { get; private set; }

        /// <summary>
        /// 端口号
        /// </summary>
        [DataMember]
        public int Port { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Address, Port);
        }
    }

    /// <summary>
    /// 代理类型
    /// </summary>
    public enum TrayAppServiceProxyType
    {

    }
}

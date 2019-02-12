using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using Common.Communication.Wcf.Encoders;
using Common.Contracts;
using Common.Contracts.Service;

namespace Common.Communication.Wcf.Service
{
    /// <summary>
    /// 实体类消息
    /// </summary>
    public class EntityMessage : Message
    {
        public EntityMessage()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data"></param>
        public EntityMessage(CommunicateData data)
        {
            Data = data;
        }

        /// <summary>
        /// 实体
        /// </summary>
        public CommunicateData Data { get; set; }

        /// <summary>
        /// 整型值，用于分发调用时的负载均衡
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 调用方法
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 调用者
        /// </summary>
        public ServiceEndPoint Caller { get; set; }

        /// <summary>
        /// 会话标识
        /// </summary>
        public Guid SessionId { get; set; }

        /// <summary>
        /// 调用设置
        /// </summary>
        public CallingSettings Settings { get; set; }

        private static readonly MessageVersion _messageVersion = MessageVersion.Default;

        private readonly MessageHeaders _headers = new MessageHeaders(_messageVersion);

        /// <summary>
        /// Headers
        /// </summary>
        public override MessageHeaders Headers
        {
            get { return _headers; }
        }

        protected override void OnWriteBodyContents(System.Xml.XmlDictionaryWriter writer)
        {
            
        }

        private MessageProperties _properties;

        /// <summary>
        /// Properties
        /// </summary>
        public override MessageProperties Properties
        {
            get { return _properties ?? (_properties = new MessageProperties()); }
        }

        /// <summary>
        /// Message Version
        /// </summary>
        public override MessageVersion Version
        {
            get { return _messageVersion; }
        }
    }
}

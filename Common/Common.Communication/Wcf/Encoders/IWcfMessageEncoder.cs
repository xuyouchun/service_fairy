using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf.Service;
using Common.Contracts;
using System.ServiceModel.Channels;

namespace Common.Communication.Wcf.Encoders
{
    /// <summary>
    /// WCF消息编码器
    /// </summary>
    interface IWcfMessageEncoder
    {
        /// <summary>
        /// 序列化方式
        /// </summary>
        DataFormat Format { get; }

        /// <summary>
        /// ContentType
        /// </summary>
        string GetContentType();

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="message"></param>
        /// <param name="maxMessageSize"></param>
        /// <param name="bufferManager"></param>
        /// <param name="messageOffset"></param>
        /// <param name="strategy"></param>
        /// <returns></returns>
        ArraySegment<byte> Serialize(EntityMessage message, int maxMessageSize, BufferManager bufferManager, int messageOffset, IWcfMessageEncoderStrategy strategy);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="message"></param>
        /// <param name="strategy"></param>
        void Deserialize(ref EntityMessage message, byte[] buffer, int offset, int count, IWcfMessageEncoderStrategy strategy);
    }
}

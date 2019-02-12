using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf.Service;
using Common.Contracts;
using System.ServiceModel.Channels;

namespace Common.Communication.Wcf.Encoders
{
    abstract class WcfMessageEncoderBase : IWcfMessageEncoder
    {
        public WcfMessageEncoderBase(DataFormat format)
        {
            Format = format;
        }

        /// <summary>
        /// 格式
        /// </summary>
        public DataFormat Format { get; private set; }

        /// <summary>
        /// 获取ContentType
        /// </summary>
        /// <returns></returns>
        public virtual string GetContentType()
        {
            return "application/octet-stream";
        }

        public IWcfMessageEncoderStrategy Strategy { get; private set; }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public abstract ArraySegment<byte> Serialize(EntityMessage message, int maxMessageSize, BufferManager bufferManager, int messageOffset, IWcfMessageEncoderStrategy strategy);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="message"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public abstract void Deserialize(ref EntityMessage message, byte[] buffer, int offset, int count, IWcfMessageEncoderStrategy strategy);
    }
}

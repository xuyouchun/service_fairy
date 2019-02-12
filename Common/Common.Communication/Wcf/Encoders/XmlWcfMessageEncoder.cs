using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf.Service;
using Common.Contracts;
using System.ServiceModel.Channels;

namespace Common.Communication.Wcf.Encoders
{
    class XmlWcfMessageEncoder : WcfMessageEncoderBase
    {
        public XmlWcfMessageEncoder()
            : base(DataFormat.Xml)
        {

        }

        public override string GetContentType()
        {
            return "text/xml";
        }

        public override ArraySegment<byte> Serialize(EntityMessage message, int maxMessageSize, BufferManager bufferManager, int messageOffset, IWcfMessageEncoderStrategy strategy)
        {
            throw new NotImplementedException();
        }

        public override void Deserialize(ref EntityMessage message, byte[] buffer, int offset, int count, IWcfMessageEncoderStrategy strategy)
        {
            throw new NotImplementedException();
        }
    }
}

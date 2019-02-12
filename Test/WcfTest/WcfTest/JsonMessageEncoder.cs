using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.Xml;
using System.IO;

namespace WcfTest
{
    public class JsonMessageEncoder : MessageEncoder
    {
        public JsonMessageEncoder(JsonMessageEncoderFactory encoderFactory)
        {
            _factory = encoderFactory;
            _innerEncoder = _factory.InnerMessageEncodingBindingElement.CreateMessageEncoderFactory().Encoder;
        }

        private readonly JsonMessageEncoderFactory _factory;
        private readonly MessageEncoder _innerEncoder;

        public override string ContentType
        {
            get { return "application/soap+msbin1"; /* _innerEncoder.ContentType;*/ }
        }

        public override string MediaType
        {
            get { return ContentType; }
        }

        public override MessageVersion MessageVersion
        {
            get { return _innerEncoder.MessageVersion; }
        }

        public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
        {
            return _innerEncoder.ReadMessage(buffer, bufferManager, contentType);
        }

        public override Message ReadMessage(System.IO.Stream stream, int maxSizeOfHeaders, string contentType)
        {
            return _innerEncoder.ReadMessage(stream, maxSizeOfHeaders, contentType);
        }

        public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
        {
            return _innerEncoder.WriteMessage(message, maxMessageSize, bufferManager, messageOffset);
        }

        public override void WriteMessage(Message message, System.IO.Stream stream)
        {
            _innerEncoder.WriteMessage(message, stream);
        }
    }

    class MyMessage : Message
    {
        public MyMessage(object entity)
        {
            Entity = entity;
        }

        public object Entity { get; private set; }

        public override MessageHeaders Headers
        {
            get { return null; }
        }

        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            
        }

        public override MessageProperties Properties
        {
            get { return new MessageProperties(); }
        }

        public override MessageVersion Version
        {
            get { return MessageVersion.Default; }
        }
    }

}

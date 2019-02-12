using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.Xml;
using System.ServiceModel;

namespace WcfTest
{
    public class JsonMessageEncodingBindingElement : MessageEncodingBindingElement
    {
        public JsonMessageEncodingBindingElement(MessageEncodingBindingElement innerMessageEncodingBindingElement)
        {
            _innerMessageEncodingBindingElement = innerMessageEncodingBindingElement;
            _readerQuotas = new XmlDictionaryReaderQuotas();
        }

        private MessageEncodingBindingElement _innerMessageEncodingBindingElement;
        private XmlDictionaryReaderQuotas _readerQuotas;

        public override MessageEncoderFactory CreateMessageEncoderFactory()
        {
            return new JsonMessageEncoderFactory(_innerMessageEncodingBindingElement);
        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return _innerMessageEncodingBindingElement.MessageVersion;
            }
            set
            {
                _innerMessageEncodingBindingElement.MessageVersion = value;
            }
        }

        public override BindingElement Clone()
        {
            return new JsonMessageEncodingBindingElement(_innerMessageEncodingBindingElement);
        }

        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            context.BindingParameters.Add(this);
            return context.BuildInnerChannelFactory<TChannel>();
        }

        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
        {
            context.BindingParameters.Add(this);
            return context.BuildInnerChannelListener<TChannel>();
        }

        public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
        {
            context.BindingParameters.Add(this);
            return context.CanBuildInnerChannelFactory<TChannel>();
        }

        public override bool CanBuildChannelListener<TChannel>(BindingContext context)
        {
            context.BindingParameters.Add(this);
            return context.CanBuildInnerChannelListener<TChannel>();
        }

        public override T GetProperty<T>(BindingContext context)
        {
            if (typeof(T) == typeof(XmlDictionaryReaderQuotas))
                return _readerQuotas as T;

            return base.GetProperty<T>(context);
        }
    }
}

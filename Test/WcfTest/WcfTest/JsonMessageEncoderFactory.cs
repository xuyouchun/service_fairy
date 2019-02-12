using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace WcfTest
{
    public class JsonMessageEncoderFactory : MessageEncoderFactory
    {
        public JsonMessageEncoderFactory(MessageEncodingBindingElement innerMessageEncodingBindingElement)
        {
            this._innerMessageEncodingBindingElement = innerMessageEncodingBindingElement;
        }

        private MessageEncodingBindingElement _innerMessageEncodingBindingElement;

        public MessageEncodingBindingElement InnerMessageEncodingBindingElement
        {
            get { return _innerMessageEncodingBindingElement; }
        }

        public override MessageEncoder Encoder
        {
            get { return new JsonMessageEncoder(this); }
        }

        public override MessageVersion MessageVersion
        {
            get { return _innerMessageEncodingBindingElement.MessageVersion; }
        }
    }
}

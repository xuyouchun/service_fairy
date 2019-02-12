using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;

namespace WcfTest
{
    public class EntityMessage : Message
    {
        public EntityMessage(object entity, string action)
        {
            Entity = entity;
            _headers.Action = action;
            //_message = Message.CreateMessage(MessageVersion.Soap12, "http://tempuri.org/ITestService/Operate");
        }

        public object Entity { get; private set; }

        private readonly MessageHeaders _headers = new MessageHeaders(MessageVersion.Default);

        public override MessageHeaders Headers
        {
            get { return _headers; }
        }

        protected override void OnWriteBodyContents(System.Xml.XmlDictionaryWriter writer)
        {
            
        }

        private readonly MessageProperties _properties = new MessageProperties();

        public override MessageProperties Properties
        {
            get { return _properties; }
        }

        public override MessageVersion Version
        {
            get { return MessageVersion.Default; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.Xml;
using System.IO;

namespace WcfTest
{
    public class JsonMessageFormatter : IClientMessageFormatter, IDispatchMessageFormatter
    {
        public object DeserializeReply(Message message, object[] parameters)
        {
            return null;
        }

        public Message SerializeRequest(MessageVersion messageVersion, object[] parameters)
        {
            return null;
        }

        public void DeserializeRequest(Message message, object[] parameters)
        {
            parameters[0] = new Input(1, 2);
            return;
        }

        public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
        {
            

            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;

namespace Common.Communication.Wcf.Behaviors
{
    class MessageFormatter : IClientMessageFormatter, IDispatchMessageFormatter
    {
        public object DeserializeReply(Message message, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Message SerializeRequest(MessageVersion messageVersion, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public void DeserializeRequest(Message message, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
        {
            throw new NotImplementedException();
        }
    }
}

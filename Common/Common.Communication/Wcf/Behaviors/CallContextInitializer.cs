using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using Common.Communication.Wcf.Service;
using System.ServiceModel.Channels;

namespace Common.Communication.Wcf.Behaviors
{
    class CallContextInitializer : ICallContextInitializer
    {
        public CallContextInitializer(WcfListener listener)
        {
            _listener = listener;
        }

        private readonly WcfListener _listener;

        public void AfterInvoke(object correlationState)
        {
            
        }

        public object BeforeInvoke(System.ServiceModel.InstanceContext instanceContext, System.ServiceModel.IClientChannel channel, System.ServiceModel.Channels.Message message)
        {
            EntityMessage msg = message as EntityMessage;
            if (msg == null)
                return null;

            if (message.Properties.ContainsKey(HttpRequestMessageProperty.Name))
            {
                HttpRequestMessageProperty requestProperty = (HttpRequestMessageProperty)message.Properties[HttpRequestMessageProperty.Name];


            }

            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel;
using Common.Communication.Wcf.Service;
using Common.Communication.Wcf.Encoders;
using Common.Contracts;

namespace Common.Communication.Wcf.Behaviors
{
    class MessageInspector : IClientMessageInspector, IDispatchMessageInspector
    {
        public MessageInspector(WcfListener listener)
        {

        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            return null;
        }

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            EntityMessage msg = reply as EntityMessage;
            if (msg == null || msg.Data == null)
                return;

            HttpResponseMessageProperty rspProperty = null;
            if (reply.Properties.ContainsKey(HttpResponseMessageProperty.Name))
                rspProperty = reply.Properties[HttpResponseMessageProperty.Name] as HttpResponseMessageProperty;

            if (rspProperty == null)
                reply.Properties[HttpResponseMessageProperty.Name] = (rspProperty = new HttpResponseMessageProperty());

            string contentType = EncoderUtility.GetContentType(msg.Data.DataFormat);
            if (contentType != null)
                rspProperty.Headers["content-type"] = contentType;
        }
    }
}

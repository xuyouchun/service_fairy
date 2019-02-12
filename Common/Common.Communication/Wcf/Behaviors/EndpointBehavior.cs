using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Common.Communication.Wcf.Behaviors
{
    class EndpointBehavior : IEndpointBehavior
    {
        public EndpointBehavior(WcfListener listener)
        {
            _listener = listener;
        }

        private readonly WcfListener _listener;

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new MessageInspector(_listener));
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new MessageInspector(_listener));

            foreach (var operation in endpointDispatcher.DispatchRuntime.Operations)
            {
                operation.CallContextInitializers.Add(new CallContextInitializer(_listener));
            }
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Common.Communication.Wcf.Behaviors
{
    class OperationBehavior : IOperationBehavior
    {
        public OperationBehavior(WcfListener listener)
        {
            _listener = listener;
        }

        private readonly WcfListener _listener;

        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
            
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            /*
            clientOperation.SerializeRequest = true;
            clientOperation.DeserializeReply = true;
            clientOperation.Formatter = new MessageFormatter();
             * */
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            /*
            dispatchOperation.DeserializeRequest = true;
            dispatchOperation.SerializeReply = true;
            dispatchOperation.Formatter = new MessageFormatter();

            dispatchOperation.ParameterInspectors.Add(new ParameterInspector());
            */

            dispatchOperation.Invoker = new OperationInvoker(_listener);
        }

        public void Validate(OperationDescription operationDescription)
        {
            
        }
    }
}

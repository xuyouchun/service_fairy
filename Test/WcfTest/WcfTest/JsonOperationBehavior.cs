using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace WcfTest
{
    public class JsonOperationBehavior : IOperationBehavior
    {
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
            
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            clientOperation.SerializeRequest = true;
            clientOperation.DeserializeReply = true;
            clientOperation.Formatter = new JsonMessageFormatter();
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            //dispatchOperation.DeserializeRequest = true;
            //dispatchOperation.SerializeReply = true;
            dispatchOperation.Formatter = new JsonMessageFormatter();
            //dispatchOperation.ParameterInspectors.Add(new JsonParameterInspector());
            //dispatchOperation.Invoker = new JsonOperationInvoker();
        }

        public void Validate(OperationDescription operationDescription)
        {
            
        }
    }
}

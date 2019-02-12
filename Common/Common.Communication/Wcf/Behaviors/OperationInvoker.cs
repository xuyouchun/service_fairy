using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using Common.Communication.Wcf.Strategies;
using Common.Communication.Wcf.Service;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Contracts;
using Common.Package;

namespace Common.Communication.Wcf.Behaviors
{
    class OperationInvoker : IOperationInvoker
    {
        public OperationInvoker(WcfListener listener)
        {
            _listener = listener;
        }

        private readonly WcfListener _listener;

        public object[] AllocateInputs()
        {
            return new object[1];
        }

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            outputs = null;
            EntityMessage input = (EntityMessage)inputs[0];

            try
            {
                IWcfCommunicationStrategy strategy = WcfFactory.CreateConnectionStrategy(_listener.Option.Type);
                return strategy.Invoke(_listener, input);
            }
            catch (ServiceException ex)
            {
                LogManager.LogError(ex);
                CommunicateData communicateData = DataBufferParser.Serialize(null, input.Data.DataFormat, ex.StatusCode, ex.Message);
                return new EntityMessage(communicateData);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                CommunicateData communicateData = DataBufferParser.Serialize(null, input.Data.DataFormat, (int)ServiceStatusCode.ServerError);
                return new EntityMessage(communicateData);
            }
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronous
        {
            get { return true; }
        }
    }
}

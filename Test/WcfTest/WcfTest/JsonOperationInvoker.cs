using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;

namespace WcfTest
{
    public class JsonOperationInvoker : IOperationInvoker
    {
        public object[] AllocateInputs()
        {
            return new object[1];
        }

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            outputs = null;

            return new object[] {
                new Output { Sum = 3, Diff = -1 }
            };
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            return null;
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            outputs = null;
            return null;
        }

        public bool IsSynchronous
        {
            get { return true; }
        }
    }
}

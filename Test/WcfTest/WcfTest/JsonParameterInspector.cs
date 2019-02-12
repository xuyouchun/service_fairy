using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;

namespace WcfTest
{
    public class JsonParameterInspector : IParameterInspector
    {
        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            return;
        }

        public object BeforeCall(string operationName, object[] inputs)
        {
            return null;
        }
    }
}

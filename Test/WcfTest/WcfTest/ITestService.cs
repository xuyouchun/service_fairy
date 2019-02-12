using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;

namespace WcfTest
{
    [ServiceContract(Name = "svr")]
    public interface ITestService
    {
        [OperationContract(Action = "s")]
        Output Operate(Input input);
    }
}


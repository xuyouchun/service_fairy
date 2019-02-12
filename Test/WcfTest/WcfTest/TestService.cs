using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;

namespace WcfTest
{
    public class TestService : ITestService
    {
        public Output Operate(Input input)
        {
            return new Output { Diff = 1, Sum = 3 };
        }
    }
}

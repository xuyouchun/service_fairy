using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.SystemInvoke;

namespace TestEvent
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SystemInvoker invoker = new SystemInvoker("127.0.0.1:8090"))
            {
                invoker.Test.StartTest("mytest", "args");

                return;
            }
        }
    }
}

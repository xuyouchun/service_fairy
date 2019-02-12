using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class MyCoreClass
    {
        IMyInterface _interface = new MyInterface();

        public void Call()
        {
            _interface.MyMethod();
        }
    }

    class MyInterface : IMyInterface
    {
        public void MyMethod()
        {
            Console.WriteLine("Hello MyMethod");
        }
    }
}

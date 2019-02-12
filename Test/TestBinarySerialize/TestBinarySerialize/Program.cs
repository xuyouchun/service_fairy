using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using System.Diagnostics;
using Common.Package;
using ClassLibrary1;

namespace TestBinarySerialize
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain domain = AppDomain.CreateDomain("MyAppDomain");
            //Class1 obj = (Class1)domain.CreateInstanceFromAndUnwrap("ClassLibrary1.dll", "ClassLibrary1.Class1");
            Class1 obj = new Class1();

            Input input = new Input { InputString = "Hello World!" };
            Output output = obj.Request(input);


            CountStopwatch sw = CountStopwatch.StartNew();

            for (int k = 0; k < 5; k++)
            {
                ThreadUtility.StartNew(delegate {

                    while (true)
                    {
                        obj.Request(input);
                        sw.Increment();
                    }

                });
            }

            sw.AutoTrace();
        }

        [Serializable]
        class MyClass
        {
            public string UserName = "UserName";

            public int UserId = 100;

            public int Age = 20;

            public string Detail = "Detail";
        }

        [Serializable]
        class MyClass2
        {
            MyClass Obj1 = new MyClass(), Obj2 = new MyClass();
        }
    }
}

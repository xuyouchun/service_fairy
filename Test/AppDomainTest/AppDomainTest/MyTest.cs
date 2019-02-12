using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AppDomainTest
{
    class MyTest : MarshalByRefObject, ICloneable
    {
        public object Clone()
        {
            string exePath = @"D:\Work\Dev\Test\AppDomainTest\ClassLibrary1\bin\Debug\ClassLibrary1.dll";
            Assembly assembly = Assembly.UnsafeLoadFrom(exePath);

            return "Hello World!";
        }
    }
}

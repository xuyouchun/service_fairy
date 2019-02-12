using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using System.Runtime.Serialization;

namespace CommonTest
{
    class Json
    {
        [DataContract]
        class MyClass
        {
            public MyClass(int value)
            {
                Value = value;
            }

            [DataMember]
            public string Msg { get; set; }

            [DataMember]
            public int Value { get; private set; }
        }

        public static void MyMethod()
        {
            
        }
    }
}

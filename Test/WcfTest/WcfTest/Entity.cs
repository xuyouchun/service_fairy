using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WcfTest
{
    [DataContract]
    public class Input
    {
        public Input()
        {

        }

        public Input(int a, int b)
        {
            A = a;
            B = b;
        }

        [DataMember]
        public int A { get; set; }

        [DataMember]
        public int B { get; set; }
    }

    [DataContract]
    public class Output
    {
        [DataMember]
        public int Sum { get; set; }

        [DataMember]
        public int Diff { get; set; }
    }
}

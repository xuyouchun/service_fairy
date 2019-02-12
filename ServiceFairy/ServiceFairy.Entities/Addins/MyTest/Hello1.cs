using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ServiceFairy.Entities.Addins.MyTest
{
    [Serializable, DataContract]
    public class Hello1_Request : RequestEntity
    {
        [DataMember]
        public string Arg { get; set; }
    }

    [Serializable, DataContract]
    public class Hello1_Reply : ReplyEntity
    {
        [DataMember]
        public string ReturnValue { get; set; }
    }
}

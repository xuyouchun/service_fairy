using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TestKeepConnection
{
    [DataContract]
    public class ReplyWrapper<T>
    {
        [DataMember(Name = "method")]
        public string Method { get; set; }

        [DataMember(Name = "body")]
        public T Body { get; set; }

        [DataMember(Name = "statusCode")]
        public int StatusCode { get; set; }

        [DataMember(Name = "statusReason")]
        public int StatusReason { get; set; }

        [DataMember(Name = "statusDesc")]
        public string StatusDesc { get; set; }
    }

    [DataContract]
    public class RequestWrapper<T>
    {
        [DataMember(Name = "method")]
        public string Method { get; set; }

        [DataMember(Name = "action")]
        public string Action { get; set; }

        [DataMember(Name = "body")]
        public T Body { get; set; }

        [DataMember(Name = "sid")]
        public string SID { get; set; }
    }
}

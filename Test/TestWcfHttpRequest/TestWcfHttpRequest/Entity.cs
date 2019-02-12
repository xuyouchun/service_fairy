using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TestWcfHttpRequest
{
    [DataContract, Serializable]
    public class RequestEntity
    {
        public RequestEntity(int value, string message)
        {
            Value = value;
            Message = message;
        }

        [DataMember]
        public int Value { get; private set; }

        [DataMember]
        public string Message { get; private set; }
    }

    [DataContract, Serializable]
    public class ReplyEntity
    {
        public ReplyEntity(int value, string message)
        {
            Value = value;
            Message = message;
        }

        [DataMember]
        public int Value { get; private set; }

        [DataMember]
        public string Message { get; private set; }
    }
}

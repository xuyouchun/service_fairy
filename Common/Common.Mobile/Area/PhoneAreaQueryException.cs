using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Mobile.Area
{
    [Serializable]
    public class PhoneAreaQueryException : Exception
    {
        public PhoneAreaQueryException() { }
        public PhoneAreaQueryException(string message) : base(message) { }
        public PhoneAreaQueryException(string message, Exception inner) : base(message, inner) { }
        protected PhoneAreaQueryException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

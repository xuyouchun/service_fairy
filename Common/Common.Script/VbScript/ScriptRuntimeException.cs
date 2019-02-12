using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript
{
    [global::System.Serializable]
    public class ScriptRuntimeException : ScriptException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ScriptRuntimeException() { }
        public ScriptRuntimeException(string message) : base(message) { }
        public ScriptRuntimeException(string message, Exception inner) : base(message, inner) { }
        protected ScriptRuntimeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Statements.StatementAnalysers;

namespace Common.Script.VbScript
{
    [global::System.Serializable]
    public class ScriptException : ApplicationException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ScriptException() { }
        public ScriptException(string message) : base(message) { }
        public ScriptException(string message, Exception inner) : base(message, inner) { }
        protected ScriptException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        /// <summary>
        /// 当前行号
        /// </summary>
        public int CurrentLine
        {
            get
            {
                CharacterAnalyserLine line = CharacterAnalyserLine.Current;
                if (line != null)
                    return line.LineNumber;

                return -1;
            }
        }
    }
}

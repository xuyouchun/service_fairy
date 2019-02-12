using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript
{
    /// <summary>
    /// 语句块的执行结果
    /// </summary>
    public class StatementExecuteResult
    {
        internal StatementExecuteResult(Exception error)
        {
            Error = error;
        }

        /// <summary>
        /// 出现的错误
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool Success { get { return Error == null; } }

        /// <summary>
        /// 是否该错误发生在当前行
        /// </summary>
        public bool RaiseByCurrentLine { get; internal set; }
    }
}

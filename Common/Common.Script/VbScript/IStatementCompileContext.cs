using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript
{
    /// <summary>
    /// 语句编译上下文环境
    /// </summary>
    public interface IStatementCompileContext
    {
        /// <summary>
        /// 表达式编译上下文环境
        /// </summary>
        IExpressionCompileContext ExpressionCompileContext { get; }
    }
}

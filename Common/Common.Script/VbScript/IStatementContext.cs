using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript
{
    /// <summary>
    /// 语句块的上下文执行环境
    /// </summary>
    public interface IStatementContext
    {
        /// <summary>
        /// 开始执行某条语句
        /// </summary>
        /// <param name="context"></param>
        /// <param name="statement"></param>
        /// <returns>是否执行该条语句</returns>
        bool BeforeExecuteStatement(RunningContext context, Statement statement);

        /// <summary>
        /// 结束执行某条语句
        /// </summary>
        /// <param name="context"></param>
        /// <param name="statement"></param>
        /// <param name="result"></param>
        void EndExecuteStatement(RunningContext context, Statement statement, StatementExecuteResult result);
    }
}

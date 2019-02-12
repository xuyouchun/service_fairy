using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// 语句块的创建器
    /// </summary>
    interface IStatementCreator
    {
        /// <summary>
        /// 创建语句块
        /// </summary>
        /// <param name="expressions"></param>
        /// <param name="statements"></param>
        /// <returns></returns>
        Statement Create(Expression[] expressions, Statement[] statements);
    }
}

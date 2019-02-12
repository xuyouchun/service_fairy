using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions
{
    /// <summary>
    /// 函数的接口
    /// </summary>
    interface IFunction
    {
        /// <summary>
        /// 计算结果
        /// </summary>
        /// <param name="context">上下文执行环境</param>
        /// <param name="values"></param>
        /// <returns></returns>
        Value Execute(IExpressionContext context, Value[] values);
    }
}

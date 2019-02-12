using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions
{
    /// <summary>
    /// 用于执行由字符串指定的表达式，在运行时编译执行
    /// </summary>
    [Function("Eval", typeof(string))]
    [FunctionInfo("执行指定的表达式", "expression")]
    class EvalFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return Expression.Parse(values[0]).Execute(context);
        }
    }
}

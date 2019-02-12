using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Maths
{
    /// <summary>
    /// 返回大于或等于该指定双精度浮点数的最小整数。
    /// </summary>
    [Function("Ceiling", typeof(ValueTypes.NumberType))]
    [FunctionInfo("求大于或等于该指定双精度浮点数的最小整数", "num")]
    class CeilingFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return Math.Ceiling((double)values[0]);
        }
    }
}

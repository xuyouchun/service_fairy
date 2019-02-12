using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Maths
{
    /// <summary>
    /// 返回 e 的指定次幂
    /// </summary>
    [Function("Exp", typeof(ValueTypes.NumberType))]
    [FunctionInfo("求e的幂数", "num")]
    class ExpFunction:FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return Math.Exp(values[0]);
        }
    }
}

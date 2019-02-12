using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Maths
{
    /// <summary>
    /// 计算乘方
    /// </summary>
    [Function("Pow", typeof(ValueTypes.NumberType), typeof(ValueTypes.NumberType))]
    [FunctionInfo("求乘方", "x, y")]
    class PowFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return Math.Pow(values[0], values[1]);
        }
    }
}

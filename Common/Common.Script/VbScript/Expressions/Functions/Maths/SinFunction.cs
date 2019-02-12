using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Maths
{
    [Function("Sin", typeof(ValueTypes.NumberType))]
    [FunctionInfo("求正弦", "num")]
    class SinFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return Math.Sin(values[0]);
        }
    }
}

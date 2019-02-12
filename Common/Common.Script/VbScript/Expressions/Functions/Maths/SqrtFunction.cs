using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Maths
{
    [Function("Sqrt", typeof(ValueTypes.NumberType))]
    [FunctionInfo("求平方根", "num")]
    class SqrtFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return Math.Sqrt(values[0]);
        }
    }
}

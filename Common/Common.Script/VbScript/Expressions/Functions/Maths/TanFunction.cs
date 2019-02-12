using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Maths
{
    [Function("Tan", typeof(ValueTypes.NumberType))]
    [FunctionInfo("求正切", "num")]
    class TanFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return Math.Tan(values[0]);
        }
    }
}

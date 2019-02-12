using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Maths
{
    [Function("Atan", typeof(ValueTypes.NumberType))]
    [FunctionInfo("求反正切", "num")]
    class AtanFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return Math.Atan(values[0]);
        }
    }
}

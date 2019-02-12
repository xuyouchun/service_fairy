using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Maths
{
    [Function("Asin", typeof(ValueTypes.NumberType))]
    [FunctionInfo("求反正弦", "num")]
    class AsinFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return Math.Asin(values[0]);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Maths
{
    [Function("Cos", typeof(ValueTypes.NumberType))]
    [FunctionInfo("求余弦", "num")]
    class CosFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return Math.Cos(values[0]);
        }
    }
}

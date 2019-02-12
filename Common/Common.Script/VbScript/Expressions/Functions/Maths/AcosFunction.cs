using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Maths
{
    [Function("Acos", typeof(ValueTypes.NumberType))]
    [FunctionInfo("求反余弦", "num")]
    class AcosFunction:FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return Math.Acos(values[0]);
        }
    }
}

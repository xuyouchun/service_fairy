using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Maths
{
    [Function("Log", typeof(ValueTypes.NumberType), typeof(ValueTypes.NumberType))]
    [FunctionInfo("求对数", "num, base")]
    class LogFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return Math.Log(values[0], values[1]);
        }
    }
}

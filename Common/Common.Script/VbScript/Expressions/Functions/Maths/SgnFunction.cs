using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Maths
{
    [Function("Sgn", typeof(ValueTypes.NumberType))]
    [FunctionInfo("如果数值大于零，则返回1，如果小于零，返回-1，等于零时返回0", "num")]
    class SgnFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            object v = values[0].InnerValue;
            if (v is decimal)
            {
                decimal value = (decimal)v;
                return value < 0 ? -1 : value > 0 ? 1 : 0;
            }
            else
            {
                double value = values[0];
                return value < 0 ? -1 : value > 0 ? 1 : 0;
            }
        }
    }
}

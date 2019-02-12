using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Convert
{
    [Function("Oct", typeof(ValueTypes.IntegerType))]
    [FunctionInfo("转换为八进制", "value")]
    class OctFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            long num = values[0];

            return System.Convert.ToString(num, 8);
        }
    }
}

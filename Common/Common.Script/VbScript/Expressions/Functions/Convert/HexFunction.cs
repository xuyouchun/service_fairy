using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Convert
{
    [Function("Hex", typeof(ValueTypes.IntegerType))]
    [FunctionInfo("转换为十六进制", "value")]
    class HexFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            long num = values[0];

            return System.Convert.ToString(num, 16);
        }
    }
}

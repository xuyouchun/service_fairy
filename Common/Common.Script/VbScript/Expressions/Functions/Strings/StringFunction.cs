using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Strings
{
    [Function("String", typeof(ValueTypes.IntegerType), typeof(string))]
    [FunctionInfo("返回由指定的字符组成的指定长度的字符串", "count, char")]
    class StringFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            int num = values[0];
            if (num <= 0)
                return string.Empty;

            string s = values[1];
            char c = s == null || s.Length == 0 ? ' ' : s[0];
            return string.Empty.ToString().PadRight(num, c);
        }
    }
}

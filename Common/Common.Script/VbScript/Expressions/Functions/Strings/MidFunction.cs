using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Strings
{
    [Function("Mid", false, typeof(string), typeof(ValueTypes.IntegerType))]
    [FunctionInfo("截取指定字符串中指定起始位置指定长度的部分", "str, start [,length]")]
    class MidFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            string s = values[0];
            if (s == null)
                return Value.Void;

            int start = values[1];
            if (values.Length == 2)
            {
                return s.Substring(start);
            }
            else
            {
                int length = values[2];
                return s.Substring(start, length);
            }
        }
    }
}

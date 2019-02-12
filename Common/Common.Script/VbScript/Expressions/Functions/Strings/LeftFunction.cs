using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Strings
{
    [Function("Left", typeof(string), typeof(int))]
    [FunctionInfo("截取指定字符串左边指定长度的部分", "str, length")]
    class LeftFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            int strLength = ((string)values[0]).Length;
            int leftLength = (int)values[1];
            if (leftLength >= strLength)
                return (string)values[0];
            else
                return ((string)values[0]).Substring(0, leftLength);
        }
    }
}

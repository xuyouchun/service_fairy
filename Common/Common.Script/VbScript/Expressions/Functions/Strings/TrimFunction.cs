using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Strings
{
    [Function("Trim", typeof(string))]
    [FunctionInfo("去掉指定字符串两边的空格", "str")]
    class TrimFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return ((string)values[0]).Trim();
        }
    }
}

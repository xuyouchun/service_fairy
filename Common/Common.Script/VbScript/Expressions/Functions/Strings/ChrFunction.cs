using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Strings
{
    [Function("Chr", typeof(ValueTypes.IntegerType))]
    [FunctionInfo("返回指定UNICODE码的字符", "value")]
    class ChrFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return ((char)(int)values[0]).ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Strings
{
    [Function("RTrim", typeof(string))]
    [FunctionInfo("去掉指定字符串右边的空格", "str")]
    class RTrimFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            string s = values[0];
            if (s == null)
                return Value.Void;

            return s.TrimEnd();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Strings
{
    [Function("LTrim", typeof(string))]
    [FunctionInfo("去掉指定字符串左边的空白字符", "str")]
    class LTrimFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            string s = values[0];
            if (s == null)
                return Value.Void;

            return s.TrimStart();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Strings
{
    [Function("Replace", typeof(string), typeof(string), typeof(string))]
    [FunctionInfo("替换字符串中指定的字符串", "str, find, replace")]
    class ReplaceFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            string s = values[0], s1 = values[1], s2 = values[2];
            if (s == null || s1 == null || s2 == null)
                return Value.Void;

            return s.Replace(s1, s2);
        }
    }
}

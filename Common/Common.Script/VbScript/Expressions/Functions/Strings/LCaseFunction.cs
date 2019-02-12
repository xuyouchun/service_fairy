using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Strings
{
    [Function("LCase", typeof(string))]
    [FunctionInfo("将字符串转换为小写", "str")]
    class LCaseFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            string s = values[0];
            if (s == null)
                return s;

            return s.ToLower();
        }
    }
}

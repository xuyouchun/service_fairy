using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Strings
{
    [Function("UCase", typeof(string))]
    [FunctionInfo("将字符串转换为大写", "str")]
    class UCaseFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            string s = values[0];
            if (s == null)
                return s;

            return s.ToUpper();
        }
    }
}

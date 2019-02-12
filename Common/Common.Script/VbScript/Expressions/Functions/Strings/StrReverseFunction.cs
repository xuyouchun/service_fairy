using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Strings
{
    [Function("StrReverse", typeof(object))]
    [FunctionInfo("反转字符串", "str")]
    class StrReverseFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            object obj = values[0].InnerValue;
            if (obj == null)
                return null;

            string s = obj.ToString();
            char[] chs = new char[s.Length];

            for (int k = 0; k < s.Length; k++)
            {
                chs[chs.Length - k] = s[k];
            }

            return new String(chs);
        }
    }
}

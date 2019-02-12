using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Strings
{
    [Function("StrComp", false, typeof(string), typeof(string))]
    [FunctionInfo("比较字符串", "str1, str2 [,option]")]
    class StrCompFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            string s1 = values[0], s2 = values[1];
            int kind = values.Length < 3 || (int)values[2] == VbConstValues.vbTextCompare ? VbConstValues.vbTextCompare : VbConstValues.vbBinaryCompare;

            return string.Compare(s1, s2, kind == VbConstValues.vbTextCompare);
        }
    }
}

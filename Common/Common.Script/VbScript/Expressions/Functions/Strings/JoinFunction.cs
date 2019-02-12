using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Values;

namespace Common.Script.VbScript.Expressions.Functions.Strings
{
    [Function("Join", typeof(ArrayObject), typeof(string))]
    [FunctionInfo("将指定数组中的元素用指定的字符串连在一起", "array, separator")]
    class JoinFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            ArrayObject array = (ArrayObject)values[0];
            string s = values[1];

            StringBuilder sb = new StringBuilder();
            foreach (object item in array)
            {
                if (sb.Length > 0)
                    sb.Append(s);

                sb.Append(item);
            }

            return sb.ToString();
        }
    }
}

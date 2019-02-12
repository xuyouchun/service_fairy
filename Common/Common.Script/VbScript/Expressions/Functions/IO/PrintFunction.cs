using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.IO
{
    /// <summary>
    /// 输出数值
    /// </summary>
    [Function("Print", false)]
    [FunctionInfo("输出一组数值，中间用逗号隔开", "[value1 [,value2 ...]]")]
    class PrintFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            string[] strs = ConvertToStringArray(values);
            context.Output(string.Join(", ", strs));

            return Value.Void;
        }

        public static string[] ConvertToStringArray(Value[] values)
        {
            string[] strs = new string[values.Length];
            for (int k = 0; k < values.Length; k++)
            {
                strs[k] = values[k];
            }

            return strs;
        }
    }
}

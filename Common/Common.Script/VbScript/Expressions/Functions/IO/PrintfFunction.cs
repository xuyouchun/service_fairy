using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.IO
{
    [Function("Printf", false, typeof(string))]
    [FunctionInfo("格式化输出一组数值，参数用{0}、{1}等作为占位符，例如：PrintFmt(\"{0},{1}\", a, b)", "format [,value1 ...]")]
    class PrintfFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            string format = values[0];
            context.Output(string.Format(format, ConvertParamsToObjectArray(values)));

            return Value.Void;
        }

        public static object[] ConvertParamsToObjectArray(Value[] values)
        {
            List<object> objects = new List<object>();
            for (int k = 1; k < values.Length; k++)
            {
                objects.Add(values[k].ToString());
            }

            return objects.ToArray();
        }
    }
}

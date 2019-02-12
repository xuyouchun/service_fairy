using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.IO
{
    [Function("PrintfLn", false, typeof(string))]
    [FunctionInfo("格式化输出一组数值，参数用{0}、{1}等作为占位符，例如：PrintFmt(\"{0},{1}\", a, b)，末尾加换行符", "format [,value1 ...]")]
    class PrintfLnFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            string format = values[0];
            context.Output(string.Format(format, PrintfFunction.ConvertParamsToObjectArray(values)) + "\r\n");

            return Value.Void;
        }
    }
}

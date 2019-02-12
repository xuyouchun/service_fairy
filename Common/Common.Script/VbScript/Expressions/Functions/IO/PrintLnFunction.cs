using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.IO
{
    /// <summary>
    /// 输出一行
    /// </summary>
    [Function("PrintLn", false)]
    [FunctionInfo("输出一组数值，中间用逗号隔开，末尾加换行符", "[value1 [,value2 ...]]")]
    class PrintLnFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            string[] strs = PrintFunction.ConvertToStringArray(values);
            context.Output(string.Join(", ", strs) + "\r\n");

            return Value.Void;
        }
    }
}

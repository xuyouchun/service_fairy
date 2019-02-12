using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Strings
{
    /// <summary>
    /// 计算字符串的长度
    /// </summary>
    [Function("Len", typeof(string))]
    [FunctionInfo("求指定字符串的长度", "str")]
    class LenFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return ((string)values[0]).Length;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Strings
{
    [Function("CStr", typeof(object))]
    [FunctionInfo("转换为字符串", "value")]
    class CStrFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return values[0].ToString();
        }
    }
}

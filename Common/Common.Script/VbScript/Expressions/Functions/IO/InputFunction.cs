using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.IO
{
    [Function("Input", false)]
    [FunctionInfo("输入一个数值", "[title [,def [,description]]]")]
    class InputFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            string title = string.Empty, defValue = string.Empty, description = string.Empty;
            if (values.Length >= 1)
                title = values[0];
            if (values.Length >= 2)
                defValue = values[1];
            if (values.Length >= 3)
                description = values[2];

            return context.Input(title, defValue, description);
        }
    }
}

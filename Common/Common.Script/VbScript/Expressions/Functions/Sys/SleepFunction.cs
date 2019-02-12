using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Common.Script.VbScript.Expressions.Functions.Sys
{
    [Function("Sleep", typeof(ValueTypes.IntegerType))]
    [FunctionInfo("等待指定的毫秒数", "millseconds")]
    class SleepFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(values[0]));
            return Value.Void;
        }
    }
}

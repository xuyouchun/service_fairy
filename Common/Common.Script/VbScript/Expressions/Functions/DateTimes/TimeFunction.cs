using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.DateTimes
{
    [Function("Time")]
    [FunctionInfo("返回当前时间的“时间”部分")]
    class TimeFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return new DateTime(DateTime.Now.TimeOfDay.Ticks);
        }
    }
}

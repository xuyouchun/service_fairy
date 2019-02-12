using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.DateTimes
{
    [Function("Date")]
    [FunctionInfo("返回当前日期")]
    class DateFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return DateTime.Now.Date;
        }
    }
}

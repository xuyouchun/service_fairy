using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.DateTimes
{
    [Function("Day", typeof(object))]
    [FunctionInfo("返回指定日期的“日”部分", "time")]
    class DayFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            try
            {
                DateTime dt = (DateTime)System.Convert.ChangeType(values[0].InnerValue, typeof(DateTime));
                return dt.Day;
            }
            catch
            {
                return Value.Void;
            }
        }
    }
}

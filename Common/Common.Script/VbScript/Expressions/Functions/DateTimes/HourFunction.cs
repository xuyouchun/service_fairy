using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.DateTimes
{
    [Function("Hour", typeof(object))]
    [FunctionInfo("返回指定时间的小时部分", "time")]
    class HourFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            try
            {
                DateTime dt = (DateTime)System.Convert.ChangeType(values[0].InnerValue, typeof(DateTime));
                return dt.Hour;
            }
            catch
            {
                return Value.Void;
            }
        }
    }
}

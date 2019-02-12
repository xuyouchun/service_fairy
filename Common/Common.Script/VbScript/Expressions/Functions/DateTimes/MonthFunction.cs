using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.DateTimes
{
    [Function("Month", typeof(object))]
    [FunctionInfo("返回指定时间的“月”部分", "time")]
    class MonthFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            try
            {
                DateTime dt = (DateTime)System.Convert.ChangeType(values[0].InnerValue, typeof(DateTime));
                return dt.Month;
            }
            catch
            {
                return Value.Void;
            }
        }
    }
}

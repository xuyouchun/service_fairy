using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.DateTimes
{
    [Function("Year", typeof(object))]
    [FunctionInfo("返回指定时间的“年”部分", "time")]
    class YearFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            try
            {
                DateTime dt = (DateTime)System.Convert.ChangeType(values[0].InnerValue, typeof(DateTime));
                return dt.Year;
            }
            catch
            {
                return Value.Void;
            }
        }
    }
}

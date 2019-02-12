using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.DateTimes
{
    [Function("Minute", typeof(object))]
    [FunctionInfo("返回指定时间的分钟部分", "time")]
    class MinuteFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            try
            {
                DateTime dt = (DateTime)System.Convert.ChangeType(values[0].InnerValue, typeof(DateTime));
                return dt.Minute;
            }
            catch
            {
                return Value.Void;
            }
        }
    }
}

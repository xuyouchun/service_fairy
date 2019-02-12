using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.DateTimes
{
    [Function("Second", typeof(object))]
    [FunctionInfo("返回指定时间的“秒”部分", "time")]
    class SecondFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            try
            {
                DateTime dt = (DateTime)System.Convert.ChangeType(values[0].InnerValue, typeof(DateTime));
                return dt.Second;
            }
            catch
            {
                return Value.Void;
            }
        }
    }
}

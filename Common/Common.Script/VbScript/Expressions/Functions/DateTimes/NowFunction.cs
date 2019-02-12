using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.DateTimes
{
    [Function("Now", false, typeof(object))]
    [FunctionInfo("以指定的格式返回当前时间", "[format]")]
    class NowFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            DateTime dt = DateTime.Now;

            if (values.Length == 0)
                return dt.ToString();

            Value format = values[0];
            if (format.GetValueType() == typeof(int))
            {
                switch ((int)format)
                {
                    case VbConstValues.vbLongDate:
                        return dt.ToLongDateString();

                    case VbConstValues.vbLongTime:
                        return dt.ToLongTimeString();

                    case VbConstValues.vbShortDate:
                        return dt.ToShortDateString();

                    case VbConstValues.vbShortTime:
                        return dt.ToShortTimeString();

                    default:
                        return dt.ToString();
                }
            }

            return dt.ToString(values[0]);
        }
    }
}

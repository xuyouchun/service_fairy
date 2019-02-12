using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Client.Script.Expressions.Functions.Maths
{
    /// <summary>
    /// 计算正切值为两个指定数字的商的角度。
    /// </summary>
    [Function("Atan2", typeof(ValueTypes.NumberType), typeof(ValueTypes.NumberType))]
    class Atan2Function:FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return Math.Atan2(values[0], values[1]);
        }
    }
}

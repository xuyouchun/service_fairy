using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Maths
{
    /// <summary>
    /// 返回小于或等于指定双精度浮点数的最大整数
    /// </summary>
    [Function("Floor", typeof(ValueTypes.NumberType))]
    [FunctionInfo("求小于或等于指定双精度浮点数的最大整数", "num")]
    class FloorFunction:FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return Math.Floor((double)values[0]);
        }
    }
}

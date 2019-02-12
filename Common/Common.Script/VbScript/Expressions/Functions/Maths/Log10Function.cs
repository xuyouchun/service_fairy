using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Maths
{
    /// <summary>
    /// 计算以10为底的指定次数的幂
    /// </summary>
    [Function("Log10", typeof(ValueTypes.NumberType))]
    [FunctionInfo("求以10为底的对数", "num")]
    class Log10Function : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return Math.Log10(values[0]);
        }
    }
}

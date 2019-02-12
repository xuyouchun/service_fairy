using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Maths
{
    /// <summary>
    /// 计算指定角度的双曲余弦值
    /// </summary>
    [Function("Cosh", typeof(ValueTypes.NumberType))]
    [FunctionInfo("求双曲余弦", "num")]
    class CoshFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return Math.Cosh(values[0]);
        }
    }
}

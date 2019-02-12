using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Maths
{
    /// <summary>
    /// 返回0.0到1.0之间的随机数
    /// </summary>
    [Function("Random")]
    [FunctionInfo("返回0.0到1.0之间的随机数")]
    class RandomFunction : FunctionBase
    {
        private static readonly Random _Random = new Random();

        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            return _Random.NextDouble();
        }
    }
}

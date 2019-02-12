using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Convert
{
    [Function("CCur", typeof(object))]
    [FunctionInfo("转换为Currency型", "value")]
    class CCurFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            try
            {
                return (decimal)values[0];
            }
            catch (InvalidCastException)
            {
                throw new ScriptRuntimeException("转换为Currency时出现错误");
            }
        }
    }
}

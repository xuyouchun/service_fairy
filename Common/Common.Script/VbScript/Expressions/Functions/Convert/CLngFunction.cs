using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Convert
{
    [Function("CLng", typeof(object))]
    [FunctionInfo("转换为长整型", "value")]
    class CLngFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            try
            {
                return (long)values[0];
            }
            catch (InvalidCastException)
            {
                throw new ScriptRuntimeException("转换为Long类型时出现错误");
            }
        }
    }
}

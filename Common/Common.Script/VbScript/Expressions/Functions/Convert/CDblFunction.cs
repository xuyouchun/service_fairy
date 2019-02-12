using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Convert
{
    [Function("CDbl", typeof(object))]
    [FunctionInfo("转换为Double型", "value")]
    class CDblFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            try
            {
                return (double)values[0];
            }
            catch (InvalidCastException)
            {
                throw new ScriptRuntimeException("转换为Double类型时出现错误");
            }
        }
    }
}

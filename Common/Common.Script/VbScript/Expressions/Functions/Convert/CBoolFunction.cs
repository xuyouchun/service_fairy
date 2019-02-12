using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Convert
{
    [Function("CBool", typeof(object))]
    [FunctionInfo("转换为布尔型", "value")]
    class CBoolFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            try
            {
                return (bool)values[0];
            }
            catch (InvalidCastException)
            {
                throw new ScriptRuntimeException("转换为Bool类型时出现错误");
            }
        }
    }
}

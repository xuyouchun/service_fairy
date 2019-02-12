using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Convert
{
    [Function("CInt", typeof(object))]
    [FunctionInfo("转换为整型", "value")]
    class CIntFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            try
            {
                return (int)values[0];
            }
            catch (InvalidCastException)
            {
                throw new ScriptRuntimeException("转换为Int类型时出现错误");
            }
        }
    }
}

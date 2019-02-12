using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Convert
{
    [Function("CByte", typeof(object))]
    [FunctionInfo("转换为Byte型", "value")]
    class CByteFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            try
            {
                return (byte)values[0];
            }
            catch (InvalidCastException)
            {
                throw new ScriptRuntimeException("转换为Byte类型时出现错误");
            }
        }
    }
}

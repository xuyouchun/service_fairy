using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Convert
{
    [Function("CShort", typeof(object))]
    [FunctionInfo("转换为短整型", "value")]
    class CShortFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            try
            {
                return (short)values[0];
            }
            catch (InvalidCastException)
            {
                throw new ScriptRuntimeException("转换为Short类型时出现错误");
            }
        }
    }
}

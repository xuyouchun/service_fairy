using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions.Convert
{
    [Function("CDate", typeof(object))]
    [FunctionInfo("转换为时间型", "value")]
    class CDateFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            try
            {
                return (DateTime)values[0];
            }
            catch (InvalidCastException)
            {
                throw new ScriptRuntimeException("转换为Date时出现错误");
            }
        }
    }
}

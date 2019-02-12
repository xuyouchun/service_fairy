using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions
{
    [Function("Execute", typeof(string))]
    [FunctionInfo("执行指定的语句", "statement")]
    class ExecuteFunction : FunctionBase
    {
        protected override Value OnExecute(IExpressionContext context, Value[] values)
        {
            if (!RunningContext.IsInStatementContext)
                throw new ScriptRuntimeException("缺少上下文运行环境");

            string s = (string)values[0];

            Statement.Parse(s, null).Execute(RunningContext.Current);
            return Value.Void;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions.Functions;
using Common.Script.VbScript.Statements;

namespace Common.Script.VbScript.Expressions
{
    /// <summary>
    /// 函数表达式
    /// </summary>
    class FunctionExpression : Expression
    {
        public FunctionExpression(string functionName, Expression[] parameters)
        {
            FunctionName = functionName;
            Parameters = parameters;
        }

        public string FunctionName { get; private set; }

        public Expression[] Parameters { get; private set; }

        protected override Value OnExecute(IExpressionContext context)
        {
            Value[] values = new Value[Parameters.Length];
            for (int k = 0; k < values.Length; k++)
            {
                values[k] = Parameters[k].Execute(context);
            }

            if (RunningContext.IsInStatementContext)
            {
                // 搜索用脚本自定义的函数
                FunctionStatement funcStatement = RunningContext.Current.GetFunctionStatement(FunctionName);
                if (funcStatement != null)
                    return _CallUserFunction(funcStatement, values);
            }

            // 由用户决定如何运行
            Value returnValue;
            if (context.ExecuteFunction(FunctionName, values, out returnValue))
                return returnValue ?? Value.Void;

            // 搜索内部函数
            FunctionInfo fInfo = FunctionManager.GetFunctionInfo(FunctionName);
            if (fInfo == null)
                throw new ScriptException("不支持函数“" + FunctionName + "”");

            var rCtx = RunningContext.Current;
            return fInfo.CreateFunction().Execute(rCtx == null ? new DefaultExpressionContext() : rCtx.ExpressionContext, values);
        }

        public Value _CallUserFunction(FunctionStatement funcStatement, Value[] values)
        {
            RunningContext runningContext = RunningContext.Current;
            IExpressionContext oldExpCtx = runningContext.ExpressionContext;
            IExpressionContext newExpCtx = new FunctionExpressionContext(oldExpCtx, runningContext.GlobalExpressonContext);

            try
            {
                // 参数赋值
                VariableExpression[] funcParams = funcStatement.Parameters;
                int paramCount = Math.Min(funcParams.Length, values.Length);
                for (int k = 0; k < paramCount; k++)
                {
                    newExpCtx.Declare(funcParams[k].Name);
                    if (k < Parameters.Length)
                        newExpCtx.SetValue(funcParams[k].Name, values[k]);
                }

                // 调用函数并返回结果
                runningContext.ExpressionContext = newExpCtx;
                runningContext.CallStack.Push(funcStatement, null);
                funcStatement.Execute(runningContext);
                return funcStatement.GetResult(runningContext);
            }
            finally
            {
                runningContext.CallStack.Pop();
                runningContext.ExpressionContext = oldExpCtx;
            }
        }

        public override Expression[] GetAllSubExpressions()
        {
            return Parameters;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(FunctionName).Append("(");

            for (int k = 0; k < Parameters.Length; k++)
            {
                if (k > 0)
                    sb.Append(", ");

                sb.Append(Parameters[k].ToString());
            }

            sb.Append(")");
            return sb.ToString();
        }
    }
}

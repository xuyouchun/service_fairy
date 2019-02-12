using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Statements;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript.Values
{
    /// <summary>
    /// 类的成员函数
    /// </summary>
    class ObjectFunctionStatementMember : ObjectFunctionMember
    {
        public ObjectFunctionStatementMember(Class owner, string name, ObjectMemberAccessRight accessRight, FunctionStatement function)
            : base(owner, name, accessRight)
        {
            Function = function;
        }

        /// <summary>
        /// 函数
        /// </summary>
        public FunctionStatement Function { get; private set; }

        protected override Value OnExecute(RunningContext context, Object obj, Expression[] parameters)
        {
            IExpressionContext oldCtx = context.ExpressionContext;

            try
            {
                context.CallStack.Push(Function, obj);
                IExpressionContext newExpCtx = _CreateNewExpressionContext(context, obj, oldCtx);
                ValidateAccessRight(context);

                // 参数赋值
                VariableExpression[] funcParams = Function.Parameters;
                for (int k = 0; k < funcParams.Length; k++)
                {
                    newExpCtx.Declare(funcParams[k].Name);
                    if (k < parameters.Length)
                        newExpCtx.SetValue(funcParams[k].Name, parameters[k].Execute(context.ExpressionContext));
                }

                // 调用函数并返回结果
                context.ExpressionContext = newExpCtx;
                Function.Execute(context);
                return Function.GetResult(context);
            }
            finally
            {
                context.CallStack.Pop();
                context.ExpressionContext = oldCtx;
            }
        }

        private IExpressionContext _CreateNewExpressionContext(RunningContext context, Object obj, IExpressionContext oldCtx)
        {
            return new ObjectExpressionContextAdapter(oldCtx, new LocalExpressionContext(obj.ExpressionContext, context.GlobalExpressonContext));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript.Values
{
    /// <summary>
    /// 构造函数
    /// </summary>
    class ObjectVariableMember : ObjectMember
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="accessRight"></param>
        /// <param name="variableExpression"></param>
        public ObjectVariableMember(Class ownerClass, string name, ObjectMemberAccessRight accessRight, VariableExpression variableExpression)
            : base(ownerClass, name, accessRight)
        {
            VariableExpression = variableExpression;
        }

        /// <summary>
        /// 变量表达式
        /// </summary>
        public VariableExpression VariableExpression { get; private set; }

        protected override Value OnExecute(RunningContext context, Object obj, Expression[] parameters)
        {
            ValidateAccessRight(context);

            IExpressionContext oldCtx = context.ExpressionContext;

            try
            {
                context.ExpressionContext = new ObjectExpressionContextAdapter(oldCtx, obj.ExpressionContext);
                return VariableExpression.Execute(context.ExpressionContext);
            }
            finally
            {
                context.ExpressionContext = oldCtx;
            }
        }

        public void SetValue(RunningContext context, Object obj, Value value)
        {
            ValidateAccessRight(context);

            IExpressionContext oldCtx = context.ExpressionContext;

            try
            {
                context.ExpressionContext = new ObjectExpressionContextAdapter(oldCtx, obj.ExpressionContext);
                VariableExpression.SetValue(context.ExpressionContext, value);
            }
            finally
            {
                context.ExpressionContext = oldCtx;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions
{
    /// <summary>
    /// 等于运算符
    /// </summary>
    [Expression(Operators.Equals, typeof(Creator))]
    class EqualsExpression : DualityExpression
    {
        public EqualsExpression(Expression expression1, Expression expression2)
            : base(expression1, expression2)
        {
            
        }

        protected override Value OnExecute(IExpressionContext context)
        {
            return Expression1.Execute(context) == Expression2.Execute(context);
        }

        class Creator : ExpressionCreator
        {
            #region IExpressionCreator 成员

            public override Expression Create(Expression[] parameters)
            {
                ValidateParameters(parameters);

                return new EqualsExpression(parameters[0], parameters[1]);
            }

            #endregion
        }
    }
}

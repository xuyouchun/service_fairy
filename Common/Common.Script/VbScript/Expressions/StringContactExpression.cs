using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions
{
    [Expression(Operators.StringContact, typeof(Creator))]
    class StringContactExpression : DualityExpression
    {
        public StringContactExpression(Expression exp1, Expression exp2)
            : base(exp1, exp2)
        {

        }

        protected override Value OnExecute(IExpressionContext context)
        {
            return Expression1.Execute(context).ToString() + Expression2.Execute(context).ToString();
        }

        class Creator : ExpressionCreator
        {
            #region IExpressionCreator 成员

            public override Expression Create(Expression[] parameters)
            {
                ValidateParameters(parameters);

                return new StringContactExpression(parameters[0], parameters[1]);
            }

            #endregion
        }
    }
}

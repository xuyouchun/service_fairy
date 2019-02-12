using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions
{
    [Expression(Operators.And, typeof(Creator))]
    class AndExpression : DualityExpression
    {
        public AndExpression(Expression exp1, Expression exp2)
            : base(exp1, exp2)
        {

        }

        protected override Value OnExecute(IExpressionContext context)
        {
            return Expression1.Execute(context) & Expression2.Execute(context);
        }

        class Creator : ExpressionCreator
        {

            public override Expression Create(Expression[] parameters)
            {
                ValidateParameters(parameters);

                return new AndExpression(parameters[0], parameters[1]);
            }
        }
    }
}

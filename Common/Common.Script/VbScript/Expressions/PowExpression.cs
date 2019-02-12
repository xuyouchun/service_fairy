using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions
{
    [Expression(Operators.Pow, typeof(Creator))]
    class PowExpression : DualityExpression
    {
        public PowExpression(Expression exp1, Expression exp2)
            : base(exp1, exp2)
        {

        }

        protected override Value OnExecute(IExpressionContext context)
        {
            return Value.Pow(Expression1.Execute(context), Expression2.Execute(context));
        }

        public override string ToString()
        {
            return Expression1.ToString() + " ^ " + Expression2.ToString();
        }

        class Creator : ExpressionCreator
        {
            #region IExpressionCreator 成员

            public override Expression Create(Expression[] parameters)
            {
                ValidateParameters(parameters);

                return new PowExpression(parameters[0], parameters[1]);
            }

            #endregion
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions
{
    [Expression(Operators.Minus, typeof(Creator))]
    class MinusExpression : UnitaryExpression
    {
        public MinusExpression(Expression exp)
            : base(exp)
        {

        }

        protected override Value OnExecute(IExpressionContext context)
        {
            return -Expression.Execute(context);
        }

        class Creator : ExpressionCreator
        {
            #region IExpressionCreator 成员

            public override Expression Create(Expression[] parameters)
            {
                ValidateParameters(parameters);

                return new MinusExpression(parameters[0]);
            }

            #endregion
        }
    }
}

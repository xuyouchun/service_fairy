using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions
{
    [Expression(Operators.Not, typeof(Creator))]
    class NotExpression : UnitaryExpression
    {
        public NotExpression(Expression exp)
            : base(exp)
        {

        }

        protected override Value OnExecute(IExpressionContext context)
        {
            return !Expression.Execute(context);
        }

        class Creator : ExpressionCreator
        {
            #region IExpressionCreator 成员

            public override Expression Create(Expression[] parameters)
            {
                ValidateParameters(parameters);

                return new NotExpression(parameters[0]);
            }

            #endregion
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions
{
    [Expression(Operators.LessThanOrEquals, typeof(Creator))]
    class LessThanOrEqualsExpression : DualityExpression
    {
        public LessThanOrEqualsExpression(Expression exp1, Expression exp2)
            : base(exp1, exp2)
        {

        }

        protected override Value OnExecute(IExpressionContext context)
        {
            return Expression1.Execute(context) <= Expression2.Execute(context);
        }


        class Creator : ExpressionCreator
        {
            #region IExpressionCreator 成员

            public override Expression Create(Expression[] parameters)
            {
                ValidateParameters(parameters);

                return new LessThanOrEqualsExpression(parameters[0], parameters[1]);
            }

            #endregion
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions
{
    /// <summary>
    /// 一元运算符
    /// </summary>
    abstract class UnitaryExpression : Expression
    {
        public UnitaryExpression(Expression exp)
        {
            if (exp == null)
                throw new ArgumentNullException("exp");

            Expression = exp;
        }

        /// <summary>
        /// 表达式
        /// </summary>
        public Expression Expression { get; private set; }

        public override string ToString()
        {
            ExpressionAttribute attr = ExpressionAttribute.GetAttribute(this.GetType());
            if (attr == null)
                return string.Empty;

            OperatorAttribute opAttr = OperatorAttribute.GetAttribute(attr.OperatorType);

            return opAttr.OperatorName + " " + Expression.ToString();
        }

        public override Expression[] GetAllSubExpressions()
        {
            return new Expression[] { Expression };
        }

        protected abstract class ExpressionCreator : IExpressionCreator
        {
            protected void ValidateParameters(Expression[] parameters)
            {
                if (parameters.Length != 1)
                    throw new ScriptException("错误的参数个数");
            }

            #region IExpressionCreator 成员

            public abstract Expression Create(Expression[] parameters);

            #endregion
        }
    }
}

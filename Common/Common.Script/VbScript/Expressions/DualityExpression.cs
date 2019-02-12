using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions
{
    /// <summary>
    /// 二元运算符
    /// </summary>
    abstract class DualityExpression : Expression
    {
        public DualityExpression(Expression exp1, Expression exp2)
        {
            if (exp1 == null || exp2 == null)
                throw new ArgumentNullException(exp1 == null ? "exp1" : "exp2");

            Expression1 = exp1;
            Expression2 = exp2;
        }

        /// <summary>
        /// 表达式1
        /// </summary>
        public Expression Expression1 { get; private set; }

        /// <summary>
        /// 表达式2
        /// </summary>
        public Expression Expression2 { get; private set; }

        public override string ToString()
        {
            ExpressionAttribute attr = ExpressionAttribute.GetAttribute(this.GetType());
            if (attr == null)
                return string.Empty;

            OperatorAttribute opAttr = OperatorAttribute.GetAttribute(attr.OperatorType);
            return "(" + Expression1.ToString() + " " + opAttr.OperatorName + " " + Expression2.ToString() + ")";
        }

        public override Expression[] GetAllSubExpressions()
        {
            return new Expression[] { Expression1, Expression2 };
        }

        protected abstract class ExpressionCreator : IExpressionCreator
        {
            protected void ValidateParameters(Expression[] parameters)
            {
                if (parameters.Length != 2)
                    throw new ScriptException("错误的参数个数");
            }

            #region IExpressionCreator 成员

            public abstract Expression Create(Expression[] parameters);

            #endregion
        }
    }
}

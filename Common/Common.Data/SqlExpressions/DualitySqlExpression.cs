using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Data.SqlExpressions
{
    /// <summary>
    /// 二元表达式
    /// </summary>
    public class DualitySqlExpression : SqlExpressionBase
    {
        internal DualitySqlExpression(SqlExpressionOperator @operator, SqlExpression exp1, SqlExpression exp2)
        {
            Contract.Requires(exp1 != null && exp2 != null);

            Operator = @operator;
            Expression1 = exp1;
            Expression2 = exp2;
        }

        /// <summary>
        /// 表达式1
        /// </summary>
        public SqlExpression Expression1 { get; private set; }

        /// <summary>
        /// 表达式2
        /// </summary>
        public SqlExpression Expression2 { get; private set; }

        /// <summary>
        /// 操作符
        /// </summary>
        public SqlExpressionOperator Operator { get; private set; }

        protected override string OnToString(ISqlExpressionContext context)
        {
            return "(" + Expression1.ToString(context) + " "
                + SqlExpressionOperatorAttribute.GetOpText(Operator) + " "
                + Expression2.ToString(context) + ")";
        }

        public override IEnumerator<SqlExpression> GetEnumerator()
        {
            yield return Expression1;
            yield return Expression2;
        }
    }
}

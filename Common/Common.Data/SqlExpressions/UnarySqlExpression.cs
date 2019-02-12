using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Data.SqlExpressions
{
    /// <summary>
    /// 一元运算符
    /// </summary>
    public class UnarySqlExpression : SqlExpressionBase
    {
        internal UnarySqlExpression(SqlExpressionOperator @operator, SqlExpression exp)
        {
            Contract.Requires(exp != null);

            Operator = @operator;
            Expression = exp;
        }

        /// <summary>
        /// 运算符
        /// </summary>
        public SqlExpressionOperator Operator { get; private set; }

        /// <summary>
        /// 表达式
        /// </summary>
        public SqlExpression Expression { get; private set; }

        protected override string OnToString(ISqlExpressionContext context)
        {
            return "(" + SqlExpressionOperatorAttribute.GetOpText(Operator) + Expression + ")";
        }
    }
}

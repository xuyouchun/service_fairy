using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Data.UnionTable.SqlExpressions
{
    /// <summary>
    /// 一元运算符
    /// </summary>
    public class UnarySqlExpression : SqlExpression
    {
        public UnarySqlExpression(SqlExpressionOperator @operator, SqlExpression exp)
        {
            Contract.Requires(exp != null);

            Operator = @operator;
            Exp = exp;
        }

        /// <summary>
        /// 运算符
        /// </summary>
        public SqlExpressionOperator Operator { get; private set; }

        /// <summary>
        /// 表达式
        /// </summary>
        public SqlExpression Exp { get; private set; }

        public override string ToString()
        {
            return "(" + SqlExpressionOperatorAttribute.GetOpText(Operator) + Exp + ")";
        }

        public override IEnumerator<SqlExpression> GetEnumerator()
        {
            yield return Exp;
        }
    }
}

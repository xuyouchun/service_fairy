using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Data.UnionTable.SqlExpressions
{
    /// <summary>
    /// 二元表达式
    /// </summary>
    public class DualitySqlExpression : SqlExpression
    {
        public DualitySqlExpression(SqlExpressionOperator @operator, SqlExpression exp1, SqlExpression exp2)
        {
            Contract.Requires(exp1 != null && exp2 != null);

            Operator = @operator;
            Exp1 = exp1;
            Exp2 = exp2;
        }

        /// <summary>
        /// 表达式1
        /// </summary>
        public SqlExpression Exp1 { get; private set; }

        /// <summary>
        /// 表达式2
        /// </summary>
        public SqlExpression Exp2 { get; private set; }

        /// <summary>
        /// 操作符
        /// </summary>
        public SqlExpressionOperator Operator { get; private set; }

        public override string ToString()
        {
            return "(" + Exp1.ToString() + " "
                + SqlExpressionOperatorAttribute.GetOpText(Operator) + " "
                + Exp2.ToString() + ")";
        }

        public override IEnumerator<SqlExpression> GetEnumerator()
        {
            yield return Exp1;
            yield return Exp2;
        }
    }
}

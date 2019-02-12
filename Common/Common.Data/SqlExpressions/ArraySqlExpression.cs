using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace Common.Data.SqlExpressions
{
    /// <summary>
    /// 数组表达式
    /// </summary>
    public class ArraySqlExpression : SqlExpressionBase
    {
        internal ArraySqlExpression(SqlExpression[] exps)
        {
            Contract.Requires(exps != null);

            Expressions = exps ?? Array<SqlExpression>.Empty;
        }

        /// <summary>
        /// 该数组中的所有表达式
        /// </summary>
        public SqlExpression[] Expressions { get; private set; }

        protected override string OnToString(ISqlExpressionContext context)
        {
            return "(" + Expressions.Select(exp => exp.ToString(context)).JoinBy(", ") + ")";
        }

        public static readonly ArraySqlExpression Empty = new ArraySqlExpression(Array<SqlExpression>.Empty);

        public override IEnumerator<SqlExpression> GetEnumerator()
        {
            for (int k = 0; k < Expressions.Length; k++)
            {
                yield return Expressions[k];
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Collection;
using Common.Data.UnionTable.SqlExpressions.Analyzer;

namespace Common.Data.UnionTable.SqlExpressions
{
    /// <summary>
    /// 查询表达式
    /// </summary>
    public abstract class SqlExpression : IEnumerable<SqlExpression>
    {
        /// <summary>
        /// 获取所涉及到的字段
        /// </summary>
        /// <returns></returns>
        public abstract override string ToString();

        /// <summary>
        /// 获取子表达式
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator<SqlExpression> GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static IEnumerable<SqlExpression> ForEachAllExpressions(SqlExpression expression)
        {
            Contract.Requires(expression != null);

            foreach (SqlExpression exp in expression)
            {
                yield return exp;

                foreach (SqlExpression exp0 in ForEachAllExpressions(exp))
                {
                    yield return exp0;
                }
            }
        }

        /// <summary>
        /// 获取全部字段名
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string[] GetAllFieldNames(SqlExpression expression)
        {
            var list = from exp in ForEachAllExpressions(expression)
                       let f = exp as ISqlExpressionWithFieldName
                       where f != null
                       select f.GetFieldNames();

            return list.SelectMany(v => v).Distinct(IgnoreCaseEqualityComparer.Instance).ToArray();
        }

        /// <summary>
        /// 创建二元运算符
        /// </summary>
        /// <param name="operator"></param>
        /// <param name="exp1"></param>
        /// <param name="exp2"></param>
        /// <returns></returns>
        public static DualitySqlExpression Duality(SqlExpressionOperator @operator, SqlExpression exp1, SqlExpression exp2)
        {
            return new DualitySqlExpression(@operator, exp1, exp2);
        }

        /// <summary>
        /// 创建一元运算符
        /// </summary>
        /// <param name="operator"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static UnarySqlExpression Unary(SqlExpressionOperator @operator, SqlExpression exp)
        {
            return new UnarySqlExpression(@operator, exp);
        }

        /// <summary>
        /// 值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        public static ValueExpression Value(object value, FieldType fieldType)
        {
            return new ValueExpression(value, fieldType);
        }

        /// <summary>
        /// 变量表达式
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static VariableSqlExpression Variable(string fieldName)
        {
            return new VariableSqlExpression(fieldName);
        }

        /// <summary>
        /// 将字符串转换为表达式
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static SqlExpression Parse(string exp)
        {
            Contract.Requires(exp != null);

            return SqlExpressionAnalyzer.Analyze(exp);
        }
    }
}

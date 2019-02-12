using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Data.UnionTable.Metadata;
using Common.Utility;
using Common.Data.SqlExpressions;
using Common.Collection;

namespace Common.Data.UnionTable.MsSql
{
    /// <summary>
    /// Sql-Server的数据查询连接
    /// </summary>
    partial class MsSqlUtConnection : UtConnectionBase
    {
        public MsSqlUtConnection(UtContext utCtx, MsSqlConnectionPoint conPoint, int partialIndex)
            : base(utCtx, partialIndex)
        {
            ConnectionPoint = conPoint;
        }

        /// <summary>
        /// 连接点
        /// </summary>
        public MsSqlConnectionPoint ConnectionPoint { get; private set; }

        private readonly IgnoreCaseEqualityComparer _ig = IgnoreCaseEqualityComparer.Instance;

        #region Class SqlExpressionContext ...

        class SqlExpressionContext : ISqlExpressionContext
        {
            public SqlExpressionContext(MsSqlUtConnection owner, string defaultGroup)
            {
                _owner = owner;
                _defaultGroup = defaultGroup;
            }

            private readonly MsSqlUtConnection _owner;
            private readonly string _defaultGroup;

            public object GetValue(string parameterName)
            {
                return null;
            }

            public string ToStringCallback(SqlExpression sqlExp)
            {
                DualitySqlExpression dualExp = sqlExp as DualitySqlExpression;
                VariableSqlExpression varExp;

                if (dualExp != null)
                {
                    SqlExpressionOperator op = dualExp.Operator;
                    if (op == SqlExpressionOperator.NotEquals || op == SqlExpressionOperator.Equals)
                    {
                        ValueExpression valueExp = dualExp.Expression2 as ValueExpression;
                        if (valueExp != null && valueExp.IsEmpty())
                        {
                            return "(" + dualExp.Expression1.ToString(this)
                                + ((op == SqlExpressionOperator.Equals) ? " Is Null" : "Is Not Null") + ")";
                        }
                    }
                }
                else if ((varExp = sqlExp as VariableSqlExpression) != null)
                {
                    return SqlUtility.ReviseFullColumnName(varExp.VariableName, _defaultGroup);
                }

                return null;
            }
        }

        #endregion

    }
}

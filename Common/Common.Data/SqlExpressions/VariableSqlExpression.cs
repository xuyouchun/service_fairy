using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Data.SqlExpressions
{
    /// <summary>
    /// 变量表达式
    /// </summary>
    public class VariableSqlExpression : SqlExpressionBase, ISqlExpressionWithFieldName
    {
        internal VariableSqlExpression(string fieldName)
        {
            Contract.Requires(fieldName != null);

            VariableName = fieldName;
        }

        public string VariableName { get; private set; }

        protected override string OnToString(ISqlExpressionContext context)
        {
            return "[" + VariableName + "]";
        }

        public string[] GetFieldNames()
        {
            return new string[] { VariableName };
        }
    }
}

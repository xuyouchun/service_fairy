using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Data.UnionTable.SqlExpressions
{
    /// <summary>
    /// 变量表达式
    /// </summary>
    public class VariableSqlExpression : SqlExpression, ISqlExpressionWithFieldName
    {
        public VariableSqlExpression(string fieldName)
        {
            Contract.Requires(fieldName != null);

            FieldName = fieldName;
        }

        public string FieldName { get; private set; }

        public override string ToString()
        {
            return "[" + FieldName + "]";
        }

        public override IEnumerator<SqlExpression> GetEnumerator()
        {
            yield break;
        }

        public string[] GetFieldNames()
        {
            return new string[] { FieldName };
        }
    }
}

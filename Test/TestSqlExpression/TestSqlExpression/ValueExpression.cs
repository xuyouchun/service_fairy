using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data.UnionTable.SqlExpressions
{
    /// <summary>
    /// 值的表达式
    /// </summary>
    public class ValueExpression : SqlExpression
    {
        public ValueExpression(object value, FieldType fieldType)
        {
            Value = value;
            FieldType = fieldType;
        }

        /// <summary>
        /// 值
        /// </summary>
        public new object Value { get; private set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public FieldType FieldType { get; private set; }

        /// <summary>
        /// 空值
        /// </summary>
        public static readonly ValueExpression DBNull = new ValueExpression(null, FieldType.DBNull);

        public override string ToString()
        {
            return UnionTableUtility.ReviseToMsSqlValue(Value, FieldType);
        }

        public override IEnumerator<SqlExpression> GetEnumerator()
        {
            yield break;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data.SqlExpressions
{
    /// <summary>
    /// 值的表达式
    /// </summary>
    public class ValueExpression : SqlExpressionBase
    {
        internal ValueExpression(object value, DbColumnType fieldType)
        {
            Value = value;
            FieldType = fieldType;
        }

        internal ValueExpression(object value)
            : this(value, DbColumnType.AnsiString)
        {

        }

        /// <summary>
        /// 值
        /// </summary>
        public new object Value { get; private set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public DbColumnType FieldType { get; private set; }

        /// <summary>
        /// 空值
        /// </summary>
        public static readonly ValueExpression DBNull = new ValueExpression(null, DbColumnType.DBNull);

        /// <summary>
        /// 是否为空值
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return Value == null || Value is System.DBNull || FieldType == DbColumnType.DBNull;
        }

        protected override string OnToString(ISqlExpressionContext context)
        {
            return SqlUtility.ReviseToMsSqlValue(Value, FieldType);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using System.Reflection;

namespace Common.Data.SqlExpressions
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    class SqlExpressionOperatorAttribute : Attribute
    {
        static SqlExpressionOperatorAttribute()
        {
            _opAttrs = ReflectionUtility.SearchMembers<SqlExpressionOperator, SqlExpressionOperatorAttribute, SqlExpressionOperatorAttribute>(
                typeof(SqlExpressionOperator),
                (attrs, mInfo) => (SqlExpressionOperator)((System.Reflection.FieldInfo)mInfo).GetValue(null),
                (attrs, mInfo) => attrs[0],
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
            );
        }

        public SqlExpressionOperatorAttribute(string opText, int weight, int opCount)
        {
            OpText = opText ?? string.Empty;
            Weight = weight;
            OpCount = opCount;
        }

        /// <summary>
        /// 文本
        /// </summary>
        public string OpText { get; private set; }

        /// <summary>
        /// 优先级（值越小越高）
        /// </summary>
        public int Weight { get; private set; }

        /// <summary>
        /// 操作数的个数
        /// </summary>
        public int OpCount { get; private set; }

        private static readonly Dictionary<SqlExpressionOperator, SqlExpressionOperatorAttribute> _opAttrs;

        /// <summary>
        /// 获取操作符的文本表示
        /// </summary>
        /// <param name="operator"></param>
        /// <returns></returns>
        public static string GetOpText(SqlExpressionOperator @operator)
        {
            SqlExpressionOperatorAttribute attr = GetOpAttr(@operator);
            return attr == null ? null : attr.OpText;
        }

        /// <summary>
        /// 获取操作符的Attribute
        /// </summary>
        /// <param name="operator"></param>
        /// <returns></returns>
        public static SqlExpressionOperatorAttribute GetOpAttr(SqlExpressionOperator @operator)
        {
            return _opAttrs.GetOrDefault(@operator);
        }

        /// <summary>
        /// 获取全部的操作符
        /// </summary>
        /// <returns></returns>
        public static SqlExpressionOperator[] GetAllOperators()
        {
            return _opAttrs.Keys.ToArray();
        }
    }
}

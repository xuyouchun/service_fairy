using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions
{
    /// <summary>
    /// 表达式的特性
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    class ExpressionAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="operatorType">操作符类型</param>
        /// <param name="creatorType">创建器</param>
        public ExpressionAttribute(Operators operatorType, Type creatorType)
        {
            OperatorType = operatorType;
            CreatorType = creatorType;
        }

        /// <summary>
        /// 操作符类型
        /// </summary>
        public Operators OperatorType { get; private set; }

        /// <summary>
        /// 表达式的创建器
        /// </summary>
        public Type CreatorType { get; private set; }

        private static readonly Dictionary<Type, ExpressionAttribute> _Dict = new Dictionary<Type, ExpressionAttribute>();

        public static ExpressionAttribute GetAttribute(Type type)
        {
            if (type == null)
                return null;

            ExpressionAttribute attr;
            if (!_Dict.TryGetValue(type, out attr))
            {
                _Dict.Add(type, attr = _GetAttr(type));
            }

            return attr;
        }

        private static ExpressionAttribute _GetAttr(Type type)
        {
            object[] objs = type.GetCustomAttributes(typeof(ExpressionAttribute), false);
            if (objs.Length == 0)
                return null;

            return (ExpressionAttribute)objs[0];
        }
    }
}

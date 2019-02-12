using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class StatementAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="statementType">语句块类型</param>
        /// <param name="creatorType">创建器类型</param>
        public StatementAttribute(StatementType statementType, Type creatorType)
        {
            StatementType = statementType;
            CreatorType = creatorType;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="statementType"></param>
        public StatementAttribute(StatementType statementType)
            : this(statementType, null)
        {

        }

        /// <summary>
        /// 语句块类型
        /// </summary>
        public StatementType StatementType { get; private set; }

        /// <summary>
        /// 创建器的类型
        /// </summary>
        public Type CreatorType { get; private set; }
    }
}

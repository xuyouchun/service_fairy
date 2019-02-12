using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data.UnionTable.SqlExpressions
{
    public enum SqlExpressionOperator
    {
        Unknown = 0,

        /// <summary>
        /// 加
        /// </summary>
        [SqlExpressionOperator("+", 110, 2)]
        Add,

        /// <summary>
        /// 减
        /// </summary>
        [SqlExpressionOperator("-", 110, 2)]
        Sub,

        /// <summary>
        /// 乘
        /// </summary>
        [SqlExpressionOperator("*", 100, 2)]
        Mul,

        /// <summary>
        /// 除
        /// </summary>
        [SqlExpressionOperator("/", 100, 2)]
        Div,

        /// <summary>
        /// 取余
        /// </summary>
        [SqlExpressionOperator("Mod", 100, 2)]
        Mod,

        /// <summary>
        /// 大于
        /// </summary>
        [SqlExpressionOperator(">", 120, 2)]
        Large,

        /// <summary>
        /// 大于或等于
        /// </summary>
        [SqlExpressionOperator(">=", 120, 2)]
        LargeEquals,

        /// <summary>
        /// 等于
        /// </summary>
        [SqlExpressionOperator("=", 120, 2)]
        Equals,

        /// <summary>
        /// 不等于
        /// </summary>
        [SqlExpressionOperator("<>", 120, 2)]
        NotEquals,

        /// <summary>
        /// 小于
        /// </summary>
        [SqlExpressionOperator("<", 120, 2)]
        Little,

        /// <summary>
        /// 小于或等于
        /// </summary>
        [SqlExpressionOperator("<=", 120, 2)]
        LittleEquals,

        /// <summary>
        /// 逻辑否
        /// </summary>
        [SqlExpressionOperator("Not", 130, 1)]
        Not,

        /// <summary>
        /// 逻辑与
        /// </summary>
        [SqlExpressionOperator("And", 140, 2)]
        And,

        /// <summary>
        /// 逻辑或
        /// </summary>
        [SqlExpressionOperator("Or", 150, 2)]
        Or,
    }
}

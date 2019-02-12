using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript
{
    /// <summary>
    /// 表达式的类型
    /// </summary>
    public enum ExpressionType
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown,

        /// <summary>
        /// 普通表达式
        /// </summary>
        Expression,

        /// <summary>
        /// VB常量
        /// </summary>
        VbConst,

        /// <summary>
        /// 普通常量
        /// </summary>
        Const,

        /// <summary>
        /// 变量
        /// </summary>
        Variable,
    }
}

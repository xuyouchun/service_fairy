using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data.SqlExpressions.Analyzer
{
    /// <summary>
    /// 符号的类型
    /// </summary>
    enum SymbolType
    {
        /// <summary>
        /// 运算符
        /// </summary>
        Operation,

        /// <summary>
        /// 括号
        /// </summary>
        Bricket,

        /// <summary>
        /// 变量名
        /// </summary>
        Variable,

        /// <summary>
        /// 值
        /// </summary>
        Value,

        /// <summary>
        /// 参数
        /// </summary>
        Parameter,

        /// <summary>
        /// 逗号
        /// </summary>
        Comma,

        /// <summary>
        /// 函数
        /// </summary>
        Function,
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Client.Script.Expressions
{
    /// <summary>
    /// 操作符类型
    /// </summary>
    enum OperatorType
    {
        [Operator("+", 100, 2, 1)]
        Add,

        [Operator("-", 100, 2, 1)]
        Substract,

        [Operator("*", 110, 2, 1)]
        Multiply,

        [Operator("/", 110, 2, 1)]
        Divide,

        [Operator("%", 110, 2, 1)]
        Mod,

        [Operator("-", 120, 1, 0)]
        Minus,

        [Operator("&&", 30, 2, 1)]
        And,

        [Operator("||", 20, 2, 1)]
        Or,

        [Operator("!", 120, 1, 0)]
        Not,

        [Operator("==", 70, 2, 1)]
        Equals,

        [Operator("!=", 70, 2, 1)]
        NotEquals,

        [Operator(">", 80, 2, 1)]
        LargeThan,

        [Operator(">=", 80, 2, 1)]
        LargeThanOrEquals,

        [Operator("<", 80, 2, 1)]
        LessThan,

        [Operator("<=", 80, 2, 1)]
        LessThanOrEquals,
    }
}

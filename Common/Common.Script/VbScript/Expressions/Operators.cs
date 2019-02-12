using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions
{
    /// <summary>
    /// 操作符类型
    /// </summary>
    enum Operators
    {
        [Operator("+", 100, 2, 1)]
        Add,

        [Operator("-", 100, 2, 1)]
        Substract,

        [Operator("*", 110, 2, 1)]
        Multiply,

        [Operator("/", 110, 2, 1)]
        Divide,

        [Operator("^", 115, 2, 1)]
        Pow,

        [Operator("MOD", 110, 2, 1, ShowInList = true)]
        Mod,

        [Operator("-", 120, 1, 0)]
        Minus,

        [Operator(".", 130, 2, 1)]
        Member,

        [Operator("NEW", 140, 1, 0, ShowInList = true)]
        New,

        [Operator("AND", 30, 2, 1, ShowInList = true)]
        And,

        [Operator("OR", 20, 2, 1, ShowInList = true)]
        Or,

        [Operator("NOT", 120, 1, 0, ShowInList = true)]
        Not,

        [Operator("=", 70, 2, 1)]
        Equals,

        [Operator("<>", 70, 2, 1)]
        NotEquals,

        [Operator(">", 80, 2, 1)]
        LargeThan,

        [Operator(">=", 80, 2, 1)]
        LargeThanOrEquals,

        [Operator("<", 80, 2, 1)]
        LessThan,

        [Operator("<=", 80, 2, 1)]
        LessThanOrEquals,

        [Operator("&", 90, 2, 1)]
        StringContact,
    }
}

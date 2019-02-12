using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// 语句块的类型
    /// </summary>
    enum StatementType
    {
        [StatementType("Main")]
        Main,

        [StatementType("Void")]
        Void,

        [StatementType("Dim")]
        Dim,

        [StatementType("Const")]
        Const,

        [StatementType("OptionExplicit")]
        OptionExplicit,

        [StatementType("Simple")]
        Simple,

        [StatementType("Complex")]
        Complex,

        [StatementType("If")]
        If,

        [StatementType("If Else")]
        IfElse,

        [StatementType("While")]
        While,

        [StatementType("Loop")]
        Loop,

        [StatementType("For")]
        For,

        [StatementType("Select")]
        Select,

        [StatementType("Exit")]
        Exit,

        [StatementType("Assign")]
        Assign,

        [StatementType("Function")]
        Function,

        [StatementType("Sub")]
        Sub,

        [StatementType("Class")]
        Class,

        [StatementType("Label")]
        Label,

        [StatementType("GoTo")]
        GoTo,

        [StatementType("OnError")]
        OnError,
    }
}

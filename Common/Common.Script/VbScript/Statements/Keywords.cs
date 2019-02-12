using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// 文本分析器分析出行的类型
    /// </summary>
    enum Keywords
    {
        [Keyword("DIM")]
        Dim,

        [Keyword("OPTION EXPLICIT")]
        OptionExplicit,

        [Keyword("IF")]
        If,

        [Keyword("END IF")]
        EndIf,

        [Keyword("ELSE")]
        Else,

        [Keyword("ELSEIF")]
        ElseIf,

        [Keyword("THEN")]
        Then,

        [Keyword("WHILE")]
        While,

        [Keyword("END WHILE")]
        EndWhile,

        [Keyword("EXIT WHILE")]
        ExitWhile,

        [Keyword("WEND")]
        WEnd,

        [Keyword("SELECT")]
        Select,

        [Keyword("END SELECT")]
        EndSelect,

        [Keyword("CASE")]
        Case,

        [Keyword("LOOP")]
        Loop,

        [Keyword("EXIT LOOP")]
        ExitLoop,

        [Keyword("UNTIL")]
        Until,

        [Keyword("=", false)]
        Assign,

        [Keyword("SET")]
        Set,

        [Keyword("FOR")]
        For,

        [Keyword("TO")]
        To,

        [Keyword("NEXT")]
        Next,

        [Keyword("STEP")]
        Step,

        [Keyword("EXIT FOR")]
        ExitFor,

        [Keyword("FUNCTION")]
        Function,

        [Keyword("END FUNCTION")]
        EndFunction,

        [Keyword("EXIT FUNCTION")]
        ExitFunction,

        [Keyword("SUB")]
        Sub,

        [Keyword("END SUB")]
        EndSub,

        [Keyword("EXIT SUB")]
        ExitSub,

        [Keyword("CLASS")]
        Class,

        [Keyword("END CLASS")]
        EndClass,

        [Keyword("PUBLIC")]
        Public,

        [Keyword("PRIVATE")]
        Private,

        [Keyword("CONST")]
        Const,

        [Keyword("CALL")]
        Call,

        [Keyword("GOTO")]
        GoTo,

        [Keyword("ON ERROR")]
        OnError,

        [Keyword("RESUME NEXT")]
        ResumeNext,

        Label,
    }
}

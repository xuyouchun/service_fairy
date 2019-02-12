using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Client.Script.Statements.StatementAnalysers
{
    /// <summary>
    /// 文本分析器分析出行的类型
    /// </summary>
    enum CharacterAnalyserStatementPartType
    {
        Unknown,

        Simple,

        BeginIf,

        EndIf,

        Else,

        ElseIf,

        BeginWhile,

        EndWhile,

        BeginSwitch,

        EndSwitch,

        Case,

        BeginDoWhile,

        EndDoWhile,

        SetValue,

        Remark,
    }
}

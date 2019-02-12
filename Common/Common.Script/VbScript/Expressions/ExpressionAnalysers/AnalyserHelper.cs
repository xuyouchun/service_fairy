using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.ExpressionAnalysers
{
    static class AnalyserHelper
    {
        public static ScriptException CreateException()
        {
            return new ScriptException("语法错误");
        }
    }
}

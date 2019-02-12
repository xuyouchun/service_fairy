using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements.StatementAnalysers.StatementAnalyserStackContexts
{
    class OnErrorStatementAnalyserStackContext : StatementAnalyserStackContext
    {
        public OnErrorStatementAnalyserStackContext(StatementAnalyserStack analyserStack, IStatementCompileContext context)
            : base(analyserStack, StatementType.OnError, context)
        {

        }

        private bool _MetOnError = false, _HasEnd = false;
        private string _LabelName;
        private Keywords? _Action;

        public override void PushExpression(Expression expression)
        {
            throw new ScriptException("On Error语法错误");
        }

        public override void PushStatement(Statement statement)
        {
            if (statement is GoToStatement)
            {
                _LabelName = ((GoToStatement)statement).LabelName;
                _Action = Keywords.GoTo; 
                _HasEnd = true;
                return;
            }

            throw new ScriptException("On Error语法错误");
        }

        public override bool PushKeyword(Keywords keyword)
        {
            if (keyword == Keywords.ResumeNext)
            {
                _HasEnd = true;
                _Action = Keywords.ResumeNext;
                return true;
            }

            throw new ScriptException("On Error遇到不能识别的关键字" + KeywordManager.GetKeywordInfo(keyword).Name);
        }

        public override bool HasEnd()
        {
            return _HasEnd;
        }

        public override Statement GetStatement()
        {
            if (_Action == null)
                throw new ScriptException("On Error语法错误");

            return new OnErrorStatement(_LabelName);
        }
    }
}

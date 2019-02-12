using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements.StatementAnalysers.StatementAnalyserStackContexts
{
    class WhileStatementAnalyserStackContext : StatementAnalyserStackContext
    {
        public WhileStatementAnalyserStackContext(StatementAnalyserStack analyserStack, IStatementCompileContext context)
            : base(analyserStack, StatementType.While, context)
        {

        }

        private Expression _WhileExpression;

        private readonly ComplexStatementBuilder _BodyBuilder = new ComplexStatementBuilder();
        private bool _HasEnd = false;

        public override void PushExpression(Expression expression)
        {
            if (_WhileExpression == null)
                _WhileExpression = expression;
            else
                PushStatement(CreateSimpleStatement(expression));
        }

        public override void PushStatement(Statement statement)
        {
            _BodyBuilder.AddStatement(statement);
        }

        public override bool PushKeyword(Keywords keyword)
        {
            if (keyword == Keywords.EndWhile || keyword == Keywords.WEnd)
            {
                _HasEnd = true;
                return true;
            }
            
            if(!base.PushKeyword(keyword))
                throw new ScriptException("WHILE语句遇到不可识别的关键字" + KeywordManager.GetKeywordInfo(keyword));

            return true;
        }

        public override bool HasEnd()
        {
            return _HasEnd;
        }

        public override Statement GetStatement()
        {
            if (_WhileExpression == null)
                throw new ScriptException("While语句缺少条件表达式");

            Expressions.Add(_WhileExpression);
            Statements.Add(_BodyBuilder.GetStatement());

            return base.GetStatement();
        }
    }
}

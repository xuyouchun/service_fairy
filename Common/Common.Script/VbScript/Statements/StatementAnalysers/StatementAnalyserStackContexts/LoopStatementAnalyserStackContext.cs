using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements.StatementAnalysers.StatementAnalyserStackContexts
{
    class LoopStatementAnalyserStackContext : StatementAnalyserStackContext
    {
        public LoopStatementAnalyserStackContext(StatementAnalyserStack analyserStack, IStatementCompileContext context)
            : base(analyserStack, StatementType.Loop, context)
        {

        }

        private bool _IsMetUntil = false, _HasEnd = false;
        private Expression _Condition;
        private readonly ComplexStatementBuilder _BodyBuilder = new ComplexStatementBuilder();

        public override void PushExpression(Expression expression)
        {
            if (!_IsMetUntil)
            {
                PushStatement(CreateSimpleStatement(expression));
            }
            else
            {
                if (_Condition == null)
                {
                    _Condition = expression;
                    _HasEnd = true;
                }
            }
        }

        public override void PushStatement(Statement statement)
        {
            if (_Condition != null)
                throw new ScriptException("LOOP...UNTIL语句语法错误");

            _BodyBuilder.AddStatement(statement);
        }

        public override bool PushKeyword(Keywords keyword)
        {
            if (keyword == Keywords.Until)
            {
                if (_IsMetUntil)
                    throw new ScriptException("LOOP...UNTIL语句中遇到多余的UNTIL关键字");

                _IsMetUntil = true;
                return true;
            }

            if(!base.PushKeyword(keyword))
                throw new ScriptException("LOOP...UNTIL语句语法错误，遇到未识别的关键字" + keyword);

            return true;
        }

        public override bool HasEnd()
        {
            return _HasEnd;
        }

        public override Statement GetStatement()
        {
            if (_Condition == null)
                throw new ScriptException("LOOP...UNTIL语句中缺少条件表达式");

            Expressions.Add(_Condition);
            Statements.Add(_BodyBuilder.GetStatement());

            return base.GetStatement();
        }
    }
}

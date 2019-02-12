using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements.StatementAnalysers.StatementAnalyserStackContexts
{
    class IfStatementAnalyserStackContext : StatementAnalyserStackContext
    {
        public IfStatementAnalyserStackContext(StatementAnalyserStack analyserStack, IStatementCompileContext context)
            : this(analyserStack, false, context)
        {

        }

        public IfStatementAnalyserStackContext(StatementAnalyserStack analyserStack, bool isInElseIf, IStatementCompileContext context)
            : base(analyserStack, StatementType.If, context)
        {
            _IsInElseIf = isInElseIf;
        }

        private readonly ComplexStatementBuilder _IfBodyBuilder = new ComplexStatementBuilder();
        private readonly ComplexStatementBuilder _ElseBodyBuilder = new ComplexStatementBuilder();
        private Expression _IfExpression;
        private bool _HasEnd = false, _IsMetNewLine = false;
        private readonly bool _IsInElseIf;

        public override void PushExpression(Expression expression)
        {
            if (_IfExpression == null)
            {
                _IfExpression = expression;
            }
            else
            {
                //if (!metThenKeyword)
                //    throw new ScriptException("IF语句语法错误，缺少THEN关键字");

                PushStatement(CreateSimpleStatement(expression));
            }
        }

        public override void PushStatement(Statement statement)
        {
            if (StatementType == StatementType.If)
            {
                _IfBodyBuilder.AddStatement(statement);

                if (!_IsMetNewLine)
                    _HasEnd = true;
            }
            else
            {
                _ElseBodyBuilder.AddStatement(statement);
            }
        }

        bool metThenKeyword = false;

        public override bool PushKeyword(Keywords keyword)
        {
            if (keyword == Keywords.Then)
            {
                if (_IfExpression == null)
                    throw new ScriptException("IF语句语法错误，缺少IF条件表达式");

                if(metThenKeyword)
                    throw new ScriptException("IF语句语法错误，多余的THEN关键字");

                metThenKeyword = true;
            }
            else if (keyword == Keywords.Else)
            {
                StatementType = StatementType.IfElse;
            }
            else if (keyword == Keywords.EndIf)
            {
                foreach (StatementAnalyserStackContext stackContext in AnalyserStack.GetStackContexts())
                {
                    IfStatementAnalyserStackContext ifCtx = stackContext as IfStatementAnalyserStackContext;
                    if (ifCtx == null)
                        break;

                    ifCtx._HasEnd = true;
                    if (!ifCtx._IsInElseIf)
                        break;
                }
            }
            else
            {
                if (!base.PushKeyword(keyword))
                    throw new ScriptException("IF语句语法错误");
            }

            return true;
        }

        public override void EndNewLine()
        {
            _IsMetNewLine = true;
        }

        public override bool HasEnd()
        {
            return _HasEnd;
        }

        public override Statement GetStatement()
        {
            if (_IfExpression == null)
                throw new ScriptException("IF语句语法错误，缺少IF条件表达式");

            Expressions.Add(_IfExpression);
            Statements.Add(_IfBodyBuilder.GetStatement());
            if (StatementType == StatementType.IfElse)
                Statements.Add(_ElseBodyBuilder.GetStatement());

            return base.GetStatement();
        }
    }
}

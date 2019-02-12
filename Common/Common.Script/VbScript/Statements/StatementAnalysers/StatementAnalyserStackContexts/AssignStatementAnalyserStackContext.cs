using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript.Statements.StatementAnalysers.StatementAnalyserStackContexts
{
    class AssignStatementAnalyserStackContext : StatementAnalyserStackContext
    {
        public AssignStatementAnalyserStackContext(StatementAnalyserStack analyserStack, IStatementCompileContext context)
            : base(analyserStack, StatementType.Assign, context)
        {

        }

        private ILeftValueExpression _LeftValue;
        private Expression _RightValue;
        private bool _HasMetAssignOperator = false, _HasEnd = false;

        public override void PushExpression(Expression expression)
        {
            if (_LeftValue == null && !_HasMetAssignOperator)
            {
                _LeftValue = expression as ILeftValueExpression;
                if (_LeftValue == null)
                    throw new ScriptException("指定的表达式“" + expression + "”不能作为左值");
            }
            else if (_RightValue == null && _HasMetAssignOperator)
            {
                _RightValue = expression;
                _HasEnd = true;
            }
            else
            {
                throw new ScriptException("赋值语句语法错误");
            }
        }

        public override void PushStatement(Statement statement)
        {
            throw new ScriptException("赋值语句语法错误");
        }

        public override bool PushKeyword(Keywords keyword)
        {
            if (keyword == Keywords.Assign)
            {
                if (_HasMetAssignOperator)
                    throw new ScriptException("多余的赋值运算符");

                _HasMetAssignOperator = true;
                return true;
            }

            if (!base.PushKeyword(keyword))
                throw new ScriptException("赋值语句语法错误");

            return true;
        }

        public override bool HasEnd()
        {
            return _HasEnd;
        }

        public override Statement GetStatement()
        {
            if (_LeftValue == null || _RightValue == null)
                throw new ScriptException("赋值语句语法错误");

            return new AssignStatement(_LeftValue, _RightValue);
        }
    }
}

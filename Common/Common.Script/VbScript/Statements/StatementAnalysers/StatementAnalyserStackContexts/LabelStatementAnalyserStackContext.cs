using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript.Statements.StatementAnalysers.StatementAnalyserStackContexts
{
    /// <summary>
    /// 用于加载Label语句
    /// </summary>
    class LabelStatementAnalyserStackContext : StatementAnalyserStackContext
    {
        public LabelStatementAnalyserStackContext(StatementAnalyserStack analyserStack, IStatementCompileContext context)
            : base(analyserStack, StatementType.Label, context)
        {

        }

        private string _LabelName;
        private bool _HasEnd = false;

        public override void PushExpression(Expression expression)
        {
            if (_LabelName == null)
            {
                if (expression.GetType() != typeof(VariableExpression))
                    throw new ScriptException("LABEL名字格式错误:" + ((VariableExpression)expression).Name);

                _LabelName = ((VariableExpression)expression).Name;
                _HasEnd = true;
            }
            else
            {
                throw new ScriptException("LABEL语句中遇到多余的标签");
            }
        }

        public override bool PushKeyword(Keywords keyword)
        {
            throw new ScriptException("LABEL语句中遇到不能识别的关键字:" + KeywordManager.GetKeywordInfo(keyword));
        }

        public override void EndNewLine()
        {
            if (_LabelName == null)
                throw new ScriptException("LABEL语句未指定名字");
        }

        public override void PushStatement(Statement statement)
        {
            throw new ScriptException("LABEL语句格式错误");
        }

        public override bool HasEnd()
        {
            return _HasEnd;
        }

        public override Statement GetStatement()
        {
            if (_LabelName == null)
                throw new ScriptException("LABEL语句未指定名字");

            return new LabelStatement(_LabelName);
        }
    }
}

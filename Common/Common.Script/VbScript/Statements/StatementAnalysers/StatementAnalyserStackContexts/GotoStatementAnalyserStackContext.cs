using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript.Statements.StatementAnalysers.StatementAnalyserStackContexts
{
    /// <summary>
    /// 用于加载GOTO语句
    /// </summary>
    class GotoStatementAnalyserStackContext : StatementAnalyserStackContext
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="analyserStack"></param>
        public GotoStatementAnalyserStackContext(StatementAnalyserStack analyserStack, IStatementCompileContext context)
            : base(analyserStack, StatementType.GoTo, context)
        {

        }

        private string _LabelName;
        private bool _HasEnd = false;

        public override void PushExpression(Expression expression)
        {
            if (_LabelName == null)
            {
                if (expression.GetType() != typeof(VariableExpression))
                    throw new ScriptException("GOTO语句语法错误");

                _LabelName = ((VariableExpression)expression).Name;

                _HasEnd = true;
            }
            else
            {
                throw new ScriptException("GOTO语句遇到多余的标签");
            }
        }

        public override void EndNewLine()
        {
            if (_LabelName == null)
                throw new ScriptException("GOTO语句未指定标签");
        }

        public override bool HasEnd()
        {
            return _HasEnd;
        }

        public override bool PushKeyword(Keywords keyword)
        {
            throw new ScriptException("GOTO语句中遇到不能识别的关键字:" + KeywordManager.GetKeywordInfo(keyword));
        }

        public override Statement GetStatement()
        {
            if (_LabelName == null)
                throw new ScriptException("GOTO语句未指定标签");

            return new GoToStatement(_LabelName);
        }
    }
}

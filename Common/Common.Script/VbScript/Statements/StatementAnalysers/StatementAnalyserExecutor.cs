using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements.StatementAnalysers
{
    /// <summary>
    /// 分析器
    /// </summary>
    unsafe class StatementAnalyserExecutor
    {
        public StatementAnalyserExecutor(char* ptr, IStatementCompileContext context)
        {
            _AnalyserStack = new StatementAnalyserStack(context);
            _CharacterAnalyserExecutor = new CharacterAnalyserExecutor(ptr, new StatementCompileContextProxy(context, _AnalyserStack));
            _CharacterAnalyserExecutor.NewKeyword += new CharacterAnalyserExecutorNewKeywordEventHandler(_CharacterAnalyserExecutor_NewStatementPart);
            _CharacterAnalyserExecutor.NewExpression += new CharacterAnalyserStackNewExpressionEventHandler(_CharacterAnalyserExecutor_NewExpression);
            _CharacterAnalyserExecutor.BeginNewLine += new EventHandler(_CharacterAnalyserExecutor_BeginNewLine);
            _CharacterAnalyserExecutor.EndNewLine += new EventHandler(_CharacterAnalyserExecutor_EndNewLine);
        }

        #region Class StatementCompileContextProxy ...

        class StatementCompileContextProxy : IStatementCompileContext
        {
            public StatementCompileContextProxy(IStatementCompileContext context, StatementAnalyserStack stack)
            {
                _ExpressionCompileContext = new ExpressionCompileContextProxy(context.ExpressionCompileContext, stack);
            }

            private readonly IExpressionCompileContext _ExpressionCompileContext;

            #region IStatementCompileContext 成员

            public IExpressionCompileContext ExpressionCompileContext
            {
                get { return _ExpressionCompileContext; }
            }

            #endregion
        }

        #endregion

        #region Class ExpressionCompileContextProxy ...

        class ExpressionCompileContextProxy : IExpressionCompileContext
        {
            public ExpressionCompileContextProxy(IExpressionCompileContext context, StatementAnalyserStack stack)
            {
                _Context = context;
                _Stack = stack;
            }

            private readonly IExpressionCompileContext _Context;
            private readonly StatementAnalyserStack _Stack;

            #region IExpressionCompileContext 成员

            public Value GetConstValue(string name)
            {
                Value value = _Stack.GetConstValue(name);
                if ((object)value != null)
                    return value;

                value = _Context.GetConstValue(name);
                if ((object)value != null)
                    return value;

                return null;
            }

            #endregion
        }

        #endregion

        void _CharacterAnalyserExecutor_NewExpression(object sender, CharacterAnalyserStackNewExpressionEventArgs e)
        {
            _AnalyserStack.PushExpression(e.Expression);
        }

        void _CharacterAnalyserExecutor_NewStatementPart(object sender, CharacterAnalyserExecutorNewKeywordEventArgs e)
        {
            _AnalyserStack.PushKeyword(e.Keyword);
        }

        void _CharacterAnalyserExecutor_BeginNewLine(object sender, EventArgs e)
        {
            _AnalyserStack.BeginNewLine();
        }

        void _CharacterAnalyserExecutor_EndNewLine(object sender, EventArgs e)
        {
            _AnalyserStack.EndNewLine();
        }

        private readonly CharacterAnalyserExecutor _CharacterAnalyserExecutor;
        private readonly StatementAnalyserStack _AnalyserStack;

        /// <summary>
        /// 执行分析过程
        /// </summary>
        /// <returns></returns>
        public Statement Analyse()
        {
            _CharacterAnalyserExecutor.Analyse();

            return _AnalyserStack.GetStatement();
        }
    }
}

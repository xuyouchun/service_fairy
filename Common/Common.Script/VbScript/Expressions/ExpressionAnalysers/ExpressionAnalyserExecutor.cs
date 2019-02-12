using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.ExpressionAnalysers
{
    /// <summary>
    /// 分析器
    /// </summary>
    unsafe class ExpressionAnalyserExecutor
    {
        public ExpressionAnalyserExecutor(char* ptr, IExpressionCompileContext compileContext)
        {
            _Ptr = ptr;

            _CharacterStack = new CharacterAnalyserExecutor(ptr);
            _CharacterStack.NewExpression += new CharacterAnalyserStackNewExpressionEventHandler(_CharacterStack_NewExpression);
            _CharacterStack.NewOperator += new CharacterAnalyserStackNewOperatorEventHandler(_CharacterStack_NewOperator);
            _CharacterStack.BeginNewExpressionGroup += new EventHandler(_CharacterStack_BeginNewExpressionGroup);
            _CharacterStack.EndNewExpressionGroup += new EventHandler(_CharacterStack_EndNewExpressionGroup);
            _CharacterStack.NewDynamicExpression += new CharacterAnalyserStackNewDynamicExpressionEventHandler(_CharacterStack_NewDynamicExpression);
            _CharacterStack.EndExpression += new EventHandler(_CharacterStack_EndExpression);

            _AnalyserStackManager = new ExpressionAnalyserStackManager(compileContext);
        }

        private readonly char* _Ptr;

        /// <summary>
        /// 加载表达式
        /// </summary>
        /// <returns></returns>
        public Expression[] Parse()
        {
            _CharacterStack.Analyse();

            return _AnalyserStackManager.GetExpressions();
        }

        private readonly CharacterAnalyserExecutor _CharacterStack;
        private readonly ExpressionAnalyserStackManager _AnalyserStackManager;

        void _CharacterStack_BeginNewExpressionGroup(object sender, EventArgs e)
        {
            _AnalyserStackManager.BeginExpressionGroup();
        }

        void _CharacterStack_EndNewExpressionGroup(object sender, EventArgs e)
        {
            _AnalyserStackManager.EndExpressionGroup();
        }

        /// <summary>
        /// 生成新的运算符
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _CharacterStack_NewOperator(object sender, CharacterAnalyserStackNewOperatorEventArgs e)
        {
            _AnalyserStackManager.PushOperator(e.OperatorInfo);
        }

        /// <summary>
        /// 生成新的表达式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _CharacterStack_NewExpression(object sender, CharacterAnalyserStackNewExpressionEventArgs e)
        {
            _AnalyserStackManager.PushExpression(e.Expression);
        }

        void _CharacterStack_NewDynamicExpression(object sender, CharacterAnalyserStackNewDynamicExpressionEventArgs e)
        {
            _AnalyserStackManager.PushDynamicExpression(e.Name);
        }

        void _CharacterStack_EndExpression(object sender, EventArgs e)
        {
            _AnalyserStackManager.EndExpression();
        }
    }
}

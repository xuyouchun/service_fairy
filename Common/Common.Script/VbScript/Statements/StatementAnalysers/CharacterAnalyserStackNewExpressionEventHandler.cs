using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements.StatementAnalysers
{
    /// <summary>
    /// 生成新的表达式
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    delegate void CharacterAnalyserStackNewExpressionEventHandler(object sender, CharacterAnalyserStackNewExpressionEventArgs e);

    /// <summary>
    /// 生成新的表达式事件的参数
    /// </summary>
    class CharacterAnalyserStackNewExpressionEventArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="exp"></param>
        public CharacterAnalyserStackNewExpressionEventArgs(Expression exp)
        {
            Expression = exp;
        }

        /// <summary>
        /// 表达式
        /// </summary>
        public Expression Expression { get; private set; }
    }
}

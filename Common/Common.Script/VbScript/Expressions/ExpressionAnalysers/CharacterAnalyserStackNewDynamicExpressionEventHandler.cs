using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.ExpressionAnalysers
{
    /// <summary>
    /// 生成新的函数表达式
    /// </summary>
    /// <param name="sender"></param>
    delegate void CharacterAnalyserStackNewDynamicExpressionEventHandler(object sender, CharacterAnalyserStackNewDynamicExpressionEventArgs e);

    /// <summary>
    /// 生成新的函数表达式事件的参数
    /// </summary>
    class CharacterAnalyserStackNewDynamicExpressionEventArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="functionName"></param>
        public CharacterAnalyserStackNewDynamicExpressionEventArgs(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}

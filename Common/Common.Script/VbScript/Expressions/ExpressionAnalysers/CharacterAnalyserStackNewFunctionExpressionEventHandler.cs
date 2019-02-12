using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Client.Script.Expressions.ExpressionAnalysers
{
    /// <summary>
    /// 生成新的函数表达式
    /// </summary>
    /// <param name="sender"></param>
    delegate void CharacterAnalyserStackNewFunctionExpressionEventHandler(object sender, CharacterAnalyserStackNewFunctionExpressionEventArgs e);

    /// <summary>
    /// 生成新的函数表达式事件的参数
    /// </summary>
    class CharacterAnalyserStackNewFunctionExpressionEventArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="functionName"></param>
        public CharacterAnalyserStackNewFunctionExpressionEventArgs(string functionName)
        {
            FunctionName = functionName;
        }

        public string FunctionName { get; private set; }
    }
}

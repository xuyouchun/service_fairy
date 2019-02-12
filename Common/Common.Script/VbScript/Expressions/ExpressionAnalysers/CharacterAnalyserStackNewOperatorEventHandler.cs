using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.ExpressionAnalysers
{
    /// <summary>
    /// 生成新的运算符
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    delegate void CharacterAnalyserStackNewOperatorEventHandler(object sender, CharacterAnalyserStackNewOperatorEventArgs e);

    /// <summary>
    /// 生成新的运算符事件的参数
    /// </summary>
    class CharacterAnalyserStackNewOperatorEventArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="operatorInfo"></param>
        public CharacterAnalyserStackNewOperatorEventArgs(OperatorInfo operatorInfo)
        {
            OperatorInfo = operatorInfo;
        }

        /// <summary>
        /// 运算符
        /// </summary>
        public OperatorInfo OperatorInfo { get; private set; }
    }
}

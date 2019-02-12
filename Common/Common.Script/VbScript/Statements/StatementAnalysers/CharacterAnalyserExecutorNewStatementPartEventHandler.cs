using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Client.Script.Statements.StatementAnalysers
{
    /// <summary>
    /// 生成新的语句块
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    delegate void CharacterAnalyserExecutorNewStatementPartEventHandler(object sender, CharacterAnalyserExecutorNewStatementPartEventArgs e);

    /// <summary>
    /// 生成新的语句块事件的参数
    /// </summary>
    class CharacterAnalyserExecutorNewStatementPartEventArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lineInfo"></param>
        public CharacterAnalyserExecutorNewStatementPartEventArgs(CharacterAnalyserStatementPartType partType)
        {
            PartType = partType;
        }

        /// <summary>
        /// 语句块
        /// </summary>
        public CharacterAnalyserStatementPartType PartType { get; private set; }
    }
}

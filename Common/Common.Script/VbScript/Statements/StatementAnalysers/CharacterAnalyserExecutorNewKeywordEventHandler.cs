using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements.StatementAnalysers
{
    /// <summary>
    /// 生成新的语句块
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    delegate void CharacterAnalyserExecutorNewKeywordEventHandler(object sender, CharacterAnalyserExecutorNewKeywordEventArgs e);

    /// <summary>
    /// 生成新的语句块事件的参数
    /// </summary>
    class CharacterAnalyserExecutorNewKeywordEventArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lineInfo"></param>
        public CharacterAnalyserExecutorNewKeywordEventArgs(Keywords keyword)
        {
            Keyword = keyword;
        }

        /// <summary>
        /// 关键字
        /// </summary>
        public Keywords Keyword { get; private set; }
    }
}

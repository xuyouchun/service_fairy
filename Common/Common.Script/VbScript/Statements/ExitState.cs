using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// 退出语句的类型
    /// </summary>
    class ExitState
    {
        public ExitState(StatementType statementType)
        {
            StatementType = statementType;
        }

        /// <summary>
        /// 退出至哪种类型的语句
        /// </summary>
        public StatementType StatementType { get; private set; }
    }
}

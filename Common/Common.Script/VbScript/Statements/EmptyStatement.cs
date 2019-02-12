using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// 空的语句块
    /// </summary>
    class EmptyStatement : StatementBase
    {
        protected override void OnExecute(RunningContext context)
        {
            
        }

        internal override StatementType GetStatementType()
        {
            return StatementType.Void;
        }

        public override string ToString()
        {
            return "Void";
        }
    }
}

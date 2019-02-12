using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Client.Script.Statements
{
    /// <summary>
    /// 空的语句块
    /// </summary>
    class VoidStatement : StatementBase
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

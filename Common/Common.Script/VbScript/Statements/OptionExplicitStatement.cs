using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// 用于标明变量都需要事先声明
    /// </summary>
    class OptionExplicitStatement : StatementBase
    {
        protected override void OnExecute(RunningContext context)
        {
            context.IsOptionExplicit = true;
        }

        internal override StatementType GetStatementType()
        {
            return StatementType.OptionExplicit;
        }

        public override string ToString()
        {
            return "Option Explicit";
        }
    }
}

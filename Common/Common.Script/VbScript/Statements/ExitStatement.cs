using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// 用于终止循环语句的执行
    /// </summary>
    class ExitStatement : StatementBase
    {
        public ExitStatement(StatementType exitStatementType)
        {
            ExitStatementType = exitStatementType;
        }

        public StatementType ExitStatementType { get; private set; }

        protected override void OnExecute(RunningContext context)
        {
            context.ExitState = new ExitState(ExitStatementType);
        }

        internal override StatementType GetStatementType()
        {
            return StatementType.Exit;
        }

        public override string ToString()
        {
            return "Exit " + ExitStatementType;
        }
    }
}

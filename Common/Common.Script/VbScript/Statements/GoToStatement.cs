using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Common;

namespace Common.Script.VbScript.Statements
{
    [Statement(StatementType.GoTo)]
    class GoToStatement : StatementBase
    {
        public GoToStatement(string labelName)
        {
            LabelName = labelName;
        }

        public string LabelName { get; private set; }

        protected override void OnExecute(RunningContext context)
        {
            LabelLibrary library = context.GetCurrentField().GetLabelLibrary();
            LabelPath lp = library.GetLabelPath(LabelName);

            if (lp == null)
                throw new ScriptRuntimeException("未定义标签：" + LabelName);

            context.GoToLabelState = new GoToLabelState(context, lp);
        }

        internal override StatementType GetStatementType()
        {
            return StatementType.GoTo;
        }

        public override string ToString()
        {
            return "GOTO " + LabelName;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Common;

namespace Common.Script.VbScript.Statements
{
    [Statement(StatementType.OnError)]
    class OnErrorStatement : StatementBase
    {
        public OnErrorStatement(string gotoLabel)
        {
            _GotoLabel = gotoLabel;
        }

        private readonly string _GotoLabel;

        protected override void OnExecute(RunningContext context)
        {
            if (_GotoLabel == null)
                context.GetCurrentField().OnErrorResumeNext = true;
            else
            {
                LabelLibrary library = context.GetCurrentField().GetLabelLibrary();
                LabelPath lp = library.GetLabelPath(_GotoLabel);

                if (lp == null)
                    throw new ScriptRuntimeException("未定义标签：" + _GotoLabel);

                context.GoToLabelState = new GoToLabelState(context, lp);
            }
        }

        public override string ToString()
        {
            return "ON ERROR " + (_GotoLabel == null ? "RESUME NEXT" : ("GOTO " + _GotoLabel));
        }
    }
}

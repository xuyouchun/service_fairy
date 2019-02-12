using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// 标签
    /// </summary>
    [Statement(StatementType.Label)]
    class LabelStatement : StatementBase
    {
        public LabelStatement(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        protected override void OnExecute(RunningContext context)
        {
            
        }

        internal override StatementType GetStatementType()
        {
            return StatementType.Label;
        }

        public override string ToString()
        {
            return "Label: " + Name;
        }
    }
}

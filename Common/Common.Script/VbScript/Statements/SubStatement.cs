using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript.Statements
{
    [Statement(StatementType.Sub)]
    class SubStatement : FunctionStatement
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="subName">过程名</param>
        /// <param name="parameters">参数</param>
        /// <param name="body">函数体</param>
        /// <param name="ownerClassName">所属类名</param>
        public SubStatement(string subName, VariableExpression[] parameters, Statement body, string ownerClassName)
            : base(subName, parameters, body, ownerClassName)
        {
            
        }

        internal override StatementType GetStatementType()
        {
            return StatementType.Sub;
        }

        protected override void OnExecute(RunningContext context)
        {
            Body.Execute(context);
        }

        public override Value GetResult(RunningContext context)
        {
            return Value.Void;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    class MainStatement : Statement, IGotoSupported, IStatementField
    {
        public MainStatement(Statement[] statements)
        {
            Statements = statements;
        }

        public Statement[] Statements { get; private set; }

        protected override void OnExecute(RunningContext context)
        {
            foreach (Statement statement in Statements)
            {
                if (!statement.Execute(context))
                    break;
            }
        }

        #region IGotoSupported 成员

        void IGotoSupported.Execute(RunningContext context, Statement statement)
        {
            int index = ((IList<Statement>)Statements).IndexOf(statement);

            for (int k = 0; k < index; k++)
            {
                DimStatement dim = Statements[k] as DimStatement;
                if (dim != null)
                    dim.Execute(context);
            }

            for (int k = index; k < Statements.Length; k++)
            {
                Statement sta = Statements[k];
                if (sta is GoToStatement)  // 如果遇到GOTO语句
                {
                    context.GoToLabelState = null;
                    sta.Execute(context);
                    break;
                }
                else if (!Statements[k].Execute(context))
                {
                    break;
                }
            }
        }

        #endregion

        internal override StatementType GetStatementType()
        {
            return StatementType.Main;
        }

        internal override IEnumerable<Statement> GetChildStatements()
        {
            return Statements;
        }

        internal override IEnumerable<Expression> GetAllExpressions()
        {
            yield break;
        }

        public override string ToString()
        {
            return "主函数";
        }

        #region IStatementField 成员

        private LabelLibrary _LabelLibrary;

        public LabelLibrary GetLabelLibrary()
        {
            return _LabelLibrary ?? (_LabelLibrary = LabelLibrary.LoadFromStatement(this));
        }

        public bool OnErrorResumeNext { get; set; }

        #endregion
    }
}

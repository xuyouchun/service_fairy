using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// 复合语句代码块
    /// </summary>
    [Statement(StatementType.Complex, typeof(Creator))]
    class ComplexStatement : StatementBase, IGotoSupported
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="statements">语句块的集合</param>
        public ComplexStatement(Statement[] statements)
        {
            Statements = statements;
        }

        /// <summary>
        /// 语句块的集合
        /// </summary>
        public Statement[] Statements { get; private set; }

        /// <summary>
        /// 执行
        /// </summary>
        protected override void OnExecute(RunningContext context)
        {
            IExpressionContext oldContext = context.ExpressionContext;
            context.ExpressionContext = new LocalExpressionContext(oldContext, context.GlobalExpressonContext);  // 用于支持局部变量

            try
            {
                foreach (Statement statement in Statements)
                {
                    if (!statement.Execute(context))
                        return;
                }
            }
            finally
            {
                context.ExpressionContext = oldContext;
            }
        }

        #region IGotoSupported 成员

        void IGotoSupported.Execute(RunningContext context, Statement statement)
        {
            int index = ((IList<Statement>)Statements).IndexOf(statement);

            IExpressionContext oldContext = context.ExpressionContext;
            context.ExpressionContext = new LocalExpressionContext(oldContext, context.GlobalExpressonContext);  // 用于支持局部变量

            try
            {
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
            finally
            {
                context.ExpressionContext = oldContext;
            }
        }

        #endregion

        internal override IEnumerable<Statement> GetChildStatements()
        {
            return Statements;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Statement statement in Statements)
            {
                sb.AppendLine(statement.ToString());
            }

            return sb.ToString();
        }

        class Creator : IStatementCreator
        {
            #region IStatementCreator 成员

            public Statement Create(Expression[] expressions, Statement[] statements)
            {
                if (expressions.Length != 0)
                    throw new ScriptException("语法错误");

                return new ComplexStatement(statements);
            }

            #endregion
        }
    }
}

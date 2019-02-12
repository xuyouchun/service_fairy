using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// Switch分支语句
    /// </summary>
    [Statement(StatementType.Select, typeof(Creator))]
    class SelectStatement : StatementBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="cases"></param>
        /// <param name="defaultStatement"></param>
        public SelectStatement(Expression valueExpression, SelectStatementCase[] cases, Statement caseElseStatement)
        {
            ValueExpression = valueExpression;
            Cases = cases;
            CaseElseStatement = caseElseStatement;
        }

        /// <summary>
        /// 值的表达式
        /// </summary>
        public Expression ValueExpression { get; private set; }

        /// <summary>
        /// Case组合
        /// </summary>
        public SelectStatementCase[] Cases { get; private set; }

        /// <summary>
        /// default语句块
        /// </summary>
        public Statement CaseElseStatement { get; private set; }

        /// <summary>
        /// 执行
        /// </summary>
        protected override void OnExecute(RunningContext context)
        {
            Value value = ValueExpression.Execute(context.ExpressionContext);

            foreach (SelectStatementCase @case in Cases)
            {
                if (value == @case.Expression.Execute(context.ExpressionContext))
                {
                    @case.Statement.Execute(context);
                    return;
                }
            }

            CaseElseStatement.Execute(context);
        }

        public override string ToString()
        {
            return "Select...Case...";
        }

        internal override IEnumerable<Statement> GetChildStatements()
        {
            foreach (SelectStatementCase @case in Cases)
            {
                yield return @case.Statement;
            }
        }

        internal override IEnumerable<Expression> GetAllExpressions()
        {
            yield return ValueExpression;

            foreach (var item in Cases)
            {
                yield return item.Expression;
            }
        }

        class Creator : IStatementCreator
        {

            #region IStatementCreator 成员

            public Statement Create(Expression[] expressions, Statement[] statements)
            {
                int v = statements.Length - expressions.Length + 1;
                if (v < 0 || v > 1 || expressions.Length == 0)
                    throw new ScriptException("SWITCH语句语法错误");

                Statement defaultStatement = v == 1 ? statements[statements.Length - 1] : null;
                return new SelectStatement(expressions[0], _GetSwitchStatementCases(expressions, statements), defaultStatement);
            }

            private SelectStatementCase[] _GetSwitchStatementCases(Expression[] expressions, Statement[] statements)
            {
                SelectStatementCase[] cases = new SelectStatementCase[expressions.Length - 1];
                for (int k = 0; k < cases.Length; k++)
                {
                    cases[k] = new SelectStatementCase(expressions[k + 1], statements[k]);
                }

                return cases;
            }

            #endregion
        }
    }

    /// <summary>
    /// Case组合
    /// </summary>
    class SelectStatementCase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="statement"></param>
        public SelectStatementCase(Expression expression, Statement statement)
        {
            Expression = expression;
            Statement = statement;
        }

        /// <summary>
        /// 表达式
        /// </summary>
        public Expression Expression { get; private set; }

        /// <summary>
        /// 语句
        /// </summary>
        public Statement Statement { get; private set; }
    }
}

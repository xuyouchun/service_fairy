using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// 以单条语句组成的代码块
    /// </summary>
    [Statement(StatementType.Simple, typeof(Creator))]
    class SimpleStatement : StatementBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="expression"></param>
        public SimpleStatement(Expression expression)
        {
            Expression = expression;
        }

        public Expression Expression { get; private set; }

        /// <summary>
        /// 执行
        /// </summary>
        protected override void OnExecute(RunningContext context)
        {
            Expression.Execute(context.ExpressionContext);
        }

        internal override IEnumerable<Expression> GetAllExpressions()
        {
            yield return Expression;
        }

        public override string ToString()
        {
            return Expression.ToString();
        }

        class Creator : IStatementCreator
        {

            #region IStatementCreator 成员

            public Statement Create(Expression[] expressions, Statement[] statements)
            {
                if (expressions.Length != 1 || statements.Length != 0)
                    throw new ScriptException("语句语法错误");

                return new SimpleStatement(expressions[0]);
            }

            #endregion
        }
    }
}

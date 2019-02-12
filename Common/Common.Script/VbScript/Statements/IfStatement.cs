using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// IF语句
    /// </summary>
    [Statement(StatementType.If, typeof(Creator))]
    class IfStatement : StatementBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="body">执行体</param>
        public IfStatement(Expression condition, Statement body)
        {
            Condition = condition;
            Body = body;
        }

        /// <summary>
        /// 条件
        /// </summary>
        public Expression Condition { get; private set; }

        /// <summary>
        /// 执行体
        /// </summary>
        public Statement Body { get; private set; }

        /// <summary>
        /// 执行
        /// </summary>
        protected override void OnExecute(RunningContext context)
        {
            Value result = Condition.Execute(context.ExpressionContext);
            if (result.GetValueType() != typeof(bool))
                throw new ScriptException("IF语句的表达式非BOOL型");

            if ((bool)result)
                Body.Execute(context);
        }

        public override string ToString()
        {
            return "If...";
        }

        internal override IEnumerable<Statement> GetChildStatements()
        {
            yield return Body;
        }

        internal override IEnumerable<Expression> GetAllExpressions()
        {
            yield return Condition;
        }

        class Creator : IStatementCreator
        {

            #region IStatementCreator 成员

            public Statement Create(Expression[] expressions, Statement[] statements)
            {
                if (expressions.Length != 1 || statements.Length != 1)
                    throw new ScriptException("IF语句语法错误");

                return new IfStatement(expressions[0], statements[0]);
            }

            #endregion
        }
    }
}

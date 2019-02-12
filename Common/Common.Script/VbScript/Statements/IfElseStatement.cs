using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// If...Else...语句
    /// </summary>
    [Statement(StatementType.IfElse, typeof(Creator))]
    class IfElseStatement : StatementBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="ifBody">If语句的代码体</param>
        /// <param name="elseBody">Else语句的代码体</param>
        public IfElseStatement(Expression condition, Statement ifBody, Statement elseBody)
        {
            Condition = condition;
            IfBody = ifBody;
            ElseBody = elseBody;
        }

        /// <summary>
        /// 条件表达式
        /// </summary>
        public Expression Condition { get; private set; }

        /// <summary>
        /// If语句的代码体
        /// </summary>
        public Statement IfBody { get; private set; }

        /// <summary>
        /// Else语句的代码体
        /// </summary>
        public Statement ElseBody { get; private set; }

        /// <summary>
        /// 执行
        /// </summary>
        protected override void OnExecute(RunningContext context)
        {
            Value result = Condition.Execute(context.ExpressionContext);
            if (result.GetValueType() != typeof(bool))
                throw new ScriptException("IF语句的表达式非BOOL型");

            if ((bool)result)
                IfBody.Execute(context);
            else
                ElseBody.Execute(context);
        }

        public override string ToString()
        {
            return "If...Else...";
        }

        internal override IEnumerable<Statement> GetChildStatements()
        {
            yield return IfBody;
            yield return ElseBody;
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
                if (expressions.Length != 1 || statements.Length != 2)
                    throw new ScriptException("IF...Else...语句语法错误");

                return new IfElseStatement(expressions[0], statements[0], statements[1]);
            }

            #endregion
        }
    }
}

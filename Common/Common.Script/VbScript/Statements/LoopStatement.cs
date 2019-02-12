using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    /// <summary>
    /// Do...While...语句块
    /// </summary>
    [Statement(StatementType.Loop, typeof(Creator))]
    class LoopStatement : StatementBase, IGotoSupported
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="condition">循环条件</param>
        /// <param name="body">循环体</param>
        public LoopStatement(Expression condition, Statement body)
        {
            Condition = condition;
            Body = body;
        }

        /// <summary>
        /// 循环条件
        /// </summary>
        public Expression Condition { get; private set; }

        /// <summary>
        /// 循环体
        /// </summary>
        public Statement Body { get; private set; }

        /// <summary>
        /// 执行
        /// </summary>
        protected override void OnExecute(RunningContext context)
        {
            do
            {
                if (!Body.Execute(context))
                    return;

            } while (_GetConditionResult(context));
        }

        #region IGotoSupported 成员

        void IGotoSupported.Execute(RunningContext context, Statement statement)
        {
            do
            {
                if (!Body.Execute(context))
                    return;

            } while (_GetConditionResult(context));
        }

        #endregion

        private bool _GetConditionResult(RunningContext context)
        {
            Value result = Condition.Execute(context.ExpressionContext);
            if (result.GetValueType() != typeof(bool))
                throw new ScriptException("WHILE语句的循环条件非BOOL型");

            return (bool)result;
        }

        public override string ToString()
        {
            return "LOOP...UNTIL";
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
                    throw new ScriptException("DO...WHILE语句语法错误");

                return new LoopStatement(expressions[0], statements[0]);
            }

            #endregion
        }
    }
}

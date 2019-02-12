using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript.Statements
{
    [Statement(StatementType.Assign, typeof(Creator))]
    class AssignStatement : StatementBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lValue">左值</param>
        /// <param name="rValue">右值</param>
        public AssignStatement(ILeftValueExpression lValue, Expression rValue)
        {
            LeftValue = lValue;
            RightValue = rValue;
        }

        /// <summary>
        /// 左值
        /// </summary>
        public ILeftValueExpression LeftValue { get; private set; }

        /// <summary>
        /// 右值
        /// </summary>
        public Expression RightValue { get; private set; }

        protected override void OnExecute(RunningContext context)
        {
            LeftValue.SetValue(context.ExpressionContext, RightValue.Execute(context.ExpressionContext));
        }

        internal override IEnumerable<Expression> GetAllExpressions()
        {
            Expression exp = LeftValue as Expression;
            if (exp != null)
                yield return exp;

            yield return RightValue;
        }

        public override string ToString()
        {
            return string.Format("{0} = {1}", LeftValue, RightValue);
        }

        class Creator : IStatementCreator
        {

            #region IStatementCreator 成员

            public Statement Create(Expression[] expressions, Statement[] statements)
            {
                if (expressions.Length != 2 && statements.Length != 0)
                    throw new ScriptException("赋值语句语法错误");

                ILeftValueExpression lValue = expressions[0] as ILeftValueExpression;
                if (lValue == null)
                    throw new ScriptException("表达式“" + expressions[0].ToString() + "”不能作为左值");

                return new AssignStatement(lValue, expressions[1]);
            }

            #endregion
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Values;
using Common.Script.VbScript.Values.Objects;

namespace Common.Script.VbScript.Expressions
{
    /// <summary>
    /// 创建新对象
    /// </summary>
    [Expression(Operators.New, typeof(Creator))]
    class NewExpression : UnitaryExpression
    {
        public NewExpression(VariableExpression exp)
            : base(exp)
        {

        }

        protected override Value OnExecute(IExpressionContext context)
        {
            if (!RunningContext.IsInStatementContext)
                throw new ScriptRuntimeException("New表达式缺少上下文运行环境");

            VariableExpression varExp = (VariableExpression)Expression;

            Class classInfo = _GetClass(varExp.Name);
            if (classInfo == null)
                throw new ScriptRuntimeException(string.Format("未定义类{0}", varExp.Name));

            return classInfo.CreateObject(RunningContext.Current);
        }

        private Class _GetClass(string name)
        {
            Class cls = RunningContext.Current.GetClass(name);
            if (cls != null)
                return cls;

            return ClassManager.GetClass(name);
        }

        class Creator : ExpressionCreator
        {
            #region IExpressionCreator 成员

            public override Expression Create(Expression[] parameters)
            {
                ValidateParameters(parameters);

                if (parameters[0].GetType() != typeof(VariableExpression))
                    throw new ScriptException("NEW表达式语法错误");

                return new NewExpression((VariableExpression)parameters[0]);
            }

            #endregion
        }
    }
}

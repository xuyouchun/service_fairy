using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript.Statements
{
    [Statement(StatementType.For, typeof(Creator))]
    class ForStatement : StatementBase, IGotoSupported
    {
        public ForStatement(VariableExpression varExp, Expression initExp, Expression toExp, Expression stepExp, Statement body)
        {
            VarExpression = varExp;
            InitExpression = initExp;
            ToExpression = toExp;
            StepExpression = stepExp;
            Body = body;
        }

        /// <summary>
        /// 循环变量
        /// </summary>
        public VariableExpression VarExpression { get; private set; }

        /// <summary>
        /// 初始值
        /// </summary>
        public Expression InitExpression { get; private set; }

        /// <summary>
        /// 变量终止点
        /// </summary>
        public Expression ToExpression { get; private set; }

        /// <summary>
        /// 步长
        /// </summary>
        public Expression StepExpression { get; private set; }

        /// <summary>
        /// 循环体
        /// </summary>
        public Statement Body { get; private set; }

        protected override void OnExecute(RunningContext context)
        {
            IStatementContext stCtx = context.StatementContext;
            Value toValue = _GetToValue(context), initValue = _GetInitValue(context);
            Value stepValue = _GetStepValue(context, initValue, toValue);
            bool isForward = (double)stepValue > 0;
            VarExpression.SetValue(context.ExpressionContext, initValue);
            Value variable;

            while (_GetCondition(variable = _GetVariableValue(context), toValue, isForward))
            {
                if (!Body.Execute(context))
                    return;

                VarExpression.SetValue(context.ExpressionContext, variable + stepValue);
            }
        }

        #region IGotoSupported 成员

        void IGotoSupported.Execute(RunningContext context, Statement statement)
        {
            IStatementContext stCtx = context.StatementContext;
            Value stepValue = null, toValue = null, initValue = _GetInitValue(context);
            bool isForward = true;

            VarExpression.SetValue(context.ExpressionContext, initValue);
            Value variable = Value.Void;
            bool hasInit = false;

            do
            {
                if (hasInit)
                    VarExpression.SetValue(context.ExpressionContext, variable + stepValue);

                if (!Body.Execute(context))
                    return;

                if (!hasInit)
                {
                    toValue = _GetToValue(context);
                    stepValue = _GetStepValue(context, initValue, toValue);
                    isForward = (double)stepValue > 0;

                    hasInit = true;
                }

            } while (_GetCondition(variable = _GetVariableValue(context), toValue, isForward));
        }

        #endregion

        private bool _GetCondition(Value value, Value end, bool isForward)
        {
            if (isForward)
                return value <= end;

            return value >= end;
        }

        private Value _GetInitValue(RunningContext context)
        {
            Value init = InitExpression.Execute(context.ExpressionContext);
            if (!init.IsNumber())
                throw new ScriptException("FOR语句中初始变量表达式类型错误");

            return init;
        }

        private Value _GetStepValue(RunningContext context, Value initValue, Value toValue)
        {
            if (StepExpression != Expression.Empty)
            {
                Value step = StepExpression.Execute(context.ExpressionContext);
                if (!step.IsNumber())
                    throw new ScriptException("FOR语句中STEP表达式类型错误");

                return step;
            }

            return initValue > toValue ? -1 : 1;
        }

        private Value _GetVariableValue(RunningContext context)
        {
            Value variable = VarExpression.Execute(context.ExpressionContext);
            if (!variable.IsNumber())
                throw new ScriptException("FOR语句中循环变量表达式类型错误");

            return variable;
        }

        private Value _GetToValue(RunningContext context)
        {
            Value toValue = ToExpression.Execute(context.ExpressionContext);
            if (!toValue.IsNumber())
                throw new ScriptException("FOR语句中TO表达式类型错误");

            return toValue;
        }

        public override string ToString()
        {
            return "For...Next";
        }

        internal override IEnumerable<Statement> GetChildStatements()
        {
            yield return Body;
        }

        internal override IEnumerable<Expression> GetAllExpressions()
        {
            return new Expression[] { VarExpression, InitExpression, ToExpression, StepExpression };
        }

        class Creator : IStatementCreator
        {

            #region IStatementCreator 成员

            public Statement Create(Expression[] expressions, Statement[] statements)
            {
                if (expressions.Length != 4 || statements.Length != 1)
                    throw new ScriptException("FOR语句语法错误");

                VariableExpression varExp = expressions[0] as VariableExpression;
                if (varExp == null)
                    throw new ScriptException("FOR语句的循环变量类型错误");

                return new ForStatement(varExp, expressions[1], expressions[2], expressions[3], statements[0]);
            }

            #endregion
        }
    }
}

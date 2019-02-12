using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Values;
using VbObject = global::Common.Script.VbScript.Values.Object;

namespace Common.Script.VbScript.Expressions
{
    [Expression(Operators.Member, typeof(Creator))]
    class MemberExpression : DualityExpression, ILeftValueExpression
    {
        public MemberExpression(Expression exp1, VariableExpression exp2)
            : base(exp1, exp2)
        {

        }

        protected override Value OnExecute(IExpressionContext context)
        {
            if (!RunningContext.IsInStatementContext)
                throw new ScriptRuntimeException("缺少上下文运行环境");

            VbObject obj = _GetObject(RunningContext.Current);
            ObjectMember member = _GetObjectMember(obj);
            return member.Execute(RunningContext.Current, obj, ((VariableExpression)Expression2).GetParameters());
        }

        private ObjectMember _GetObjectMember(VbObject obj)
        {
            string memberName = ((VariableExpression)Expression2).Name;
            ObjectMember member = obj.OwnerClass.GetMember(memberName);
            if (member == null)
                throw new ScriptRuntimeException(string.Format("类{0}不包含成员运算符{1}", obj.OwnerClass.Name, memberName));

            return member;
        }

        private VbObject _GetObject(RunningContext context)
        {
            global::Common.Script.VbScript.Values.Object obj = Expression1.Execute(context.ExpressionContext) as VbObject;
            if (obj == null)
                throw new ScriptRuntimeException(string.Format("在非对象类型的变量{0}上访问其成员", Expression1));
            return obj;
        }

        public void SetValue(IExpressionContext context, Value value)
        {
            if (!RunningContext.IsInStatementContext)
                throw new ScriptRuntimeException("缺少上下文运行环境");

            VbObject obj = _GetObject(RunningContext.Current);
            ObjectMember member = _GetObjectMember(obj);
            ObjectVariableMember varMember = member as ObjectVariableMember;

            if (varMember == null)
                throw new ScriptRuntimeException(string.Format(string.Format("表达式{0}不能作为左值", member)));

            varMember.SetValue(RunningContext.Current, obj, value);
        }

        class Creator : ExpressionCreator
        {
            #region IExpressionCreator 成员

            public override Expression Create(Expression[] parameters)
            {
                ValidateParameters(parameters);

                VariableExpression varExp = parameters[1] as VariableExpression;
                if (varExp == null)
                    throw new ScriptException("获取成员的表达式语法错误");

                return new MemberExpression(parameters[0], varExp);
            }

            #endregion
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Values;
using Common.Script.VbScript.Common;

namespace Common.Script.VbScript.Expressions
{
    /// <summary>
    /// 动态类型，将在运行时识别是数组还是函数
    /// </summary>
    class DynamicExpression : VariableExpression, ILeftValueExpression
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        public DynamicExpression(string name, Expression[] parameters)
            : base(name)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// 参数
        /// </summary>
        public Expression[] Parameters { get; private set; }

        private Expression _RuntimeExpression = null;

        /// <summary>
        /// 在运行时获取实现的类型
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Expression GetRuntimeExpression(IExpressionContext context)
        {
            if (_RuntimeExpression != null)
                return _RuntimeExpression;

            // 尝试执行数组
            ArrayExpression arrExp = _TryGetArrayExpression(context);
            if (arrExp != null)
                return _RuntimeExpression = arrExp;

            if (RunningContext.IsInStatementContext)
            {
                // 尝试执行成员函数
                FunctionCallStackItem caller = RunningContext.Current.CallStack.Current;
                if (caller != null && caller.Object != null)
                {
                    string ownerClassName = caller.Function.OwnerClassName;
                    Class clsInfo;
                    ObjectFunctionMember funcMember;

                    if (string.Compare(caller.Object.OwnerClass.Name, ownerClassName, true) == 0
                        && (clsInfo = RunningContext.Current.GetClass(ownerClassName)) != null
                        && (funcMember = clsInfo.GetMember(Name) as ObjectFunctionMember) != null)
                    {
                        return _RuntimeExpression = new MemberExpression(new ValueExpression(caller.Object), this);
                    }
                }
            }

            // 尝试执行外部函数
            return _RuntimeExpression = new FunctionExpression(Name, Parameters);
        }

        private ArrayExpression _TryGetArrayExpression(IExpressionContext context)
        {
            ArrayObject arr;
            if ((arr = context.GetValue(Name) as ArrayObject) != null)
                return new ArrayExpression(Name, Parameters);

            return null;
        }

        protected override Value OnExecute(IExpressionContext context)
        {
            return GetValue(context);
        }

        public override Value GetValue(IExpressionContext context)
        {
            return GetRuntimeExpression(context).Execute(context);
        }

        public override void SetValue(IExpressionContext context, Value value)
        {
            ArrayExpression arrExp = _TryGetArrayExpression(context);
            if (arrExp == null)
                throw new ScriptRuntimeException(string.Format("表达式{0}不能作为左值", this.ToString()));

            arrExp.SetValue(context, value);
        }

        public override void Declare(IExpressionContext context)
        {
            new ArrayExpression(Name, Parameters).Declare(context);
        }

        public override Expression[] GetParameters()
        {
            return Parameters;
        }

        public override Expression[] GetAllSubExpressions()
        {
            return Parameters;
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name).Append("(");

            for (int k = 0; k < Parameters.Length; k++)
            {
                if (k > 0)
                    sb.Append(", ");

                sb.Append(Parameters[k].ToString());
            }

            sb.Append(")");
            return sb.ToString();
        }
    }
}
